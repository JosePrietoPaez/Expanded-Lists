using ExpandedLists;
using NUnit.Framework;
using System;

namespace ExpandedLists.nUnitTests {
	[TestFixture]
	public class AListSerieTests {

		private readonly ListSequence<int> _listaConElementos = new();
		private readonly ListSequence<int?> _listaConElementosNula = new();

		[SetUp]
		public void SetUp() {
			_listaConElementos.IsEmpty = true;
			_listaConElementosNula.IsEmpty = true;
			for (int i = 0; i < 10; i++) {
				_listaConElementos.InsertLast(i);
				_listaConElementosNula.InsertLast(i);
			}
		}

		[Test]
		public void PonerInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			aListSerie.InsertFirst(
				elem);

			// Assert
			Assert.That(aListSerie.Count, Is.EqualTo(1));
		}

		[Test]
		public void PonerInicio_StateUnderTest_ListaConValoresNulos() {
			// Arrange
			var aListSerie = new ListSequence<int?>();
			int? elem = null;

			// Act
			aListSerie.InsertFirst(
				elem);

			// Assert
			Assert.That(aListSerie.Count, Is.EqualTo(1));
		}

		[Test]
		public void Poner_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);
			int pos = 0;

			// Act
			aListSerie.InsertAt(
				elem,
				pos);

			// Assert
			Assert.That(aListSerie.Count, Is.EqualTo(1));

			elem = 2;

			aListSerie.InsertAt(elem, pos);

			Assert.That(aListSerie[pos], Is.EqualTo(elem));

			elem = 3; pos = 1;

			aListSerie.InsertAt(elem,pos);

			Assert.That(aListSerie[pos], Is.EqualTo(elem));
		}

		[Test]
		public void PonerFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			aListSerie.InsertLast(
				elem);

			// Assert
			Assert.That(aListSerie.Count, Is.EqualTo(1));

			aListSerie.InsertLast(
				elem+1);

			Assert.That(aListSerie.Last, Is.EqualTo(elem+1));
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(-1)]
		public void PonerVarios_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = 45;
			int num = value;
			int pos = 1;

			// Act
			aListSerie.InsertFirst(123);

			aListSerie.InsertLast(678);

			aListSerie.InsertMultiple(
				elem,
				num,
				pos);

			// Assert
			Assert.That(aListSerie[0],Is.EqualTo(123)); //Asegurar que se mantienen el resto de elementos

			for (int i = 1; i < aListSerie.Count; i++) {
				if (i == aListSerie.Count-1) {
					Assert.That(aListSerie[i], Is.EqualTo(678));
				} else {
					Assert.That(aListSerie[i], Is.EqualTo(elem)); //Asegurar que se han colocado los elementos correctos
				}
			}

			Assert.That(aListSerie.Count, Is.EqualTo(2+Math.Max(0,num))); //Asegurar que la longitud es correcta
		}

		[Test]
		public void BorrarInicio_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>(_listaConElementos);

			// Act
			var elementoInicial = aListSerie.First;
			var result = aListSerie.RemoveFirst();

			// Assert
			Assert.That(elementoInicial, Is.EqualTo(result));
			Assert.That(aListSerie.Count, Is.EqualTo(_listaConElementos.Count-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<float>();
			float elem = 15.0f;

			// Act
			aListSerie.InsertMultiple(15.0f, 10, 0);

			var result = aListSerie.Remove(
				elem);

			// Assert
			Assert.That(result, Is.Not.EqualTo(-1));

			elem = 0;

			result = aListSerie.Remove(elem);

			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void Borrar_StateUnderTest_ExpectedBehavior1() {
			// Arrange
			var aListSerie = new ListSequence<int>(_listaConElementos);
			int pos = 0;

			// Act
			var result = aListSerie.RemoveAt(
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarFin_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.RemoveLast();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarVarios_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int num = 0;
			int pos = 0;

			// Act
			var result = aListSerie.RemoveMultiple(
				num,
				pos);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarUltimos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.RemoveLast(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void BorrarTodos_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			aListSerie.Clear();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void PrimerElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.First;

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Elemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int pos = 0;

			// Act
			var result = aListSerie[pos];

			// Assert
			Assert.Fail();
		}

		[Test]
		public void UltimoElemento_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.Last;

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Posicion_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Position(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Ocurrencias_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Appareances(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Pertenece_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
			int elem = default(int);

			// Act
			var result = aListSerie.Contains(
				elem);

			// Assert
			Assert.Fail();
		}

		[Test]
		public void ToString_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.ToString();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void GetEnumerator_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.GetEnumerator();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Equals_StateUnderTest_ExpectedBehavior() {
			// Arrange
			var aListSerie = new ListSequence<int>();
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
			var aListSerie = new ListSequence<int>();

			// Act
			var result = aListSerie.GetHashCode();

			// Assert
			Assert.Fail();
		}

		[Test]
		public void Longitud_StateUnderTest_ExpectedBehavior() {
			var aListSerie = new ListSequence<int?>();

			aListSerie.Count++;

			Assert.That(aListSerie.Count, Is.EqualTo(1));
		}

		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(0)]
		[TestCase(2)]
		public void Multiplicar_StateUnderTest_ExpectedBehavior(int value) {
			// Arrange
			var aListSerie = new ListSequence<int>(_listaConElementos);
			int indice = 0;

			// Act
			var result = aListSerie.Multiply(value);

			// Assert
			Assert.That(result.Count, Is.EqualTo(_listaConElementos.Count * Math.Abs(value)));
			if (value < 0) {
				_listaConElementos.Reverse(); //Para recorrer en la direccion correcta
			}
			foreach (var item in result) {
				Assert.That(item, Is.EqualTo(_listaConElementos
					[indice++%_listaConElementos.Count])); //Asegura que se recorra circularmente
			}
		}

		[TestCase(100)]
		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(0)]
		public void Demostracion_GeneracionDeElementos(int value) {
			static double funcion(int num) => Math.Pow(2, num);
			var serie = new ListSequence<double>(funcion);

			if (value < 0) {
				Assert.Throws<ArgumentOutOfRangeException>(() => serie.Count = value);
			} else {
				serie.Count = value;
				Assert.That(serie.Count, Is.EqualTo(value));

				for (int i = 0; i < value; i++) {
					Assert.That(serie[i], Is.EqualTo(funcion(i)));
				}
			}



		}
	}
}
