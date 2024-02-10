using System;
using System.Diagnostics;

namespace Listas {
	/// <summary>
	/// Resulta que CodeContracts está muerto, así que, para no volver a poner todos los ifs he creado esta clase
	/// </summary>
	public static class Contrato {
		
		/// <summary>
		/// Lanza una excepción si <c>predicate</c> no es <c>true</c>
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="predicate"></param>
		/// <param name="message"></param>
		/// <exception cref="TException"></exception>
		public static void Requires<TException>(bool predicate, string message = "") where TException : Exception,new() {
			if (!predicate) {
				Debug.Write(message);
				throw new TException();
			}
		}
	}
}
