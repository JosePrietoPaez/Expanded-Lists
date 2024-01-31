using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listas {
	public interface ISerie<T> : IListaNombrada<T>, IListaDinamica<T> {
		new ISerie<T> Clonar();
	}
}
