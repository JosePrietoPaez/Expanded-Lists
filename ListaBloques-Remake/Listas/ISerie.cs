using Listas;
using System;
using System.Collections.Generic;

namespace Listas
{
	public interface ISerie<T> : IListaArbitraria<T>
	{
		string Nombre { get; set; }

		/// <summary>
		/// Devuelve un objeto String que representa la lista, pero con los elementos en el orden inverso
		/// </summary>
		String SringInverso();
	}
}
