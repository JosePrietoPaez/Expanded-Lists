using System;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// Las métodos de esta interfaz permiten realizar cambios en la estructura de la list de forma más directa que con <see cref="IBlockList{E, B}"/>
	/// </summary>
	/// <remarks>
	/// Es posible que, debido a la naturaleza de estos métodos, cada implementación imponga restricciones adicionales al uso de estos métodos
	/// </remarks>
	/// <typeparam name="E"></typeparam>
	/// <typeparam name="B"></typeparam>
	public interface IDynamicBlockList<E,B> : IBlockList<E,B> where B : Block<E> {

		/// <summary>
		/// Devuelve la list con <c>block</c> insertado
		/// </summary>
		/// <remarks>
		/// Equivalente a list.<see cref="IDynamicBlockList{E, B}.Add(B)"/>
		/// </remarks>
		/// <param name="list"></param>
		/// <param name="block"></param>
		/// <returns>
		/// Nueva list con sus bloques y <c>block</c>
		/// </returns>
		static IDynamicBlockList<E, B> operator +(IDynamicBlockList<E, B> list, B block) => list.Add(block);

		/// <summary>
		/// Cambia el block en <c>position</c> a <c>block</c>
		/// </summary>
		/// <param name="block"></param>
		/// <param name="position"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// El block que estaba en <c>position</c>
		/// </returns>
		B SetBlock(B block, int position);

		/// <summary>
		/// Coloca el block en la list en la posición indicada
		/// </summary>
		/// <param name="block"></param>
		/// <returns>La posición en la que se ha insertado</returns>
		void Insert(B block, int position);

		/// <summary>
		/// Intercambia los bloques con estas posiciones
		/// </summary>
		/// <remarks>
		/// <param name="first"></param>
		/// <param name="second"></param>
		void SwapBlock(int first, int second);

		/// <summary>
		/// Devuelve una list de bloques como la llamada, con <c>block</c>
		/// </summary>
		IDynamicBlockList<E, B> Add(B block);

		/// <summary>
		/// Crea una list dinámica nueva igual a la llamada, la list será del mismo tipo
		/// </summary>
		/// <remarks>
		/// Las interfaces que extiendan de <see cref="ILista{T}"/> deberían sobrescribir este método
		/// </remarks>
		/// <returns>
		/// Lista igual a la llamada
		/// </returns>
		IDynamicBlockList<E, B> CloneDynamicBlocks();

	}
}
