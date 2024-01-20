using System;
using System.IO;
using System.Text;
using Listas;
using Operaciones;

namespace ProgramaDivisibilidad {
	public class CalculadoraDivisibilidadCLI {
		private const int MAX_ARGS = 64;
		private const string ERROR_NUEMRICO = "El divisor, base y el número de coeficientes deben ser números enteros positivos\r\ndivisor y base deben ser mayor que 1";
		public static int Main(string[] args) {
			bool[] flags;
			(string[] argsFinales,flags) = ObtenerFlags(args); //Obtenemos los flags y los argumentos numéricos de args
			int salida = 0; //Salida del programa
			if (flags[DatosFlags.DIRECTO_I]) { //Si args incluye d
				if (DatosFlags.NumeroArgumentosCorrecto(argsFinales.Length, Flag.Directo)) {
					if (!long.TryParse(argsFinales[0], out long divisor) ||
						!long.TryParse(argsFinales[1], out long @base) ||
						!int.TryParse(argsFinales[2], out int coeficientes) ||
						divisor <= 1 && @base <= 1 && coeficientes <= 0) { //Se comprueba que los argumentos sean correctos
						Console.WriteLine(ERROR_NUEMRICO);
						salida = 1;
						flags[DatosFlags.AYUDA_I] = true;
					} else {
						Calculos calc = new(@base);
						if (flags[DatosFlags.TODOS_I]) {
							ISerie<ISerie<long>> series = new ArrayListSerie<ISerie<long>>();
							calc.ReglasDivisibilidad(series, divisor, coeficientes);
							Console.WriteLine(SerieRectangularString(series, flags[DatosFlags.INVERSO_I]));
						} else {

						}
					}
				} else {
					flags[DatosFlags.AYUDA_I] = true;
				}
			}
			if (flags[DatosFlags.AYUDA_I]) {
				EscribirArchivo("Ayuda.txt");
			} else if (flags[DatosFlags.CORTA_I]) {
				EscribirArchivo("AyudaCorta.txt");
			} else {
				salida = IniciarAplicacion(flags);
			}
			return salida;
		}

		private static int IniciarAplicacion(bool[] flags) {
			return 0;
		}

		private static void EscribirArchivo(string ruta) {
			TextReader entradaAntigua = CambiarEntrada(ruta);
			string? linea;
			while ((linea = Console.ReadLine()) != null)
			{
				Console.WriteLine(linea);
			}
			RestablecerEntrada(entradaAntigua);
		}

		private static (string[],bool[]) ObtenerFlags(string[] args) {
			int indiceFlags = InicioArgs(args); //Debería ser 0 pero no estoy seguro
			bool[] flags = new bool[MAX_ARGS];
			string[] argumentos = new string[args.Length-indiceFlags-1];
			if (indiceFlags > -1) {
				args.CopyTo(argumentos,indiceFlags+1); //Extraemos los argumentos
				if (indiceFlags > 0) {
					ulong indiceEnum = DatosFlags.StringAIndice(args[indiceFlags].Remove(0,1));
					for (ulong i = 1; i <= ulong.MaxValue; i <<= 1) { //Inicializamos flags
						flags[i] = (i&indiceEnum) == 0;
					}
				}
			} else {
				argumentos = [];
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

		private static string SerieRectangularString(ISerie<ISerie<long>> serie, bool inverso) {
			StringBuilder stringBuilder = new();
			foreach (var item in serie) {
				if (inverso) {
					stringBuilder.Append(item.StringInverso());
				} else {
					stringBuilder.Append(item.ToString());
				}
				stringBuilder.Append('\n');
			}
			return stringBuilder.ToString();
		}
	}
}
