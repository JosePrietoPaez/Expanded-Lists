using Listas;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Listas
{
	public interface IListaDinamica<T> : IListaArbitraria<T>
	{
		/// <summary>
		/// Esta propiedad representa a la función que genera nuevas instancias de <c>T</c> que se usarán para rellenar la lista
		/// en el caso de que se intente poner <c>Vacia</c> a <c>true</c> o <c>Longitud</c> a un valor mayor al actual
		/// </summary>
		/// <remarks>
		/// El argumento usado para la función será el tamaño de la lista en el momento de invocarla
		/// <para>
		/// Por defecto genera la instancia por defecto de la clase, por ello se recomienda instanciarla en el constructor o mediante esta propiedad
		/// </para>
		/// </remarks>
		Func<int,T?> FuncionDeGeneracion { get; set; }

		/// <summary>
		/// Esta propiedad permite obtener la longitud de la lista al leerla y cambiarla al escribirla
		/// </summary>
		/// <remarks>
		/// En el caso de aumentar la longitud mediante esta propiedad, se rellenará la lista con referencias a <c>InstanciaDeRespaldo</c>
		/// <para>
		/// En el caso de reducirla se eliminarán los últimos elementos, la cantidad dependiendo del sustraendo
		/// </para>
		/// Se lanzará una excepción en el caso de reducir la longitud a un número negativo
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		new int Longitud { get; set; }

		/// <summary>
		/// Esta propiedad permite ver si la lista esta vacía al leerla y ponerla vacía si se escribe
		/// </summary>
		/// <remarks>
		/// En el caso de intentar operación <c>lista.Vacia = false</c>, se insertará a <c>InstanciaDeRespaldo</c> si la lista estaba vacía
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		new bool Vacia { get; set; }

		/// <summary>
		/// Introduce <see cref="IListaDinamica{T}.FuncionDeGeneracion"/> en la lista
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IListaDinamica{T}.Longitud"/><c>++</c>
		/// <para>
		/// Necesita que <see cref="IListaDinamica{T}.FuncionDeGeneracion"/> no genere elementos nulos si la lista no los admite</para>
		/// </remarks>
		/// <returns></returns>
		static IListaDinamica<T> operator ++(IListaDinamica<T> serie) {
			Contrato.Requires<InvalidOperationException>
				(CompatibleEnLista(serie.FuncionDeGeneracion.Invoke(serie.Longitud)), "La función de generación ha creado un elemento nulo");
			serie.Longitud++;
			return serie;
		}

		/// <summary>
		/// Elimina el último elemento de la lista 
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IListaDinamica{T}.Longitud"/><c>--</c>
		/// </remarks>
		/// <returns>Lista igual que <c>lista</c> sin el último elemento</returns>
		static IListaDinamica<T> operator --(IListaDinamica<T> serie) {
			serie.Longitud--;
			return serie;
		}

	}
}
