using System;
using System.Collections.Generic;

namespace Listas.Bloques {
	public interface IListaBloquesDinamica<E,B> : IListaBloques<E,B> where B : Bloque<E> {

		Func<int,int> FuncionDeExtension { get; set; }

		B SetBloque(B bloque, int posicion);

		void IntercambiarBloques(int primero, int segundo);

	}
}
