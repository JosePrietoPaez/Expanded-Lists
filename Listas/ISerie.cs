namespace Listas {
	/// <summary>
	/// Una serie se representa como una <see cref="IListaDinamica{T}"/> que también es <seealso cref="IListaNombrada{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISerie<T> : IListaNombrada<T>, IListaDinamica<T> {
		ISerie<T> ClonarSerie();
	}
}
