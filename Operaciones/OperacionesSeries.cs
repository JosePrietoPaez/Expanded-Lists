﻿using Listas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Operaciones
{
	public class OperacionesSeries {
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
		public static void PotenciaProgresiva(IListaArbitraria<long> serie, long @base, int inicio, int fin, int incremento, int pos) {
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
		public static void PotenciaProgresiva(IListaArbitraria<double> serie, double @base, double inicio, double fin, double incremento, int pos) {
			ArgumentOutOfRangeException.ThrowIfGreaterThan(inicio, fin);
			double num = Math.Pow(@base, inicio), cont = inicio;
			double iteraciones = (fin - inicio) / incremento;
			for (int i = pos; i <= iteraciones; i++) {
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
		public static void PotenciaModProgresiva(ISerie<long> serie, long @base, long mod, int inicio, int fin, int incremento, int pos) {
			if (inicio < 0 || fin < 0) throw new ArgumentException("Las potencias son de exponentes no negativos");
			ArgumentOutOfRangeException.ThrowIfGreaterThan(inicio, fin,"La última potencia no puede ser menor que la primera");
			long num = (long)Math.Pow(@base, inicio);
			int iteraciones = (fin - inicio) / incremento;
			for (int i = pos, cont = inicio; i <= iteraciones; i++) {
				serie.Poner(num, i);
				cont += incremento;
				for (int j = 0; j < incremento; j++) {
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
		public static void PotenciaProgresiva(ISerie<long> serie, long @base, int fin, int pos) {
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
		public static void PotenciaProgresiva(ISerie<double> serie, double @base, double fin, int pos) {
			PotenciaProgresiva(serie, @base, 1, fin, 1, pos);
		}

		/// <summary>
		/// Escribe, a partir de la posición <c>pos</c>, las potencias de <c>base</c> en aumento desde 1 hasta <c>fin</c>
		/// </summary>
		/// <remarks>
		/// Ambos 1 y <c>fin</c> están incluidos
		/// <para>
		/// <c>fin</c> no puede ser menor que 1
		/// </para>
		/// </remarks>
		/// <param name="fin">exponente de la ultima potencia</param>
		/// <param name="mod">modulo que aplicar a la potencia</param>
		/// <param name="pos">la posición de inicio</param>
		/// <param name="base">la base de la potencia</param>
		/// <param name="serie">la serie que modificar</param>
		public static void PotenciaModProgresiva(ISerie<long> serie, long @base, long mod, int fin, int pos) {
			PotenciaModProgresiva(serie, @base, mod, 1, fin, 1, pos);
		}

		/// <summary>
		/// Indica si <c>arr</c> solo tiene valores <c>false</c>
		/// </summary>
		/// <returns>
		/// <c>true</c> si <c>arr</c> no tiene ningún valor a <c>true</c>
		/// </returns>
		public static bool ArrayFalso(bool[] arr) {
			bool hayTrue = false;
			for (int i = 0; i < arr.Length && !hayTrue; i++) hayTrue |= arr[i];
			return !hayTrue;
		}

		/// <summary>
		/// Incrementa el valor de <c>arr</c> en 1 de la misma forma que con un número binario
		/// </summary>
		/// <remarks>
		/// Puede producir overflow, haciendo que todos los valores se cambien a <c>false</c>
		/// </remarks>
		public static void IncrementarArray(bool[] arr) {
			int i = 0;
			bool seguir = true;
			while (i < arr.Length && seguir) {
				seguir = arr[i];
				arr[i] = !arr[i];
				if (!arr[i]) i++;
			}
		}

		public static long Producto(ILista<long> serie) {
			long res = 1;
			foreach (long elem in serie) {
				res *= elem;
			}
			return res;
		}
	}
}
