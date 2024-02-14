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
			_bloqueVacio.BorrarTodos();
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
		public void BorrarUltimo_ConElementos() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			var lleno = bloque.Lleno;
			var cantidad = listBloques.CantidadBloques;
			var ultimo = bloque.UltimoElemento;
			var longitudInicial = bloque.Longitud;

			// Act
			var result = listBloques.BorrarUltimo();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(ultimo, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarUltimo_SinElementos() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.BorrarUltimo());
		}

		[Test]
		public void BorrarPrimero_ConElementos_UnBloque() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			var lleno = bloque.Lleno;
			var cantidad = listBloques.CantidadBloques;
			var primero = bloque.PrimerElemento;
			var longitudInicial = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarPrimero();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_ConElementos_VariosBloques() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			var bloque = _bloqueConMenosElementos;

			// Act
			listBloques.Insertar(_bloqueConMenosElementos, 0);
			listBloques.Insertar(_bloqueConElementos, 1);
			listBloques.InsertarUltimo(123);
			var lleno = listBloques.GetBloque(listBloques.CantidadBloques - 1).Lleno;
			var cantidad = listBloques.CantidadBloques;
			var primero = bloque.PrimerElemento;
			var longitudInicial = listBloques.Longitud;
			var result = listBloques.BorrarPrimero();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.CantidadBloques);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_SinElementos() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.BorrarPrimero());
		}

		[Test]
		public void BorrarTodos_ConElementos() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			listBloques.InsertarUltimo(1);

			// Act
			listBloques.BorrarTodos();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia);
				Assert.That(listBloques.Longitud, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_SinElementos() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();

			// Act
			listBloques.BorrarTodos();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia);
				Assert.That(listBloques.Longitud, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_Elemento_SinOcurrencias() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = 1234;
			listBloques.Insertar(_bloqueConElementos, 0);
			int longitud = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarTodos(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Longitud, Is.EqualTo(longitud));
				Assert.That(result, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_Elemento_ConOcurrencias() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			int elemento = 1;
			listBloques.Insertar(_bloqueConElementos, 0);
			listBloques.Insertar(_bloqueConMenosElementos, 1);
			int longitud = listBloques.Longitud;

			// Act
			var result = listBloques.BorrarTodos(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Longitud, Is.EqualTo(longitud));
				Assert.That(result, Is.GreaterThan(0));
			});
		}

		//Falla por Borrar(int)
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
			int elemento = default;

			// Act
			var result = listBloques.Insertar(
				elemento);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaVacia() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			ArrayBloque<int> bloque = _bloques[1];
			int posicion = 0;

			// Act
			listBloques.Insertar(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia, Is.False);
				Assert.That(listBloques.CantidadBloques, Is.EqualTo(2));
				Assert.That(listBloques.Longitud, Is.EqualTo(_bloques[1].Longitud));
			});
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaNoVacia() {
			// Arrange
			var listBloques = new ListBloques<int, ArrayBloque<int>>();
			ArrayBloque<int> bloque = _bloqueConMenosElementos;
			listBloques.Insertar(_bloqueConElementos, 0);
			int posicion = 1, cantidad = listBloques.CantidadBloques, longitud = listBloques.Longitud;

			// Act
			listBloques.Insertar(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.Vacia, Is.False);
				Assert.That(listBloques.CantidadBloques, Is.EqualTo(cantidad + 1));
				Assert.That(listBloques.Longitud, Is.EqualTo(longitud + bloque.Longitud));
			});
		}

		[Test]
		public void InsertarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var listBloques = new ListBloques<int,ArrayBloque<int>>();
			int elemento = default;

			// Act
			listBloques.InsertarUltimo(
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
			listBloques.InsertarPrimero(
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

		[TestCase(-1)]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(100)]
		public void Longitud_CasosCambioDeValor(int value) {
			// Arrange
			static int funcion(int n) => (int)Math.Pow(n, 2);
			var listBloques = new ListBloques<int, ArrayBloque<int>>(n => 10, funcion);
			int ejemplo = 1234;
			listBloques.InsertarUltimo(ejemplo);
			listBloques.InsertarUltimo(ejemplo);
			listBloques.InsertarUltimo(ejemplo);
			var longitud = listBloques.Longitud;

			// Act
			if (value < 0) {
				Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.Longitud = value);
			} else {
				listBloques.Longitud = value;

				// Assert
				Assert.That(listBloques.Longitud, Is.EqualTo(value));
				if (value <= longitud) {
					foreach (var item in listBloques) {
						Assert.That(item, Is.EqualTo(ejemplo));
					}
				} else {
					for (int i = 0; i < value; i++) {
						Assert.That(listBloques[i], Is.EqualTo(ejemplo).Or.EqualTo(funcion(i)));
					}
				}
			}
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
			var result = listBloques.PrimerElemento;

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
			var result = listBloques.UltimoElemento;

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
