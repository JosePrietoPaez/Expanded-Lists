using System;

namespace Listas {
	public static class Mensajes {
		public static string RangoLista(int indice, int limite) {
			return $"La posición indicada no es válida({indice}, con longitud {limite})";
		}

		public static string VacioBloque => "El bloque está vacío";

		public static string VacioLista => "La lista está vacía";

		public static string VacioSerie => "La serie está vacía";

		public static string GeneracionNula => "La función de generación ha creado un elemento nulo";

		public static string LongitudNegativa => "La longitud no puede ser negativa";

		public static string ArgumentoNegativo => "Se ha pasado un argumento negativo";

		public static string BloqueNoLleno => "Se ha pasado un bloque no lleno como parámetro";

		public static string BloqueNulo => "Se ha pasado un bloque nulo";
	}
}
