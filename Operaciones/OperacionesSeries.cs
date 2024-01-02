using Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operaciones
{
	public class OperacionesSeries
	{
		/**
     * Escribe, a partir de la posici&oacute;n {@code pos}, las potencias de {@code raiz} en aumento desde {@code desde} hasta {@code hasta}
     * <p>Ambos {@code desde} y {@code hasta} est&aacute;n incluidos</p>
     * <p>{@code hasta} no puede ser menor que {@code desde} y ninguno puede ser menor que 0</p>
     * Se alargar&aacute; la serie si no cabe
     * @param serie la serie que modificar
     * @param raiz la raiz de la potencia
     * @param pos la posici&oacute;n de inicio
     * @param desde exponente de la primera potencia
     * @param hasta exponente de la &uacute;ltima potencia
     * @param incremento diferencia entre el exponente en cada posici&oacute;n adyacente
     */
		public static void PotenciaProgresiva(ISerie<long> serie, long raiz, int desde, int hasta, int incremento, int pos)
		{
			if (desde < 0 || hasta < 0) throw new ArgumentOutOfRangeException(nameof(desde));
			ArgumentOutOfRangeException.ThrowIfGreaterThan(desde, hasta);
			int iteraciones = (hasta - desde) / incremento;
			long num = (long)Math.Pow(raiz, desde);
			for (int i = pos, cont = desde; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				num = (long)Math.Pow(raiz, cont);
			}
		}

		/**
		 * Escribe, a partir de la posici&oacute;n {@code pos}, las potencias de {@code raiz} en aumento desde {@code desde} hasta {@code hasta}
		 * <p>Ambos {@code desde} y {@code hasta} est&aacute;n incluidos</p>
		 * <p>{@code hasta} no puede ser menor que {@code desde}</p>
		 * Se alargar&aacute; la serie si no cabe
		 * @param serie la serie que modificar
		 * @param raiz la raiz de la potencia
		 * @param pos la posici&oacute;n de inicio
		 * @param desde exponente de la primera potencia
		 * @param hasta exponente de la &uacute;ltima potencia
		 * @param incremento diferencia entre el exponente en cada posici&oacute;n adyacente
		 */
		public static void PotenciaProgresiva(ISerie<Double> serie, double raiz, double desde, double hasta, double incremento, int pos)
		{
			if (desde > hasta) throw new ArgumentOutOfRangeException("La última potencia no puede ser menor que la primera");
			double num = Math.Pow(raiz, desde), cont = desde;
			double iteraciones = (hasta - desde) / incremento;
			for (int i = pos; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				num = Math.Pow(raiz, cont);
			}
		}

		/**
		 * Escribe, a partir de la posici&oacute;n {@code pos}, las potencias de {@code raiz} en aumento desde {@code desde} hasta {@code hasta}
		 * <p>Ambos {@code desde} y {@code hasta} est&aacute;n incluidos</p>
		 * <p>{@code hasta} no puede ser menor que {@code desde} y ninguno puede ser menor que 0</p>
		 * Se alargar&aacute; la serie si no cabe
		 * @param serie la serie que modificar
		 * @param raiz la raiz de la potencia
		 * @param pos la posici&oacute;n de inicio
		 * @param mod m&oacute;dulo que aplicar a la potencia
		 * @param desde exponente de la primera potencia
		 * @param hasta exponente de la &uacute;ltima potencia
		 * @param incremento diferencia entre el exponente en cada posici&oacute;n adyacente
		 */
		public static void PotenciaModProgresiva(ISerie<long> serie, long raiz, long mod, int desde, int hasta, int incremento, int pos)
		{
			if (desde < 0 || hasta < 0) throw new ArgumentException("Las potencias son de exponentes no negativos");
			ArgumentOutOfRangeException.ThrowIfGreaterThan(desde, hasta,"La última potencia no puede ser menor que la primera");
			long num = (long)Math.Pow(raiz, desde);
			int iteraciones = (hasta - desde) / incremento;
			for (int i = pos, cont = desde; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				for (int j = 0; j < incremento; j++)
				{
					num = CalculosEstatico.ProductoMod(num, raiz, mod);
				}
			}
		}

		/**
		 * Escribe, a partir de la posici&oacute;n {@code pos}, las primeras {@code hasta} potencias de {@code raiz}, desde 1
		 * <p>{@code hasta} no puede ser menor que 1 y ninguno puede ser menor que 0</p>
		 * Se alargar&aacute; la serie si no cabe
		 * @param serie la serie que modificar
		 * @param raiz la raiz de la potencia
		 * @param pos la posici&oacute;n de inicio
		 * @param hasta exponente de la &uacute;ltima potencia
		 */
		public static void PotenciaProgresiva(ISerie<long> serie, long raiz, int hasta, int pos)
		{
			PotenciaProgresiva(serie, raiz, 1, hasta, 1, pos);
		}

		/**
		 * Escribe, a partir de la posici&oacute;n {@code pos}, las primeras {@code hasta} potencias de {@code raiz}, desde 1
		 * <p>{@code hasta} no puede ser menor que 1 y ninguno puede ser menor que 0</p>
		 * Se alargar&aacute; la serie si no cabe
		 * @param serie la serie que modificar
		 * @param raiz la raiz de la potencia
		 * @param pos la posici&oacute;n de inicio
		 * @param hasta exponente de la &uacute;ltima potencia
		 */
		public static void PotenciaProgresiva(ISerie<Double> serie, double raiz, double hasta, int pos)
		{
			PotenciaProgresiva(serie, raiz, 1, hasta, 1, pos);
		}

		/**
		 * Escribe, a partir de la posici&oacute;n {@code pos}, las potencias de {@code raiz} en aumento desde 1 hasta {@code hasta}
		 * <p> 1 y {@code hasta} est&aacute;n incluidos</p>
		 * <p>{@code hasta} no puede ser menor que 1 y no puede ser menor que 0</p>
		 * Se alargar&aacute; la serie si no cabe
		 * @param serie la serie que modificar
		 * @param raiz la raiz de la potencia
		 * @param pos la posici&oacute;n de inicio
		 * @param mod m&oacute;dulo que aplicar a la potencia
		 * @param hasta exponente de la &uacute;ltima potencia
		 */
		public static void PotenciaModProgresiva(ISerie<long> serie, long raiz, long mod, int hasta, int pos)
		{
			PotenciaModProgresiva(serie, raiz, mod, 1, hasta, 1, pos);
		}

		/**
		 * Indica si {@code arr} solo tiene valores false
		 * @return {@code true} si {@code arr} no tiene ning&uacute;n valor a {@code true}
		 */
		public static bool ArrayFalso(bool[] arr)
		{
			bool res = true;
			for (int i = 0; i < arr.Length && res; i++) res = !arr[i];
			return res;
		}
		/**
		 * Incrementa el valor de {@code arr} en 1 de la misma forma que con un n&uacute;mero binario
		 * <p>Puede producir overflow</p>
		 */
		public static void IncrementarArray(bool[] arr)
		{
			int i = 0;
			bool seguir = true;
			while (i < arr.Length && seguir)
			{
				if (!arr[i])
				{
					arr[i] = true;
					seguir = false;
				}
				else
				{
					arr[i] = false;
					i++;
				}
			}
		}

		public static long Producto(ISerie<long> serie)
		{
			long res = 1;
			foreach (long elem in
             serie)
			{
				res *= elem;
			}
			return res;
		}
	}
}
