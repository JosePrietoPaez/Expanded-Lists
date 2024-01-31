namespace Listas.Bloques {

	/// <summary>
	/// Las listas de bloques guardan sus elementos en bloques, estos bloques pueden ser obtenidos desde su lista,
	/// <see cref="E"/> es el tipo de los elementos, <see cref="B"/> es el tipo de los bloques
	/// </summary>
	/// <typeparam name="E"/>
	/// <typeparam name="B">
	/// </typeparam>
	public interface IListaBloques<E,B> : ILista<E> where B : Bloque<E> {

		/// <summary>
		/// Esta propiedad permite obtener la cantidad de bloques que se están usando
		/// </summary>
		/// <remarks>
		/// No se podrán acceder a bloques con índices no menores que este
		/// </remarks>
		int CantidadBloques { get; }

		/// <summary>
		/// Devuelve la lista con <c>bloque</c> insertado
		/// </summary>
		/// <remarks>
		/// Equivalente a lista.<see cref="IListaBloques{T, U}.Sumar(U)"/>
		/// </remarks>
		/// <param name="lista"></param>
		/// <param name="bloque"></param>
		/// <returns>
		/// Nueva lista con sus bloques y <c>bloque</c>
		/// </returns>
		static IListaBloques<E, B> operator +(IListaBloques<E, B> lista, B bloque) => lista.Sumar(bloque);

		/// <summary>
		/// Devuelve la lista con <c>bloque</c> borrado
		/// </summary>
		/// <remarks>
		/// Equivalente a lista.<see cref="IListaBloques{T, U}.Restar(U)"/>
		/// </remarks>
		/// <param name="lista"></param>
		/// <param name="bloque"></param>
		/// <returns>
		/// Nueva lista con sus bloques sin <c>bloque</c>
		/// </returns>
		static IListaBloques<E, B> operator -(IListaBloques<E, B> lista, B bloque) => lista.Restar(bloque);

		/// <summary>
		/// Obtiene el bloque en <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> debe ser no negativo y menor que <see cref="IListaBloques{T, U}.CantidadBloques"/>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns>Bloque en la posición indicada</returns>
		B GetBloque(int posicion);

		/// <summary>
		/// Coloca el bloque en la lista
		/// </summary>
		/// <param name="bloque"></param>
		/// <returns>La posición en la que se ha insertado</returns>
		int Insertar(B bloque);

		/// <summary>
		/// Borra el bloque de la lista si está
		/// </summary>
		/// <param name="bloque"></param>
		/// <returns>La posición en la que estaba o -1 si no estaba</returns>
		int Borrar(B bloque);

		/// <summary>
		/// Borra el bloque indicado
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> debe ser no negativo y menor que la cantidad de bloques
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns>El bloque eliminado</returns>
		B BorrarBloque(int posicion);

		/// <summary>
		/// Borra <c>num</c> bloques desde <c>posicion</c>, o hasta que no haya más
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

		/// <summary>
		/// Devuelve la posición del bloque que contiene el elemento en <c>posicion</c>
		/// </summary>
		/// <returns>
		/// Bloque que contiene a pos o -1 si no está en la lista
		/// </returns>
		int BuscarBloque(int posicion);

		/// <summary>
		/// Devuelve la posición del primer bloque que contiene a un elemento igual a <c>elemento</c>
		/// </summary>
		/// <returns>
		/// Bloque que contiene a pos o -1 si no está en la lista
		/// </returns>
		int BuscarBLoque(E elemento);

		/// <summary>
		/// Devuelve una lista de bloques como la llamada, con <c>bloque</c>
		/// </summary>
		IListaBloques<E,B> Sumar(B bloque);

		/// <summary>
		/// Devuelve una lista de bloques como la llamada, sin <c>bloque</c>
		/// </summary>
		IListaBloques<E,B> Restar(B bloque);

		/// <summary>
		/// Crea una lista nueva igual a la llamada, la lista será del mismo tipo
		/// </summary>
		/// <remarks>
		/// Las interfaces que extiendan de <see cref="ILista{T}"/> deberían sobrescribir este método
		/// </remarks>
		/// <returns>
		/// Lista igual a la llamada
		/// </returns>
		new IListaBloques<E, B> Clonar();

		IEnumerable<B> GetBloques();
	}
}
