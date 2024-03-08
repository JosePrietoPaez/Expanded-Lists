using System;
using System.Reflection;

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
		public static void Requires<TException>(bool predicate, string message = "", string parametro = "") where TException : Exception,new() {
			if (!predicate) {
				ConstructorInfo? constructor;
				if (new TException() is ArgumentOutOfRangeException) {
					throw new ArgumentOutOfRangeException(parametro,message);
				} else if (new TException() is ArgumentException) {
					throw new ArgumentException(message,parametro);
				}
				constructor = typeof(TException).GetConstructor([typeof(string)]);
				throw constructor?.Invoke([message]) as TException ?? throw new NotImplementedException("¿Qué clase de excepción no permite añadir mensaje?");
			}
		}
	}
}
