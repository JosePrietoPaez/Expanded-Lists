using Listas.Bloques;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Listas {
	public sealed class LinkedListaBloques<E,B> :
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
			} else if (tam > 1 && _bloques[^1].Vacio && !_bloques[tam - 2].Lleno) {
				_bloques.RemoveAt(_bloques.Count-1);
				_posiciones.Remove(tam - 1);
			}
		}

		private E? Generar() {
			E? generado = _generadora.Invoke(Longitud);
			Contract.Requires<InvalidOperationException>(ILista<E>.CompatibleEnLista(generado),Mensajes.GeneracionNula);
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

		/// <summary>
		/// Crea una <see cref="LinkedListaBloques{E}"/> que genera nuevos elementos con <c>generadora</c>,
		/// guardados en bloques con capacidad dictada por <c>extensora</c>
		/// </summary>
		public LinkedListaBloques(Func<int,int> extensora, Func<int,E?> generadora) {
			_extensora = extensora;
			_generadora = generadora;
			_bloques.Add(CrearInstancia(_extensora(0)));
			_posiciones.Add(0);
		}

		/// <summary>
		/// Crea una <see cref="LinkedListaBloques{E}"/> con bloques de capacidad definida por <c>extensora</c>
		/// </summary>
		public LinkedListaBloques(Func<int,int> extensora) : this(extensora, n => default) { }

		/// <summary>
		/// Crea una <see cref="LinkedListaBloques{E}"/> con bloques con la capacidad especificada
		/// </summary>
		public LinkedListaBloques(int capacidad) : this(n => capacidad, n => default) { }

		/// <summary>
		/// Crea una <see cref="LinkedListaBloques{E}"/> con bloques con capacidad para 10 elementos
		/// </summary>
		public LinkedListaBloques() : this(n => 10, n => default) { }

		public E this[int posicion] { 
			get {
				Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud,
					Mensajes.RangoLista(posicion,Longitud));
				int bloque = BuscarBloque(posicion);
				return _bloques[bloque][posicion - _posiciones[bloque]];
			} 
			set => throw new NotImplementedException(); }

		E ILista<E>.this[int posicion] => this[posicion];

		public Func<int, E?> FuncionDeGeneracion { get => _generadora; set => _generadora = value; }
		public int Longitud { get => _posiciones[^1] + _bloques[^1].Longitud;
			set {
				if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
				int longitud = Longitud;
				if (value == longitud) return;
				while (longitud > value) { //Si se quiere reducir el tamaño
					BorrarFin();
					longitud--;
				}
				while (longitud < value) { //Si se quiere aumentar el tamaño
#pragma warning disable CS8604 // Posible argumento de referencia nulo
					InsertarFin(Generar());
#pragma warning restore CS8604 // Posible argumento de referencia nulo
					longitud++;
				}
			}
		}
		public bool Vacia { get => _bloques[0].Vacio; set {
				if (value) {
					_bloques.Clear();
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
			throw new NotImplementedException();
		}

		public B BorrarBloque(int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud, Mensajes.RangoLista(posicion,Longitud));
			B aux;
			if (posicion == _posiciones.Count - 1) {
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

		public E BorrarFin() {
			Contract.Requires<InvalidOperationException>(Longitud > 0, Mensajes.VacioLista);
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
		public E BorrarInicio() {
			Contract.Requires<InvalidOperationException>(Longitud > 0, Mensajes.VacioLista);
			E? acarreo = default, acarreo2;
			B bloque = _bloques[^1];
			bool borrar = false; // Guarda si se ha borrado el último bloque
			if (bloque.Vacio) { // Para el último bloque
				borrar = true; // Si está vacío se borrará al final
			} else {
				acarreo = bloque.EliminarInicio();
			}
			if (_bloques.Count > 1) { // Para el último bloque
				bloque = _bloques[^2];
				acarreo2 = bloque.EliminarInicio();
				if (acarreo != null) {
					bloque.InsertarUltimo(acarreo);
				}
				acarreo = acarreo2;
				for (int i = _bloques.Count - 3; i <= 0; i--) {
					bloque = _bloques[i];
					acarreo2 = bloque.EliminarInicio();
					bloque.InsertarUltimo(acarreo);
					acarreo = acarreo2;
				}
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
			Debug.Assert(acarreo != null);
			return acarreo;
		}

		public void BorrarTodos() {
			throw new NotImplementedException();
		}

		public int BorrarTodos(E elemento) {
			throw new NotImplementedException();
		}

		public void BorrarTodosBloques() {
			throw new NotImplementedException();
		}

		public int BorrarUltimos(E elemento) {
			int borrados = 0;
			while (!Vacia && (UltimoElemento()?.Equals(elemento)??elemento is null)) { // Si el último elemento es nulo, se compara elemento son null
				BorrarFin();
				borrados++;
			}
			return borrados;
		}

		public int BorrarVariosBloques(int num, int posicion) {
			throw new NotImplementedException();
		}

		public int BuscarBloque(int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Longitud,
				Mensajes.RangoLista(posicion, Longitud));
			return EncontrarBinarioRecursivo(_posiciones, 0, posicion);
		}

		public int BuscarBloque(E elemento) {
			throw new NotImplementedException();
		}

		public IListaArbitraria<E> Clonar() {
			throw new NotImplementedException();
		}

		public bool Contiene(E elemento) {
			bool esta = false;
			for (int i = 0; i < _bloques.Count && !esta; i++) {
				esta = _bloques[i].Contiene(elemento);
			}
			return esta;
		}

		public ILista<E> Diferencia(ILista<E> lista) {
			throw new NotImplementedException();
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
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud, Mensajes.RangoLista(posicion, Longitud));
			E? acarreo = default, acarreo2;
			B bloque = _bloques[^1];
			bool borrar = false, borrado = false; // Borrado es para comprobar que se ha encontrado el elemento y se ha borrado antes de tiempo
			if (bloque.Vacio) {
				if (_posiciones[^1] <= posicion) { // Si el elemento está al final, se borra
					acarreo = bloque.Eliminar(posicion - _posiciones[^1]);
					borrado = true;
				} else {
					acarreo = bloque.EliminarInicio();
				}
			} else {
				borrar = true;
			}
			if (!borrado && _bloques.Count > 1) { // Para el penúltimo bloque, que no debería estar vacío
				bloque = _bloques[^2];
				if (_posiciones[^2] <= posicion) {
					acarreo2 = bloque.Eliminar(posicion - _posiciones[^2]);
					borrado = true;
				} else {
					acarreo2 = bloque.EliminarInicio();
				}
				if (acarreo != null) { // Si está vacío se elimina porque el último bloque tendrá espacio
					bloque.InsertarUltimo(acarreo); // No se borra para evitar modificaciones mientras se itera
				}
				acarreo = acarreo2;
			}
			if (!borrado && _bloques.Count > 2) {
				int numBloque = _bloques.Count - 3, // numBloque pasa a ser al antepenúltimo
					limiteInfBloque = _posiciones[numBloque]; // Para minimizar las llamadas
				// Coloca el primer elemento de cada bloque al final del anterior hasta llegar al bloque con la posición
				while (posicion < limiteInfBloque) {
					bloque = _bloques[numBloque];
					acarreo2 = bloque.EliminarInicio();
					bloque.InsertarUltimo(acarreo);
					acarreo = acarreo2;
					numBloque--;
					limiteInfBloque = _posiciones[numBloque];
				}
				int posEnBloque = posicion - limiteInfBloque;
				bloque = _bloques[numBloque];
				acarreo2 = bloque.Eliminar(posEnBloque);
				bloque.InsertarUltimo(acarreo);
				acarreo = acarreo2;
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
			Debug.Assert(acarreo != null);
			return acarreo;
		}

		public int EliminarVarios(int num, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < Longitud, Mensajes.RangoLista(posicion,Longitud));
			int borrados = 0;
			while (posicion < Longitud) {
				Eliminar(num);
				borrados++;
			}
			return borrados;
		}

		public B GetBloque(int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion < _bloques.Count,
				Mensajes.RangoLista(posicion,Longitud));
			return _bloques[posicion];
		}

		public IEnumerable<B> GetBloques() {
			return _bloques.AsEnumerable();
		}

		public IEnumerator<E> GetEnumerator() {
			throw new NotImplementedException();
		}

		public void Insertar(E elemento, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Longitud,
				Mensajes.RangoLista(posicion,Longitud));
			if (posicion == 0) {
				InsertarInicio(elemento);
			} else if (posicion == Longitud) {
				InsertarFin(elemento);
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
					acarreo = _bloques[posicionBloque].InsertarInicio(elemento);
					colocado = acarreo == null;
					AsegurarEspacio();
				}
			}
		}

		/// <summary>
		/// Inserta el elemento al final de la lista
		/// </summary>
		/// <remarks>
		/// Llama directamente a <see cref="InsertarFin(E)"/>
		/// </remarks>
		/// <param name="elemento"></param>
		/// <returns>
		/// Última posición de la lista
		/// </returns>
		public int Insertar(E elemento) {
			throw new NotImplementedException();
		}

		public int Insertar(B bloque) {
			throw new NotImplementedException();
		}

		public void InsertarFin(E elemento) {
			_bloques[^1].InsertarUltimo(elemento);
			AsegurarEspacio();
		}

		public void InsertarInicio(E elemento) {
			bool colocado = false;
			int posicion = 0;
			E? acarreo;
			while (!colocado) {
				acarreo = _bloques[posicion].InsertarInicio(elemento);
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
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= Longitud,
				Mensajes.RangoLista(posicion,Longitud));
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

		public void IntercambiarBloques(int primero, int segundo) {
			throw new NotImplementedException();
		}

		public void Invertir() {
			throw new NotImplementedException();
		}

		public IListaArbitraria<E> Multiplicar(int factor) {
			throw new NotImplementedException();
		}

		public int Ocurrencias(E elemento) {
			int veces = 0;
			E aux;
			foreach (Bloque<E> bloque in _bloques) {
				for (int j = 0; j < bloque.Longitud; j++) {
					aux = bloque[j];
					if (aux?.Equals(elemento) ?? elemento is null) {
						veces++;
					}
				}
			}
			return veces;
		}

		// Como BorrarInicio() se ha cambiado para aprovechar el uso de List
		public int Posicion(E elemento) {
			B bloque;
			int numBloque = -1, posicionEnBloque = -1;
			bool encontrado = false;
			for (int i = 0; i < _bloques.Count && !encontrado; i++) {
				bloque = _bloques[i];
				numBloque++;
				foreach (var posibleElem in bloque){
					posicionEnBloque++;
					encontrado = posibleElem?.Equals(elemento) ?? elemento is null;
				}
			}
			if (encontrado) {
				return _posiciones[numBloque] + posicionEnBloque;
			} else {
				return -1;
			}
		}

		public E PrimerElemento() {
			Contract.Requires<InvalidOperationException>(!Vacia, Mensajes.VacioLista);
			return _bloques[0].PrimerElemento();
		}

		public ILista<E> Restar(E elemento) {
			throw new NotImplementedException();
		}

		public IListaBloques<E, B> Restar(B bloque) {
			throw new NotImplementedException();
		}

		public B SetBloque(B bloque, int posicion) {
			throw new NotImplementedException();
		}

		public ILista<E> Sumar(E elemento) {
			throw new NotImplementedException();
		}

		public IListaBloques<E, B> Sumar(B bloque) {
			throw new NotImplementedException();
		}

		public E UltimoElemento() {
			Contract.Requires<InvalidOperationException>(!Vacia, Mensajes.VacioLista);
			Bloque<E> ultimo = _bloques[^1];
			if (ultimo.Vacio) {
				return _bloques[^2].UltimoElemento();
			}
			return ultimo.UltimoElemento();
		}

		public ILista<E> Unir(ILista<E> segunda) {
			throw new NotImplementedException();
		}

		ILista<E> ILista<E>.Clonar() {
			throw new NotImplementedException();
		}

		IListaBloques<E, B> IListaBloques<E, B>.Clonar() {
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}
	}
}
