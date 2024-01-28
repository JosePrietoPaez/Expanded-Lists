using System;
using System.Collections.Generic;

namespace Listas.Bloques {
	public interface IBloque<T> : IEnumerable<T> {

		/// <summary>
		/// Devuelve una nueva instancia del mismo tipo que la llamante
		/// </summary>
		/// <remarks>
		/// La capacidad debe ser positiva
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		IBloque<T> CrearInstancia(int capacidad);

		T this[int index] { get; set; }

		bool Lleno { get; }

		bool Vacio { get; set; }

		int Capacidad { get; }

		int Longitud { get; }

		/// <summary>
		/// Añade el elemento al principio del bloque, desplazando el resto
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno se quitará el último elemento y se devolverá
		/// </remarks>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		T? InsertarInicio(T elemento);

		/// <summary>
		/// Añade el elemento al bloque en la posición indicada, desplazando el resto
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno se quitará el último elemento y se devolverá
		/// </remarks>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		T? Insertar(T elemento, int posicion);

		/// <summary>
		/// Añade el elemento al final del bloque en la última posición
		/// </summary>
		/// <remarks>
		/// Si el bloque está lleno no se meterá
		/// </remarks>
		/// <returns>
		/// Elemento en la última posición del bloque o <c>null</c> si no existe
		/// </returns>
		T? InsertarUltimo(T elemento);

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
		T PrimerElemento();

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
		T UltimoElemento();

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
		T EliminarInicio();

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
		T Eliminar(int posicion);

		/// <summary>
		/// Elimina el elemento al final del bloque y lo devuelve
		/// </summary>
		/// <remarks>
		/// La lista debe tener elementos para usar este método
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// Elemento quitado de la lista
		/// </returns>
		T EliminarUltimo();

		/// <summary>
		/// Devuelve <c>true</c> si contiene <c>elemento</c>
		/// </summary>
		/// <returns>
		/// Booleano que representa si se tiene el elemento
		/// </returns>
		bool Contiene(object elemento);
	}
}
