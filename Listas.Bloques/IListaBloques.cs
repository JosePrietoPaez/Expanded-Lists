namespace Listas.Bloques {

	/// <summary>
	/// Las listas de bloques guardan sus elementos en bloques, estos bloques pueden ser obtenidos desde su lista
	/// </summary>
	/// <remarks>
	/// Esta interfaz no obliga a que la lista cree bloques si lo necesita, permitiendo que se quede sin espacio
	/// </remarks>
	/// <typeparam name="T">
	/// </typeparam>
	public interface IListaBloques<T,U> : ILista<T> where U : IBloque<T> {

		/// <summary>
		/// Esta propiedad permite obtener la cantidad de bloques que se están usando
		/// </summary>
		int CantidadBloques { get; }

		/// <summary>
		/// Obtiene el bloque en <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> debe ser no negativo y menor que <see cref="IListaBloques{T, U}.CantidadBloques"/>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns>Bloque en la posición indicada</returns>
		U GetBloque(int posicion);

		/// <summary>
		/// Coloca el bloque en la lista
		/// </summary>
		/// <param name="bloque"></param>
		/// <returns>La posición en la que se ha insertado</returns>
		int Insertar(U bloque);

		/// <summary>
		/// Borra el bloque de la lista si está
		/// </summary>
		/// <param name="bloque"></param>
		/// <returns>La posición en la que estaba o -1 si no estaba</returns>
		int Borrar(U bloque);

		/// <summary>
		/// Borra el bloque indicado
		/// </summary>
		/// <remarks>
		/// <c>posicion debe ser no negativo y</c>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns>El bloque eliminado</returns>
		U BorrarBloque(int posicion);

		/// <summary>
		/// Borra <c>num</c> bloques desde la posición <c>posicion</c>, o hasta que no haya más
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último bloque
		/// </remarks>
		/// <param name="num">número de bloques que eliminar</param>
		/// <param name="posicion">posición donde empezar a borrar</param>
		/// <returns>Número de elementos borrados</returns>
		int BorrarVariosBloques(int num, int posicion);

		/// <summary>
		/// Elimina todos los bloques con sus elementos de la lista, dejándola vacía
		/// </summary>
		void BorrarTodosBloques();

	}
}
