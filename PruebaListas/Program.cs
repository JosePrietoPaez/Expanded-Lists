using System;

namespace PruebaListas
{
	internal class Program
	{
		static void Main(string[] args)
		{
			bool[] flags;
			(string[] argsFinales,flags) = ObtenerFlags(args);
		}

		private static (string[],bool[]) ObtenerFlags(string[] args)
		{
			int numFlags = InicioArgs(args);
			bool[] flags = new bool[numFlags];
			string[] argumentos = new string[args.Length - numFlags];
			Array.Copy(args,argumentos,numFlags);
			if (numFlags > 0)
			{
				ulong indiceEnum = StringsFlags.StringAIndice(args);
				for (ulong i = 1; i <= ulong.MaxValue; i >>= 1) 
				{
					if ((i&indiceEnum) == 0)
					{
						flags[i] = true;
					}
				}
			}
			return (argumentos,flags);
		}

		private static int InicioArgs(string[] args)
		{
			int indice = 0;
			while (args[indice].StartsWith('-'))
			{
				indice++;
			}
			return indice;
		}

		private static void IniciarAplicacion()
		{
			throw new NotImplementedException();
		}
	}
}
