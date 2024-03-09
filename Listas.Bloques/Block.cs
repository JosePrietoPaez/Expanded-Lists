using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// Los bloques son estructuras de datos que almacenan objetos,
	/// las listas de bloques delegan les delegan las operaciones de búsqueda al no saber como se implementan
	/// </summary>
	/// <remarks>
	/// Todos los tipos de bloques deben tener un constructor de un argumento int para especificar su capacidad
	/// <para>
	/// Se espera que la capacidad de un bloque no cambie desde su creación
	/// </para>
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public abstract class Block<T>(in int capacidad) : IEnumerable<T> {

		/// <summary>
		/// Crea una instancia del mismo tipo que el bloque
		/// </summary>
		/// <returns></returns>
		public static B CreateInstance<B>(int capacidad) where B : Block<T>{
			ConstructorInfo? constructor = typeof(B).GetConstructor([typeof(int)]);
			return constructor?.Invoke([capacidad]) as B?? throw new NotImplementedException("No se ha implementado el constructor con argumento int");
		}

		public static B CopyInstance<B>(Block<T> bloque) where B : Block<T> {
			B clon = CreateInstance<B>(bloque.Capacity);
			foreach (var elemento in bloque) {
				clon.InsertLast(elemento);
			}
			return clon;
		}

		/// <summary>
		/// Permite obtener o cambiar el elemento en la posición indicada, no permite la inserción de elementos
		/// </summary>
		/// <param name="index"></param>
		/// <returns>
		/// Elemento del bloque en la posición indicada
		/// </returns>
		abstract public T this[int index] { get; set; }

		/// <summary>
		/// Esta propiedad permite consultar el bloque para ver si queda espacio
		/// </summary>
		abstract public bool IsFull { get; }

		/// <summary>
		/// Esta propiedad permite consultar el bloque para ver si hay elementos
		/// </summary>
		abstract public bool IsEmpty { get; }

		/// <summary>
		/// Este campo permite obtener la cantidad de elementos que podrán ser guardados el en bloque
		/// </summary>
		/// <remarks>
		/// Debe ser positiva
		/// </remarks>
		public readonly int Capacity = capacidad > 0 ? capacidad : throw new ArgumentOutOfRangeException("Los bloques deben tener capacidad positiva");

		/// <summary>
		/// Esta propiedad permite obtener la cantidad de elementos guardados en el bloque
		/// </summary>
		/// <remarks>
		/// No puede ser mayor que <see cref="Block{T}.Capacity"/>
		/// <para>
		/// El setter solo puede reducir la longitud
		/// </para>
		/// </remarks>
		abstract public int Length { get; set; }

		/// <summary>
		/// Añade el elemento al principio del bloque, desplazando el resto
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno se quitará el último elemento y se devolverá
		/// </remarks>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		abstract public T? InsertFirst(T elemento);

		/// <summary>
		/// Añade el elemento al bloque en la posición indicada, desplazando el resto
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno se quitará el último elemento y se devolverá
		/// <para>
		/// <c>posicion</c> debe ser no negativo y no mayor que la longitud, o menor que esta si el bloque está lleno
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		abstract public T? Insert(T elemento, int posicion);

		/// <summary>
		/// Añade el elemento al final del bloque en la última posición
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno se cambiará el último elemento por <c>elemento</c>
		/// </remarks>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		abstract public T? InsertLast(T elemento);

		/// <summary>
		/// Devuelve el primer elemento del bloque, si hay
		/// </summary>
		/// <remarks>
		/// Debe haber elementos para usar este método
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Primer elemento del bloque
		/// </returns>
		abstract public T First { get; }

		/// <summary>
		/// Devuelve el último elemento del bloque, si hay
		/// </summary>
		/// <remarks>
		/// Debe haber elementos para usar este método
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Último elemento del bloque
		/// </returns>
		abstract public T Last { get; }

		/// <summary>
		/// Elimina el elemento al inicio del bloque y lo devuelve
		/// </summary>
		/// <remarks>
		/// Debe haber elementos para usar este método
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// Elemento quitado del bloque
		/// </returns>
		abstract public T RemoveFirst();

		/// <summary>
		/// Elimina el elemento de la posición indicada del bloque y lo devuelve
		/// </summary>
		/// <remarks>
		/// El bloque debe tener elementos y <c>posicion</c> debe ser no negativa y menor que la longitud del bloque
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// Elemento quitado del bloque
		/// </returns>
		abstract public T RemoveAt(int posicion);

		/// <summary>
		/// Elimina el elemento al final del bloque y lo devuelve
		/// </summary>
		/// <remarks>
		/// La lista debe tener elementos para usar este método
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Elemento quitado de la lista
		/// </returns>
		abstract public T RemoveLast();

		/// <summary>
		/// Elimina todos los elementos del bloque, dejándolo vacío
		/// </summary>
		abstract public void Clear();

		/// <summary>
		/// Devuelve <c>true</c> si contiene <c>elemento</c>
		/// </summary>
		/// <returns>
		/// Booleano que representa si se tiene el elemento
		/// </returns>
		abstract public bool Contains(object? elemento);

		/// <summary>
		/// Invierte el orden de los elementos del bloque
		/// </summary>
		abstract public void Reverse();

		abstract public IEnumerator<T> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
