using System;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// This interface allows more direct ways of altering a block list's block structure
	/// </summary>
	/// <remarks>
	/// Check implementation details when using these methods
	/// </remarks>
	public interface IDynamicBlockList<E,B> : IBlockList<E,B> where B : Block<E> {

		/// <summary>
		/// Returns a new list with <c>block</c> inserted
		/// </summary>
		/// <remarks>
		/// Equivalent to <see cref="IDynamicBlockList{E, B}.Add(B)"/>
		/// </remarks>
		/// <returns>
		/// Nueva list con sus bloques y <c>block</c>
		/// </returns>
		static IDynamicBlockList<E, B> operator +(IDynamicBlockList<E, B> list, B block) => list.Add(block);

		/// <summary>
		/// Sets the block at <c>position</c> to <c>block</c>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// The block at <c>position</c>
		/// </returns>
		B SetBlock(B block, int position);

		/// <summary>
		/// Inserts <c>block</c> at <c>position</c>
		/// </summary>
		void Insert(B block, int position);

		/// <summary>
		/// Swaps the blocks at these positions
		/// </summary>
		/// <remarks>
		/// Both positions must be retrievable using <see cref="IBlockList{E, B}.GetBlock(int)"/>
		/// </remarks>
		void SwapBlock(int first, int second);

		/// <summary>
		/// Inserts <c>block</c> into a new block list
		/// </summary>
		IDynamicBlockList<E, B> Add(B block);

		/// <summary>
		/// Creates a dynamic block list equal to <c>this</c>, with the same type
		/// </summary>
		/// <returns>
		/// Block list identical to <c>this</c>
		/// </returns>
		IDynamicBlockList<E, B> CloneDynamicBlocks();

	}
}
