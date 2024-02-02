using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;

namespace Listas.Bloques {
	public class ArrayBloque<T>(int capacidad) : Bloque<T> {

		private readonly T[] _array = new T[capacidad];
		private int _longitud = 0;

		public override T this[int index] {
			get {
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _longitud,
					Mensajes.RangoLista(index,_longitud));
				return _array[index];
			}
			set {
				Contract.Requires<ArgumentOutOfRangeException>(index >= 0 && index < _longitud,
					Mensajes.RangoLista(index, _longitud));
				_array[index] = value;
			}
		}

		public new bool Lleno => _longitud == _array.Length;

		public new bool Vacio => _longitud == 0;

		public new int Capacidad => _array.Length;

		public new int Longitud => _longitud;

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
			Contract.Requires<IndexOutOfRangeException>(posicion >= 0 && posicion < _longitud,
				Mensajes.RangoLista(posicion, _longitud));
			T aux = _array[posicion];
			Array.Copy(_array, posicion + 1, _array, posicion, _array.Length - posicion - 1);
			_longitud--;
			return aux;
		}

		public override T EliminarInicio() {
			return Eliminar(0);
		}

		public override T EliminarUltimo() {
			Contract.Requires<InvalidOperationException>(_longitud > 0, Mensajes.VacioBloque);
			_longitud--;
			return _array[_longitud];
		}

		public override IEnumerator<T> GetEnumerator() {
			for (int i = 0; i < _longitud; i++) {
				yield return _array[i];
			}
		}

		public override T? Insertar(T elemento, int posicion) {
			Contract.Requires<ArgumentOutOfRangeException>(posicion >= 0 && posicion <= _longitud && (posicion != _longitud || _longitud != _array.Length)
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

		public override T? InsertarInicio(T elemento) {
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

		public override T PrimerElemento() {
			return _array[0];
		}

		public override T UltimoElemento() {
			return _array[_longitud - 1];
		}

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
	}
}
