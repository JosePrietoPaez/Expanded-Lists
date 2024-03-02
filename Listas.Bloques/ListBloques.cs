using Listas.Bloques;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Listas {

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
	public sealed class ListBloques<E,B> :
		IListaDinamica<E>, IListaBloquesDinamica<E, B> where B : Bloque<E>{

		private readonly List<B> _bloques = [];

		private Func<int, int> _extensora;
		private Func<int, E?> _generadora;
		private readonly List<int> _posiciones = [];

		private static B CrearInstancia(int capacidad) {
			return Bloque<E>.CrearInstancia<B>(capacidad);
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
			if (_bloques[^1].Lleno) {
				int longitudNueva;
				longitudNueva = _extensora(tam);
				_bloques.Add(CrearInstancia(_extensora(Longitud)));
				_posiciones.Add(_posiciones[tam - 1] + _bloques[tam - 1].Capacidad); //Se mete la primera posición del nuevo bloque
			} else if (tam > 1 && _bloques[^1].Vacio && !_bloques[^2].Lleno) {
				_bloques.RemoveAt(_bloques.Count-1);
				_posiciones.RemoveAt(tam - 1);
			}
		}

		private E? Generar() {
			E? generado = _generadora.Invoke(Longitud);
			Contrato.Requires<InvalidOperationException>(ILista<E>.CompatibleEnLista(generado),Mensajes.GeneracionNula);
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
			return Bloque<E>.CopiarInstancia<B>(bloque);
		}

		/// <summary>
		/// Crea una <see cref="ListBloques{E, B}"/> que genera nuevos elementos con <c>generadora</c>,
		/// guardados en bloques con capacidad dictada por <c>extensora</c>
		/// </summary>
		public ListBloques(Func<int,int> extensora, Func<int,E?> generadora) {
			_extensora = extensora;
			_generadora = generadora;
			_bloques.Add(CrearInstancia(_extensora(0)));
			_posiciones.Add(0);
		}

		/// <summary>
		/// Crea una <see cref="ListBloques{E, B}"/> con bloques de capacidad definida por <c>extensora</c>
		/// </summary>
		public ListBloques(Func<int,int> extensora) : this(extensora, n => default) { }

		/// <summary>
		/// Crea una <see cref="ListBloques{E, B}"/> con bloques con la capacidad especificada
		/// </summary>
		public ListBloques(int capacidad) : this(n => capacidad, n => default) { }

		/// <summary>
		/// Crea una <see cref="ListBloques{E, B}"/> con bloques con capacidad para 10 elementos
		/// </summary>
		public ListBloques() : this(n => 10, n => default) { }

		/// <summary>
		/// Crea un <see cref="ListBloques{E, B}"/> con los elementos de col con bloques de capacidad 10
		/// </summary>
		/// <param name="col"></param>
		public ListBloques(IEnumerable<E> col) : this(col, n => 10, n => default) { }

		/// <summary>
		/// Crea un <see cref="ListBloques{E, B}"/> con los elementos de <c>col</c> con bloques con la capacidad obtenida de <c>extensora</c>
		/// </summary>
		/// <remarks>
		/// Si <c>lista</c> es una <see cref="IListaBloques{E, B}"/> se copiarán las capacidades de sus bloques
		/// </remarks>
		/// <param name="col"></param>
		/// <param name="extensora"></param>
		/// <param name="generadora"></param>
		public ListBloques(IEnumerable<E> col, Func<int, int> extensora, Func<int, E?> generadora) : this() {
			if (col is ListBloques<E,B> lista) {
				for (int i = 0; i < lista.CantidadBloques - 1; i++) {
					Insertar(Bloque<E>.CopiarInstancia<B>(lista.GetBloque(i)),i);
				}
				_bloques[^1] = Bloque<E>.CopiarInstancia<B>(lista.GetBloque(CantidadBloques-1));
			} else {
				foreach (var item in col) {
					InsertarUltimo(item);
				}
			}
			_extensora = extensora;
			_generadora = generadora;
		}

		public E this[int posicion] { 
			get {
				Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud,
					Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
				int bloque = BuscarBloque(posicion);
				return _bloques[bloque][posicion - _posiciones[bloque]];
			}
			set {
				Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud,
					Mensajes.RangoLista(posicion, Longitud),nameof(posicion));
				int bloque = BuscarBloque(posicion);
				_bloques[bloque][posicion - _posiciones[bloque]] = value;
			}
		}

		E ILista<E>.this[int posicion] => this[posicion];

		public Func<int, E?> FuncionDeGeneracion { get => _generadora; set => _generadora = value; }
		public int Longitud { get => _posiciones[^1] + _bloques[^1].Longitud;
			set {
				Contrato.Requires<ArgumentOutOfRangeException>(value >= 0, Mensajes.ArgumentoNegativo,
					nameof(value));
				int longitud = Longitud;
				if (value == longitud) return;
				while (longitud > value) { //Si se quiere reducir el tamaño
					BorrarUltimo();
					longitud--;
				}
				while (longitud < value) { //Si se quiere aumentar el tamaño
					InsertarUltimo(Generar());
					longitud++;
				}
			}
		}
		public bool Vacia { get => _bloques[0].Vacio; set {
				if (value) {
					BorrarTodos();
				} else {
					if (Vacia) {
						Longitud++;
					}
				}
			}
		}
		public Func<int, int> FuncionDeExtension { get => _extensora; set => _extensora = value; }

		public int CantidadBloques => _bloques.Count;

		bool ILista<E>.Vacia => Vacia;

		int ILista<E>.Longitud => Longitud;

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
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Mensajes.RangoLista(posicion,CantidadBloques), nameof(posicion));
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

		public E BorrarUltimo() {
			Contrato.Requires<InvalidOperationException>(Longitud > 0, Mensajes.VacioLista);
			B ultimo = _bloques[^1];
			E aux;
			if (ultimo.Vacio) { //Si el ultimo bloque está vacío el anterior no lo está
				aux = _bloques[^2].EliminarUltimo();
			} else {
				aux = ultimo.EliminarUltimo();
			}
			AsegurarEspacio();
			return aux;
		}

		// Este método ha sido cambiado al usar un List en lugar de LinkedList como en la versión de Java, antes se iteraba mediante un iterable de _bloques
		public E BorrarPrimero() {
			Contrato.Requires<InvalidOperationException>(Longitud > 0, Mensajes.VacioLista);
			object? acarreo = null; // Si dejo que sea E? no permite null, si es default no funciona para los tipos de valor
			E? acarreo2;
			B bloque = _bloques[^1];
			bool borrar = bloque.Vacio; // Guarda si se ha borrado el último bloque
			if (!bloque.Vacio) { // Para el último bloque
				acarreo = bloque.EliminarPrimero();
			}
			if (_bloques.Count > 1) { // Para el último bloque
				bloque = _bloques[^2];
				acarreo2 = bloque.EliminarPrimero();
				if (acarreo != null) {
					bloque.InsertarUltimo((E)acarreo);
				}
				acarreo = acarreo2;
				for (int i = _bloques.Count - 3; i >= 0; i--) {
					bloque = _bloques[i];
					acarreo2 = bloque.EliminarPrimero();
					bloque.InsertarUltimo((E?)acarreo);
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

		public void BorrarTodos() {
			_bloques.Clear();
			_posiciones.Clear();
			_bloques.Add(CrearInstancia(_extensora(0)));
			_posiciones.Add(0);
		}

		//Este método podría optimizarse para no tener que buscarlo cada vez
		public int BorrarTodos(E elemento) {
			int[] indices = Ocurrencias(elemento);
			foreach (int i in indices.Reverse()) { // Si se hace en orden los indices no sirven
				Eliminar(i);
			}
			return indices.Length;
		}

		public int BorrarUltimos(E elemento) {
			int borrados = 0;
			while (!Vacia && Equals(UltimoElemento,elemento)) { // Si el último elemento es nulo, se compara elemento son null
				BorrarUltimo();
				borrados++;
			}
			return borrados;
		}

		public int BorrarVariosBloques(int num, int posicion) {
			Contrato.Requires<ArgumentOutOfRangeException>(num >= 0,Mensajes.ArgumentoNegativo,
				nameof(num));
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud
				,Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
			int contador = 0;
			while (posicion + contador < CantidadBloques & contador < num) {
				BorrarBloque(posicion);
				contador++;
			}
			AsegurarEspacio();
			return contador;
		}

		public int BuscarBloque(int posicion) {
			if (posicion < 0 | posicion >= Longitud) return -1;
			return EncontrarBinarioRecursivo(_posiciones, 0, posicion);
		}

		public int BuscarBloque(E elemento) {
			int pos = 0;
			bool encontrado = false;
			foreach (Bloque<E> bloque in _bloques) {
				if (bloque.Contiene(elemento)) {
					encontrado = true;
					break;
				}
				pos++;
			}
			return encontrado ? pos : -1;
		}

		public IListaArbitraria<E> ClonarArbitraria() {
			return ClonarDinamica();
		}

		public bool Contiene(E elemento) {
			bool esta = false;
			for (int i = 0; i < _bloques.Count && !esta; i++) {
				esta = _bloques[i].Contiene(elemento);
			}
			return esta;
		}

		public ILista<E> Diferencia(ILista<E> lista) {
			ListBloques<E, B> nueva = new(this);
			foreach (var item in lista) {
				nueva.BorrarTodos(item);
			}
			return nueva;
		}

		public int Eliminar(E elemento) {
			int pos = Posicion(elemento);
			if (pos != -1) {
				Eliminar(pos);
			}
			return pos;
		}

		// Como BorrarInicio(), este método ha recibido cambios para aprovechar que se una un List
		public E Eliminar(int posicion) {
			E elemento = this[posicion];
			EliminarVarios(1, posicion);
			return elemento;
		}

		private E EliminarArchivado(int posicion) {
		Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud,
				Mensajes.RangoLista(posicion, Longitud), nameof(posicion));
			object? acarreo = null;
			E? acarreo2;
			B bloque = _bloques[^1];
			bool borrar = bloque.Vacio, borrado = false; // borrado guarda si se ha encontrado el elemento y se ha borrado antes de tiempo
			if (!bloque.Vacio) {
				if (_posiciones[^1] <= posicion) { // Si el elemento está al final, se borra
					acarreo = bloque.Eliminar(posicion - _posiciones[^1]);
					borrado = true;
				} else {
					acarreo = bloque.EliminarPrimero();
				}
			}
			if (!borrado && _bloques.Count > 1) { // Para el penúltimo bloque, que no debería estar vacío
				bloque = _bloques[^2];
				if (_posiciones[^2] <= posicion) {
					acarreo2 = bloque.Eliminar(posicion - _posiciones[^2]);
					borrado = true;
				} else {
					acarreo2 = bloque.EliminarPrimero();
				}
				if (acarreo != null) { // Si está vacío se elimina porque el último bloque tendrá espacio
					bloque.InsertarUltimo((E)acarreo); // No se borra para evitar modificaciones mientras se itera
				}
				acarreo = acarreo2;
			}
			if (!borrado && _bloques.Count > 2) {
				int numBloque = _bloques.Count - 3, // numBloque pasa a ser al antepenúltimo
					limiteInfBloque = _posiciones[numBloque]; // Para minimizar las llamadas
				// Coloca el primer elemento de cada bloque al final del anterior hasta llegar al bloque con la posición
				while (posicion < limiteInfBloque) {
					bloque = _bloques[numBloque];
					acarreo2 = bloque.EliminarPrimero();
					bloque.InsertarUltimo((E)acarreo);
					acarreo = acarreo2;
					numBloque--;
					limiteInfBloque = _posiciones[numBloque];
				}
				int posEnBloque = posicion - limiteInfBloque;
				bloque = _bloques[numBloque];
				acarreo2 = bloque.Eliminar(posEnBloque);
				bloque.InsertarUltimo((E)acarreo);
				acarreo = acarreo2;
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
			Debug.Assert(acarreo != null);
			return (E)acarreo;
		}

		public int EliminarVarios(int num, int posicion) { // Podría sustituir a Eliminar(int)
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud,
				Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
			int borrados = 0; // Guarda los elementos que han sido borrados
			if (num > 0) {
				int paraBorrar = Math.Min(num, Longitud - posicion); // Si hay menos de num elementos a partir de posicion, se borran todos
				if (paraBorrar == Longitud - posicion) {
					BorrarUltimos(paraBorrar);
				} else { // Si se quieren borrar menos elementos que los posibles dado posicion
					while (posicion + borrados + num < Longitud) {
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
			int bloqueBorrable = CantidadBloques - (_bloques[^1].Vacio ? 2 : 1), borrados = 0;
			bool borradoUltimo = _bloques[^1].Vacio;
			while (cantidad - borrados >= _bloques[bloqueBorrable].Longitud) { // Borramos bloques ya que no hay que mover elementos
				borrados += _bloques[bloqueBorrable].Longitud;
				BorrarBloque(bloqueBorrable--);
				if (!borradoUltimo) {
					borradoUltimo = true;
					bloqueBorrable--;
				}
			}
			if (borradoUltimo) { // Si se ha borrado alguno o el último estaba vacío, los elementos por borrar deben estar en el penúltimo
				_bloques[^2].Longitud -= cantidad - borrados;
			} else { // Es posible que no se haya borrado ningún bloque
				_bloques[^1].Longitud -= cantidad;
			}
			AsegurarEspacio();
		}

		public B GetBloque(int posicion) {
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < _bloques.Count,
				Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
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

		public void Insertar(E elemento, int posicion) {
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Longitud,
				Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
			if (posicion == 0) {
				InsertarPrimero(elemento);
			} else if (posicion == Longitud) {
				InsertarUltimo(elemento);
			} else {
				bool colocado;
				int posicionBloque = BuscarBloque(posicion);
				E? acarreo = _bloques[posicionBloque].Insertar(elemento, posicion - _posiciones[posicionBloque]);
				colocado = acarreo == null;
				AsegurarEspacio();
				while (!colocado) {
					posicionBloque++;
					Debug.Assert(acarreo != null);
					elemento = acarreo;
					acarreo = _bloques[posicionBloque].InsertarPrimero(elemento);
					colocado = acarreo == null;
					AsegurarEspacio();
				}
			}
		}

		/// <summary>
		/// Inserta el elemento al final de la lista
		/// </summary>
		/// <remarks>
		/// Llama directamente a <see cref="InsertarUltimo(E)"/>
		/// </remarks>
		/// <param name="elemento"></param>
		/// <returns>
		/// Última posición de la lista
		/// </returns>
		public int Insertar(E elemento) {
			InsertarUltimo(elemento);
			return Longitud - 1;
		}

		public void Insertar(B bloque, int posicion) {
			Contrato.Requires<ArgumentNullException>(bloque is not null, Mensajes.BloqueNulo, nameof(bloque));
			Debug.Assert(bloque is not null);
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Mensajes.RangoLista(posicion,CantidadBloques), nameof(posicion));
			_bloques.Insert(posicion,bloque);
			if (posicion == 0) {
				_posiciones.Insert(0, 0);
			} else {
				_posiciones.Insert(posicion,_posiciones[posicion]);
			}
			
			for (int i = Math.Max(1,posicion + 1); i < _posiciones.Count; i++) { // Se mantienen las longitudes de bloques anteriores
				_posiciones[i] = _posiciones[i] + bloque.Longitud;
			}
		}

		public void InsertarUltimo(E elemento) {
			_bloques[^1].InsertarUltimo(elemento);
			AsegurarEspacio();
		}

		public void InsertarPrimero(E elemento) {
			bool colocado = false;
			int posicion = 0;
			E? acarreo;
			while (!colocado) {
				acarreo = _bloques[posicion].InsertarPrimero(elemento);
				if (acarreo == null) {
					colocado = true;
				} else { //Intenta colocar el acarreo al principio del siguiente bloque
					elemento = acarreo;
					posicion++;
				}
				AsegurarEspacio();
			}
		}

		public void InsertarVarios(E elemento, int num, int posicion) {
			if (num < 1) return;
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Longitud,
				Mensajes.RangoLista(posicion,Longitud), nameof(posicion));
			List<E> acarreo = [];
			int bloque = BuscarBloque(posicion), inicial = _posiciones[bloque], posEnBloque = posicion - inicial;
			Bloque<E> actual = _bloques[bloque];
			for (int i = 0; i < num; i++, posEnBloque++) { //Se colocan los elem
				E aux;
				bool habraAcarreo = actual.Lleno;
				aux = actual.Insertar(elemento, posEnBloque);
				if (habraAcarreo) {
					acarreo.Add(aux);
				}
				if (posEnBloque == actual.Capacidad - 1) { //Para evitar que coloque en posiciones inválidas
					posEnBloque = -1;
					AsegurarEspacio();
					bloque++;
					actual = _bloques[bloque];
				}
			}
			while (acarreo.Count > 0) {
				E? acarreoBloque, elementoInsertar;
				for (int indiceLista = 0; indiceLista < acarreo.Count; indiceLista++, posEnBloque++) {
					elementoInsertar = acarreo[indiceLista];
					acarreoBloque = actual.Insertar(elementoInsertar, posEnBloque);
					acarreo[indiceLista] = acarreoBloque;
					if (posEnBloque == actual.Capacidad - 1) {
						posEnBloque = -1;
						AsegurarEspacio();
						bloque++;
						actual = _bloques[bloque];
					}
				}
				int i = 0;
				while (i < acarreo.Count) { //Elimina los elementos nulos necesarios
					if (acarreo[i] == null) {
						acarreo.RemoveAt(i);
					} else {
						i++;
					}
				}
			}
		}

		/// <inheritdoc/>
		/// <remarks>
		/// Esta clase no permite cambiar la posición del último bloque
		/// </remarks>
		public void IntercambiarBloques(int primero, int segundo) {
			if (primero == segundo) return;
			Contrato.Requires<ArgumentOutOfRangeException>(primero >= 0 && primero < _posiciones.Count,
				Mensajes.RangoLista(primero,Longitud), nameof(primero));
			Contrato.Requires<ArgumentOutOfRangeException>(segundo >= 0 && segundo < _posiciones.Count,
				Mensajes.RangoLista(segundo,Longitud), nameof(segundo));
			(_bloques[segundo], _bloques[primero]) = (_bloques[primero], _bloques[segundo]);
			int deltaL = _bloques[primero].Longitud - _bloques[segundo].Longitud;
			for (int i = primero + 1; i <= segundo; i++) {
				_posiciones[i] = _posiciones[i] + deltaL;
			}
			AsegurarEspacio();
		}

		/// <inheritdoc/>
		/// <remarks>
		/// En esta clase si el último bloque está vacío, se cambia por otro después de invertirlo y se introduce otro
		/// </remarks>
		public void Invertir() {
			foreach (var bloque in _bloques) { // Se invierten todos los bloques, después se invierte el orden de estos
				bloque.Invertir();
			}
			if (_bloques[^1].Vacio) {
				_bloques.Reverse(0,_bloques.Count - 2); // Si el último está vacío se deja al final
			} else {
				_bloques.Reverse();
				B antiguo = _bloques[0];
				_bloques[0] = CrearInstancia(antiguo.Longitud);
				foreach (var item in antiguo) {
					_bloques[0].InsertarUltimo(item);
				}
			} // Después de este if, se ha invertido la lista, pero se deben recalcular las longitudes
			_posiciones[0] = 0;
			for (int i = 1; i < _bloques.Count; i++) {
				_posiciones[i] = _posiciones[i-1] + _bloques[i-1].Longitud;
			}
			AsegurarEspacio();
		}

		public IListaArbitraria<E> Multiplicar(int factor) {
			ListBloques<E, B> listaNueva = new(this);
			if (factor == 0) {
				listaNueva.Vacia = true;
			} else {
				if (factor < 0) {
					listaNueva.Invertir();
				} else {
					B nuevo = CrearInstancia(listaNueva._bloques[^1].Longitud); //Crea un bloque con la longitud justa
					foreach (var item in listaNueva._bloques[^1]) {
						nuevo.InsertarUltimo(item);
					}
					listaNueva._bloques[^1] = nuevo;
				}
				for (int i = 0; i < _bloques.Count * Math.Abs(factor); i++) {
					listaNueva.Insertar(Clonar(_bloques[i%CantidadBloques]), i);
				}
			}
			return listaNueva;
		}

		public int[] Ocurrencias(E elemento) {
			int veces = 0, indice = 0;
			int[] ocurrencias = [], cambio;
			foreach (Bloque<E> bloque in _bloques) {
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
		public int Posicion(E elemento) {
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

		public E PrimerElemento {
			get {
				Contrato.Requires<InvalidOperationException>(!Vacia, Mensajes.VacioLista);
				return _bloques[0].PrimerElemento;
			}
		}

		public ILista<E> Restar(E elemento) {
			ListBloques<E, B> nueva = new(this);
			nueva.BorrarTodos(elemento);
			return nueva;
		}

		public IListaBloques<E, B> Restar(B bloque) {
			ListBloques<E,B> nueva = new(this);
			nueva.BorrarBloque(nueva.Posicion(bloque));
			return nueva;
		}

		/// <inheritdoc/>
		/// <remarks>
		/// En esta clase solo se permite que <c>bloque</c> no este lleno si se cambia por el último
		/// <para><c>posicion</c> debe ser una posición de bloque válida</para>
		/// </remarks>
		/// <exception cref="ArgumentException"></exception>
		public B SetBloque(B bloque, int posicion) {
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < CantidadBloques,
				Mensajes.RangoLista(posicion,CantidadBloques), nameof(posicion));
			Contrato.Requires<ArgumentNullException>(bloque != null && (bloque.Lleno || posicion == _bloques.Count - 1),
				Mensajes.BloqueNoLleno, nameof(bloque)); // Si el bloque no está lleno debe cambiarse por el último
			Debug.Assert(bloque != null);
			B antiguo = _bloques[posicion];
			_bloques[posicion] = bloque;
			AsegurarEspacio();
			return antiguo;
		}

		public ILista<E> Sumar(E elemento) {
			ListBloques<E, B> nueva = new(this);
			nueva.InsertarUltimo(elemento);
			return nueva;
		}

		public IListaBloquesDinamica<E, B> Sumar(B bloque) {
			ListBloques<E, B> nueva = new(this);
			nueva.Insertar(bloque,nueva.CantidadBloques-1);
			return nueva;
		}

		public E UltimoElemento {
			get {
				Contrato.Requires<InvalidOperationException>(!Vacia, Mensajes.VacioLista);
				Bloque<E> ultimo = _bloques[^1];
				if (ultimo.Vacio) {
					return _bloques[^2].UltimoElemento;
				}
				return ultimo.UltimoElemento;
			}
		}

		public ILista<E> Unir(ILista<E> segunda) {
			ListBloques<E,B> nueva = new(this);
			foreach (var item in segunda) {
				nueva.InsertarUltimo(item);
			}
			return nueva;
		}

		public ILista<E> Clonar() {
			return ClonarArbitraria();
		}

		public IListaBloques<E, B> ClonarBloques() {
			return ClonarBloquesDinamica();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IListaBloquesDinamica<E, B> ClonarBloquesDinamica() {
			return new ListBloques<E, B>(this, FuncionDeExtension, FuncionDeGeneracion);
		}

		/// <summary>
		/// Comprueba si <c>obj</c> es una <see cref="ILista{E}"/> con los mismos elementos
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>
		/// <c>true</c> si es una <see cref="ILista{E}"/> con los mismos elementos, si no <c>false</c>
		/// </returns>
		public override bool Equals(object? obj) {
			bool iguales = ReferenceEquals(this, obj);
			if (!iguales && obj is ILista<E> lista) {
				iguales = lista.Longitud == Longitud;
				for (int i = 0; i < Longitud & !iguales; i++) {
					iguales = Equals(lista[i],this[i]);
				}
			}
			return iguales;
		}

		public override int GetHashCode() {
			int codigo = Longitud;
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

		public IListaDinamica<E> ClonarDinamica() {
			return new ListBloques<E,B>(this,FuncionDeExtension,FuncionDeGeneracion);
		}
	}
}
