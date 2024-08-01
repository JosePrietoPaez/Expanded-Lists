using System.Collections.Generic;
using System;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// Block lists use <see cref="Block{T}"/> to store their elements, these blocks may be retrieved from its list.
	/// </summary>
	/// <remarks>
	/// <see cref="E"/> is the elements' type, <see cref="B"/> is the blocks' type.
	/// <para>It is recommended not to insert or remove elements from blocks being used by a block list.</para>
	/// </remarks>
	public interface IBlockList<E,B> : IExpandedList<E> where B : Block<E> {

		/// <summary>
		/// Gets the amount of block used by the list.
		/// </summary>
		/// <remarks>
		/// Only blocks with an index smaller than this may be accessed.
		/// </remarks>
		int BlockCount { get; }

		/// <summary>
		/// Returns a new list equal to <c>list</c> without <c>block</c>.
		/// </summary>
		/// <remarks>
		/// Equivalent to <see cref="Subtract(B)"/>.
		/// </remarks>
		/// <returns>
		/// New list like <c>list</c> without <c>block</c>.
		/// </returns>
		static IBlockList<E, B> operator -(IBlockList<E, B> list, B block) => list.Subtract(block);

		/// <summary>
		/// Gets the block at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="IBlockList{T, U}.BlockCount"/>.
		/// </remarks>
		/// <returns>
		/// Block at <c>position</c>.
		/// </returns>
		B GetBlock(int position);

		/// <summary>
		/// Removes <c>block</c> from the list if its contained.
		/// </summary>
		/// <returns>
		/// The position that contained <c>block</c> or <c>-1</c> if it was not in the list.
		/// </returns>
		int Remove(B block);

		/// <summary>
		/// Removes the block at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="IBlockList{T, U}.BlockCount"/>.
		/// </remarks>
		/// <returns>
		/// The block at <c>position</c>.
		/// </returns>
		B RemoveBlockAt(int position);

		/// <summary>
		/// Removes <c>amount</c> blocks starting at <c>position</c>, or until the end of the list is reached.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="IBlockList{T, U}.BlockCount"/>.
		/// <para><c>amount</c> should be positive, but if not the list will not be altered.</para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// Number of blocks removed.
		/// </returns>
		int RemoveMultipleBlocks(int amount, int position);

		/// <summary>
		/// Gets the block containing the element at <c>position</c>.
		/// </summary>
		/// <returns>
		/// Block containing <c>position</c> or <c>-1</c> if <c>position</c> is not in the list.
		/// </returns>
		int GetBlockContainingPosition(int position);

		/// <summary>
		/// Gets the first block containing an object equal to<c>element</c>.
		/// </summary>
		/// <returns>
		/// Block containing <c>element</c> or <c>-1</c> if <c>element</c> is not in the list.
		/// </returns>
		int GetBlockContainingElement(E element);

		/// <summary>
		/// Gets the position of <c>block</c>.
		/// </summary>
		/// <returns>
		/// The position of <c>block</c> in the list or <c>-1</c> if <c>block</c> is not in the list.
		/// </returns>
		int Position(B block);

		/// <summary>
		/// Returns a list like <c>this</c>, without <c>block</c>.
		/// </summary>
		IBlockList<E,B> Subtract(B block);

		/// <summary>
		/// Creates a block list equal to <c>this</c>, with the same type.
		/// </summary>
		/// <returns>
		/// List equal to <c>this</c>.
		/// </returns>
		IBlockList<E, B> CloneBlock();

		IEnumerable<B> GetBlockEnumerable();
		
	}
}
