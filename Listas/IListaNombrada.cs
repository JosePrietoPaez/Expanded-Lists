using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listas {
	public interface IListaNombrada<T> : ILista<T> {

		string Nombre { get; set; }

		/// <summary>
		/// Devuelve un objeto String que representa la lista, pero con los elementos en el orden inverso
		/// </summary>
		String ToStringInverso();
	}
}
