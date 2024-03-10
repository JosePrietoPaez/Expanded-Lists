using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ExpandedLists.Blocks {

	/// <summary>
	/// Blocks are data structures like arrays,
	/// block lists delegate many operations to their blocks to ensure efficiency
	/// </summary>
	/// <remarks>
	/// Blocks have a fixed capacity, and their insertion methods return their last element
	/// <para>
	/// All block types must have a constructor taking an <c>int</c>
	/// </para>
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public abstract class Block<T>(in int capacidad) : IEnumerable<T> {

		/// <summary>
		/// Creates an instance of the specified type of block with the given length
		/// </summary>
		/// <returns></returns>
		public static B CreateInstance<B>(int capacidad) where B : Block<T>{
			ConstructorInfo? constructor = typeof(B).GetConstructor([typeof(int)]);
			return constructor?.Invoke([capacidad]) as B?? throw new NotImplementedException("No se ha implementado el constructor con argumento int");
		}

		/// <summary>
		/// Creates an instance of the specified type of block
		/// and inserts the elements of <c>bloque</c>
		/// </summary>
		public static B CopyInstance<B>(Block<T> bloque) where B : Block<T> {
			B clon = CreateInstance<B>(bloque.Capacity);
			foreach (var element in bloque) {
				clon.InsertLast(element);
			}
			return clon;
		}

		/// <summary>
		/// Gets or sets the element at <c>index</c>
		/// </summary>
		/// <returns>
		/// Element at <c>index</c>
		/// </returns>
		abstract public T this[int index] { get; set; }

		/// <summary>
		/// Determines whether the block is full
		/// </summary>
		abstract public bool IsFull { get; }

		/// <summary>
		/// Determines whether the block is empty
		/// </summary>
		abstract public bool IsEmpty { get; }

		/// <summary>
		/// Gets the capacity of this block
		/// </summary>
		/// <remarks>
		/// Must be positive
		/// </remarks>
		public readonly int Capacity = capacidad > 0 ? capacidad : throw new ArgumentOutOfRangeException("Blocks must have a positive capacity");

		/// <summary>
		/// Gets or sets the amount of elements in the block
		/// </summary>
		/// <remarks>
		/// Not greater than <see cref="Block{T}.Capacity"/>
		/// <para>
		/// Can only set to a lower value
		/// </para>
		/// </remarks>
		abstract public int Count { get; set; }

		/// <summary>
		/// Inserts <c>element</c> at the start of the block
		/// </summary>
		/// <remarks>
		/// If the block is full the last element is removed and returned,
		/// it is recommended to check <see cref="IsFull"/> before using the returned element
		/// </remarks>
		/// <returns>
		/// Last element if block is full, otherwise unspecified, might be null
		/// </returns>
		abstract public T? InsertFirst(T element);

		/// <summary>
		/// Inserts <c>element</c> at <c>position</c>
		/// </summary>
		/// <remarks>
		/// If the block is full the last element is removed and returned,
		/// it is recommended to check <see cref="IsFull"/> before using the returned element
		/// <para>
		/// <c>position</c> must not be negative and must be lesser than <see cref="Count"/>
		/// </para>
		/// </remarks>
		/// <returns>
		/// Last element if block is full, otherwise unspecified, might be null
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		abstract public T? Insert(T element, int position);

		/// <summary>
		/// Inserts <c>element</c> at the end of the block
		/// </summary>
		/// <remarks>
		/// If the block is full, the last element will be swapped for <c>element</c>
		/// <para>
		/// If the block is full the last element is removed and returned,
		/// it is recommended to check <see cref="IsFull"/> before using the returned element
		/// </para>
		/// </remarks>
		/// <returns>
		/// Last element if the block is full, otherwise unspecified, might be null
		/// </returns>
		abstract public T? InsertLast(T element);

		/// <summary>
		/// Gets the first element of the block, if it has any
		/// </summary>
		/// <remarks>
		/// The block must have elements
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// First element of the block
		/// </returns>
		abstract public T First { get; }

		/// <summary>
		/// Gets the last element of the block, if it has any
		/// </summary>
		/// <remarks>
		/// The block must have elements
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Last element of the block
		/// </returns>
		abstract public T Last { get; }

		/// <summary>
		/// Removes the first element of the block
		/// </summary>
		/// <remarks>
		/// The block must have elements
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// First element of the block
		/// </returns>
		abstract public T RemoveFirst();

		/// <summary>
		/// Removes the element at <c>position</c>
		/// </summary>
		/// <remarks>
		/// The block must have elements and <c>position</c> must not be negative and must be lesser than <see cref="Count"/>
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// Element removed from the block
		/// </returns>
		abstract public T RemoveAt(int position);

		/// <summary>
		/// Removes the last element of the block
		/// </summary>
		/// <remarks>
		/// The block must have elements
		/// </remarks>
		/// <exception cref="IndexOutOfRangeException"></exception>
		/// <returns>
		/// Last element of the block
		/// </returns>
		abstract public T RemoveLast();

		/// <summary>
		/// Removes all the elements from the block
		/// </summary>
		abstract public void Clear();

		/// <summary>
		/// Determines whether the block contains <c>element</c>
		/// </summary>
		/// <returns>
		/// <c>true</c> if <c>element</c> is contained in the block, otherwise <c>false</c>
		/// </returns>
		abstract public bool Contains(object? element);

		/// <summary>
		/// Reverses the order of the elements in the block
		/// </summary>
		abstract public void Reverse();

		abstract public IEnumerator<T> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
