using Listas;
using NUnit.Framework;
using System;

namespace Listas.nUnitTests {
	[TestFixture]
	public class AListSerieTests {

		private readonly ArrayListSerie<int> _listaConElementos = new();
		private readonly ArrayListSerie<int?> _listaConElementosNula = new();

		[SetUp]
		public void SetUp() {
			_listaConElementos.Vacia = true;
			_listaConElementosNula.Vacia = true;
			for (int i = 0; i < 10; i++) {
				_listaConElementos.PonerFin(i);
				_listaConElementosNula.PonerFin(i);
			}
		}

		[Test]
		public void PonerInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int elem = default(int);

			// Act
			aListSerie.PonerInicio(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[Test]
		public void PonerInicio_StateUnderTest_ListaConValoresNulos() {
			// Arrange
			var aListSerie = new ArrayListSerie<int?>();
			int? elem = null;

			// Act
			aListSerie.PonerInicio(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[Test]
		public void Poner_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int elem = default(int);
			int pos = 0;

			// Act
			aListSerie.Poner(
				elem,
				pos);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));

			elem = 2;

			aListSerie.Poner(elem, pos);

			Assert.That(aListSerie.Elemento(pos), Is.EqualTo(elem));

			elem = 3; pos = 1;

			aListSerie.Poner(elem,pos);

			Assert.That(aListSerie.Elemento(pos), Is.EqualTo(elem));
		}

		[Test]
		public void PonerFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int elem = default(int);

			// Act
			aListSerie.PonerFin(
				elem);

			// Assert
			Assert.That(aListSerie.Longitud, Is.EqualTo(1));

			aListSerie.PonerFin(
				elem+1);

			Assert.That(aListSerie.UltimoElemento(), Is.EqualTo(elem+1));
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(-1)]
		public void PonerVarios_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int elem = 45;
			int num = value;
			int pos = 1;

			// Act
			aListSerie.PonerInicio(123);

			aListSerie.PonerFin(678);

			aListSerie.PonerVarios(
				elem,
				num,
				pos);

			// Assert
			Assert.That(aListSerie.Elemento(0),Is.EqualTo(123)); //Asegurar que se mantienen el resto de elementos

			for (int i = 1; i < aListSerie.Longitud; i++) {
				if (i == aListSerie.Longitud-1) {
					Assert.That(aListSerie.Elemento(i), Is.EqualTo(678));
				} else {
					Assert.That(aListSerie.Elemento(i), Is.EqualTo(elem)); //Asegurar que se han colocado los elementos correctos
				}
			}

			Assert.That(aListSerie.Longitud, Is.EqualTo(2+Math.Max(0,num))); //Asegurar que la longitud es correcta
		}

		[Test]
		public void Cambiar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int pos = 0;
			int elem = default(int);

			// Act
			var result = aListSerie.Cambiar(
				pos,
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>(_listaConElementos);

			// Act
			var elementoInicial = aListSerie.PrimerElemento();
			var result = aListSerie.BorrarInicio();

			// Assert
			Assert.That(elementoInicial, Is.EqualTo(result));
			Assert.That(aListSerie.Longitud, Is.EqualTo(_listaConElementos.Longitud-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<float>();
			float elem = 15.0f;

			// Act
			aListSerie.PonerVarios(15.0f, 10, 0);

			var result = aListSerie.Borrar(
				elem);

			// Assert
			Assert.That(result, Is.Not.EqualTo(-1));

			elem = 0;

			result = aListSerie.Borrar(elem);

			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>(_listaConElementos);
			int pos = 0;

			// Act
			var result = aListSerie.Borrar(
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.BorrarFin();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int num = 0;
			int pos = 0;

			// Act
			var result = aListSerie.BorrarVarios(
				num,
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarUltimos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
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
			var aListSerie = new ArrayListSerie<int>();

			// Act
			aListSerie.BorrarTodos();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PrimerElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.PrimerElemento();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Elemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
			int pos = 0;

			// Act
			var result = aListSerie.Elemento(
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void UltimoElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.UltimoElemento();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Posicion_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
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
			var aListSerie = new ArrayListSerie<int>();
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
			var aListSerie = new ArrayListSerie<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Pertenece(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ToString_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.ToString();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetEnumerator_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.GetEnumerator();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Equals_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ArrayListSerie<int>();
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
			var aListSerie = new ArrayListSerie<int>();

			// Act
			var result = aListSerie.GetHashCode();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Longitud_StateUnderTest_ExpectedBehavior() {
			var aListSerie = new ArrayListSerie<int?>();

			aListSerie.Longitud++;

			Assert.That(aListSerie.Longitud, Is.EqualTo(1));
		}

		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(0)]
		[TestCase(2)]
		public void Multiplicar_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ArrayListSerie<int>(_listaConElementos);
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
	}
}
