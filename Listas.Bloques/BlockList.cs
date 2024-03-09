using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// Esta clase usa una instancia de <see cref="List{T}"/> para guardar las instancias de <c>B</c>
	/// </summary>
	/// <remarks>
	/// Las instancias de estas clases siempre tendrán un bloque con espacio para insertar más datos
	/// <para>
	/// Se usa una función para generar elementos y otra para calcular las nuevas longitudes de los bloques, 
	/// pueden ser cambiadas mediante propiedades
	/// </para>
	/// </remarks>
	/// <typeparam name="E"></typeparam>
	/// <typeparam name="B"></typeparam>
	public sealed class BlockList<E,B> :
		IDynamicList<E>, IDynamicBlockList<E, B> where B : Block<E>{

		private readonly List<B> _bloques = [];

		private Func<int, int> _extensora;
		private Func<int, E?> _generadora;
		private readonly List<int> _posiciones = [];

		private static B CrearInstancia(int capacidad) {
			return Block<E>.CreateInstance<B>(capacidad);
		}

		/// <summary>
		/// Asegura que exista espacio en la lista creando un bloque al final si, y solo si, el último bloque está lleno
		/// </summary>
		/// <remarks>
		/// Si el último bloque está vacío y el penúltimo no está lleno, se borrará el último,
		/// siempre que haya al menos 2 bloques
		/// </remarks>
		private void AsegurarEspacio() {
			int tam = _bloques.Count;
			if (_bloques[^1].IsFull) {
				int longitudNueva;
				longitudNueva = _extensora(tam);
				_bloques.Add(CrearInstancia(_extensora(Count)));
				_posiciones.Add(_posiciones[tam - 1] + _bloques[tam - 1].Capacity); //Se mete la primera posición del nuevo bloque
			} else if (tam > 1 && _bloques[^1].IsEmpty && !_bloques[^2].IsFull) {
				_bloques.RemoveAt(_bloques.Count-1);
				_posiciones.RemoveAt(tam - 1);
			}
		}

		private E? Generar() {
			E? generado = _generadora.Invoke(Count);
			Contract.Requires<InvalidOperationException>(IExList<E>.CompatibleEnLista(generado),Messages.NullGeneration);
			return generado;
		}

		/// <summary>
		/// Usa búsqueda binaria para encontrar el bloque de <c>pos</c> en lista
		/// </summary>
		/// <returns>
		/// La posición del bloque de <c>posicion</c>
		/// </returns>
		private static int EncontrarBinarioRecursivo(List<int> lista, int inicio, int posicion) {
			int tam = lista.Count;
			if (tam < 2) return inicio;
			if (tam == 2) { //Si hay dos suponemos que estará en uno
				if (posicion < lista[1]) return inicio; //está en el bloque 0
				else return inicio + 1;
			}
			int medio = tam / 2; //Bloque entre el primero y el último
			if (lista[medio] <= posicion && posicion < lista[medio + 1]) { //Si encontramos el bloque
				return inicio + medio;
			}
			if (posicion < lista[medio]) { //Si está en un bloque anterior
				return EncontrarBinarioRecursivo(lista.GetRange(0, medio), inicio, posicion);
			}
			return inicio + EncontrarBinarioRecursivo(lista.GetRange(medio + 1, tam - medio - 1), medio + 1, posicion);
		}

		private static B Clonar(B bloque) {
			return Block<E>.CopyInstance<B>(bloque);
		}

		/// <summary>
		/// Crea una <see cref="BlockList{E, B}"/> que genera nuevos elementos con <c>generadora</c>,
		/// guardados en bloques con capacidad dictada por <c>extensora</c>
		/// </summary>
		public BlockList(Func<int,int> extensora, Func<int,E?> generadora) {
			_extensora = extensora;
			_generadora = generadora;
			_bloques.Add(CrearInstancia(_extensora(0)));
			_posiciones.Add(0);
		}

		/// <summary>
		/// Crea una <see cref="BlockList{E, B}"/> con bloques de capacidad definida por <c>extensora</c>
		/// </summary>
		public BlockList(Func<int,int> extensora) : this(extensora, n => default) { }

		/// <summary>
		/// Crea una <see cref="BlockList{E, B}"/> con bloques con la capacidad especificada
		/// </summary>
		public BlockList(int capacidad) : this(n => capacidad, n => default) { }

		/// <summary>
		/// Crea una <see cref="BlockList{E, B}"/> con bloques con capacidad para 10 elementos
		/// </summary>
		public BlockList() : this(n => 10, n => default) { }

		/// <summary>
		/// Crea un <see cref="BlockList{E, B}"/> con los elementos de col con bloques de capacidad 10
		/// </summary>
		/// <param name="col"></param>
		public BlockList(IEnumerable<E> col) : this(col, n => 10, n => default) { }

		/// <summary>
		/// Crea un <see cref="BlockList{E, B}"/> con los elementos de <c>col</c> con bloques con la capacidad obtenida de <c>extensora</c>
		/// </summary>
		/// <remarks>
		/// Si <c>lista</c> es una <see cref="IBlockList{E, B}"/> se copiarán las capacidades de sus bloques
		/// </remarks>
		/// <param name="col"></param>
		/// <param name="extensora"></param>
		/// <param name="generadora"></param>
		public BlockList(IEnumerable<E> col, Func<int, int> extensora, Func<int, E?> generadora) : this() {
			if (col is BlockList<E,B> lista) {
				for (int i = 0; i < lista.CantidadBloques - 1; i++) {
					Insert(Block<E>.CopyInstance<B>(lista.GetBloque(i)),i);
				}
				_bloques[^1] = Block<E>.CopyInstance<B>(lista.GetBloque(CantidadBloques-1));
			} else {
				foreach (var item in col) {
					InsertLast(item);
				}
			}
			_extensora = extensora;
			_generadora = generadora;
		}

		public E this[int posicion] { 
			get {
				Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Count,
					Messages.ListRange(posicion,Count), nameof(posicion));
				int bloque = BuscarBloque(posicion);
				return _bloques[bloque][posicion - _posiciones[bloque]];
			}
			set {
				Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Count,
					Messages.ListRange(posicion, Count),nameof(posicion));
				int bloque = BuscarBloque(posicion);
				_bloques[bloque][posicion - _posiciones[bloque]] = value;
			}
		}

		E IExList<E>.this[int posicion] => this[posicion];

		public Func<int, E?> GeneratorFunction { get => _generadora; set => _generadora = value; }
		public int Count { get => _posiciones[^1] + _bloques[^1].Length;
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
		public bool IsEmpty { get => _bloques[0].IsEmpty; set {
				if (value) {
					Clear();
				} else {
					if (IsEmpty) {
						Count++;
					}
				}
			}
		}
		public Func<int, int> FuncionDeExtension { get => _extensora; set => _extensora = value; }

		public int CantidadBloques => _bloques.Count;

		bool IExList<E>.IsEmpty => IsEmpty;

		int IExList<E>.Count => Count;

		public int Borrar(B bloque) {
			int i = 0; bool encontrado = false;
			while (i < _bloques.Count && !(encontrado = _bloques[i].Equals(bloque))) {
				i++;
			}
			if (encontrado) {
				BorrarBloque(i); //Lo borra si está
			} else {
				i = -1;
			}
			return i;
		}

		public B BorrarBloque(int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Messages.ListRange(posicion,CantidadBloques), nameof(posicion));
			B aux;
			if (posicion == CantidadBloques - 1) {
				aux = _bloques[^1];
				_bloques[posicion] = CrearInstancia(_extensora(posicion));
			} else {
				int longBloque = _posiciones[posicion + 1] - _posiciones[posicion];
				_posiciones.RemoveAt(posicion);
				aux = _bloques[posicion];
				_bloques.RemoveAt(posicion);
				for (int i = posicion; i < _posiciones.Count; i++) {
					_posiciones[i] -= longBloque;
				}
			}
			AsegurarEspacio();
			return aux;
		}

		public E RemoveLast() {
			Contract.Requires<InvalidOperationException>(Count > 0, Messages.EmptyList);
			B ultimo = _bloques[^1];
			E aux;
			if (ultimo.IsEmpty) { //Si el ultimo bloque está vacío el anterior no lo está
				aux = _bloques[^2].RemoveLast();
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
			B bloque = _bloques[^1];
			bool borrar = bloque.IsEmpty; // Guarda si se ha borrado el último bloque
			if (!bloque.IsEmpty) { // Para el último bloque
				acarreo = bloque.RemoveFirst();
			}
			if (_bloques.Count > 1) { // Para el último bloque
				bloque = _bloques[^2];
				acarreo2 = bloque.RemoveFirst();
				if (acarreo != null) {
					bloque.InsertLast((E)acarreo);
				}
				acarreo = acarreo2;
				for (int i = _bloques.Count - 3; i >= 0; i--) {
					bloque = _bloques[i];
					acarreo2 = bloque.RemoveFirst();
					bloque.InsertLast((E?)acarreo);
					acarreo = acarreo2;
				}
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
			Debug.Assert(acarreo is not null);
			return (E)acarreo;
		}

		public void Clear() {
			_bloques.Clear();
			_posiciones.Clear();
			_bloques.Add(CrearInstancia(_extensora(0)));
			_posiciones.Add(0);
		}

		//Este método podría optimizarse para no tener que buscarlo cada vez
		public int Clear(E elemento) {
			int[] indices = Appareances(elemento);
			foreach (int i in indices.Reverse()) { // Si se hace en orden los indices no sirven
				Remove(i);
			}
			return indices.Length;
		}

		public int RemoveLast(E elemento) {
			int borrados = 0;
			while (!IsEmpty && Equals(Last,elemento)) { // Si el último elemento es nulo, se compara elemento son null
				RemoveLast();
				borrados++;
			}
			return borrados;
		}

		public int BorrarVariosBloques(int num, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(num >= 0,Messages.NegativeArgument,
				nameof(num));
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Count
				,Messages.ListRange(posicion,Count), nameof(posicion));
			int contador = 0;
			while (posicion + contador < CantidadBloques & contador < num) {
				BorrarBloque(posicion);
				contador++;
			}
			AsegurarEspacio();
			return contador;
		}

		public int BuscarBloque(int posicion) {
			if (posicion < 0 | posicion >= Count) return -1;
			return EncontrarBinarioRecursivo(_posiciones, 0, posicion);
		}

		public int BuscarBloque(E elemento) {
			int pos = 0;
			bool encontrado = false;
			foreach (Block<E> bloque in _bloques) {
				if (bloque.Contains(elemento)) {
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

		public bool Contains(E elemento) {
			bool esta = false;
			for (int i = 0; i < _bloques.Count && !esta; i++) {
				esta = _bloques[i].Contains(elemento);
			}
			return esta;
		}

		public IExList<E> Difference(IExList<E> lista) {
			BlockList<E, B> nueva = new(this);
			foreach (var item in lista) {
				nueva.Clear(item);
			}
			return nueva;
		}

		public int Remove(E elemento) {
			int pos = Position(elemento);
			if (pos != -1) {
				Remove(pos);
			}
			return pos;
		}

		// Como BorrarInicio(), este método ha recibido cambios para aprovechar que se una un List
		public E Remove(int posicion) {
			E elemento = this[posicion];
			RemoveMultiple(1, posicion);
			return elemento;
		}

		private E EliminarArchivado(int posicion) {
		Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Count,
				Messages.ListRange(posicion, Count), nameof(posicion));
			object? acarreo = null;
			E? acarreo2;
			B bloque = _bloques[^1];
			bool borrar = bloque.IsEmpty, borrado = false; // borrado guarda si se ha encontrado el elemento y se ha borrado antes de tiempo
			if (!bloque.IsEmpty) {
				if (_posiciones[^1] <= posicion) { // Si el elemento está al final, se borra
					acarreo = bloque.RemoveAt(posicion - _posiciones[^1]);
					borrado = true;
				} else {
					acarreo = bloque.RemoveFirst();
				}
			}
			if (!borrado && _bloques.Count > 1) { // Para el penúltimo bloque, que no debería estar vacío
				bloque = _bloques[^2];
				if (_posiciones[^2] <= posicion) {
					acarreo2 = bloque.RemoveAt(posicion - _posiciones[^2]);
					borrado = true;
				} else {
					acarreo2 = bloque.RemoveFirst();
				}
				if (acarreo != null) { // Si está vacío se elimina porque el último bloque tendrá espacio
					bloque.InsertLast((E)acarreo); // No se borra para evitar modificaciones mientras se itera
				}
				acarreo = acarreo2;
			}
			if (!borrado && _bloques.Count > 2) {
				int numBloque = _bloques.Count - 3, // numBloque pasa a ser al antepenúltimo
					limiteInfBloque = _posiciones[numBloque]; // Para minimizar las llamadas
				// Coloca el primer elemento de cada bloque al final del anterior hasta llegar al bloque con la posición
				while (posicion < limiteInfBloque) {
					bloque = _bloques[numBloque];
					acarreo2 = bloque.RemoveFirst();
					bloque.InsertLast((E)acarreo);
					acarreo = acarreo2;
					numBloque--;
					limiteInfBloque = _posiciones[numBloque];
				}
				int posEnBloque = posicion - limiteInfBloque;
				bloque = _bloques[numBloque];
				acarreo2 = bloque.RemoveAt(posEnBloque);
				bloque.InsertLast((E)acarreo);
				acarreo = acarreo2;
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
			return (E)acarreo;
		}

		public int RemoveMultiple(int num, int posicion) { // Podría sustituir a Eliminar(int)
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Count,
				Messages.ListRange(posicion,Count), nameof(posicion));
			int borrados = 0; // Guarda los elementos que han sido borrados
			if (num > 0) {
				int paraBorrar = Math.Min(num, Count - posicion); // Si hay menos de num elementos a partir de posicion, se borran todos
				if (paraBorrar == Count - posicion) {
					BorrarUltimos(paraBorrar);
				} else { // Si se quieren borrar menos elementos que los posibles dado posicion
					while (posicion + borrados + num < Count) {
						E movido = this[posicion + borrados + num];
						this[posicion + borrados] = movido;
						borrados++;
					}
					BorrarUltimos(paraBorrar);
					borrados = num;
				}
			}
			return borrados;
		}

		/// <summary>
		/// Borra los últimos <c>cantidad</c> elementos
		/// </summary>
		/// <param name="cantidad"></param>
		private void BorrarUltimos(int cantidad) {
			int bloqueBorrable = CantidadBloques - (_bloques[^1].IsEmpty ? 2 : 1), borrados = 0;
			bool borradoUltimo = _bloques[^1].IsEmpty;
			while (cantidad - borrados >= _bloques[bloqueBorrable].Length) { // Borramos bloques ya que no hay que mover elementos
				borrados += _bloques[bloqueBorrable].Length;
				BorrarBloque(bloqueBorrable--);
				if (!borradoUltimo) {
					borradoUltimo = true;
					bloqueBorrable--;
				}
			}
			if (borradoUltimo) { // Si se ha borrado alguno o el último estaba vacío, los elementos por borrar deben estar en el penúltimo
				_bloques[^2].Length -= cantidad - borrados;
			} else { // Es posible que no se haya borrado ningún bloque
				_bloques[^1].Length -= cantidad;
			}
			AsegurarEspacio();
		}

		public B GetBloque(int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < _bloques.Count,
				Messages.ListRange(posicion,Count), nameof(posicion));
			return _bloques[posicion];
		}

		public IEnumerable<B> GetBloques() {
			return _bloques.AsEnumerable();
		}

		public IEnumerator<E> GetEnumerator() {
			foreach (var bloque in _bloques) {
				foreach (var elemento in bloque) {
					yield return elemento;
				}
			}
		}

		public void InsertAt(E elemento, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Count,
				Messages.ListRange(posicion,Count), nameof(posicion));
			if (posicion == 0) {
				InsertFirst(elemento);
			} else if (posicion == Count) {
				InsertLast(elemento);
			} else {
				int posicionBloque = BuscarBloque(posicion);
				bool colocado = !_bloques[posicionBloque].IsFull;
				E? acarreo = _bloques[posicionBloque].Insert(elemento, posicion - _posiciones[posicionBloque]);
				AsegurarEspacio();
				while (!colocado) {
					posicionBloque++;
					elemento = acarreo;
					colocado = !_bloques[posicionBloque].IsFull;
					acarreo = _bloques[posicionBloque].InsertFirst(elemento);
					AsegurarEspacio();
				}
			}
		}

		/// <summary>
		/// Inserta el elemento al final de la lista
		/// </summary>
		/// <remarks>
		/// Llama directamente a <see cref="InsertLast(E)"/>
		/// </remarks>
		/// <param name="elemento"></param>
		/// <returns>
		/// Última posición de la lista
		/// </returns>
		public int Add(E elemento) {
			InsertLast(elemento);
			return Count - 1;
		}

		public void Insert(B bloque, int posicion) {
			Contract.Requires<ArgumentNullException>(bloque is not null, Messages.NullBlock, nameof(bloque));
			Debug.Assert(bloque is not null);
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Messages.ListRange(posicion,CantidadBloques), nameof(posicion));
			_bloques.Insert(posicion,bloque);
			if (posicion == 0) {
				_posiciones.Insert(0, 0);
			} else {
				_posiciones.Insert(posicion,_posiciones[posicion]);
			}
			
			for (int i = Math.Max(1,posicion + 1); i < _posiciones.Count; i++) { // Se mantienen las longitudes de bloques anteriores
				_posiciones[i] = _posiciones[i] + bloque.Length;
			}
		}

		public void InsertLast(E elemento) {
			_bloques[^1].InsertLast(elemento);
			AsegurarEspacio();
		}

		public void InsertFirst(E elemento) {
			bool colocado = false;
			int posicion = 0;
			E? acarreo, elementoParaInsertar = elemento;
			while (!colocado) {
				colocado = !_bloques[posicion].IsFull;
				acarreo = _bloques[posicion].InsertFirst(elementoParaInsertar);
				if (!colocado) { //Intenta colocar el acarreo al principio del siguiente bloque
					elementoParaInsertar = acarreo;
					posicion++;
				}
				AsegurarEspacio();
			}
		}

		public void InsertMultiple(E elemento, int num, int posicion) {
			if (num < 1) return;
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Count,
				Messages.ListRange(posicion,Count), nameof(posicion));
			var temporalFuncion = GeneratorFunction;
			GeneratorFunction = n => elemento; // Para alargar la lista y poder mover los elementos
			Count += num;
			GeneratorFunction = temporalFuncion;
			for (int i = Count-1; i >= posicion + num; i--) {
				this[i] = this[i - num];
			}
			for (int i = posicion; i < posicion + num; i++) {
				this[i] = elemento;
			}
		}


		/// <inheritdoc/>
		/// <remarks>
		/// Esta clase no permite cambiar la posición del último bloque
		/// </remarks>
		public void SwapBlock(int primero, int segundo) {
			if (primero == segundo) return;
			Contract.Requires<ArgumentOutOfRangeException>(primero >= 0 && primero < _posiciones.Count,
				Messages.ListRange(primero,Count), nameof(primero));
			Contract.Requires<ArgumentOutOfRangeException>(segundo >= 0 && segundo < _posiciones.Count,
				Messages.ListRange(segundo,Count), nameof(segundo));
			(_bloques[segundo], _bloques[primero]) = (_bloques[primero], _bloques[segundo]);
			int deltaL = _bloques[primero].Length - _bloques[segundo].Length;
			for (int i = primero + 1; i <= segundo; i++) {
				_posiciones[i] = _posiciones[i] + deltaL;
			}
			AsegurarEspacio();
		}

		/// <inheritdoc/>
		/// <remarks>
		/// En esta clase si el último bloque está vacío, se cambia por otro después de invertirlo y se introduce otro
		/// </remarks>
		public void Reverse() {
			foreach (var bloque in _bloques) { // Se invierten todos los bloques, después se invierte el orden de estos
				bloque.Reverse();
			}
			if (_bloques[^1].IsEmpty) {
				_bloques.Reverse(0,_bloques.Count - 1); // Si el último está vacío se deja al final
			} else {
				_bloques.Reverse();
				B antiguo = _bloques[0];
				_bloques[0] = CrearInstancia(antiguo.Length);
				foreach (var item in antiguo) {
					_bloques[0].InsertLast(item);
				}
			} // Después de este if, se ha invertido la lista, pero se deben recalcular las longitudes
			_posiciones[0] = 0;
			for (int i = 1; i < _bloques.Count; i++) {
				_posiciones[i] = _posiciones[i-1] + _bloques[i-1].Length;
			}
			AsegurarEspacio();
		}

		public IUnsortedList<E> Multiply(int factor) {
			BlockList<E, B> listaNueva = new(FuncionDeExtension,GeneratorFunction);
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

		public int[] Appareances(E elemento) {
			int veces = 0, indice = 0;
			int[] ocurrencias = [], cambio;
			foreach (Block<E> bloque in _bloques) {
				foreach (E elem in bloque) {
					if (Equals(elem, elemento)) {
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
		public int Position(E elemento) {
			int posicion = -1;
			bool encontrado = false;
			for (int i = 0; i < _bloques.Count && !encontrado; i++) {
				foreach (var posibleElem in _bloques[i]){
					posicion++;
					encontrado = Equals(posibleElem,elemento);
					if (encontrado) break;
				}
			}
			if (encontrado) {
				return posicion;
			} else {
				return -1;
			}
		}

		public int Posicion(B bloque) {
			int posicion = -1;
			for (int i = 0; i < _bloques.Count && posicion < 0; i++) {
				if (bloque.Equals(_bloques[i])) {
					posicion = i;
				}
			}
			return posicion;
		}

		public E First {
			get {
				Contract.Requires<InvalidOperationException>(!IsEmpty, Messages.EmptyList);
				return _bloques[0].First;
			}
		}

		public IExList<E> Substract(E elemento) {
			BlockList<E, B> nueva = new(this);
			nueva.Clear(elemento);
			return nueva;
		}

		public IBlockList<E, B> Restar(B bloque) {
			BlockList<E,B> nueva = new(this);
			nueva.BorrarBloque(nueva.Posicion(bloque));
			return nueva;
		}

		/// <inheritdoc/>
		/// <remarks>
		/// En esta clase solo se permite que <c>bloque</c> no este lleno si se cambia por el último
		/// <para><c>posicion</c> debe ser una posición de bloque válida</para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public B SetBlock(B bloque, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Messages.ListRange(posicion,CantidadBloques), nameof(posicion));
			Contract.Requires<ArgumentNullException>(bloque != null && (bloque.IsFull || posicion == _bloques.Count - 1),
				Messages.NonFullBlock, nameof(bloque)); // Si el bloque no está lleno debe cambiarse por el último
			B antiguo = _bloques[posicion];
			_bloques[posicion] = bloque;
			int deltaLongitud = bloque.Length - antiguo.Length; // Se deben cambiar las posiciones al cambiar el bloque
			if (deltaLongitud != 0)
				for (int i = posicion + 1; i < _posiciones.Count; i++) {
					_posiciones[i] += deltaLongitud;
				}
			AsegurarEspacio(); // Seguramente no haga nada aquí
			return antiguo;
		}

		public IExList<E> Add(E elemento) {
			BlockList<E, B> nueva = new(this);
			nueva.InsertLast(elemento);
			return nueva;
		}

		public IDynamicBlockList<E, B> Add(B bloque) {
			BlockList<E, B> nueva = new(this);
			nueva.Insert(bloque,nueva.CantidadBloques-1);
			return nueva;
		}

		public E Last {
			get {
				Contract.Requires<InvalidOperationException>(!IsEmpty, Messages.EmptyList);
				Block<E> ultimo = _bloques[^1];
				if (ultimo.IsEmpty) {
					return _bloques[^2].Last;
				}
				return ultimo.Last;
			}
		}

		public IExList<E> Join(IExList<E> segunda) {
			BlockList<E,B> nueva = new(this);
			foreach (var item in segunda) {
				nueva.InsertLast(item);
			}
			return nueva;
		}

		public IExList<E> Clone() {
			return CloneUnsorted();
		}

		public IBlockList<E, B> ClonarBloques() {
			return CloneDynamicBlocks();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IDynamicBlockList<E, B> CloneDynamicBlocks() {
			return new BlockList<E, B>(this, FuncionDeExtension, GeneratorFunction);
		}

		/// <summary>
		/// Comprueba si <c>obj</c> es una <see cref="IExList{E}"/> con los mismos elementos
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>
		/// <c>true</c> si es una <see cref="IExList{E}"/> con los mismos elementos, si no <c>false</c>
		/// </returns>
		public override bool Equals(object? obj) {
			bool iguales = ReferenceEquals(this, obj);
			if (!iguales && obj is IExList<E> lista) {
				iguales = lista.Count == Count;
				for (int i = 0; i < Count & !iguales; i++) {
					iguales = Equals(lista[i],this[i]);
				}
			}
			return iguales;
		}

		public override int GetHashCode() {
			int codigo = Count;
			foreach (var bloque in _bloques) {
				codigo ^= bloque.GetHashCode();
			}
			return codigo;
		}

		public override string ToString() {
			StringBuilder stringBuilder = new();
			stringBuilder.Append('[');
			int i = 0;
			foreach (B b in _bloques) {
				stringBuilder.Append(b);
				if (i++ < CantidadBloques - 1) {
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public IDynamicList<E> CloneDynamic() {
			return new BlockList<E,B>(this,FuncionDeExtension,GeneratorFunction);
		}
	}
}
