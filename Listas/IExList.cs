using System;
using System.Collections.Generic;

namespace ExpandedLists
{
	/// <summary>
	/// Las listas contienen elementos del tipo parámetro <c>T</c>, dependiendo de la implementación podrán permitir elementos nulos o no
	/// </summary>
	/// <remarks>
	/// Todas las listas de esta biblioteca implementan esta interfaz
	/// <para>
	/// Las otras subinterfaces ofrecen más métodos y propiedades
	/// </para>
	/// </remarks>
	/// <typeparam name="T">
	/// </typeparam>
	public interface IExList<T> : IEnumerable<T>
	{

		static bool CompatibleEnLista(T? obj) => obj is not null || (obj == null && default(T) == null);

		/// <summary>
		/// Esta propiedad permite ver si la list esta vacía
		/// </summary>
		bool IsEmpty { get;}

		/// <summary>
		/// Esta propiedad permite obtener la longitud de la list
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		int Count { get;}

		/// <summary>
		/// Obtiene el element en la posición <c>position</c>
		/// </summary>
		/// <remarks>
		/// <para>Es de solo lectura</para>
		/// Equivalente a 
		/// <see cref="IExList{T}.Elemento(int)"/>
		/// </remarks>
		/// <param name="position"></param>
		/// <returns></returns>
		T this[int position] { get; }

		/// <summary>
		/// Inserta el element en la list
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IExList{T}.Add(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>list</c> con <c>element</c>
		/// </returns>
		static IExList<T> operator +(IExList<T> list, T element) => list.AddNew(element);

		/// <summary>
		/// Inserta el element en la list
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IExList{T}.Join(IExList{T})"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>primera</c> y <c>segunda</c>
		/// </returns>
		static IExList<T> operator +(IExList<T> first, IExList<T> second) => first.Join(second);

		/// <summary>
		/// Crea una list con los elementos de <c>list</c> sin <c>element</c>
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IExList{T}.Substract(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>list</c> sin <c>element</c>
		/// </returns>
		static IExList<T> operator -(IExList<T> list, T element) => list.Substract(element);

		/// <summary>
		/// Crea una list con los elementos de <c>list</c> sin <c>element</c>
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="IExList{T}.Diferencia(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>list</c> sin <c>element</c>
		/// </returns>
		static IExList<T> operator -(IExList<T> minuend, IExList<T> subtrahend) => minuend.Difference(subtrahend);

		/// <summary>
		/// Introduce <c>element</c> en la list
		/// </summary>
		/// <param name = "element">element que colocar</param>
		/// <returns>
		/// Devuelve la posición en la que se ha insertado
		/// </returns>
		int Add(T element);

		/// <summary>
		/// Borra la primera ocurrencia de <c>element</c> de la list y devuelve su posición como resultado
		/// </summary>
		/// <param name="element">element que quitar</param>
		/// <returns>la primera posición de element o <c>-1</c> si no está en la list</returns>
		int Remove(T element);

		/// <summary>
		/// Borra el element en la posición <c>position</c> de la list y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// <c>position</c> no puede ser menor que 0 o mayor que el índice del último element
		/// <para>La list no puede estar vacía</para>
		/// </remarks>
		/// <param name="position">la posición del element que se borrará</param>
		/// <returns>El element borrado</returns>
		T Remove(int position);

		/// <summary>
		/// Borra <c>num</c> elementos desde la posición <c>position</c>, o hasta que no haya más
		/// </summary>
		/// <remarks>
		/// <c>position</c> no puede ser menor que 0 o mayor que el índice del último element
		/// <para>
		/// Si <c>num</c> no es positivo, no se modificará la list
		/// </para>
		/// </remarks>
		/// <param name="amount">número de elementos que eliminar</param>
		/// <param name="position">posición donde empezar a borrar</param>
		/// <returns>Número de elementos borrados</returns>
		int RemoveMultiple(int amount, int position);

		/// <summary>
		/// Borra el primer element de la list y lo devuelve como resultado
		/// </summary>
		/// <returns>El primer element de la list</returns>
		T RemoveFirst();

		/// <summary>
		/// Borra el último element de la list y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// La list no puede estar vacía
		/// </remarks>
		/// <returns>El último element de la list</returns>
		T RemoveLast();

		/// <summary>
		/// Elimina todos los elementos de la list, dejándola vacía
		/// </summary>
		void Clear();

		/// <summary>
		/// Borra todas las ocurrencias de <c>element</c> de la list y devuelve la cantidad de veces que estaba en la list
		/// </summary>
		/// <param name="element">element que quitar</param>
		/// <returns>Número de veces que <c>element</c> estaba en la list</returns>
		int Clear(T element);

		/// <summary>
		/// Esta propiedad permite leer directamente el primer element de la list
		/// </summary>
		/// <returns>
		/// Primer element de la list
		/// </returns>
		T First { get; }

		/// <summary>
		/// Esta propiedad permite leer directamente el último element de la list
		/// </summary>
		/// <returns>
		/// Último element de la list
		/// </returns>
		T Last { get; }

		/// <summary>
		/// Devuelve la posición de la primera ocurrencia de <c>element</c> en la list
		/// </summary>
		/// <returns>
		/// Posición de <c>element</c> en la list o -1 si no existe
		/// </returns>
		int Position(T element);

		/// <summary>
		/// Devuelve las posiciones de todas las ocurrencias de <c>element</c> en la list de menor a mayor
		/// </summary>
		/// <param name="element">element que contar</param>
		/// <returns>Array con los indices que contienen a <c>element</c></returns>
		int[] Appareances(T element);

		/// <summary>
		/// Indica si <c>element</c> está en la list
		/// </summary>
		/// <param name="element">element que buscar</param>
		/// <returns><c>true</c> si <c>element</c> se encuentra en la list o <c>false</c> si no está</returns>
		bool Contains(T element);

		/// <summary>
		/// Invierte el orden de los elementos de la list
		/// </summary>
		void Reverse();

		/// <summary>
		/// Crea una list igual a la invocada e inserta el element en la list
		/// </summary>
		/// <remarks>
		/// La posición depende de la implementación
		/// <para>Para saber en que posición se encuentra al insertarla use
		/// <see cref="IExList{T}.AddNew(T)"/>
		/// </para>
		/// </remarks>
		/// <returns>
		/// Nueva list con los elementos anteriores y <c>element</c>
		/// </returns>
		IExList<T> AddNew(T element);

		/// <summary>
		/// Crea una list con los elementos de la list llamada y la pasada como argumento
		/// </summary>
		/// <remarks>
		/// La posición de los elementos depende de la implementación
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de las dos listas
		/// </returns>
		IExList<T> Join(IExList<T> segunda);

		/// <summary>
		/// Crea una list nueva igual a la llamada, la list será del mismo tipo
		/// </summary>
		/// <remarks>
		/// Las interfaces que extiendan de <see cref="IExList{T}"/> deberían sobrescribir este método
		/// </remarks>
		/// <returns>
		/// Lista igual a la llamada
		/// </returns>
		IExList<T> Clone();

		/// <summary>
		/// Crea una list igual a la invocada y elimina el element en la list
		/// </summary>
		/// <remarks>Para saber cuantas veces se encontraba en la list use
		/// <see cref="IExList{T}.Clear(T)"/> en su lugar
		/// </remarks>
		/// <returns>
		/// Nueva list con los elementos anteriores sin <c>element</c>
		/// </returns>
		IExList<T> Substract(T element);

		/// <summary>
		/// Crea una list igual a la invocada y elimina los elementos de la list pasada por argumento
		/// </summary>
		/// <returns>
		/// Nueva list con los elementos anteriores sin los de <c>list</c>
		/// </returns>
		IExList<T> Difference(IExList<T> list);
	}
}
