using Listas;
using NUnit.Framework;
using System;

namespace Listas.nUnitTests {
	[TestFixture]
	public class AListSerieTests {

		private readonly ListSerie<int> _listaConElementos = new();
		private readonly ListSerie<int?> _listaConElementosNula = new();

		[SetUp]
		public void SetUp() {
			_listaConElementos.Vacia = true;
			_listaConElementosNula.Vacia = true;
			for (int i = 0; i < 10; i++) {
				_listaConElementos.InsertarUltimo(i);
				_listaConElementosNula.InsertarUltimo(i);
			}
		}

		[Test]
		public void PonerInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			aListSerie.InsertarPrimero(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[Test]
		public void PonerInicio_StateUnderTest_ListaConValoresNulos() {
			// Arrange
			var aListSerie = new ListSerie<int?>();
			int? elem = null;

			// Act
			aListSerie.InsertarPrimero(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[Test]
		public void Poner_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);
			int pos = 0;

			// Act
			aListSerie.Insertar(
				elem,
				pos);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));

			elem = 2;

			aListSerie.Insertar(elem, pos);

			Assert.That(aListSerie[pos], Is.EqualTo(elem));

			elem = 3; pos = 1;

			aListSerie.Insertar(elem,pos);

			Assert.That(aListSerie[pos], Is.EqualTo(elem));
		}

		[Test]
		public void PonerFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			aListSerie.InsertarUltimo(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));

			aListSerie.InsertarUltimo(
				elem+1);

			Assert.That(aListSerie.UltimoElemento, Is.EqualTo(elem+1));
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(-1)]
		public void PonerVarios_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = 45;
			int num = value;
			int pos = 1;

			// Act
			aListSerie.InsertarPrimero(123);

			aListSerie.InsertarUltimo(678);

			aListSerie.InsertarVarios(
				elem,
				num,
				pos);

			// Assert
			Assert.That(aListSerie[0],Is.EqualTo(123)); //Asegurar que se mantienen el resto de elementos

			for (int i = 1; i < aListSerie.Longitud; i++) {
				if (i == aListSerie.Longitud-1) {
					Assert.That(aListSerie[i], Is.EqualTo(678));
				} else {
					Assert.That(aListSerie[i], Is.EqualTo(elem)); //Asegurar que se han colocado los elementos correctos
				}
			}

			Assert.That(aListSerie.Longitud, Is.EqualTo(2+Math.Max(0,num))); //Asegurar que la longitud es correcta
		}

		[Test]
		public void BorrarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>(_listaConElementos);

			// Act
			var elementoInicial = aListSerie.PrimerElemento;
			var result = aListSerie.BorrarPrimero();

			// Assert
			Assert.That(elementoInicial, Is.EqualTo(result));
			Assert.That(aListSerie.Longitud, Is.EqualTo(_listaConElementos.Longitud-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<float>();
			float elem = 15.0f;

			// Act
			aListSerie.InsertarVarios(15.0f, 10, 0);

			var result = aListSerie.Eliminar(
				elem);

			// Assert
			Assert.That(result, Is.Not.EqualTo(-1));

			elem = 0;

			result = aListSerie.Eliminar(elem);

			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var aListSerie = new ListSerie<int>(_listaConElementos);
			int pos = 0;

			// Act
			var result = aListSerie.Eliminar(
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.BorrarUltimo();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int num = 0;
			int pos = 0;

			// Act
			var result = aListSerie.EliminarVarios(
				num,
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarUltimos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.BorrarUltimos(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarTodos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			aListSerie.BorrarTodos();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PrimerElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.PrimerElemento;

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Elemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int pos = 0;

			// Act
			var result = aListSerie[pos];

			// Assert
			Assert.Fail();
		}

		[Test]
		public void UltimoElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.UltimoElemento;

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Posicion_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Posicion(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Ocurrencias_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Ocurrencias(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Pertenece_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Contiene(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ToString_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.ToString();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetEnumerator_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.GetEnumerator();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Equals_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();
			Object? obj = null;

			// Act
			var result = aListSerie.Equals(
				obj);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetHashCode_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSerie<int>();

			// Act
			var result = aListSerie.GetHashCode();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Longitud_StateUnderTest_ExpectedBehavior() {
			var aListSerie = new ListSerie<int?>();

			aListSerie.Longitud++;

			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(0)]
		[TestCase(2)]
		public void Multiplicar_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ListSerie<int>(_listaConElementos);
			int indice = 0;

			// Act
			var result = aListSerie.Multiplicar(value);

			// Assert
			Assert.That(result.Longitud, Is.EqualTo(_listaConElementos.Longitud * Math.Abs(value)));
			if (value < 0) {
				_listaConElementos.Invertir(); //Para recorrer en la direccion correcta
			}
			foreach (var item in result) {
				Assert.That(item, Is.EqualTo(_listaConElementos
					[indice++%_listaConElementos.Longitud])); //Asegura que se recorra circularmente
			}
		}

		[TestCase(100)]
		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(0)]
		public void Demostracion_GeneracionDeElementos(int value) {
			static double funcion(int num) => Math.Pow(2, num);
			var serie = new ListSerie<double>(funcion);

			if (value < 0) {
				Assert.Throws<ArgumentOutOfRangeException>(() => serie.Longitud = value);
			} else {
				serie.Longitud = value;
				Assert.That(serie.Longitud, Is.EqualTo(value));

				for (int i = 0; i < value; i++) {
					Assert.That(serie[i], Is.EqualTo(funcion(i)));
				}
			}



		}
	}
}
