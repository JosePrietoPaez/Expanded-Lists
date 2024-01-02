using Listas;
using System.Text;

namespace Operaciones
{
	public class Calculos
	{
		private long _raiz;

		public long Raiz
		{
			get { return _raiz; }
			set {
				if (value < 1)
				{
					throw new ArgumentException("La raiz de los cálculos debe ser mayor que 0");
				}
				this._raiz = value;
			}
		}

		/**
		 * Genera un objeto de {@code Calculos} con valor predeterminado raiz
		 * @param raiz se usa como un argumento de gran parte de las funciones de instancia
		 * @throws IllegalArgumentException si la raiz es menor que 2
		 */
		public Calculos(long raiz)
		{
			if (raiz < 1)
			{
				throw new ArgumentException("La raiz de los cálculos debe ser mayor que 0");
			}
			this._raiz = raiz;
		}

		/**
		 * Genera un objeto con valor por defecto 10
		 */
		public Calculos() : this(10L) { }

		public bool EsPrimo()
		{
			return CalculosEstatico.EsPrimo(_raiz);
		}

		/**
		 * Calcula el factorial de la raiz del objeto {@code Calculos}
		 * Equivalente a {@code Calculos.factorial(raiz)}
		 * @return raiz!
		 */
		public long Factorial()
		{
			return CalculosEstatico.Factorial(_raiz);
		}

		/**
		 * Calcula el máximo común divisor de la raiz del objeto {@code Calculos} y segundo
		 * <p>Equivalente a {@code Calculos.mcd(raiz,segundo)}</p>
		 * @return mcd(raiz, segundo)
		 */
		public long Mcd(long segundo)
		{
			return CalculosEstatico.Mcd(_raiz, segundo);
		}

		/**
		 * Devuelve una serie que contiene la descomposición en números primos de la raiz del objeto {@code Calculos}
		 * <p>Los elementos de la serie son los exponentes de los números primos en orden ascendente</p>
		 */
		public ISerie<long> DescomposicionEnPrimos()
		{
			return CalculosEstatico.DescompsicionEnPrimos(_raiz);
		}

		//Guarda en desc la descomposicion de num, usando los primos de primos
		internal static void Descomposicion(ISerie<long> primos, ISerie<long> desc, long num)
		{
			for (int j = 0; j < primos.Longitud && num > 1; j++)
			{
				while (num % primos[j] == 0)
				{
					num /= primos[j];
					desc.Cambiar(j, desc[j] + 1);
				}
			}
		}

		/**
		 * Calcula el mínimo común múltiplo de la raiz de objeto {@code Calculos} y {@code segundo}
		 * @return mcm(raiz,segundo)
		 */
		public long Mcm(long segundo)
		{
			return CalculosEstatico.Mcm(_raiz, segundo);
		}

		/**
		 * Devuelve una serie con los números primos hasta raiz incluido
		 * <p>La serie tendrá nombre nulo</p>
		 */
		public ISerie<long> PrimosHasta()
		{
			return CalculosEstatico.PrimosHasta(_raiz);
		}

		/**
		 * Devuelve el inverso multiplicativo de a módulo raiz del objeto {@code Calculos}
		 * <p>Equivalente a {@code Calculos.inversoMod(a,raiz)}
		 * @return a^-1
		 */
		public long InversoMod(long a)
		{
			return CalculosEstatico.InversoMod(a, _raiz);
		}

		/**
		 * Calcula la progresión aritmética que empieza en inicio, acaba en fin y tiene la misma cantidad de números que la raiz del objeto {@code Calculos}
		 * @param inicio primer número del sumatorio
		 * @param fin último número del sumatorio
		 * @return
		 */
		public long SumatorioIntervalo(long inicio, long fin)
		{
			return CalculosEstatico.SumatorioIntervalo(inicio, fin, _raiz);
		}

		/**
		 * Devuelve el mínimo de los absolutos de {@code un} y la raiz del objeto {@code Calculos}
		 * <p>Equivalente a {@code Calculos.minAbs(un,raiz)}</p>
		 * @return min{|{@code un}|,|{@code raiz}|}
		 */
		public long MinAbs(long un)
		{
			return CalculosEstatico.MinAbs(un, _raiz);
		}

		/**
		 * Calcula el número de cifras de {@code num} en la raiz del objeto {@code Calculos}
		 * @return número de cifras de {@code num} en la raiz del objeto
		 */
		public short Cifras(long num)
		{
			return CalculosEstatico.Cifras(num, _raiz);
		}

		/**
		 * Calcula el número de cifras de la raiz del objeto {@code Calculos} en raiz 10
		 * @return número de cifras del objeto en raiz 10
		 */
		public short Cifras10()
		{
			return CalculosEstatico.Cifras10(_raiz);
		}

		/**
		 * Calcula la cifra {@code pos} de {@code num} en la raiz del objeto {@code Calculo}
		 * @param pos cifra que calcular
		 * @return cifra {@code pos} de {@code num} en la raiz del objeto
		 */
		public byte Cifra(long num, long pos)
		{
			return CalculosEstatico.Cifra(num, pos, _raiz);
		}

		/**
		 * Devuelve un objeto {@code String} que contiene la raiz del objeto {@code Calculos} escrito con subíndices de sus cifras en raiz 10
		 * @return la raiz del objeto en subíndices
		 */
		public String NumASubindice()
		{
			return CalculosEstatico.NumASubindice(_raiz);
		}

		public char CifraASubindice() { return CalculosEstatico.CifraASubindice(Raiz); }

		/**
		 * Calcula {@code num}^{@code exp} en el módulo de la raiz del objeto {@code Calculos} multiplicando {@code num} por sí mismo y haciendo el módulo en cada paso
		 * @param num raiz de la potencia
		 * @param exp exponente
		 * @return @return {@code num}^{@code exp} en módulo {@code raiz}
		 */
		public long PotenciaMod(long num, long exp)
		{
			return CalculosEstatico.PotenciaMod(num, exp, _raiz);
		}

		public long ProductoMod(long fac1, long fac2)
		{
			return CalculosEstatico.ProductoMod(fac1, fac2, _raiz);
		}

		/**
		 * Calcula la regla de divisibilidad de {@code num} en raiz {@code raiz} con {@code cantidad} coeficiente
		 * tal que todos los coeficientes tienen el menor valor absoluto y la guarda en serie
		 * <p>{@code serie} se borra antes de crear la regla</p>
		 * @param serie serie donde guardar la regla
		 * @param cantidad número de coeficientes de la regla
		 */
		public void ReglaDivisibilidadOptima(ISerie<long> serie, long num, int cantidad)
		{
			CalculosEstatico.ReglaDivisibilidadOptima(serie, num, cantidad, _raiz);
		}

		public void ReglasDivisibilidad(ISerie<ISerie<long>> reglas, long num, int cantidad)
		{
			CalculosEstatico.ReglasDivisibilidad(reglas, num, cantidad, _raiz);
		}
	}
}
