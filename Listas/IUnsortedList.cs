using System;

namespace ExpandedLists {
	/// <summary>
	/// <see cref="IExpandedList{T}"/> whose elements may not be contained following any order.
	/// </summary>
	/// <remarks>
	/// Offers a wider variety of methods using the lack of order.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public interface IUnsortedList<T> : IExpandedList<T> {

		/// <summary>
		/// Gets or sets th element at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// Read and write overwrite of <see cref="IExpandedList{T}"/>'s indexer.
		/// </remarks>
		/// <exception cref="InvalidOperationException"></exception>
		/// <returns>
		/// Element at <c>position</c>.
		/// </returns>
		new T this[int position] { get; set; }

		/// <summary>
		/// Returns a new list with the elements from <c>list</c> repeated <c>factor</c> times.
		/// </summary>
		/// <remarks>
		/// The elements are repeated when the last element is reached.
		/// <para>
		/// Equivalent to <see cref="Multiply(int)"/>.
		/// </para>
		/// </remarks>
		/// <returns>
		/// New list with the elements from <c>list</c> repeated.
		/// </returns>
		static IUnsortedList<T> operator *(IUnsortedList<T> list,int factor) {
			return list.Multiply(factor);
		}

		/// <summary>
		/// Inserts the element in the tuple into a new list in the position in it.
		/// </summary>
		/// <remarks>
		/// Equivalent to 
		/// <see cref="IUnsortedList{T}.InsertAt(T, int)"/>.
		/// </remarks>
		/// <returns>
		/// New list with the elements of <c>list</c> and the element in the tuple.
		/// </returns>
		static IUnsortedList<T> operator +(IUnsortedList<T> list, (T, int) tuple) {
			IUnsortedList<T> nueva = list.CloneUnsorted();
			nueva.InsertAt(tuple.Item1, tuple.Item2);
			return nueva;
		}

		/// <summary>
		/// Inserts <c>element</c> at the start of the list.
		/// </summary>
		void InsertFirst(T element);

		/// <summary>
		/// Inserts <c>element</c> at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="IExpandedList{T}.Count"/>.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		void InsertAt(T element, int position);

		/// <summary>
		/// Inserts <c>element</c> at the start of the list.
		/// </summary>
		void InsertLast(T element);

		/// <summary>
		/// Inserts <c>element amount</c> times in a row in the list, starting at <c>position</c>.
		/// </summary>
		/// <remarks>
		/// <c>position</c> must not be negative and must be lesser than <see cref="IExpandedList{T}.Count"/>.
		/// <para>
		/// If <c>amount</c> is negative, the list will not be modified.
		/// </para>
		/// </remarks>
		void InsertMultiple(T element, int amount, int position);

		/// <summary>
		/// Removes the appearances of <c>element</c> at the end of the list until the start or a different element is reached.
		/// </summary>
		/// <remarks>
		/// The changes in the design of this library have made this method pointless.
		/// </remarks>
		/// <returns>
		/// Number of elements removed.
		/// </returns>
		int RemoveLast(T element);

		/// <summary>
		/// Returns a list with the elements from this list repeated <c>factor</c> times.
		/// </summary>
		/// <remarks>
		/// The elements are repeated once the last element is reached.
		/// <para>
		/// In the following example <c>list</c> would be equal to <c>new UnsortedList([1,2,3,1,2,3])</c>:
		/// <code>
		/// list = new UnsortedList([1,2,3]);
		/// list.Multiply(2);
		/// </code>
		/// </para>
		/// Multiplying by a negative int does the same operation, but also reverses the list.
		/// <para>
		/// Multiplying by 0 clears the list.
		/// </para>
		/// </remarks>
		/// <returns>
		/// New list with repeated elements.
		/// </returns>
		IUnsortedList<T> Multiply(int factor);

		/// <summary>
		/// Creates a clone of this list, with the same type.
		/// </summary>
		/// <returns>
		/// New list equal to this list.
		/// </returns>
		IUnsortedList<T> CloneUnsorted();
	}
}
