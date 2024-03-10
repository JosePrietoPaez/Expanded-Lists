namespace ExpandedLists {

	/// <summary>
	/// Contains a variety of error messages used in this library.
	/// </summary>
	public static class Messages {
		public static string ListRange(int index, int limit) {
			return $"Indexed position is invalid({index}, on length {limit})";
		}

		public static string EmptyBlock => "Block is empty";

		public static string EmptyList => "List is empty";

		public static string EmptySequence => "Sequence is empty";

		public static string NullGeneration => "Generator function returned null";

		public static string NegativeLength => "Length is negative";

		public static string NegativeArgument => "Argument is negative";

		public static string NonFullBlock => "Block is not full";

		public static string NullBlock => "Block is null";
	}
}
