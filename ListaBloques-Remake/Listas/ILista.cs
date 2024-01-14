using System;
using System.Collections.Generic;

namespace Listas
{
	/// <summary>
	/// Las listas contienen elementos del tipo parámetro <c>T</c>, dependiendo de la implementación podrán permitir elementos nulos o no
	/// </summary>
	/// <typeparam name="T">
	/// </typeparam>
	public interface ILista<T> : IEnumerable<T>
	{
		/// <summary>
		/// Esta propiedad permite ver si la lista esta vacía al leerla y ponerla vacía si se escribe
		/// </summary>
		/// <remarks>
		/// En el caso de intentar operación <c>lista.Vacia = false</c>, se insertará a <c>InstanciaDeRespaldo</c> si la lista estaba vacía
		/// </remaks>
		/// <exception cref="InvalidOperationException"> si <see cref="FuncionDeGeneracion"/> es nula y la lista no permite elementos nulos</exception>
		bool Vacia { get; set; }

		/// <summary>
		/// Esta propiedad permite obtener la longitud de la lista al leerla y cambiarla al escribirla
		/// </summary>
		/// <remarks>
		/// En el caso de aumentar la longiud mediante esta propiedad, se rellenará la lista con referencias a <c>InstanciaDeRespaldo</c>
		/// <para>
		/// En el caso de reducirla se eliminarán los últimos elementos, la cantidad dependiendo del sustraendo
		/// </para>
		/// </remarks>
		int Longitud { get; set; }

		/// <summary>
		/// Esta propiedad representa a la función que genera nuevas instancias de <c>T</c> que se usarán para rellenar la lista
		/// en el caso de que se intente poner <c>Vacia</c> a <c>true</c> o <c>Longitud</c> a un valor mayor al actual
		/// </summary>
		/// <remarks>
		/// El argumento usado para la función será el tamaño de la lista
		/// <para>
		/// Por defecto genera la instancia por defecto de la clase, por ello se recomienda instanciarla en el constructor o mediante esta propiedad
		/// </para>
		/// </remarks>
		Func<int,T?> FuncionDeGeneracion { get; set; }

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
		/// Introduce <see cref="ILista{T}.FuncionDeGeneracion"/> en la lista
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalente a 
		/// <c>lista.Longitud++</c>
		/// </remarks>
		/// Necesita que <see cref="ILista{T}.FuncionDeGeneracion"/> no sea nula si la lista no admite elementos nulos
		/// <returns></returns>
		static ILista<T> operator ++(ILista<T> lista) {
			if (lista.FuncionDeGeneracion is T) {
				throw new InvalidOperationException("La instancia de respaldo del objeto es nula");
			}
			lista.Longitud++;
			return lista;
		}

		/// <summary>
		/// Elimina el último elemento de la lista 
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalente a 
		/// <c>lista.Longitud--</c>
		/// </remarks>
		/// Necesita que <see cref="ILista{T}.FuncionDeGeneracion"/> no sea nula si la lista no admite elementos nulos
		/// <returns></returns>
		static ILista<T> operator --(ILista<T> lista) {
			lista.Longitud--;
			return lista;
		}

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
		int Borrar(T elemento);

		/// <summary>
		/// Borra el elemento en la posición <c>posicion</c> de la lista y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento
		/// <para>La lista no puede estar vacía</para>
		/// </remarks>
		/// <param name="posicion">la posición del elemento que se borrará</param>
		/// <returns>El elemento borrado</returns>
		T Borrar(int posicion);

		/// <summary>
		/// Borra <c>num</c> elementos desde la posición <c>posicion</c>, o hasta que no haya más
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento
		/// </remarks>
		/// <param name="num">número de elementos que eliminar</param>
		/// <param name="posicion">posición donde empezar a borrar</param>
		/// <returns>Número de elementos borrados</returns>
		int BorrarVarios(int num, int posicion);

		/// <summary>
		/// Elimina todos los elementos de la lista, dejándola vacía
		/// </summary>
		void BorrarTodos();

		/// <summary>
		/// Borra todas las ocurrencias <c>elemento</c> de la lista y devuelve la cantidad de veces que estaba en la lista
		/// </summary>
		/// <param name="elemento">elemento que quitar</param>
		/// <returns>Número de veces que <c>elemento</c> estaba en la lista</returns>
		int Eliminar(T elemento);

		/// <summary>
		/// Devuelve el primer elemento de la lista, si tiene
		/// </summary>
		/// <returns>
		/// Primer elemento de la lista
		/// </returns>
		T PrimerElemento();

		/// <summary>
		/// Devuelve el elemento de la posición <c>posicion</c> de la lista
		/// </summary>
		/// <remarks>
		/// <c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento
		/// </remarks>
		/// <param name="posicion">la posición del elemento</param>
		/// <returns>El elemento en la posición <c>posicion</c></returns>
		T Elemento(int posicion);

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
		bool Pertenece(T elemento);

		/// <summary>
		/// Invierte el orden de los elementos de la lista
		/// </summary>
		void Invertir();
	}
}
