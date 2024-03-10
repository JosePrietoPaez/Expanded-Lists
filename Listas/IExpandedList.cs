using System;
using System.Collections.Generic;

namespace ExpandedLists {

	/// <summary>
	/// Lists contain elements of the parameter type, they may allow the insertion of null elements.
	/// </summary>
	/// <remarks>
	/// Subinterfaces of <see cref="IExpandedList{T}"/> offer more methods to lists.
	/// </remarks>
	/// <typeparam name="T">
	/// </typeparam>
	public interface IExpandedList<T> : IEnumerable<T> {

		static bool CompatibleEnLista(T? obj) => obj is not null || (obj is null && default(T) is null);

		/// <summary>
		/// Determines whether the list contains elements or not.
		/// </summary>
		bool IsEmpty { get; }

		/// <summary>
		/// Gets the number of elements in the list.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets the first element of the list.
		/// </summary>
		/// <remarks>
		/// The list must not be empty.
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// First element in the list.
		/// </returns>
		T First { get; }

		/// <summary>
		/// Gets the last element of the list.
		/// </summary>
		/// <remarks>
		/// The list must not be empty.
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Last element in the list.
		/// </returns>
		T Last { get; }

		/// <summary>
		/// Gets the element at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// Read-only property.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// Element at <c>position</c>.
		/// </returns>
		T this[int position] { get; }

		/// <summary>
		/// Inserts the element in a new <see cref="IExpandedList{T}"/>.
		/// </summary>
		/// <remarks>
		/// Equivalent to
		/// <see cref="AddNew(T)"/>.
		/// </remarks>
		/// <returns>
		/// New <see cref="IExpandedList{T}"/> with the elements of <c>list</c> and <c>element</c>.
		/// </returns>
		static IExpandedList<T> operator +(IExpandedList<T> list, T element) => list.AddNew(element);

		/// <summary>
		/// Inserts the elements in <c>fisrt</c> and <c>second</c> in a new list.
		/// </summary>
		/// <remarks>
		/// Equivalent to
		/// <see cref="Join(IExpandedList{T})"/>.
		/// </remarks>
		/// <returns>
		/// New list with the elements from both lists.
		/// </returns>
		static IExpandedList<T> operator +(IExpandedList<T> first, IExpandedList<T> second) => first.Join(second);

		/// <summary>
		/// Creates a new list with the elements of <c>list</c> without <c>element</c>.
		/// </summary>
		/// <remarks>
		/// Equivalent to 
		/// <see cref="Subtract(T)"/>.
		/// <para>
		/// All appearances of <c>element</c> are removed.
		/// </para>
		/// </remarks>
		/// <returns>
		/// New list with the elements from <c>list</c> without <c>element</c>.
		/// </returns>
		static IExpandedList<T> operator -(IExpandedList<T> list, T element) => list.Subtract(element);

		/// <summary>
		/// Creates a new list with the elements of <c>minuend</c> not contained in <c>subtrahend</c>.
		/// </summary>
		/// <remarks>
		/// Equivalent to
		/// <see cref="Difference(IExpandedList{T})"/>.
		/// </remarks>
		/// <returns>
		/// New list with the elements of <c>minuend</c> not in <c>subtrahend</c>.
		/// </returns>
		static IExpandedList<T> operator -(IExpandedList<T> minuend, IExpandedList<T> subtrahend) => minuend.Difference(subtrahend);

		/// <summary>
		/// Inserts <c>element</c> into the list.
		/// </summary>
		/// <returns>
		/// The position <c>element</c> was inserted in.
		/// </returns>
		int Add(T element);

		/// <summary>
		/// Removes the first appearance of <c>element</c>.
		/// </summary>
		/// <returns>
		/// The position <c>element</c> was removed from or <c>-1</c> if it was not found.
		/// </returns>
		int Remove(T element);

		/// <summary>
		/// Removes the element at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and be lesser than <see cref="Count"/>.
		/// <para>The list must not be empty.</para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// The removed element.
		/// </returns>
		T RemoveAt(int position);

		/// <summary>
		/// Removes <c>amount</c> elements beginning from <c>position</c> until they are removed or the end of the list is reached.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="Count"/>.
		/// <para>
		/// If <c>amount</c> is not positive, the list will not be modified.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		/// <returns>
		/// Number of removed elements.
		/// </returns>
		int RemoveMultiple(int amount, int position);

		/// <summary>
		/// Removes the first element of the list.
		/// </summary>
		/// <remarks>
		/// The list must not be empty.
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// The first element before being removed.
		/// </returns>
		T RemoveFirst();

		/// <summary>
		/// Removes the last element of the list.
		/// </summary>
		/// <remarks>
		/// The list must not be empty.
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// The last element before being removed.
		/// </returns>
		T RemoveLast();

		/// <summary>
		/// Removes all the elements in the list.
		/// </summary>
		void Clear();

		/// <summary>
		/// Removes all the appearances of <c>element</c> in the list.
		/// </summary>
		/// <returns>
		/// Number of appearances of <c>element</c> in the list.
		/// </returns>
		int Clear(T element);

		/// <summary>
		/// Returns the position of the first appearance of <c>element</c> in the list.
		/// </summary>
		/// <returns>
		/// Position of <c>element</c> or -1 if it is not in the list.
		/// </returns>
		int Position(T element);

		/// <summary>
		/// Returns the positions which contain <c>element</c> in order.
		/// </summary>
		/// <returns>
		/// Array with the positions of <c>element</c>.
		/// </returns>
		int[] Appareances(T element);

		/// <summary>
		/// Determines whether <c>element</c> is in the list or not.
		/// </summary>
		/// <returns>
		/// <c>true</c> if <c>element</c> is contained in list or <c>false</c> if it is not.
		/// </returns>
		bool Contains(T element);

		/// <summary>
		/// Reverses the order of the elements in the list.
		/// </summary>
		void Reverse();

		/// <summary>
		/// Inserts <c>element</c> into a new list.
		/// </summary>
		/// <remarks>
		/// To insert into this list use
		/// <see cref="Add(T)"/>.
		/// </remarks>
		/// <returns>
		/// New list with the elements from this list and <c>element</c>.
		/// </returns>
		IExpandedList<T> AddNew(T element);

		/// <summary>
		/// Creates a new list with elements from this list and <c>second</c>.
		/// </summary>
		/// <remarks>
		/// The position of the elements depends on the implementation.
		/// </remarks>
		/// <returns>
		/// New list with elements from both lists.
		/// </returns>
		IExpandedList<T> Join(IExpandedList<T> second);

		/// <summary>
		/// Creates a copy of the list, with the same type.
		/// </summary>
		/// <remarks>
		/// Interfaces inheriting from <see cref="IExpandedList{T}"/> should add new methods for their type.
		/// </remarks>
		/// <returns>
		/// Copy of this list.
		/// </returns>
		IExpandedList<T> Clone();

		/// <summary>
		/// Creates a new list with the elements from this list, except <c>element</c>.
		/// </summary>
		/// <remarks>To remove <c>element</c> in this list use
		/// <see cref="Clear(T)"/>.
		/// </remarks>
		/// <returns>
		/// New list with the same elements, except<c>element</c>.
		/// </returns>
		IExpandedList<T> Subtract(T element);

		/// <summary>
		/// Creates a new list with the elements from this list that are not contained in <c>list</c>.
		/// </summary>
		/// <returns>
		/// New list with the same elements, except the ones in <c>list</c>.
		/// </returns>
		IExpandedList<T> Difference(IExpandedList<T> list);
	}
}
