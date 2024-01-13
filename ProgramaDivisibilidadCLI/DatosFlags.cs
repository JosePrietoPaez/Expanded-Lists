using System;
using System.Linq.Expressions;

namespace ProgramaDivisibilidad
{

	internal static class DatosFlags
	{
		public const int UNICO_I = 0, AYUDA_I = 1,INVERSO_I = 2;
		public const string UNICO = "u", AYUDA = "h", INVERSO = "r";
		private static readonly string[] _todos = [UNICO,AYUDA,INVERSO];
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

		internal static bool NumeroArgumentosCorrecto(int numArgs, int flag)
		{
			return flag switch {
				UNICO_I => numArgs == 3,
				INVERSO_I => numArgs == 3, //Tengo que planear los argumentos
				AYUDA_I => true,
				_ => false,
			} ;
		}
	}
}
