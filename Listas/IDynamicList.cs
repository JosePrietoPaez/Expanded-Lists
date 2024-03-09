using System;

namespace ExpandedLists {

	/// <summary>
	/// Dynamic lists expand on <see cref="IUnsortedList{T}"/> using <see cref="GeneratorFunction"/>
	/// to generate new list elements and adding them to the list when necessary
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDynamicList<T> : IUnsortedList<T> {
		/// <summary>
		/// This property represents the function that generates new instances of <c>T</c> to be used to fill the list
		/// if <see cref="IsEmpty"/> is set to <c>false</c> or <see cref="Count"/> is set to a value greater than its current value.
		/// </summary>
		/// <remarks>
		/// The argument used for the function will be the size of the list at the time of invocation.
		/// <para>
		/// By default it generates the default instance of the class, so it is recommended to instantiate it in the constructor or by using this property
		/// </para>
		/// </remarks>
		Func<int,T?> GeneratorFunction { get; set; }

		/// <summary>
		/// This property returns the amount of elements in the list when reading it and set it when writing it.
		/// </summary>
		/// <remarks>
		/// When increasing the count with this property, the list will be filled using <see cref="GeneratorFunction"/>.
		/// <para>
		/// When reducing it, the last elements will be removed.
		/// </para>
		/// Setting the count to a negative number throws an exception.
		/// </remarks>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		new int Count { get; set; }

		/// <summary>
		/// This property returns <see cref="IExList{T}.IsEmpty"/> when read and may be used to clear the list or add an element if set
		/// </summary>
		/// <remarks>
		/// When trying <c>list.IsEmpty = false</c>, <see cref="GeneratorFunction"/> will be used to add an element if list was empty.
		/// </remarks>
		new bool IsEmpty { get; set; }

		/// <summary>
		/// Returns a new list containing all the elements from <c>list</c> and a <see cref="GeneratorFunction"/>(<c>list.Count</c>)
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalent to
		/// <see cref="IDynamicList{T}.Count"/><c>++</c>
		/// <para>
		/// Needs <see cref="IDynamicList{T}.GeneratorFunction"/> not to generate null elements if not allowed</para>
		/// </remarks>
		/// <returns>A new list like <c>list</c> with one more element</returns>
		static IDynamicList<T> operator ++(IDynamicList<T> list) {
			Contract.Requires<InvalidOperationException>
				(CompatibleEnLista(list.GeneratorFunction.Invoke(list.Count)), "La función de generación ha creado un elemento nulo");
			list.Count++;
			return list;
		}

		/// <summary>
		/// Returns a new list like <c>list</c> without its last element
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalent to 
		/// <see cref="IDynamicList{T}.Count"/><c>--</c>
		/// </remarks>
		/// <returns>List like <c>list</c> without its last element</returns>
		static IDynamicList<T> operator --(IDynamicList<T> list) {
			list.Count--;
			return list;
		}

		/// <summary>
		/// Creates a clone of this list
		/// </summary>
		/// <returns>
		/// New list equal to this list
		/// </returns>
		IDynamicList<T> CloneDynamic();

	}
}
