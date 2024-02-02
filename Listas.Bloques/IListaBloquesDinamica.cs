using System;
using System.Collections.Generic;

namespace Listas.Bloques {

	/// <summary>
	/// Las métodos de esta interfaz permiten realizar cambios en la estructura de la lista de forma más directa que con <see cref="IListaBloques{E, B}"/>
	/// </summary>
	/// <remarks>
	/// Es posible que, debido a la naturaleza de estos métodos, cada implementación imponga restricciones adicionales al uso de estos métodos
	/// </remarks>
	/// <typeparam name="E"></typeparam>
	/// <typeparam name="B"></typeparam>
	public interface IListaBloquesDinamica<E,B> : IListaBloques<E,B> where B : Bloque<E> {

		Func<int,int> FuncionDeExtension { get; set; }

		/// <summary>
		/// Devuelve la lista con <c>bloque</c> insertado
		/// </summary>
		/// <remarks>
		/// Equivalente a lista.<see cref="IListaBloquesDinamica{E, B}.Sumar(B)"/>
		/// </remarks>
		/// <param name="lista"></param>
		/// <param name="bloque"></param>
		/// <returns>
		/// Nueva lista con sus bloques y <c>bloque</c>
		/// </returns>
		static IListaBloquesDinamica<E, B> operator +(IListaBloquesDinamica<E, B> lista, B bloque) => lista.Sumar(bloque);

		/// <summary>
		/// Cambia el bloque en <c>posicion</c> a <c>bloque</c>
		/// </summary>
		/// <param name="bloque"></param>
		/// <param name="posicion"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// El bloque que estaba en <c>posicion</c>
		/// </returns>
		B SetBloque(B bloque, int posicion);

		/// <summary>
		/// Coloca el bloque en la lista
		/// </summary>
		/// <param name="bloque"></param>
		/// <returns>La posición en la que se ha insertado</returns>
		int Insertar(B bloque);

		/// <summary>
		/// Intercambia los bloques con estas posiciones
		/// </summary>
		/// <remarks>
		/// <param name="primero"></param>
		/// <param name="segundo"></param>
		void IntercambiarBloques(int primero, int segundo);

		/// <summary>
		/// Devuelve una lista de bloques como la llamada, con <c>bloque</c>
		/// </summary>
		IListaBloquesDinamica<E, B> Sumar(B bloque);

	}
}
