using System;
using System.Reflection;

namespace ExpandedLists {
	/// <summary>
	/// Because CodeContracts is dead, I made this class to replace it
	/// </summary>
	public static class Contract {
		
		/// <summary>
		/// Throws an exception of the given type if <c>predicate</c> is not <c>true</c>
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
