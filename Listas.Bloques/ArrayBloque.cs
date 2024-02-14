using System;
using System.Text;

namespace Listas.Bloques {
	public class ArrayBloque<T>(int capacidad) : Bloque<T>(capacidad) {

		private readonly T[] _array = new T[capacidad];
		private int _longitud = 0;

		public override T this[int index] {
			get {
				Contrato.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _longitud,
					Mensajes.RangoLista(index,_longitud));
				return _array[index];
			}
			set {
				Contrato.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _longitud,
					Mensajes.RangoLista(index, _longitud));
				_array[index] = value;
			}
		}

		public override bool Lleno => _longitud == capacidad;

		public override bool Vacio => _longitud == 0;

		public override int Longitud => _longitud;

		public ArrayBloque(T[] array) : this(array,array.Length) { }

		public ArrayBloque(T[] array, int longitud) : this(array.Length) {
			array.CopyTo(_array, 0);
			_longitud = longitud;
		}

		public override void BorrarTodos() {
			_longitud = 0;
		}

		public override bool Contiene(object? elemento) {
			bool res = false;
			for (int i = 0; i < _array.Length && !res; i++) {
				if (elemento == null) {
					res = _array[i] == null;
				} else {
					res = elemento.Equals(_array[i]);
				}
			}
			return res;
		}

		public override T Eliminar(int posicion) {
			Contrato.Requires<IndexOutOfRangeException>(posicion >= 0 && posicion < _longitud,
				Mensajes.RangoLista(posicion, _longitud));
			T aux = _array[posicion];
			Array.Copy(_array, posicion + 1, _array, posicion, _array.Length - posicion - 1);
			_longitud--;
			return aux;
		}

		public override T EliminarPrimero() {
			return Eliminar(0);
		}

		public override T EliminarUltimo() {
			Contrato.Requires<InvalidOperationException>(_longitud > 0, Mensajes.VacioBloque);
			_longitud--;
			return _array[_longitud];
		}

		public override IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < _longitud; i++) {
				yield return _array[i];
			}
		}

		public override T? Insertar(T elemento, int posicion) {
			Contrato.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= _longitud && (posicion != _longitud || _longitud != _array.Length)
				,Mensajes.RangoLista(posicion,_longitud));
			if (posicion == _longitud) {
				return InsertarUltimo(elemento);
			}
			T ultimo = _array[^1];
			Array.Copy(_array, posicion, _array, posicion + 1, _array.Length - posicion - 1);
			_array[posicion] = elemento;
			_longitud += _longitud == _array.Length ? 0 : 1; //Si el bloque está lleno no se aumenta la capacidad
			return ultimo;
		}

		public override T? InsertarPrimero(T elemento) {
			return Insertar(elemento, 0);
		}

		public override T? InsertarUltimo(T elemento) {
			T aux = _array[^1];
			if (_longitud == _array.Length) { //Si está lleno
				_array[^1] = elemento;
			} else {
				_array[_longitud] = elemento;
				_longitud++;
			}
			return aux;
		}

		public override void Invertir() {
			T[] arrayCopia = new T[_longitud];
			Array.Copy(_array, arrayCopia, _longitud);
			for (int i = 0; i < _longitud; i++) {
				_array[i] = arrayCopia[_longitud - i - 1];
			}
		}

		public override T PrimerElemento => _array[0];

		public override T UltimoElemento => _array[_longitud - 1];

		public override string ToString() {
			StringBuilder stringBuilder = new();
			stringBuilder.Append('[');
			for (int i = 0; i < _longitud; i++) {
				stringBuilder.Append(_array[i]);
				if (i != _longitud - 1) {
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public override bool Equals(object? obj) {
			bool iguales = ReferenceEquals(this, obj); // Si son el mismo objeto, trivialmente son iguales
			if (!iguales && obj is Bloque<T> otro) { // No pueden ser iguales si otro no es un bloque
				iguales = Longitud == otro.Longitud && Capacidad == otro.Capacidad; // Para descartar bloques con distinta longitud o capacidad
				int contador = 0;
				while (iguales && contador < Longitud) {
					iguales = Equals(_array[contador],otro[contador]);
					contador++;
				}
			}
			return iguales;
		}

		/// <summary>
		/// Genera un hash basado en la longitud, capacidad y elementos entre 0 y <see cref="Bloque{T}.Capacidad"/>
		/// </summary>
		/// <remarks>
		/// Si se necesita que el bloque esté en una estructura que use su hash, no debería modificarse
		/// </remarks>
		/// <returns></returns>
		public override int GetHashCode() {
			int codigo = Longitud ^ Capacidad; // Para que se tengan en cuenta como en Equals
			for (int i = 0; i < _longitud; i++) {
				codigo ^= _array[i]?.GetHashCode() ?? 0;
			}
			return codigo;
		}

		public static implicit operator ArrayBloque<T>(T[] array) {
			return new ArrayBloque<T>(array);
		}

		public static explicit operator T[](ArrayBloque<T> bloque) {
			T[] nuevo = new T[bloque.Capacidad];
			for (int i = 0; i < bloque.Longitud; i++) {
				nuevo[i] = bloque[i];
			}
			return nuevo;
		}

	}
}
