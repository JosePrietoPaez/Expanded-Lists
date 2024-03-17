using System;
using System.Collections.Generic;
using System.Text;

namespace ExpandedLists.Blocks {
#pragma warning disable CS9107 // El parámetro se captura en el estado del tipo envolvente y su valor también se pasa al constructor base. La clase base también puede capturar el valor.
	public class ArrayBlock<T>(int capacidad) : Block<T>(capacidad), IEquatable<ArrayBlock<T>> {
#pragma warning restore CS9107 // El parámetro se captura en el estado del tipo envolvente y su valor también se pasa al constructor base. La clase base también puede capturar el valor.

		private readonly T[] _array = new T[capacidad];
		private int _length = 0;

		public override T this[int index] {
			get {
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _length,
					Messages.ListRange(index,_length));
				return _array[index];
			}
			set {
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _length,
					Messages.ListRange(index, _length));
				_array[index] = value;
			}
		}

		public override bool IsFull => _length == capacidad;

		public override bool IsEmpty => _length == 0;

		public override int Count {
			get => _length;
			set {
				Contract.Requires<ArgumentOutOfRangeException>(value <= _length,
					Messages.NegativeLength,nameof(value));
				_length = value;
			}
		}

		public ArrayBlock(T[] array) : this(array,array.Length) { }

		public ArrayBlock(T[] array, int longitud) : this(array.Length) {
			array.CopyTo(_array, 0);
			_length = longitud;
		}

		public override void Clear() {
			_length = 0;
		}

		public override bool Contains(object? elemento) {
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

		public override T RemoveAt(int posicion) {
			Contract.Requires<IndexOutOfRangeException>(posicion >= 0 && posicion < _length,
				Messages.ListRange(posicion, _length));
			T aux = _array[posicion];
			Array.Copy(_array, posicion + 1, _array, posicion, _array.Length - posicion - 1);
			_length--;
			return aux;
		}

		public override T RemoveFirst() {
			return RemoveAt(0);
		}

		public override T RemoveLast() {
			Contract.Requires<InvalidOperationException>(_length > 0, Messages.EmptyBlock);
			_length--;
			return _array[_length];
		}

		public override IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < _length; i++) {
				yield return _array[i];
			}
		}

		public override T? Insert(T elemento, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= _length && (posicion != _length || _length != _array.Length)
				,Messages.ListRange(posicion,_length));
			if (posicion == _length) {
				return InsertLast(elemento);
			}
			T ultimo = _array[^1];
			Array.Copy(_array, posicion, _array, posicion + 1, _array.Length - posicion - 1);
			_array[posicion] = elemento;
			_length += _length == _array.Length ? 0 : 1; //Si el bloque está lleno no se aumenta la capacidad
			return ultimo;
		}

		public override T? InsertFirst(T elemento) {
			return Insert(elemento, 0);
		}

		public override T? InsertLast(T elemento) {
			T? aux = _array[^1];
			if (_length == _array.Length) { //Si está lleno
				_array[^1] = elemento;
			} else {
				_array[_length] = elemento;
				_length++;
			}
			return aux;
		}

		public override void Reverse() {
			T[] arrayCopia = new T[_length];
			Array.Copy(_array, arrayCopia, _length);
			for (int i = 0; i < _length; i++) {
				_array[i] = arrayCopia[_length - i - 1];
			}
		}

		public override T First => _array[0];

		public override T Last => _array[_length - 1];

		public override string ToString() {
			StringBuilder stringBuilder = new();
			stringBuilder.Append('[');
			for (int i = 0; i < _length; i++) {
				stringBuilder.Append(_array[i]);
				if (i != _length - 1) {
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		public override bool Equals(object? obj) {
			bool iguales = ReferenceEquals(this, obj); // Si son el mismo objeto, trivialmente son iguales
			if (!iguales && obj is Block<T> otro) { // No pueden ser iguales si otro no es un bloque
				iguales = Count == otro.Count && Capacity == otro.Capacity; // Para descartar bloques con distinta longitud o capacidad
				int contador = 0;
				while (iguales && contador < Count) {
					iguales = Equals(_array[contador],otro[contador]);
					contador++;
				}
			}
			return iguales;
		}

		/// <summary>
		/// Generates a hash based on <see cref="Count"/>, <see cref="Block{T}.Capacity"/> and its elements between 0 and <see cref="Block{T}.Capacity"/>
		/// </summary>
		/// <remarks>
		/// A block should not be modified while it or a list containing it is inside a hash list
		/// </remarks>
		/// <returns></returns>
		public override int GetHashCode() {
			int codigo = Count ^ Capacity; // Para que se tengan en cuenta como en Equals
			for (int i = 0; i < _length; i++) {
				codigo ^= _array[i]?.GetHashCode() ?? 0;
			}
			return codigo;
		}

		public bool Equals(ArrayBlock<T>? other) {
			bool iguales = ReferenceEquals(other, this);
			if (!iguales && other is not null) {
				iguales = Count == other.Count && Capacity == other.Capacity;
				int contador = 0;
				while (iguales && contador < Count) {
					iguales = Equals(_array[contador], other[contador]);
					contador++;
				}
			}
			return iguales;
		}

		public static implicit operator ArrayBlock<T>(T[] array) {
			return new ArrayBlock<T>(array);
		}

		public static explicit operator T[](ArrayBlock<T> bloque) {
			T[] nuevo = new T[bloque.Capacity];
			for (int i = 0; i < bloque.Count; i++) {
				nuevo[i] = bloque[i];
			}
			return nuevo;
		}

	}
}
