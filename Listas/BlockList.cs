using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// This class uses two instances of <see cref="List{T}"/> to store instances of <see cref="B"/> and the positions of their starting elements.
	/// </summary>
	/// <remarks>
	/// Instances of this class will always have one, and only one, block with at least one free position, followed only by free positions.
	/// <para>
	/// Automatically generates a new block when the last one is filled. Its length is obtained from <see cref="ExtenderFunction"/>
	/// </para>
	/// </remarks>
	/// <typeparam name="E"></typeparam>
	/// <typeparam name="B"></typeparam>
	public sealed class BlockList<E,B> :
		IDynamicList<E>, IDynamicBlockList<E, B> where B : Block<E>{

		private readonly List<B> _blocks = [];

		private Func<int, int> _extender;
		private Func<int, E?> _generator;
		private readonly List<int> _positions = [];

		private static B CrearInstancia(int capacity) {
			return Block<E>.CreateInstance<B>(capacity);
		}

		/// <summary>
		/// Asegura que exista espacio en la list creando un block al final si, y solo si, el último block está lleno
		/// </summary>
		/// <remarks>
		/// Si el último block está vacío y el penúltimo no está lleno, se borrará el último,
		/// siempre que haya al menos 2 bloques
		/// </remarks>
		private void AsegurarEspacio() {
			int tam = _blocks.Count;
			if (_blocks[^1].IsFull) {
				int longitudNueva;
				longitudNueva = _extender(tam);
				_blocks.Add(CrearInstancia(_extender(Count)));
				_positions.Add(_positions[tam - 1] + _blocks[tam - 1].Capacity); //Se mete la primera posición del nuevo block
			} else if (tam > 1 && _blocks[^1].IsEmpty && !_blocks[^2].IsFull) {
				_blocks.RemoveAt(_blocks.Count-1);
				_positions.RemoveAt(tam - 1);
			}
		}

		private E? Generar() {
			E? generado = _generator.Invoke(Count);
			Contract.Requires<InvalidOperationException>(IExpandedList<E>.CompatibleEnLista(generado),Messages.NullGenerated);
			return generado;
		}

		/// <summary>
		/// Usa búsqueda binaria para encontrar el block de <c>position</c> en list
		/// </summary>
		/// <returns>
		/// La posición del block de <c>position</c>
		/// </returns>
		private static int EncontrarBinarioRecursivo(List<int> list, int inicio, int position) {
			int tam = list.Count;
			if (tam < 2) return inicio;
			if (tam == 2) { //Si hay dos suponemos que estará en uno
				if (position < list[1]) return inicio; //está en el block 0
				else return inicio + 1;
			}
			int medio = tam / 2; //Bloque entre el first y el último
			if (list[medio] <= position && position < list[medio + 1]) { //Si encontramos el block
				return inicio + medio;
			}
			if (position < list[medio]) { //Si está en un block anterior
				return EncontrarBinarioRecursivo(list.GetRange(0, medio), inicio, position);
			}
			return inicio + EncontrarBinarioRecursivo(list.GetRange(medio + 1, tam - medio - 1), medio + 1, position);
		}

		private static B Clonar(B block) {
			return Block<E>.CopyInstance<B>(block);
		}

		/// <summary>
		/// Creates an empty <see cref="BlockList{E, B}"/> that uses <c>generator</c> as <see cref="GeneratorFunction"/>,
		/// and <c>extender</c> as <see cref="ExtenderFunction"/>.
		/// </summary>
		public BlockList(Func<int,int> extender, Func<int,E?> generator) {
			_extender = extender;
			_generator = generator;
			_blocks.Add(CrearInstancia(_extender(0)));
			_positions.Add(0);
		}

		/// <summary>
		/// Creates an empty <see cref="BlockList{E, B}"/> that uses <c>extender</c> as <see cref="ExtenderFunction"/>.
		/// </summary>
		public BlockList(Func<int,int> extender) : this(extender, n => default) { }

		/// <summary>
		/// Creates an empty <see cref="BlockList{E, B}"/> that creates block with this capacity.
		/// </summary>
		public BlockList(int capacity) : this(n => capacity, n => default) { }

		/// <summary>
		/// Creates an empty <see cref="BlockList{E, B}"/> that generates block with capacity for 10 elements.
		/// </summary>
		public BlockList() : this(n => 10, n => default) { }

		/// <summary>
		/// Creates a <see cref="BlockList{E, B}"/> with the elements from <c>collection</c> and generates new blocks of length 10
		/// </summary>
		public BlockList(IEnumerable<E> collection) : this(collection, n => 10, n => default) { }

		/// <summary>
		/// Creates a <see cref="BlockList{E, B}"/> with the elements from <c>collection</c> in blocks with capacities obtained from <c>extender</c>
		/// </summary>
		/// <remarks>
		/// If <c>collection</c> is an <see cref="IBlockList{E, B}"/> its blocks will be copied
		/// </remarks>
		public BlockList(IEnumerable<E> collection, Func<int, int> extender, Func<int, E?> generator) : this() {
			if (collection is BlockList<E,B> list) {
				for (int i = 0; i < list.BlockCount - 1; i++) {
					Insert(Block<E>.CopyInstance<B>(list.GetBlock(i)),i);
				}
				_blocks[^1] = Block<E>.CopyInstance<B>(list.GetBlock(BlockCount-1));
				_extender = extender;
				_generator = generator;
			} else {
				_extender = extender;
				_generator = generator;
				foreach (var item in collection) {
					InsertLast(item);
				}
			}
		}

		public E this[int position] { 
			get {
				Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < Count,
					Messages.ListRange(position,Count), nameof(position));
				int block = GetBlockContainingPosition(position);
				return _blocks[block][position - _positions[block]];
			}
			set {
				Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < Count,
					Messages.ListRange(position, Count),nameof(position));
				int block = GetBlockContainingPosition(position);
				_blocks[block][position - _positions[block]] = value;
			}
		}

		E IExpandedList<E>.this[int position] => this[position];

		public Func<int, E?> GeneratorFunction { get => _generator; set => _generator = value; }
		public int Count { get => _positions[^1] + _blocks[^1].Count;
			set {
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0, Messages.NegativeArgument,
					nameof(value));
				int longitud = Count;
				if (value == longitud) return;
				while (longitud > value) { //Si se quiere reducir el tamaño
					RemoveLast();
					longitud--;
				}
				while (longitud < value) { //Si se quiere aumentar el tamaño
					InsertLast(Generar());
					longitud++;
				}
			}
		}
		public bool IsEmpty { get => _blocks[0].IsEmpty; set {
				if (value) {
					Clear();
				} else {
					if (IsEmpty) {
						Count++;
					}
				}
			}
		}
		public Func<int, int> ExtenderFunction { get => _extender; set => _extender = value; }

		public int BlockCount => _blocks.Count;

		bool IExpandedList<E>.IsEmpty => IsEmpty;

		int IExpandedList<E>.Count => Count;

		public int Remove(B block) {
			int i = 0; bool encontrado = false;
			while (i < _blocks.Count && !(encontrado = _blocks[i].Equals(block))) {
				i++;
			}
			if (encontrado) {
				RemoveBlockAt(i); //Lo borra si está
			} else {
				i = -1;
			}
			return i;
		}

		public B RemoveBlockAt(int position) {
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < BlockCount,
				Messages.ListRange(position,BlockCount), nameof(position));
			B aux;
			if (position == BlockCount - 1) {
				aux = _blocks[^1];
				_blocks[position] = CrearInstancia(_extender(position));
			} else {
				int longBloque = _positions[position + 1] - _positions[position];
				_positions.RemoveAt(position);
				aux = _blocks[position];
				_blocks.RemoveAt(position);
				for (int i = position; i < _positions.Count; i++) {
					_positions[i] -= longBloque;
				}
			}
			AsegurarEspacio();
			return aux;
		}

		public E RemoveLast() {
			Contract.Requires<InvalidOperationException>(Count > 0, Messages.EmptyList);
			B ultimo = _blocks[^1];
			E aux;
			if (ultimo.IsEmpty) { //Si el ultimo block está vacío el anterior no lo está
				aux = _blocks[^2].RemoveLast();
			} else {
				aux = ultimo.RemoveLast();
			}
			AsegurarEspacio();
			return aux;
		}

		// Este método ha sido cambiado al usar un List en lugar de LinkedList como en la versión de Java, antes se iteraba mediante un iterable de _bloques
		public E RemoveFirst() {
			Contract.Requires<InvalidOperationException>(Count > 0, Messages.EmptyList);
			object? acarreo = null; // Si dejo que sea E? no permite null, si es default no funciona para los tipos de valor
			E? acarreo2;
			B block = _blocks[^1];
			bool borrar = block.IsEmpty; // Guarda si se ha borrado el último block
			if (!block.IsEmpty) { // Para el último block
				acarreo = block.RemoveFirst();
			}
			if (_blocks.Count > 1) { // Para el último block
				block = _blocks[^2];
				acarreo2 = block.RemoveFirst();
				if (acarreo != null) {
					block.InsertLast((E)acarreo);
				}
				acarreo = acarreo2;
				for (int i = _blocks.Count - 3; i >= 0; i--) {
					block = _blocks[i];
					acarreo2 = block.RemoveFirst();
					block.InsertLast((E?)acarreo);
					acarreo = acarreo2;
				}
			}
			if (borrar) {
				_blocks.RemoveAt(_blocks.Count - 1);
				_positions.RemoveAt(_positions.Count - 1);
			}
			Debug.Assert(acarreo is not null);
			return (E)acarreo;
		}

		public void Clear() {
			_blocks.Clear();
			_positions.Clear();
			_blocks.Add(CrearInstancia(_extender(0)));
			_positions.Add(0);
		}

		public int Clear(E element) {
			int[] indices = Appareances(element);
			for (int i = indices.Length - 1; i >= 0; i--) {
				RemoveAt(indices[i]);
			}
			return indices.Length;
		}

		public int RemoveLast(E element) {
			int borrados = 0;
			while (!IsEmpty && Equals(Last,element)) { // Si el último element es nulo, se compara element son null
				RemoveLast();
				borrados++;
			}
			return borrados;
		}

		public int RemoveMultipleBlocks(int num, int position) {
			Contract.Requires<ArgumentOutOfRangeException>(num >= 0,Messages.NegativeArgument,
				nameof(num));
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < Count
				,Messages.ListRange(position,Count), nameof(position));
			int contador = 0;
			while (position + contador < BlockCount & contador < num) {
				RemoveBlockAt(position);
				contador++;
			}
			AsegurarEspacio();
			return contador;
		}

		public int GetBlockContainingPosition(int position) {
			if (position < 0 | position >= Count) return -1;
			return EncontrarBinarioRecursivo(_positions, 0, position);
		}

		public int GetBlockContainingElement(E element) {
			int pos = 0;
			bool encontrado = false;
			foreach (Block<E> block in _blocks) {
				if (block.Contains(element)) {
					encontrado = true;
					break;
				}
				pos++;
			}
			return encontrado ? pos : -1;
		}

		public IUnsortedList<E> CloneUnsorted() {
			return CloneDynamic();
		}

		public bool Contains(E element) {
			bool esta = false;
			for (int i = 0; i < _blocks.Count && !esta; i++) {
				esta = _blocks[i].Contains(element);
			}
			return esta;
		}

		public IExpandedList<E> Difference(IExpandedList<E> list) {
			BlockList<E, B> nueva = new(this);
			foreach (var item in list) {
				nueva.Clear(item);
			}
			return nueva;
		}

		public int Remove(E element) {
			int pos = Position(element);
			if (pos != -1) {
				RemoveAt(pos);
			}
			return pos;
		}

		// Como BorrarInicio(), este método ha recibido cambios para aprovechar que se una un List
		public E RemoveAt(int position) {
			E element = this[position];
			RemoveMultiple(1, position);
			return element;
		}

		private E EliminarArchivado(int position) {
		Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < Count,
				Messages.ListRange(position, Count), nameof(position));
			object? acarreo = null;
			E? acarreo2;
			B block = _blocks[^1];
			bool borrar = block.IsEmpty, borrado = false; // borrado guarda si se ha encontrado el element y se ha borrado antes de tiempo
			if (!block.IsEmpty) {
				if (_positions[^1] <= position) { // Si el element está al final, se borra
					acarreo = block.RemoveAt(position - _positions[^1]);
					borrado = true;
				} else {
					acarreo = block.RemoveFirst();
				}
			}
			if (!borrado && _blocks.Count > 1) { // Para el penúltimo block, que no debería estar vacío
				block = _blocks[^2];
				if (_positions[^2] <= position) {
					acarreo2 = block.RemoveAt(position - _positions[^2]);
					borrado = true;
				} else {
					acarreo2 = block.RemoveFirst();
				}
				if (acarreo != null) { // Si está vacío se elimina porque el último block tendrá espacio
					block.InsertLast((E)acarreo); // No se borra para evitar modificaciones mientras se itera
				}
				acarreo = acarreo2;
			}
			if (!borrado && _blocks.Count > 2) {
				int numBloque = _blocks.Count - 3, // numBloque pasa a ser al antepenúltimo
					limiteInfBloque = _positions[numBloque]; // Para minimizar las llamadas
				// Coloca el primer element de cada block al final del anterior hasta llegar al block con la posición
				while (position < limiteInfBloque) {
					block = _blocks[numBloque];
					acarreo2 = block.RemoveFirst();
					block.InsertLast((E)acarreo);
					acarreo = acarreo2;
					numBloque--;
					limiteInfBloque = _positions[numBloque];
				}
				int posEnBloque = position - limiteInfBloque;
				block = _blocks[numBloque];
				acarreo2 = block.RemoveAt(posEnBloque);
				block.InsertLast((E)acarreo);
				acarreo = acarreo2;
			}
			if (borrar) {
				_blocks.RemoveAt(_blocks.Count - 1);
				_positions.RemoveAt(_positions.Count - 1);
			}
			return (E)acarreo;
		}

		public int RemoveMultiple(int num, int position) { // Podría sustituir a Eliminar(int)
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < Count,
				Messages.ListRange(position,Count), nameof(position));
			int borrados = 0; // Guarda los elementos que han sido borrados
			if (num > 0) {
				int paraBorrar = Math.Min(num, Count - position); // Si hay menos de num elementos a partir de position, se borran todos
				if (paraBorrar == Count - position) {
					BorrarUltimos(paraBorrar);
				} else { // Si se quieren borrar menos elementos que los posibles dado position
					while (position + borrados + num < Count) {
						E movido = this[position + borrados + num];
						this[position + borrados] = movido;
						borrados++;
					}
					BorrarUltimos(paraBorrar);
					borrados = num;
				}
			}
			return borrados;
		}

		/// <summary>
		/// Borra los últimos <c>amount</c> elementos
		/// </summary>
		/// <param name="amount"></param>
		private void BorrarUltimos(int amount) {
			int bloqueBorrable = BlockCount - (_blocks[^1].IsEmpty ? 2 : 1), borrados = 0;
			bool borradoUltimo = _blocks[^1].IsEmpty;
			while (amount - borrados >= _blocks[bloqueBorrable].Count) { // Borramos bloques ya que no hay que mover elementos
				borrados += _blocks[bloqueBorrable].Count;
				RemoveBlockAt(bloqueBorrable--);
				if (!borradoUltimo) {
					borradoUltimo = true;
					bloqueBorrable--;
				}
			}
			if (borradoUltimo) { // Si se ha borrado alguno o el último estaba vacío, los elementos por borrar deben estar en el penúltimo
				_blocks[^2].Count -= amount - borrados;
			} else { // Es posible que no se haya borrado ningún block
				_blocks[^1].Count -= amount;
			}
			AsegurarEspacio();
		}

		public B GetBlock(int position) {
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < _blocks.Count,
				Messages.ListRange(position,Count), nameof(position));
			return _blocks[position];
		}

		public IEnumerable<B> GetBlockEnumerable() {
			return _blocks.AsEnumerable();
		}

		public IEnumerator<E> GetEnumerator() {
			foreach (var block in _blocks) {
				foreach (var element in block) {
					yield return element;
				}
			}
		}

		public void InsertAt(E element, int position) {
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position <= Count,
				Messages.ListRange(position,Count), nameof(position));
			if (position == 0) {
				InsertFirst(element);
			} else if (position == Count) {
				InsertLast(element);
			} else {
				int posicionBloque = GetBlockContainingPosition(position);
				bool colocado = !_blocks[posicionBloque].IsFull;
				E? acarreo = _blocks[posicionBloque].Insert(element, position - _positions[posicionBloque]);
				AsegurarEspacio();
				while (!colocado) {
					posicionBloque++;
					element = acarreo;
					colocado = !_blocks[posicionBloque].IsFull;
					acarreo = _blocks[posicionBloque].InsertFirst(element);
					AsegurarEspacio();
				}
			}
		}

		/// <inheritdoc/>
		/// <remarks>
		/// Just calls <see cref="InsertLast(E)"/>.
		/// </remarks>
		public int Add(E element) {
			InsertLast(element);
			return Count - 1;
		}

		public void Insert(B block, int position) {
			Contract.Requires<ArgumentNullException>(block is not null, Messages.NullBlock, nameof(block));
			Debug.Assert(block is not null);
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < BlockCount,
				Messages.ListRange(position,BlockCount), nameof(position));
			_blocks.Insert(position,block);
			if (position == 0) {
				_positions.Insert(0, 0);
			} else {
				_positions.Insert(position,_positions[position]);
			}
			
			for (int i = Math.Max(1,position + 1); i < _positions.Count; i++) { // Se mantienen las longitudes de bloques anteriores
				_positions[i] = _positions[i] + block.Count;
			}
		}

		public void InsertLast(E element) {
			_blocks[^1].InsertLast(element);
			AsegurarEspacio();
		}

		public void InsertFirst(E element) {
			bool colocado = false;
			int position = 0;
			E? acarreo, elementoParaInsertar = element;
			while (!colocado) {
				colocado = !_blocks[position].IsFull;
				acarreo = _blocks[position].InsertFirst(elementoParaInsertar);
				if (!colocado) { //Intenta colocar el acarreo al principio del siguiente block
					elementoParaInsertar = acarreo;
					position++;
				}
				AsegurarEspacio();
			}
		}

		public void InsertMultiple(E element, int num, int position) {
			if (num < 1) return;
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position <= Count,
				Messages.ListRange(position,Count), nameof(position));
			var temporalFuncion = GeneratorFunction;
			GeneratorFunction = n => element; // Para alargar la list y poder mover los elementos
			Count += num;
			GeneratorFunction = temporalFuncion;
			for (int i = Count-1; i >= position + num; i--) {
				this[i] = this[i - num];
			}
			for (int i = position; i < position + num; i++) {
				this[i] = element;
			}
		}


		/// <inheritdoc/>
		/// <remarks>
		/// This class does not allow moving the last block
		/// </remarks>
		public void SwapBlock(int first, int second) {
			if (first == second) return;
			Contract.Requires<ArgumentOutOfRangeException>(first >= 0 && first < _positions.Count,
				Messages.ListRange(first,Count), nameof(first));
			Contract.Requires<ArgumentOutOfRangeException>(second >= 0 && second < _positions.Count,
				Messages.ListRange(second,Count), nameof(second));
			(_blocks[second], _blocks[first]) = (_blocks[first], _blocks[second]);
			int deltaL = _blocks[first].Count - _blocks[second].Count;
			for (int i = first + 1; i <= second; i++) {
				_positions[i] = _positions[i] + deltaL;
			}
			AsegurarEspacio();
		}

		/// <inheritdoc/>
		/// <remarks>
		/// In this class, reverses the order of the blocks.
		/// <para>
		/// The last block is swapped for a block with the exact capacity needed to store its elements.
		/// </para>
		/// </remarks>
		public void Reverse() {
			foreach (var block in _blocks) { // Se invierten todos los bloques, después se invierte el orden de estos
				block.Reverse();
			}
			if (_blocks[^1].IsEmpty) {
				_blocks.Reverse(0,_blocks.Count - 1); // Si el último está vacío se deja al final
			} else {
				_blocks.Reverse();
				B antiguo = _blocks[0];
				_blocks[0] = CrearInstancia(antiguo.Count);
				foreach (var item in antiguo) {
					_blocks[0].InsertLast(item);
				}
			} // Después de este if, se ha invertido la list, pero se deben recalcular las longitudes
			_positions[0] = 0;
			for (int i = 1; i < _blocks.Count; i++) {
				_positions[i] = _positions[i-1] + _blocks[i-1].Count;
			}
			AsegurarEspacio();
		}

		public IUnsortedList<E> Multiply(int factor) {
			BlockList<E, B> listaNueva = new(ExtenderFunction,GeneratorFunction);
			if (factor == 0) {
				listaNueva.IsEmpty = true;
			} else {
				IEnumerator<E> enumerable;
				for (int i = 0; i < Math.Abs(factor); i++) {
					enumerable = GetEnumerator();
					while (enumerable.MoveNext())
						listaNueva.InsertLast(enumerable.Current);
				}
				if (factor < 0) {
					listaNueva.Reverse();
				}
			}
			return listaNueva;
		}

		public int[] Appareances(E element) {
			int veces = 0, indice = 0;
			int[] ocurrencias = [], cambio;
			foreach (Block<E> block in _blocks) {
				foreach (E elem in block) {
					if (Equals(elem, element)) {
						cambio = new int[++veces];
						Array.Copy(ocurrencias, cambio, ocurrencias.Length);
						cambio[^1] = indice;
						ocurrencias = cambio;
					}
					indice++;
				}
			}
			return ocurrencias;
		}

		// Como BorrarInicio() se ha cambiado para aprovechar el uso de List
		public int Position(E element) {
			int position = -1;
			bool encontrado = false;
			for (int i = 0; i < _blocks.Count && !encontrado; i++) {
				foreach (var posibleElem in _blocks[i]){
					position++;
					encontrado = Equals(posibleElem,element);
					if (encontrado) break;
				}
			}
			if (encontrado) {
				return position;
			} else {
				return -1;
			}
		}

		public int Position(B block) {
			int position = -1;
			for (int i = 0; i < _blocks.Count && position < 0; i++) {
				if (block.Equals(_blocks[i])) {
					position = i;
				}
			}
			return position;
		}

		public E First {
			get {
				Contract.Requires<InvalidOperationException>(!IsEmpty, Messages.EmptyList);
				return _blocks[0].First;
			}
		}

		public IExpandedList<E> Subtract(E element) {
			BlockList<E, B> nueva = new(this);
			nueva.Clear(element);
			return nueva;
		}

		public IBlockList<E, B> Subtract(B block) {
			BlockList<E,B> nueva = new(this);
			nueva.RemoveBlockAt(nueva.Position(block));
			return nueva;
		}

		/// <inheritdoc/>
		/// <remarks>
		/// In this class <c>block</c> may have free space only if it is swapped for the last block.
		/// <para><c>position</c> must be a valid block position.</para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public B SetBlock(B block, int position) {
			Contract.Requires<ArgumentOutOfRangeException>(position >= 0 && position < BlockCount,
				Messages.ListRange(position,BlockCount), nameof(position));
			Contract.Requires<ArgumentNullException>(block != null && (block.IsFull || position == _blocks.Count - 1),
				Messages.NonFullBlock, nameof(block)); // Si el block no está lleno debe cambiarse por el último
			B antiguo = _blocks[position];
			_blocks[position] = block;
			int deltaLongitud = block.Count - antiguo.Count; // Se deben cambiar las posiciones al cambiar el block
			if (deltaLongitud != 0)
				for (int i = position + 1; i < _positions.Count; i++) {
					_positions[i] += deltaLongitud;
				}
			AsegurarEspacio(); // Seguramente no haga nada aquí
			return antiguo;
		}

		public IExpandedList<E> AddNew(E element) {
			BlockList<E, B> nueva = new(this);
			nueva.InsertLast(element);
			return nueva;
		}

		public IDynamicBlockList<E, B> Add(B block) {
			BlockList<E, B> nueva = new(this);
			nueva.Insert(block,nueva.BlockCount-1);
			return nueva;
		}

		public E Last {
			get {
				Contract.Requires<InvalidOperationException>(!IsEmpty, Messages.EmptyList);
				Block<E> ultimo = _blocks[^1];
				if (ultimo.IsEmpty) {
					return _blocks[^2].Last;
				}
				return ultimo.Last;
			}
		}

		public IExpandedList<E> Join(IExpandedList<E> list) {
			BlockList<E,B> nueva = new(this);
			foreach (var item in list) {
				nueva.InsertLast(item);
			}
			return nueva;
		}

		public IExpandedList<E> Clone() {
			return CloneUnsorted();
		}

		public IBlockList<E, B> CloneBlock() {
			return CloneDynamicBlocks();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IDynamicBlockList<E, B> CloneDynamicBlocks() {
			return new BlockList<E, B>(this, ExtenderFunction, GeneratorFunction);
		}

		/// <summary>
		/// Determines if <c>obj</c> is a <see cref="IExpandedList{E}"/> with the same elements in the same order
		/// </summary>
		public override bool Equals(object? obj) {
			bool iguales = ReferenceEquals(this, obj);
			if (!iguales && obj is IExpandedList<E> list) {
				iguales = list.Count == Count;
				for (int i = 0; i < Count & !iguales; i++) {
					iguales = Equals(list[i],this[i]);
				}
			}
			return iguales;
		}

		/// <summary>
		/// Calculates a hash using its element's hashes.
		/// </summary>
		/// <returns>
		/// A hash for tis object.
		/// </returns>
		public override int GetHashCode() {
			int codigo = Count;
			foreach (var block in _blocks) {
				codigo ^= block.GetHashCode();
			}
			return codigo;
		}

		public override string ToString() {
			StringBuilder stringBuilder = new();
			stringBuilder.Append('[');
			int i = 0;
			foreach (B b in _blocks) {
				stringBuilder.Append(b);
				if (i++ < BlockCount - 1) {
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public IDynamicList<E> CloneDynamic() {
			return new BlockList<E,B>(this,ExtenderFunction,GeneratorFunction);
		}
	}
}
