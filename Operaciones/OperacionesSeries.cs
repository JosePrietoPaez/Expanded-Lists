using Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operaciones
{
	public class OperacionesSeries
	{
		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde <c>inicio</c> hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="incremento">diferencia entre el exponente en cada posición adyacente</param>
		/// <param name="inicio">exponente de la primera potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaProgresiva(ISerie<long> serie, long @base, int inicio, int fin, int incremento, int pos)
		{
			if (inicio < 0 || fin < 0) throw new ArgumentOutOfRangeException(nameof(inicio));
			ArgumentOutOfRangeException.ThrowIfGreaterThan(inicio, fin);
			int iteraciones = (fin - inicio) / incremento;
			long num = (long)Math.Pow(@base, inicio);
			for (int i = pos, cont = inicio; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				num = (long)Math.Pow(@base, cont);
			}
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde <c>inicio</c> hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="incremento">diferencia entre el exponente en cada posición adyacente</param>
		/// <param name="inicio">exponente de la primera potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaProgresiva(ISerie<double> serie, double @base, double inicio, double fin, double incremento, int pos)
		{
			if (inicio > fin) throw new ArgumentOutOfRangeException("La última potencia no puede ser menor que la primera");
			double num = Math.Pow(@base, inicio), cont = inicio;
			double iteraciones = (fin - inicio) / incremento;
			for (int i = pos; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				num = Math.Pow(@base, cont);
			}
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde <c>inicio</c> hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="incremento">diferencia entre el exponente en cada posición adyacente</param>
		/// <param name="inicio">exponente de la primera potencia</param>
		/// <param name="mod">modulo que aplicar a la potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaModProgresiva(ISerie<long> serie, long @base, long mod, int inicio, int fin, int incremento, int pos)
		{
			if (inicio < 0 || fin < 0) throw new ArgumentException("Las potencias son de exponentes no negativos");
			ArgumentOutOfRangeException.ThrowIfGreaterThan(inicio, fin,"La última potencia no puede ser menor que la primera");
			long num = (long)Math.Pow(@base, inicio);
			int iteraciones = (fin - inicio) / incremento;
			for (int i = pos, cont = inicio; i <= iteraciones; i++)
			{
				serie.Poner(num, i);
				cont += incremento;
				for (int j = 0; j < incremento; j++)
				{
					num = CalculosEstatico.ProductoMod(num, @base, mod);
				}
			}
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde 1 hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaProgresiva(ISerie<long> serie, long @base, int fin, int pos)
		{
			PotenciaProgresiva(serie, @base, 1, fin, 1, pos);
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde 1 hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaProgresiva(ISerie<double> serie, double @base, double fin, int pos)
		{
			PotenciaProgresiva(serie, @base, 1, fin, 1, pos);
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde <c>inicio</c> hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos <c>inicio</c> y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que <c>inicio</c> y ninguno puede ser menor que 0
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la última potencia</param>
		/// <param name="incremento">diferencia entre el exponente en cada posición adyacente</param>
		/// <param name="inicio">exponente de la primera potencia</param>
		/// <param name="mod">modulo que aplicar a la potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaModProgresiva(ISerie<long> serie, long raiz, long mod, int hasta, int pos)
		{
			PotenciaModProgresiva(serie, raiz, mod, 1, hasta, 1, pos);
		}

		/**
		 * Indica si {@code arr} solo tiene valores false
		 * @return {@code true} si {@code arr} no tiene ningún valor a {@code true}
		 */
		public static bool ArrayFalso(bool[] arr)
		{
			bool res = true;
			for (int i = 0; i < arr.Length && res; i++) res |= arr[i];
			return res;
		}
		/**
		 * Incrementa el valor de {@code arr} en 1 de la misma forma que con un número binario
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
