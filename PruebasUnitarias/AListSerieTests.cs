using Listas;
using Operaciones;

namespace PruebasUnitarias
{
	[TestClass]
	public class AListSerieTests
	{
		AListSerie<int> vaciaNoNulos,valoresNoNulos;
		AListSerie<int?> vaciaNulos, valoresNulos;

		readonly AListSerie<int>[] listasNoNulas = new AListSerie<int>[2];
		readonly AListSerie<int?>[] listasNulas = new AListSerie<int?>[2];

		static List<int> listaConElementosNoNulos;
		static List<int?> listaConElementosNulos;

		[ClassInitialize]
		public static void ALSTInicializar()
		{
			listaConElementosNoNulos = [];
			for (int i = 0; i < 15; i++)
			{
				listaConElementosNoNulos.Add(i);
				listaConElementosNulos.Add(i);
			}
		}

		[TestInitialize]
		public void ReiniciarListas()
		{
			listasNoNulas[0] = vaciaNoNulos;
			listasNoNulas[1] = valoresNoNulos;
			listasNulas[0] = vaciaNulos;
			listasNulas[1] = valoresNulos;

			vaciaNoNulos = new AListSerie<int>();
			vaciaNulos = new AListSerie<int?>();
			valoresNoNulos = new AListSerie<int>(listaConElementosNoNulos);
			valoresNulos = new AListSerie<int?>(listaConElementosNulos);
		}

		[TestMethod]
		public void PonerInicio_ValorNuloTipoNoNulo_ListaVacia()
		{
			// Arrange
			int elem = default;

			// Act
			vaciaNoNulos.PonerInicio(elem);

			// Assert
			Assert.AreEqual(vaciaNoNulos.Longitud,1);
		}

		[TestMethod]
		public void PonerInicio_ValorNuloEnTipoNulo_ListaVacia()
		{
			// Arrange
			vaciaNulos = new AListSerie<int?>();

			// Act
			vaciaNulos.PonerInicio(null);

			// Assert
			Assert.AreEqual(valoresNulos.Longitud, 1);
		}

		[TestMethod]
		public void PonerInicio_ValorNuloTipoNoNulo_ListaConValores()
		{
			// Arrange
			int elem = default,cantidadInicial = valoresNoNulos.Longitud;

			// Act
			valoresNoNulos.PonerInicio(elem);

			// Assert
			Assert.AreEqual(valoresNoNulos.Longitud, cantidadInicial+1);
		}

		[TestMethod]
		public void PonerInicio_ValorNuloEnTipoNulo_ListaConValores()
		{
			// Arrange
			int? elem = default, cantidadInicial = valoresNulos.Longitud;

			// Act
			valoresNulos.PonerInicio(elem);

			// Assert
			Assert.AreEqual(valoresNulos.Longitud, cantidadInicial+1);
		}

		[TestMethod]
		[DataRow(0)]
		[DataRow(1)]
		[DataRow(14)]
		[DataRow(15)]
		public void Poner_ValoresEsperadosEnTodas(int value)
		{
			// Arrange
			
			int elem = default(int);

			foreach (var lista in listasNoNulas)
			{
				if (value >= lista.Longitud)
				{
					Assert.ThrowsException<ArgumentOutOfRangeException>(() => lista.Poner(elem,value));
				}
			}

			foreach (var lista in listasNulas)
			{
				if (value >= lista.Longitud)
				{
					Assert.ThrowsException<ArgumentOutOfRangeException>(() => lista.Poner(elem, value));
				}
			}
		}

		[TestMethod]
		public void PonerFin_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			vaciaNoNulos.PonerFin(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void PonerVarios_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);
			int num = 0;
			int pos = 0;

			// Act
			vaciaNoNulos.PonerVarios(
				elem,
				num,
				pos);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Cambiar_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int pos = 0;
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.Cambiar(
				pos,
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void BorrarInicio_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.BorrarInicio();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Borrar_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.Borrar(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Borrar_StateUnderTest_ExpectedBehavior1()
		{
			// Arrange
			
			int pos = 0;

			// Act
			var result = vaciaNoNulos.Borrar(
				pos);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void BorrarFin_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.BorrarFin();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void BorrarVarios_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int num = 0;
			int pos = 0;

			// Act
			var result = vaciaNoNulos.BorrarVarios(
				num,
				pos);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void BorrarUltimos_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.BorrarUltimos(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void BorrarTodos_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			vaciaNoNulos.BorrarTodos();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void PrimerElemento_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.PrimerElemento();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Elemento_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int pos = 0;

			// Act
			var result = vaciaNoNulos.Elemento(
				pos);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void UltimoElemento_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.UltimoElemento();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Posicion_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.Posicion(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Ocurrencias_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.Ocurrencias(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Pertenece_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			int elem = default(int);

			// Act
			var result = vaciaNoNulos.Pertenece(
				elem);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void ToSringCompletoInverso_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.ToStringCompletoInverso();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void ToString_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.ToString();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void GetEnumerator_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.GetEnumerator();

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Equals_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			
			Object? obj = null;

			// Act
			var result = vaciaNoNulos.Equals(
				obj);

			// Assert
			Assert.Fail();
		}

		[TestMethod]
		public void GetHashCode_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			

			// Act
			var result = vaciaNoNulos.GetHashCode();

			// Assert
			Assert.Fail();
		}
	}
}