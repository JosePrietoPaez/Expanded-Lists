using System;
using System.Collections.Generic;

namespace Listas
{
	/// <summary>
	/// Las listas contienen elementos del tipo parámetro <c>T</c>, pueden contener elementos nulos
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ILista<T> : IEnumerable<T>
	{
		/// <summary>
		/// Esta propiedad permite ver si la lista esta vacía al leerla y ponerla vacía si se escribe
		/// </summary>
		/// <remarks>
		/// En el caso de intentar operación <c>lista.Vacia = false</c>, se insertará a <c>InstanciaDeRespaldo</c> si la lista estaba vacía
		/// </remaks>
		/// <exception cref="InvalidOperationException"> si <see cref="InstanciaDeRespaldo"/> es nula</exception>
		bool Vacia { get; set; }

		/// <summary>
		/// Esta propiedad permite obtener la longitud de la lista al leerla y cambiarla al escribirla
		/// </summary>
		/// <remarks>
		/// En el caso de aumentar la longiud mediante esta propiedad, se rellenará la lista con referencias a <c>InstanciaDeRespaldo</c>
		/// </remarks>
		int Longitud { get; set; }

		/// <summary>
		/// Esta propiedad representa a la instancia de <c>T</c> que se usará para rellenar la lista en el caso de que se intente poner <c>Vacia</c> a <c>true</c> o <c>Longitud</c> a un valor mayor al actual
		/// </summary>
		T? InstanciaDeRespaldo { get; set; }

		/// <summary>
		/// Obtiene el elemento en la posición <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// Equivalente a 
		/// <see cref="ILista{T}.Elemento(int)"/>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns></returns>
		T this[int posicion] { get; set; }

		/// <summary>
		/// Pone <see cref="ILista{T}.InstanciaDeRespaldo"/> al final de la lista 
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// Equivalente a 
		/// <c>lista.PonerFin(lista.Longitud++)</c>
		/// </remarks>
		/// Necesita que <see cref="ILista{T}.InstanciaDeRespaldo"/> no sea nula si la lista no admite elementos nulos
		/// <returns></returns>
		static ILista<T> operator ++(ILista<T> lista) {
			if (lista.InstanciaDeRespaldo is T) {
				throw new InvalidOperationException("La instancia de respaldo del objeto es nula");
			}
			lista.Longitud++; return lista;
		}

		/// <summary>
		/// Coloca <c>elemento</c> al principio de la lista
		/// </summary>
		/// <param name = "elemento">elemento que colocar</param>
		void PonerInicio(T elemento);

		/// <summary>
		/// Coloca a <c>elemento</c> en la posición posicion
		/// </summary>
		/// <remarks><c>posicion</c> no puede ser menor que 0 o mayor que el índice del último elemento</remarks>
		/// <param name="elemento">elemento que colocar</param>
		/// <param name="posicion">posición donde colocar <c>elemento</c></param>
		void Poner(T elemento, int posicion);

		/// <summary>
		/// Coloca <c>elemento</c> al final de la lista
		/// </summary>
		/// <param name="elemento">el elemento que colocar</param>
		void PonerFin(T elemento);

		/// <summary>
		/// Coloca el elemento <c>elemento num</c> veces seguidas en la lista, desde la posición <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// <c>posicion} no puede ser menor que 0 o mayor que el índice del último elemento
		/// <para>
		/// Si <c>num</c> no es positivo, el método no hará nada
		/// </para>
		/// </remarks>
		/// <param name="elemento">elemento el elemento que colocar</param>
		/// <param name="num">num la cantidad de veces que se pondrá</param>
		/// <param name="posicion">posición por la que se enpieza a poner</param>
		void PonerVarios(T elemento, int num, int posicion);

		/// <summary>
		/// Cambia el elemento de la posición <c>posicion</c> por <c>elemento</c>
		/// </summary>
		/// <param name="posicion">posición donde colocar <c>elemento</c></param>
		/// <param name="elemento">elemento que colocar</param>
		/// <returns>Elemento que estaba en la posición <c>posicion</c></returns>
		T Cambiar(int posicion, T elemento);

		/// <summary>
		/// Borra el primer elemento de la lista y lo devuelve como resultado
		/// </summary>
		/// <returns>El primer elemento de la lista</returns>
		T BorrarInicio();

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
		/// Borra el último elemento de la lista y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// La lista no puede estar vacía
		/// </remarks>
		/// <returns>El último elemento de la lista</returns>
		T BorrarFin();

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
		/// Borra las últimas ocurrencias de <c>elemento</c> hasta que llega al inicio o encuentra otro elemento
		/// </summary>
		/// <param name="elemento">elemento que eliminar del final</param>
		/// <remarks>
		/// Creado para eliminar los elementos insertados al alargar la lista con <see cref="ILista{T}.Longitud"/>
		/// </remarks>
		/// <returns>Ocurrencias de elemento quitadas</returns>
		int BorrarUltimos(T elemento);

		/// <summary>
		/// Elimina todos los elementos de la lista, dejándola vacía
		/// </summary>
		void BorrarTodos();

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

		
	}
}
