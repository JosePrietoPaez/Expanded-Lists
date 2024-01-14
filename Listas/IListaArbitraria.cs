using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Listas {
	/// <summary>
	/// <see cref="ILista{T}"/> cuyos elementos solo se identifican por su posición y no un orden
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IListaArbitraria<T> : ILista<T> {

		/// <summary>
		/// Obtiene el elemento en la posición <c>posicion</c>
		/// </summary>
		/// <remarks>
		/// <para>Permite lectura y escritura</para>
		/// Equivalente a 
		/// <see cref="ILista{T}.Elemento(int)"/>
		/// </remarks>
		/// <param name="posicion"></param>
		/// <returns></returns>
		new T this[int posicion] { get; set; }

		/// <summary>
		/// Devuelve una lista con los mismos elementos que <c>lista</c> repetidos <c>factor</c> veces
		/// </summary>
		/// <remarks>
		/// Los elementos se repiten una vez que se alcanza el último elemento
		/// <para>
		/// Equivalente a 
		/// <c>lista.Multiplicar(factor)</c>
		/// </para>
		/// Para más detalles véase <see cref="ILista{T}.Multiplicar(int)"/>
		/// </remarks>
		/// <returns>
		/// Nueva lista con los elementos repetidos
		/// </returns>
		static IListaArbitraria<T> operator *(IListaArbitraria<T> lista,int factor) {
			return lista.Multiplicar(factor);
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
		/// Borra el último elemento de la lista y lo devuelve como resultado
		/// </summary>
		/// <remarks>
		/// La lista no puede estar vacía
		/// </remarks>
		/// <returns>El último elemento de la lista</returns>
		T BorrarFin();

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
		/// Devuelve una lista con los mismos elementos que <c>lista</c> repetidos <c>factor</c> veces
		/// </summary>
		/// <remarks>
		/// Los elementos se repiten una vez que se alcanza el último elemento
		/// <para>
		/// Por ejemplo para <c>lista == [1,2,3]</c>, haciendo <c>lista.Multiplicar(2)</c> obtenemos <c>lista == [1,2,3,1,2,3]</c>
		/// </para>
		/// Multplicar por un número negativo realiza la misma operación, pero invirtiendo la lista
		/// <para>
		/// Multiplicar por 0 hace que se vacie la lista
		/// </para>
		/// </remarks>
		/// <returns>
		/// Nueva lista con los elementos repetidos
		/// </returns>
		IListaArbitraria<T> Multiplicar(int factor);
	}
}
