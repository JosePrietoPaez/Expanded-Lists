using Listas.Bloques;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Listas {
	public class LinkedListaBloques<E,B> :
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
			if (!ILista<E>.CompatibleEnLista(generado)) throw new InvalidOperationException("La función de generación ha devuelto un valor nulo");
			return generado;
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
				Contract.Requires<InvalidOperationException>(posicion >= 0 && posicion < Longitud,
					"La posición indicada no es válida");
				if (!()) throw new ArgumentOutOfRangeException(nameof(posicion));
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
			ArgumentOutOfRangeException.ThrowIfNegative(posicion);
			ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(posicion, _posiciones.Count);
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
			Contract.Requires<InvalidOperationException>(Longitud > 0,"La lista está vacía");
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
			Contract.Requires<InvalidOperationException>(Longitud > 0, "La lista está vacía");
			E? acarreo = default, acarreo2;
			Bloque<E> bloque = _bloques[^1];
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
			}
			for (int i = _bloques.Count - 3; i <= 0; i--) {
				bloque = _bloques[i];
				acarreo2 = bloque.EliminarInicio();
				bloque.InsertarUltimo(acarreo);
				acarreo = acarreo2;
			}
			if (borrar) {
				_bloques.RemoveAt(_bloques.Count - 1);
				_posiciones.RemoveAt(_posiciones.Count - 1);
			}
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
			throw new NotImplementedException();
		}

		public int BorrarVariosBloques(int num, int posicion) {
			throw new NotImplementedException();
		}

		public int BuscarBloque(int posicion) {
			throw new NotImplementedException();
		}

		public int BuscarBLoque(E elemento) {
			throw new NotImplementedException();
		}

		public IListaArbitraria<E> Clonar() {
			throw new NotImplementedException();
		}

		public bool Contiene(E elemento) {
			throw new NotImplementedException();
		}

		public ILista<E> Diferencia(ILista<E> lista) {
			throw new NotImplementedException();
		}

		public int Eliminar(E elemento) {
			throw new NotImplementedException();
		}

		public E Eliminar(int posicion) {
			throw new NotImplementedException();
		}

		public int EliminarVarios(int num, int posicion) {
			throw new NotImplementedException();
		}

		public B GetBloque(int posicion) {
			throw new NotImplementedException();
		}

		public IEnumerable<B> GetBloques() {
			throw new NotImplementedException();
		}

		public IEnumerator<E> GetEnumerator() {
			throw new NotImplementedException();
		}

		public void Insertar(E elemento, int posicion) {
			throw new NotImplementedException();
		}

		public int Insertar(E elemento) {
			throw new NotImplementedException();
		}

		public int Insertar(B bloque) {
			throw new NotImplementedException();
		}

		public void InsertarFin(E elemento) {
			throw new NotImplementedException();
		}

		public void InsertarInicio(E elemento) {
			throw new NotImplementedException();
		}

		public void InsertarVarios(E elemento, int num, int posicion) {
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public int Posicion(E elemento) {
			throw new NotImplementedException();
		}

		public E PrimerElemento() {
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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
