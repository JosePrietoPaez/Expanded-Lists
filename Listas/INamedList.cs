using System;

namespace ExpandedLists {
	public interface INamedList<T> : IExpandedList<T> {

		/// <summary>
		/// Represents the name of the sequence, can be set and read
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Returns a String that represents the list, with the elements in reverse order
		/// </summary>
		String ReverseToString();
	}
}
