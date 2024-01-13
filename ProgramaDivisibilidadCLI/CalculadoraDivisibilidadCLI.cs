using System;
using System.IO;

namespace ProgramaDivisibilidad
{
	public class CalculadoraDivisibilidadCLI
	{
		public static int Main(string[] args)
		{
			bool[] flags;
			(string argsFinales,flags) = ObtenerFlags(args);
			if (flags[DatosFlags.UNICO_I]) 
			{
				if (DatosFlags.NumeroArgumentosCorrecto(argsFinales.Length, DatosFlags.UNICO_I))
				{
					
				}
				else
				{
					flags[DatosFlags.AYUDA_I] = true;
				}
			}
			if (flags[DatosFlags.AYUDA_I])
			{
				EscribirArchivo("Ayuda.txt");
			}
			IniciarAplicacion();
			return 0;
		}

		private static void EscribirArchivo(string ruta)
		{
			TextReader entradaAntigua = CambiarEntrada(ruta);
			string? linea;
			while ((linea = Console.ReadLine()) != null)
			{
				Console.WriteLine(linea);
			}
			RestablecerEntrada(entradaAntigua);
		}

		private static (string,bool[]) ObtenerFlags(string[] args)
		{
			int indiceFlags = InicioArgs(args);
			bool[] flags = new bool[64];
			string argumentos;
			if (indiceFlags > -1) {
				argumentos = args[indiceFlags];
				if (indiceFlags > 0)
				{
					ulong indiceEnum = DatosFlags.StringAIndice(argumentos);
					for (ulong i = 1; i <= ulong.MaxValue; i <<= 1) 
					{
						if ((i&indiceEnum) == 0)
						{
							flags[i] = true;
						}
					}
				}
			} else {
				argumentos = String.Empty;
			}
			return (argumentos,flags);
		}

		private static int InicioArgs(string[] args)
		{
			int indice = 0;
			while (args.Length > indice && !args[indice].StartsWith('-'))
			{
				indice++;
			}
			if (indice == args.Length) {
				indice = -1;
			}
			return indice;
		}

		private static void IniciarAplicacion()
		{
			throw new NotImplementedException();
		}

		private static TextReader CambiarEntrada(string ruta)
		{
			TextReader entradaAntigua = Console.In;
			Console.SetIn(new StreamReader(ruta));
			return entradaAntigua;
		}

		private static void RestablecerEntrada(TextReader entradaAntigua)
		{
			TextReader entradaUsada = Console.In;
			Console.SetIn(entradaAntigua);
			entradaUsada.Close();
		}
	}
}
