using Listas;
using Listas.Bloques;
using NUnit.Framework;
using System;

namespace Listas.nUnitTests {
	[TestFixture]
	public class ListBloquesTests {

		private static ArrayBloque<int> _bloqueVacio,
			_bloqueConMenosElementos,
			_bloqueConElementos;

		private ArrayBloque<int>[] _bloques;

		[SetUp]
		public void SetUp() {
			_bloqueConElementos = new int[10] { 1, 2, 3, 4, 0, 0, 0, 0, 0, 0 };
			_bloqueConMenosElementos = new int[5] { 1, 2, 3, 4, 5 };
			_bloqueVacio = new int[10];
			_bloques = [_bloqueVacio, _bloqueConMenosElementos, _bloqueConElementos];
		}

		[Test]
		public void Borrar_BloqueEnLaLista() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.Insertar(_bloqueConElementos,0);
			listBloques.Insertar(_bloqueVacio, 1);

			var result = listBloques.Borrar(
				_bloqueConElementos);

			// Assert
			Assert.That(result, Is.EqualTo(0));
		}

		[Test]
		public void Borrar_BloqueNoEnLaLista() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Act
			listBloques.Insertar(_bloqueConElementos, 0);
			listBloques.Insertar(_bloqueVacio, 1);

			var result = listBloques.Borrar(
				_bloqueConMenosElementos);

			// Assert
			Assert.That(result, Is.EqualTo(-1));
		}

		[TestCase(-1)]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(10)]
		public void BorrarBloque_CasosDePrueba(int value) {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int cantidad;

			// Act
			for (int i = 0; i < _bloques.Length; i++) {
				listBloques.Insertar(_bloques[i], i);
			}

			cantidad = listBloques.CantidadBloques;

			// Assert
			if (value < 0 || value > listBloques.CantidadBloques) {
				Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.BorrarBloque(value));
			} else {
				listBloques.BorrarBloque(value);
				Assert.That(cantidad, Is.EqualTo(listBloques.CantidadBloques + 1));
			}
		}

		[Test]
		public void BorrarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.BorrarFin();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.BorrarInicio();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarTodos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.BorrarTodos();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarTodos_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.BorrarTodos(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarTodosBloques_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.BorrarTodosBloques();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarUltimos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.BorrarUltimos(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarVariosBloques_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int num = 0;
			int posicion = 0;

			// Act
			var result = listBloques.BorrarVariosBloques(
				num,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BuscarBloque_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int posicion = 0;

			// Act
			var result = listBloques.BuscarBloque(
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BuscarBloque_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.BuscarBloque(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Clonar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.Clonar();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Contiene_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Contiene(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Diferencia_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ILista<int> lista = null;

			// Act
			var result = listBloques.Diferencia(
				lista);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Eliminar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Eliminar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Eliminar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int posicion = 0;

			// Act
			var result = listBloques.Eliminar(
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void EliminarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int num = 0;
			int posicion = 0;

			// Act
			var result = listBloques.EliminarVarios(
				num,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetBloque_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int posicion = 0;

			// Act
			var result = listBloques.GetBloque(
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetBloques_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.GetBloques();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetEnumerator_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.GetEnumerator();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);
			int posicion = 0;

			// Act
			listBloques.Insertar(
				elemento,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Insertar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_StateUnderTest_ExpectedBehavior2() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;
			int posicion = 0;

			// Act
			listBloques.Insertar(
				bloque,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InsertarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			listBloques.InsertarFin(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InsertarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			listBloques.InsertarInicio(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void InsertarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);
			int num = 0;
			int posicion = 0;

			// Act
			listBloques.InsertarVarios(
				elemento,
				num,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void IntercambiarBloques_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int primero = 0;
			int segundo = 0;

			// Act
			listBloques.IntercambiarBloques(
				primero,
				segundo);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Invertir_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			listBloques.Invertir();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Multiplicar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int factor = 0;

			// Act
			var result = listBloques.Multiplicar(
				factor);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Ocurrencias_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Ocurrencias(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Posicion_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Posicion(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Posicion_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;

			// Act
			var result = listBloques.Posicion(
				bloque);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PrimerElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.PrimerElemento();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Restar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Restar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Restar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;

			// Act
			var result = listBloques.Restar(
				bloque);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void SetBloque_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;
			int posicion = 0;

			// Act
			var result = listBloques.SetBloque(
				bloque,
				posicion);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Sumar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default(int);

			// Act
			var result = listBloques.Sumar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Sumar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = null;

			// Act
			var result = listBloques.Sumar(
				bloque);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void UltimoElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();

			// Act
			var result = listBloques.UltimoElemento();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Unir_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ILista<int> segunda = null;

			// Act
			var result = listBloques.Unir(
				segunda);

			// Assert
			Assert.Fail();
		}
	}
}
