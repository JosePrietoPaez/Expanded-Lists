using System;
using System.Collections.Generic;

namespace Listas
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
	public interface ILista<T> : IEnumerable<T>
	{

		static bool CompatibleEnLista(T? obj) => obj is not null || (obj == null && default(T) == null);

		/// <summary>
		/// Esta propiedad permite ver si la lista esta vacía
		/// </summary>
		bool Vacia { get;}

		/// <summary>
		/// Esta propiedad permite obtener la longitud de la lista
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		int Longitud { get;}

		/// <summary>
		/// Obtiene el elemento en la posición <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// <para>Es de solo lectura</para>
		/// Equivalente a 
		/// <see cref="ILista{T}.Elemento(int)"/>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns></returns>
		T this[int posicion] { get; }

		/// <summary>
		/// Inserta el elemento en la lista
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="ILista{T}.Sumar(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>lista</c> con <c>elemento</c>
		/// </returns>
		static ILista<T> operator +(ILista<T> lista, T elemento) => lista.Sumar(elemento);

		/// <summary>
		/// Inserta el elemento en la lista
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="ILista{T}.Unir(ILista{T})"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>primera</c> y <c>segunda</c>
		/// </returns>
		static ILista<T> operator +(ILista<T> primera, ILista<T> segunda) => primera.Unir(segunda);

		/// <summary>
		/// Crea una lista con los elementos de <c>lista</c> sin <c>elemento</c>
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="ILista{T}.Restar(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>lista</c> sin <c>elemento</c>
		/// </returns>
		static ILista<T> operator -(ILista<T> lista, T elemento) => lista.Restar(elemento);

		/// <summary>
		/// Crea una lista con los elementos de <c>lista</c> sin <c>elemento</c>
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="ILista{T}.Diferencia(T)"/>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de <c>lista</c> sin <c>elemento</c>
		/// </returns>
		static ILista<T> operator -(ILista<T> primera, ILista<T> segunda) => primera.Diferencia(segunda);

		/// <summary>
		/// Introduce <c>elemento</c> en la lista
		/// </summary>
		/// <param name = "elemento">elemento que colocar</param>
		/// <returns>
		/// Devuelve la posición en la que se ha insertado
		/// </returns>
		int Insertar(T elemento);

		/// <summary>
		/// Borra la primera ocurrencia de <c>elemento</c> de la lista y devuelve su posición como resultado
		/// </summary>
		/// <param name="elemento">elemento que quitar</param>
		/// <returns>la primera posición de elemento o <c>-1</c> si no está en la lista</returns>
		int Eliminar(T elemento);

		/// <summary>
		/// Borra el elemento en la posición <c>posicion</c> de la lista y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento
		/// <para>La lista no puede estar vacía</para>
		/// </remarks>
		/// <param name="posicion">la posición del elemento que se borrará</param>
		/// <returns>El elemento borrado</returns>
		T Eliminar(int posicion);

		/// <summary>
		/// Borra <c>num</c> elementos desde la posición <c>posicion</c>, o hasta que no haya más
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento
		/// </remarks>
		/// <param name="num">número de elementos que eliminar</param>
		/// <param name="posicion">posición donde empezar a borrar</param>
		/// <returns>Número de elementos borrados</returns>
		int EliminarVarios(int num, int posicion);

		/// <summary>
		/// Elimina todos los elementos de la lista, dejándola vacía
		/// </summary>
		void BorrarTodos();

		/// <summary>
		/// Borra todas las ocurrencias de <c>elemento</c> de la lista y devuelve la cantidad de veces que estaba en la lista
		/// </summary>
		/// <param name="elemento">elemento que quitar</param>
		/// <returns>Número de veces que <c>elemento</c> estaba en la lista</returns>
		int BorrarTodos(T elemento);

		/// <summary>
		/// Devuelve el primer elemento de la lista, si tiene
		/// </summary>
		/// <returns>
		/// Primer elemento de la lista
		/// </returns>
		T PrimerElemento();

		/// <summary>
		/// Devuelve el último elemento de la lista, si hay
		/// </summary>
		/// <returns>
		/// Último elemento de la lista
		/// </returns>
		T UltimoElemento();

		/// <summary>
		/// Devuelve la posición de la primera ocurrencia de <c>elemento</c> en la lista
		/// </summary>
		/// <returns>
		/// Posición de <c>elemento</c> en la lista o -1 si no existe
		/// </returns>
		int Posicion(T elemento);

		/// <summary>
		/// Devuelve el número de veces que está <c>elemento</c> en la lista
		/// </summary>
		/// <param name="elemento">elemento que contar</param>
		/// <returns>veces que aparece <c>elemento</c></returns>
		int Ocurrencias(T elemento);

		/// <summary>
		/// Indica si <c>elemento</c> está en la lista
		/// </summary>
		/// <param name="elemento">elemento que buscar</param>
		/// <returns><c>true</c> si <c>elemento</c> se encuentra en la lista o <c>false</c> si no está</returns>
		bool Contiene(T elemento);

		/// <summary>
		/// Invierte el orden de los elementos de la lista
		/// </summary>
		void Invertir();

		/// <summary>
		/// Crea una lista igual a la invocada e inserta el elemento en la lista
		/// </summary>
		/// <remarks>
		/// La posición depende de la implementación
		/// <para>Para saber en que posición se encuentra al insertarla use
		/// <see cref="ILista{T}.Insertar(T)"/>
		/// </para>
		/// </remarks>
		/// <returns>
		/// Nueva lista con los elementos anteriores y <c>elemento</c>
		/// </returns>
		ILista<T> Sumar(T elemento);

		/// <summary>
		/// Crea una lista con los elementos de la lista llamada y la pasada como argumento
		/// </summary>
		/// <remarks>
		/// La posición de los elementos depende de la implementación
		/// <para>Para saber en que posición se encuentra al insertarla use
		/// <see cref="ILista{T}.Insertar(T)"/> en su lugar
		/// </para>
		/// </remarks>
		/// <returns>
		/// Lista con los elementos de las dos listas
		/// </returns>
		ILista<T> Unir(ILista<T> segunda);

		/// <summary>
		/// Crea una lista nueva igual a la llamada, la lista será del mismo tipo
		/// </summary>
		/// <remarks>
		/// Las interfaces que extiendan de <see cref="ILista{T}"/> deberían sobrescribir este método
		/// </remarks>
		/// <returns>
		/// Lista igual a la llamada
		/// </returns>
		ILista<T> Clonar();

		/// <summary>
		/// Crea una lista igual a la invocada y elimina el elemento en la lista
		/// </summary>
		/// <remarks>Para saber cuantas veces se encontraba en la lista use
		/// <see cref="ILista{T}.BorrarTodos(T)"/> en su lugar
		/// </remarks>
		/// <returns>
		/// Nueva lista con los elementos anteriores sin <c>elemento</c>
		/// </returns>
		ILista<T> Restar(T elemento);

		/// <summary>
		/// Crea una lista igual a la invocada y elimina los elementos de la lista pasada por argumento
		/// </summary>
		/// <returns>
		/// Nueva lista con los elementos anteriores sin los de <c>lista</c>
		/// </returns>
		ILista<T> Diferencia(ILista<T> lista);
	}
}
