using System;
using System.Collections;
using System.IO;
using System.Text;
using Listas;
using Operaciones;

namespace ProgramaDivisibilidad {
	public static class CalculadoraDivisibilidadCLI {
		private const int MAX_ARGS = 64,MAX_DATOS = 3;
		private const string ERROR_NUEMRICO = "El divisor, base y el número de coeficientes deben ser números enteros positivos\r\ndivisor y base deben ser mayor que 1"
			,SALIDA = "FIN";
		private static readonly bool[] flags = new bool[MAX_ARGS]; //Serán usados en todo el programa
		private static readonly string[] datos = new string[MAX_DATOS];
		private static bool salir = false; //Usado para saber si el usuario ha solicitado terminar la ejecución
		private static int salida = 0; //Salida del programa
		public static int Main(string[] args) {
			ObtenerFlags(args); //Obtenemos los flags y los argumentos numéricos de args
			if (flags[DatosFlags.AYUDA_I]) {
				EscribirArchivo("Ayuda.txt");
			} else if (flags[DatosFlags.CORTA_I]) {
				EscribirArchivo("AyudaCorta.txt");
			} else {
				bool exito = true; //Valor de éxito de la última llamada, usada para llamar a ayuda si falla sin cambiar los flags
				if (flags[DatosFlags.DIRECTO_I]) { //Si args incluye d
					exito = IntentarDirecto();
				} else {
					salida = IniciarAplicacion();
				}
				if (!exito) {
					EscribirArchivo("AyudaCorta.txt");
				}
			}
			return salida;
		}

		private static bool IntentarDirecto() { //Intenta dar las reglas de forma directa, devuelve true si lo consigue y false si hay algún error
			bool correcto = DatosFlags.NumeroArgumentosCorrecto(datos.Length, Flag.Directo);
			if (correcto) {
				if (!long.TryParse(datos[0], out long divisor) ||
					!long.TryParse(datos[1], out long @base) ||
					!int.TryParse(datos[2], out int coeficientes) ||
					(divisor <= 1 && @base <= 1 && coeficientes <= 0)) { //Si los argumentos no son correctos
					Console.WriteLine(ERROR_NUEMRICO);
					salida = 1;
					flags[DatosFlags.AYUDA_I] = true;
					correcto = false;
				} else { //Si los argumentos son correctos
					Console.WriteLine(ObtenerReglas(divisor,@base,coeficientes));
				}
			}
			return correcto;
		}

		private static int IniciarAplicacion() { //Si no se proporcionan los argumentos de forma directa, se establece un diálogo con el usuario para obtenerlos
			bool preguntarTodos = !flags[DatosFlags.TODOS_I], preguntarInverso = !flags[DatosFlags.INVERSO_I] //Si los flags están activados no se preguntan
				, continuar; //Si se cambia a true se continua el bucle
			Console.WriteLine($"El programa se ejecutará de forma normal, escriba {SALIDA} para cerrarlo");

			do {
				Console.WriteLine("Introduzca el divisor para calcular su regla de divisibilidad");
				ObtenerDeUsuario(out long divisor, 2, "El divisor debe ser un entero mayor que 1");
				if (salir) {
					Console.WriteLine("Se ha interrumpido el programa");
					break;
				}
				Console.WriteLine("Introduzca la base en la que se calculará la base, debe ser coprima con el divisor");
				ObtenerDeUsuarioCoprimo(out long @base, 2, "La base debe ser un entero mayor que 1 y coprima con el divisor",divisor);
				if (salir) {
					Console.WriteLine("Se ha interrumpido el programa");
					break;
				}
				Console.WriteLine("Introduzca el número de coeficientes de la regla, esto afecta al resultado");
				ObtenerDeUsuario(out int coeficientes, 1, "El número de coeficientes debe ser positivo");
				if (salir) {
					Console.WriteLine("Se ha interrumpido el programa");
					break;
				}
				if (preguntarTodos) {
					Console.WriteLine("Escriba S si quiere obtener todas las reglas de divisibilidad con coeficientes pequeños, si no se devolverá el que tenga menores coeficientes");
					ObtenerDeUsuario(out flags[DatosFlags.TODOS_I]);
					if (salir) {
						Console.WriteLine("Se ha interrumpido el programa");
						break;
					}
				}
				if (preguntarInverso) {
					Console.WriteLine("Escriba S si quiere que se escriban las reglas en orden inverso");
					ObtenerDeUsuario(out flags[DatosFlags.INVERSO_I]);
					if (salir) {
						Console.WriteLine("Se ha interrumpido el programa");
						break;
					}
				}
				Console.WriteLine("Reglas obtenidas:\n" + ObtenerReglas(divisor, @base, coeficientes));
				Console.WriteLine("Escriba S si calcular otras reglas");
				ObtenerDeUsuario(out continuar);
			} while (continuar);
			
			return salir? 2 : 0;
		}

		private static void ObtenerDeUsuario(out long dato, long minimo, string mensaje) {
			string? linea = Console.ReadLine();
			while (linea is null || !long.TryParse(linea, out dato) || dato < minimo) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
			}
			salir = linea == SALIDA;
		}

		private static void ObtenerDeUsuario(out int dato, long minimo, string mensaje) {
			string? linea = Console.ReadLine();
			while (linea is null || !int.TryParse(linea, out dato) || dato < minimo) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
			}
			salir = linea == SALIDA;
		}

		private static void ObtenerDeUsuarioCoprimo(out long dato, long minimo, string mensaje, long coprimo) {
			string? linea = Console.ReadLine();
			while (linea is null || !long.TryParse(linea, out dato) || dato < minimo || CalculosEstatico.Mcd(dato,coprimo) > 1) {
				Console.WriteLine(mensaje);
				linea = Console.ReadLine();
			}
			salir = linea == SALIDA;
		}

		private static void ObtenerDeUsuario(out bool dato) {
			string? linea = Console.ReadLine();
			while (linea is null) {
				linea = Console.ReadLine();
			}
			dato = linea.ToLower().StartsWith('s');
			salir = linea == SALIDA;
		}

		private static string ObtenerReglas(long divisor, long @base, int coeficientes) {
			Calculos calc = new(@base);
			string resultado;
			if (flags[DatosFlags.TODOS_I]) { //Si se piden las 2^coeficientes reglas
				ISerie<ISerie<long>> series = new ArrayListSerie<ISerie<long>>();
				calc.ReglasDivisibilidad(series, divisor, coeficientes);
				resultado = SerieRectangularString(series);
			} else {
				ArrayListSerie<long> serie = new();
				calc.ReglaDivisibilidadOptima(serie, divisor, coeficientes);
				resultado = StringSerieConFlags(serie);
			}
			return resultado;
		}

		private static void EscribirArchivo(string ruta) {
			string texto = File.ReadAllText(ruta);
			Console.WriteLine(texto);
		}

		private static void ObtenerFlags(string[] args) {
			int indiceFlags = InicioArgs(args); //Debería ser 0 pero no estoy seguro
			if (indiceFlags > -1) {
				CopiarArrayParcial(args,datos,indiceFlags+1,MAX_DATOS); //Extraemos los argumentos
				ulong indiceEnum = DatosFlags.StringAIndice(args[indiceFlags].Remove(0,1));
				for (int i = 0; i < MAX_ARGS; i++) { //Inicializamos flags
					flags[i] = ((1UL<<i)&indiceEnum) != 0;
				}
			}
		}

		//Copia los elementos entre inicioCopia y finCopia de original a los primeros elementos de copia
		private static void CopiarArrayParcial(string[] original, string[] copia, int inicioCopia, int finCopia) {
			int indice = 0;
			while (indice < copia.Length && inicioCopia + indice <= finCopia && indice + inicioCopia < original.Length) {
				copia[indice] = original[inicioCopia + indice++];
			}
		}

		private static int InicioArgs(string[] args) {
			int indice = 0;
			while (args.Length > indice && !args[indice].StartsWith('-')) {
				indice++;
			}
			if (indice == args.Length) {
				indice = -1;
			}
			return indice;
		}

		private static string SerieRectangularString(ISerie<ISerie<long>> serie) {
			StringBuilder stringBuilder = new();
			foreach (var item in serie) {
				if (flags[DatosFlags.INVERSO_I]) {
					stringBuilder.Append(item.ToStringInverso());
				} else {
					stringBuilder.Append(item.ToString());
				}
				stringBuilder.Append('\n');
			}
			return stringBuilder.ToString();
		}

		private static string StringSerieConFlags(ISerie<long> serie) {
			string res;
			if (flags[DatosFlags.INVERSO_I]) {
				res = serie.ToStringInverso();
			} else {
				res = serie.ToString()??string.Empty;
			}
			return res;
		}
	}
}
