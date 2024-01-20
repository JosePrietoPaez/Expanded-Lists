using System;
using System.Drawing;
using System.Linq.Expressions;

namespace ProgramaDivisibilidad
{
	internal enum Flag {
		Directo = DatosFlags.DIRECTO_I,
		Ayuda = DatosFlags.AYUDA_I,
		Inverso = DatosFlags.INVERSO_I,
		AyudaCorta = DatosFlags.CORTA_I,
		Todas = DatosFlags.TODOS_I
	}
	internal static class DatosFlags
	{
		public const int DIRECTO_I = 0, AYUDA_I = 1,INVERSO_I = 2,CORTA_I = 3, TODOS_I = 4;
		public const string DIRECTO = "d", AYUDA = "H", INVERSO = "r", CORTA = "h", TODOS = "t";
		private static readonly string[] _todos = [DIRECTO,AYUDA,INVERSO,CORTA,TODOS];
		internal static ulong StringAIndice(string array)
		{
			ulong flags = 0UL;
			foreach(var flag in _todos)
			{	int indice = 0;
				if (array.Contains(flag))
				{
					flags |=  1UL << indice++;
				}
			}
			return flags;
		}

		internal static bool NumeroArgumentosCorrecto(int numArgs, Flag flag)
		{
			return flag switch { //Tengo que planear los argumentos
				Flag.Directo => numArgs == 3,
				_ => true, //Generalmente no se comprueba
			} ;
		}
	}
}
