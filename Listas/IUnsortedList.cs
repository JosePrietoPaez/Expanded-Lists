namespace ExpandedLists {
	/// <summary>
	/// <see cref="IExList{T}"/> cuyos elementos solo se identifican por su posición y no un orden
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IUnsortedList<T> : IExList<T> {

		/// <summary>
		/// Obtiene el element en la posición <c>position</c>
		/// </summary>
		/// <remarks>
		/// <para>Permite lectura y escritura</para>
		/// Equivalente a 
		/// <see cref="IExList{T}.Elemento(int)"/>
		/// </remarks>
		/// <param name="position"></param>
		/// <returns></returns>
		new T this[int position] { get; set; }

		/// <summary>
		/// Devuelve una list con los mismos elementos que <c>list</c> repetidos <c>factor</c> veces
		/// </summary>
		/// <remarks>
		/// Los elementos se repiten una vez que se alcanza el último element
		/// <para>
		/// Para más detalles véase <see cref="IExList{T}.Multiplicar(int)"/>
		/// </para>
		/// </remarks>
		/// <returns>
		/// Nueva list con los elementos repetidos
		/// </returns>
		static IUnsortedList<T> operator *(IUnsortedList<T> list,int factor) {
			return list.Multiply(factor);
		}

		/// <summary>
		/// Inserta el element en la list
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IExList{T}.Add(T)"/>
		/// </remarks>
		/// Necesita que <see cref="IExList{T}.FuncionDeGeneracion"/> no sea nula si la list no admite elementos nulos
		/// <returns></returns>
		static IUnsortedList<T> operator +(IUnsortedList<T> list, (T, int) tuple) {
			IUnsortedList<T> nueva = list.CloneUnsorted();
			nueva.InsertAt(tuple.Item1, tuple.Item2);
			return nueva;
		}

		/// <summary>
		/// Coloca <c>element</c> al principio de la list
		/// </summary>
		/// <param name = "element">element que colocar</param>
		void InsertFirst(T element);

		/// <summary>
		/// Coloca a <c>element</c> en la posición <c>position</c>
		/// </summary>
		/// <remarks><c>position</c> no puede ser menor que 0 o mayor que el índice del último element</remarks>
		/// <param name="element">element que colocar</param>
		/// <param name="position">posición donde colocar <c>element</c></param>
		void InsertAt(T element, int position);

		/// <summary>
		/// Coloca <c>element</c> al final de la list
		/// </summary>
		/// <param name="element">el element que colocar</param>
		void InsertLast(T element);

		/// <summary>
		/// Coloca el element <c>element num</c> veces seguidas en la list, desde la posición <c>position</c>
		/// </summary>
		/// <remarks>
		/// <c>position</c> no puede ser menor que 0 o mayor que el índice del último element
		/// <para>
		/// Si <c>num</c> no es positivo, el método no hará nada
		/// </para>
		/// </remarks>
		/// <param name="element">element el element que colocar</param>
		/// <param name="num">num la cantidad de veces que se pondrá</param>
		/// <param name="position">posición por la que se empieza a poner</param>
		void InsertMultiple(T element, int num, int position);

		/// <summary>
		/// Borra las últimas ocurrencias de <c>element</c> hasta que llega al inicio o encuentra otro element
		/// </summary>
		/// <param name="element">element que eliminar del final</param>
		/// <remarks>
		/// Tiene más utilidad en <see cref="IDynamicList{T}"/>, ya que está pensado para deshacer cambios a <see cref="IDynamicList{T}.Count"/>
		/// </remarks>
		/// <returns>Ocurrencias de element quitadas</returns>
		int RemoveLast(T element);

		/// <summary>
		/// Devuelve una list con los mismos elementos que <c>list</c> repetidos <c>factor</c> veces
		/// </summary>
		/// <remarks>
		/// Los elementos se repiten una vez que se alcanza el último element
		/// <para>
		/// Por ejemplo para <c>list == [1,2,3]</c>, haciendo <c>list.Multiplicar(2)</c> obtenemos <c>list == [1,2,3,1,2,3]</c>
		/// </para>
		/// Multiplicar por un número negativo realiza la misma operación, pero invirtiendo la list
		/// <para>
		/// Multiplicar por 0 hace que se vacíe la list
		/// </para>
		/// </remarks>
		/// <returns>
		/// Nueva list con los elementos repetidos
		/// </returns>
		IUnsortedList<T> Multiply(int factor);

		/// <summary>
		/// Crea una list arbitraria nueva igual a la llamada, la list será del mismo tipo
		/// </summary>
		/// <remarks>
		/// Las interfaces que extiendan de <see cref="IExList{T}"/> deberían sobrescribir este método
		/// </remarks>
		/// <returns>
		/// Lista igual a la llamada
		/// </returns>
		IUnsortedList<T> CloneUnsorted();
	}
}
