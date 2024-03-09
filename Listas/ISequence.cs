namespace ExpandedLists {

	/// <summary>
	/// An <c>ISequence</c> is an <see cref="IDynamicList{T}"/> that is also an <seealso cref="INamedList{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISequence<T> : INamedList<T>, IDynamicList<T> {
		ISequence<T> CloneSequence();
	}
}
