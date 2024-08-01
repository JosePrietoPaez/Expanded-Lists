using ExpandedLists.Blocks;
using NUnit.Framework.Legacy;

namespace ExpandedLists.nUnitTests {
	[TestFixture]
	public class ListBloquesTests {

		private static ArrayBlock<int> _bloqueVacio,
			_bloqueConMenosElementos,
			_bloqueConElementos;

		private ArrayBlock<int>[] _bloques;

		[SetUp]
		public void SetUp() {
			_bloqueConElementos = new int[10] { 1, 2, 3, 4, 0, 0, 0, 0, 0, 0 };
			_bloqueConMenosElementos = new int[5] { 1, 2, 3, 4, 5 };
			_bloqueVacio = new int[10];
			_bloqueVacio.Clear();
			_bloques = [_bloqueVacio, _bloqueConMenosElementos, _bloqueConElementos];
		}

		[Test]
		public void Borrar_Bloque_EnLaLista_BorraElBloque() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();

			// Act
			listBloques.Insert(_bloqueConElementos,0);
			listBloques.Insert(_bloqueVacio, 1);

			var result = listBloques.Remove(
				_bloqueConElementos);

			// Assert
			Assert.That(result, Is.EqualTo(0));
		}

		[Test]
		public void Borrar_Bloque_NoEnLaLista_NoBorraYDevuelveM1() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			int longitud;

			// Act
			listBloques.Insert(_bloqueConElementos, 0);
			listBloques.Insert(_bloqueVacio, 1);
			longitud = listBloques.Count;

			var result = listBloques.Remove(
				_bloqueConMenosElementos);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(-1));
				Assert.That(longitud, Is.EqualTo(listBloques.Count));
			});
		}

		
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		public void BorrarBloque_CasosDePrueba(int value) {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			int cantidad;
			for (int i = 0; i < _bloques.Length; i++) {
				listBloques.Insert(_bloques[i], i);
			}
			cantidad = listBloques.BlockCount;

			// Act
			listBloques.RemoveBlockAt(value);
			
			// Assert
			Assert.That(cantidad, Is.EqualTo(listBloques.BlockCount + 1));
		}

		[TestCase(-1)]
		[TestCase(10)]
		public void BorrarBloque_ValoresInvalidos_LanzaExcepcion(int value) {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			int cantidad;
			for (int i = 0; i < _bloques.Length; i++) {
				listBloques.Insert(_bloques[i], i);
			}

			cantidad = listBloques.BlockCount;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.RemoveBlockAt(value));
		}

		[Test]
		public void BorrarUltimo_ConElementos_DevuelveElElementoYLoBorraYPuedeBorrarElUltimoBloque() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insert(_bloqueConMenosElementos, 0);
			var lleno = bloque.IsFull;
			var cantidad = listBloques.BlockCount;
			var ultimo = bloque.Last;
			var longitudInicial = bloque.Count;

			// Act
			var result = listBloques.RemoveLast();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.BlockCount);
				Assert.That(ultimo, Is.EqualTo(result));
				Assert.That(listBloques.Count, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarUltimo_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.RemoveLast());
		}

		[Test]
		public void BorrarPrimero_ConElementos_DosBloques_DevuelveElElementoYLoBorraYPuedeBorrarElUltimoBloque() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insert(_bloqueConMenosElementos, 0);
			var lleno = bloque.IsFull;
			var cantidad = listBloques.BlockCount;
			var primero = bloque.First;
			var longitudInicial = listBloques.Count;

			// Act
			var result = listBloques.RemoveFirst();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.BlockCount);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Count, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_ConElementos_VariosBloques_HaceLoMismoQueConDos() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			var bloque = _bloqueConMenosElementos;
			listBloques.Insert(_bloqueConMenosElementos, 0);
			listBloques.Insert(_bloqueConElementos, 1);
			listBloques.InsertLast(123);
			var lleno = listBloques.GetBlock(listBloques.BlockCount - 1).IsFull;
			var cantidad = listBloques.BlockCount;
			var primero = bloque.First;
			var longitudInicial = listBloques.Count;

			// Act
			var result = listBloques.RemoveFirst();

			// Assert
			Assert.Multiple(() => {
				Assert.That(lleno ^ cantidad == listBloques.BlockCount);
				Assert.That(primero, Is.EqualTo(result));
				Assert.That(listBloques.Count, Is.EqualTo(longitudInicial - 1));
			});
		}

		[Test]
		public void BorrarPrimero_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();

			// Assert
			Assert.Throws<InvalidOperationException>(() => listBloques.RemoveFirst());
		}

		[Test]
		public void BorrarTodos_ConElementos_VaciaLaLista() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			listBloques.InsertLast(1);

			// Act
			listBloques.Clear();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.IsEmpty);
				Assert.That(listBloques.Count, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_SinElementos_VaciaLaLista() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();

			// Act
			listBloques.Clear();

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.IsEmpty);
				Assert.That(listBloques.Count, Is.EqualTo(0));
			});
		}

		[Test]
		public void BorrarTodos_Elemento_SinOcurrencias_NoBorraNadaYDevuelve0() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			int elemento = 1234;
			listBloques.Insert(_bloqueConElementos, 0);
			int longitud = listBloques.Count;

			// Act
			var result = listBloques.Clear(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Count, Is.EqualTo(longitud));
				Assert.That(result, Is.EqualTo(0));
			});
		}

		//Falla por Insertar(B,int)
		[Test]
		public void BorrarTodos_Elemento_ConOcurrencias_LasBorraYDevuelveLaCantidad() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			int elemento = 1;
			listBloques.Insert(_bloqueConElementos, 0);
			listBloques.Insert(_bloqueConMenosElementos, 1);
			int longitud = listBloques.Count;

			// Act
			var result = listBloques.Clear(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result + listBloques.Count, Is.EqualTo(longitud));
				Assert.That(result, Is.GreaterThan(0));
				Assert.That(listBloques.Contains(elemento), Is.False);
			});
		}

		[Test]
		public void BorrarUltimos_SinElElemento_NoBorraNadaYDevuelve0() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			int elemento = 123;
			listBloques.Insert(_bloqueConElementos, 0);
			listBloques.InsertLast(elemento);

			// Act
			var result = listBloques.RemoveLast(
				elemento);

			// Assert
			Assert.That(result, Is.EqualTo(1));
		}

		[Test]
		public void BorrarVariosBloques_MasBloquesEnLista_LosBorraYDevuelveLaCantidad() {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int num = 3;
			int posicion = 5;
			var bloque = listBloques.GetBlock(8);

			// Act
			var result = listBloques.RemoveMultipleBlocks(
				num,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(num));
				Assert.That(listBloques.Count, Is.EqualTo(longitud - 10 * num));
				Assert.That(listBloques.Position(bloque), Is.GreaterThan(-1));
			});
		}

		[Test]
		public void BorrarVariosBloques_SinSuficientesBloques_LosBorraYDevuelveLaCantidad() {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int num = 3;
			int posicion = 8;

			// Act
			var result = listBloques.RemoveMultipleBlocks(
				num,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.LessThan(num));
				Assert.That(listBloques.Count, Is.EqualTo(longitud - 10 * result));
			});
		}

		[Test]
		public void BuscarBloque_Posicion_Valida_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};
			int posicion = 50;

			// Act
			var result = listBloques.GetBlockContainingPosition(
				posicion);

			// Assert
			Assert.That(listBloques.GetBlock(result).Contains(listBloques[posicion]), Is.True);
		}

		[TestCase(-1)]
		[TestCase(1000)]
		public void BuscarBloque_Posicion_NoValida_DevuelveM1(int value) {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};
			int posicion = value;

			// Act
			var result = listBloques.GetBlockContainingPosition(
				posicion);

			// Assert
			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void BuscarBloque_Elemento_EnLaLista_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};
			int elemento = 15;

			// Act
			var result = listBloques.GetBlockContainingPosition(
				elemento);

			// Assert
			Assert.That(listBloques.GetBlock(result).Contains(elemento));
		}

		[Test]
		public void BuscarBloque_Elemento_NoEnLaLista_DevuelveLaPosicion() {
			// Arrange
			var listBloques = new BlockList<int?, ArrayBlock<int?>>(n => 10, n => 10) {
				Count = 100
			};
			int? elemento = 15;

			// Act
			var result = listBloques.GetBlockContainingElement(
				elemento);

			// Assert
			Assert.That(result, Is.EqualTo(-1));
		}

		[Test]
		public void Clonar_Vacia_DevuelveListaIgual() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();

			// Act
			var result = listBloques.CloneDynamicBlocks();

			Assert.Multiple(() => {
				// Assert
				Assert.That(ReferenceEquals(result, listBloques), Is.False);
				Assert.That(result, Is.EqualTo(listBloques));
			});
		}

		[Test]
		public void Clonar_ConElementos_DevuelveListaIgual() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};

			// Act
			var result = listBloques.CloneDynamicBlocks();

			Assert.Multiple(() => {
				// Assert
				Assert.That(ReferenceEquals(result, listBloques), Is.False);
				Assert.That(result, Is.EqualTo(listBloques));
			});
		}

		[Test]
		public void Constructor_ColeccionExtensoraGeneradora_GuardaLosElementosEnBloqueConLaCapacidadIndicada() {
			// Arrange
			List<int> elementos = [];
			for (int i = 0; i < 100; i++) {
				elementos.Add(i);
			}

			// Act
			var listBloques = new BlockList<int, ArrayBlock<int>>(elementos,n => 10, n => n);

			// Assert
			Assert.That(elementos, Is.EqualTo(listBloques));
			int posicionBloque = 0;
			foreach (Block<int> bloque in listBloques.GetBlockEnumerable()) {
				Assert.That(bloque.Capacity, Is.EqualTo(listBloques.ExtenderFunction(posicionBloque)));
			}
		}

		[Test]
		public void Contiene_ConElElemento_DevuelveTrue() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};
			int elemento = 101;

			// Act
			var result = listBloques.Contains(
				elemento);

			// Assert
			Assert.That(result, Is.False);
		}

		[Test]
		public void Contiene_SinElElemento_DevuelveFalse() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = 100
			};
			int elemento = 1;

			// Act
			var result = listBloques.Contains(
				elemento);

			// Assert
			Assert.That(result, Is.True);
		}

		[Test]
		public void Diferencia_QuitaLosElementosDeSegundaDeUnaNuevaLista() {
			// Arrange
			int lPrimera = 100, lSegunda = 75;
			BlockList<int,ArrayBlock<int>> primeraLista = new(n => 10, n => n) { Count = lPrimera},
				segundaLista = new(n => 10, n => 2*n) { Count = lSegunda};

			// Act
			var result = primeraLista.Difference(
				segundaLista);

			// Assert
			Assert.Multiple(() => {
				foreach (var item in result) {
					Assert.That(primeraLista.Contains(item));
					Assert.That(segundaLista.Contains(item), Is.False);
				}
				Assert.That(primeraLista.Count, Is.EqualTo(lPrimera));
				Assert.That(segundaLista.Count, Is.EqualTo(lSegunda));
			});
		}

		[Test]
		public void Eliminar_Posicion_EnLista_BorraElElementoYLoDevuelve() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			listBloques.Insert(_bloqueConElementos, 0);
			int posicion = new Random().Next(10), longitud = listBloques.Count;
			int elemento = listBloques[posicion];

			// Act
			var result = listBloques.RemoveAt(
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(elemento));
				Assert.That(longitud, Is.EqualTo(listBloques.Count + 1));
			});
		}

		[TestCase(-1)]
		[TestCase(100)]
		public void Eliminar_Posicion_NoEnLista_LanzaUnaExcepcion(int value) {
			// Arrange
			var listBloques = new BlockList<int?, ArrayBlock<int?>>();
			listBloques.Insert(new int?[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 0);
			int posicion = value, longitud = listBloques.Count;

			// Act
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.RemoveAt(posicion));
		}

		[Test]
		public void Eliminar_Elemento_EnLista_BorraElElementoYDevuelveSuPosicion() {
			// Arrange
			var listBloques = new BlockList<int?, ArrayBlock<int?>>();
			listBloques.Insert(new int?[5] {1,2,3,4,5}, 0);
			int posicion = 3, longitud = listBloques.Count;
			int? elemento = listBloques[posicion];

			// Act
			var result = listBloques.Remove(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(posicion));
				Assert.That(longitud, Is.EqualTo(listBloques.Count + 1));
				Assert.That(listBloques.Contains(elemento), Is.False);
			});
		}

		[Test]
		public void Eliminar_Elemento_NoEnLaLista_NoBorraNadaYDevuelveM1() {
			// Arrange
			var listBloques = new BlockList<int?, ArrayBlock<int?>>();
			int longitud = 5;
			listBloques.Insert(new int?[5] { 1, 2, 3, 4, 5 }, 0);
			int? elemento = -1;

			// Act
			var result = listBloques.Remove(elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result, Is.EqualTo(-1));
				Assert.That(longitud, Is.EqualTo(listBloques.Count));
			});
		}

		[Test]
		public void EliminarVarios_MitadDeLaLista_BorraLosElementosIndicadosYMantieneElResto() {
			// Arrange
			int longitud = 100;
			static int funcion(int n) => n;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, funcion) {
				Count = longitud
			};
			int num = 10;
			int posicion = 30;

			// Act
			var result = listBloques.RemoveMultiple(
				num,
				posicion);

			// Assert
			Assert.That(longitud, Is.EqualTo(listBloques.Count + num));
			for (int i = 0; i < posicion; i++) {
				Assert.That(listBloques[i], Is.EqualTo(funcion(i)));
			}
			for (int i = posicion; i < listBloques.Count; i++) {
				Assert.That(listBloques[i], Is.EqualTo(funcion(i + num)));
			}
		}

		[Test]
		public void GetBloque_PosicionValida_DevuelveElBloque() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			var bloque = _bloqueConElementos;
			listBloques.Insert(bloque, 0);
			int posicion = 0;

			// Act
			var result = listBloques.GetBlock(
				posicion);

			// Assert
			Assert.That(result, Is.EqualTo(bloque));
		}

		[Test]
		public void GetBloques_ConBloques_DevuelveLosBloquesDeLaLista() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>> (n => 10, n => n) {
				Count = 100
			};
			int posicion = 0;

			// Act
			var result = listBloques.GetBlockEnumerable();

			// Assert
			foreach (var bloque in result) {
				Assert.That(bloque, Is.EqualTo(listBloques.GetBlock(posicion++)));
			}
		}

		[Test]
		public void GetBloques_UnBloque_TieneUnBloque() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n);

			// Act
			var result = listBloques.GetBlockEnumerable();
			var enumerator = result.GetEnumerator();

			// Assert
			Assert.That(enumerator.MoveNext(), Is.True);
			Assert.That(enumerator.MoveNext(), Is.False);
		}

		[Test]
		public void GetEnumerator_ConElementos_ContieneLosElementosDeLaLista() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(N => 10, N => N) {
				Count = 100
			};
			int i = 0;

			// Act
			var result = listBloques.GetEnumerator();

			// Assert
			while (result.MoveNext()) {
				Assert.That(result.Current, Is.EqualTo(listBloques[i++]));
			}
		}

		[Test]
		public void GetEnumerator_SinElementos_NoContieneElementos() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(N => 10, N => N);

			// Act
			var result = listBloques.GetEnumerator();

			// Assert
			Assert.That(result.MoveNext(), Is.False);
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		public void Insertar_ElementoPosicion_Valida_InsertaElElemento(int value) {
			// Arrange
			int longitud = 10;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) { Count = longitud };
			int elemento = 123;
			int posicion = value;

			// Act
			listBloques.InsertAt(
				elemento,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques[posicion], Is.EqualTo(elemento));
				Assert.That(listBloques.Count, Is.EqualTo(longitud + 1));
			});
		}

		[TestCase(-1)]
		[TestCase(200)]
		public void Insertar_ElementoPosicion_NoValida_LanzaExcepcion(int value) {
			// Arrange
			int longitud = 10;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) { Count = longitud };
			int elemento = 123;
			int posicion = value;

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.InsertAt(elemento, posicion));
		}

		[Test]
		public void Insertar_Elemento_InsertaElElementoAlFinal() {
			// Arrange
			int longitud = 15;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int elemento = 123;

			// Act
			var result = listBloques.Add(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques.Count, Is.EqualTo(longitud + 1));
				Assert.That(listBloques.Last, Is.EqualTo(elemento));
			});
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaVacia_InsertaElBloque() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			ArrayBlock<int> bloque = _bloques[1];
			int posicion = 0;

			// Act
			listBloques.Insert(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.IsEmpty, Is.False);
				Assert.That(listBloques.BlockCount, Is.EqualTo(2));
				Assert.That(listBloques.Count, Is.EqualTo(_bloques[1].Count));
			});
		}

		[Test]
		public void Insertar_BloqueNoNulo_EnListaNoVacia_InsertaElBloque() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			ArrayBlock<int> bloque = _bloqueConMenosElementos;
			listBloques.Insert(_bloqueConElementos, 0);
			int posicion = 1, cantidad = listBloques.BlockCount, longitud = listBloques.Count;

			// Act
			listBloques.Insert(
				bloque,
				posicion);

			// Assert
			Assert.Multiple(() => {
				Assert.That(listBloques.IsEmpty, Is.False);
				Assert.That(listBloques.BlockCount, Is.EqualTo(cantidad + 1));
				Assert.That(listBloques.Count, Is.EqualTo(longitud + bloque.Count));
				Assert.DoesNotThrow(() => _ = listBloques[_bloqueConElementos.Count]);
			});
		}

		[Test]
		public void InsertarUltimo_InsertaElElementoAlFinal() {
			// Arrange
			int longitud = 15;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int elemento = 123;

			// Act
			listBloques.InsertLast(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques.Count, Is.EqualTo(longitud + 1));
				Assert.That(listBloques.Last, Is.EqualTo(elemento));
			});
		}

		[Test]
		public void InsertarPrimero_InsertaElElementoAlPrincipio() {
			// Arrange
			int longitud = 15;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int elemento = 123;

			// Act
			listBloques.InsertFirst(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques.Count, Is.EqualTo(longitud + 1));
				Assert.That(listBloques.First, Is.EqualTo(elemento));
			});
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(95)]
		public void InsertarVarios_PosicionValida_ColocaLosElementos(int value) {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int elemento = 2;
			int num = 15;
			int posicion = value;

			// Act
			listBloques.InsertMultiple(
				elemento,
				num,
				posicion);

			// Assert
			Assert.That(listBloques.Count, Is.EqualTo(longitud + num));
			for (int i = 0; i < posicion; i++) {
				Assert.That(listBloques[i], Is.EqualTo(i));
			}
			for (int i = posicion; i < posicion + num; i++) {
				Assert.That(listBloques[i], Is.EqualTo(elemento));
			}
			for (int i = posicion + num; i < listBloques.Count; i++) {
				Assert.That(listBloques[i], Is.EqualTo(i - num));
			}
		}

		[TestCase(0,1)]
		[TestCase(1,2)]
		[TestCase(3,7)]
		public void IntercambiarBloques_PosicionesValidas_CambianLosBloques(int primero, int segundo) {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			ArrayBlock<int> bloquePriemro = listBloques.GetBlock(primero),
				bloqueSegundo = listBloques.GetBlock(segundo);

			// Act
			listBloques.SwapBlock(
				primero,
				segundo);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
				Assert.That(listBloques.GetBlock(primero), Is.EqualTo(bloqueSegundo));
				Assert.That(listBloques.GetBlock(segundo), Is.EqualTo(bloquePriemro));
			});
		}

		[TestCase(-1, 1)]
		[TestCase(1, 100)]
		[TestCase(-1, 100)]
		public void IntercambiarBloques_PosicionesNoValidas_LanzaExcepcion(int primero, int segundo) {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.SwapBlock(primero, segundo));
		}

		[TestCase(100)]
		[TestCase(101)]
		public void Invertir_InvierteElOrdenDeLosElementos(int value) {
			// Arrange
			int longitud = value;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			var inversa = listBloques.CloneDynamicBlocks();

			// Act
			inversa.Reverse();

			// Assert
			Assert.That(Enumerable.Reverse(inversa), Is.EqualTo(listBloques));
		}

		[Test]
		public void Longitud_EntradaNegativa_LanzaExcepcion() {
			// Arrange
			static int funcion(int n) => (int)Math.Pow(n, 2);
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, funcion);
			int ejemplo = 1234;
			listBloques.InsertLast(ejemplo);

			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.Count = -1);
		}

		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(100)]
		public void Longitud_EntradaNoNegativa_CambiaALaLongitud(int value) {
			// Arrange
			static int funcion(int n) => (int)Math.Pow(n, 2);
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, funcion);
			int ejemplo = 1234;
			listBloques.InsertLast(ejemplo);
			listBloques.InsertLast(ejemplo);
			listBloques.InsertLast(ejemplo);
			var longitud = listBloques.Count;

			// Act
			listBloques.Count = value;

			// Assert
			Assert.That(listBloques.Count, Is.EqualTo(value));
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

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(10)]
		public void Multiplicar_Positivo_RepiteLosValores(int value) {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int factor = value;

			// Act
			var result = listBloques.Multiply(
				factor);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(longitud * factor));
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
			});
			for (int i = 0; i < listBloques.Count; i++) {
				Assert.That(result[i], Is.EqualTo(listBloques[i % longitud]));
			}
		}

		[Test]
		public void Multiplicar_Cero_VaciaLaLista() {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int factor = 0;

			// Act
			var result = listBloques.Multiply(
				factor);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.IsEmpty);
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
			});
		}

		[TestCase(-1)]
		[TestCase(-2)]
		[TestCase(-10)]
		public void Multiplicar_Negativo_RepiteLosValoresYLaInvierte(int value) {
			// Arrange
			int longitud = 100;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int factor = value;

			// Act
			var result = listBloques.Multiply(
				factor);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(Math.Abs(longitud * factor)));
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
			});
			result.Reverse();
			for (int i = 0; i < listBloques.Count; i++) {
				Assert.That(result[i], Is.EqualTo(listBloques[i % longitud]));
			}
		}

		[Test]
		public void Ocurrencias_ElementoRandomEnLista_DevuelveLasOcurrencias() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			int elemento = new Random().Next(10);
			for (int i = 0; i < elemento; i++) {
				listBloques.InsertLast(0);
				listBloques.InsertLast(1);
				listBloques.InsertLast(2);
				listBloques.InsertLast(3);
				listBloques.InsertLast(4);
				listBloques.InsertLast(5);
				listBloques.InsertLast(6);
				listBloques.InsertLast(7);
				listBloques.InsertLast(8);
				listBloques.InsertLast(9);
			}

			// Act
			var result = listBloques.Appareances(elemento);

			// Assert
			Assert.That(result, Has.Length.EqualTo(listBloques.Count(n => n == elemento)));
			Assert.That(result, Is.Ordered);
		}

		[Test]
		public void Posicion_Elemento_EnListaRandom_EncuentraLaPrimeraOcurrencia() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => new Random(1337).Next(n));
			int elemento = 5, longitud = 100;
			for (int i = 0; i < 10; i++) {
				listBloques.Count = longitud;
				Assert.That(listBloques.Count, Is.EqualTo(longitud));

				// Act
				var result = listBloques.Position(
					elemento);

				Assert.Multiple(() => {
					// Assert
					Assert.That(result, Is.LessThan(listBloques.Count));
					Assert.That(result == -1, Is.EqualTo(listBloques.Appareances(elemento).Length == 0));
					if (result != -1) {
						Assert.That(listBloques[result], Is.EqualTo(elemento));
					}
					for (int i = 0; i < result; i++) { // Debe ser la primera ocurrencia
						Assert.That(listBloques[i], Is.Not.EqualTo(elemento));
					}
				});
				listBloques.IsEmpty = true;
			}
			
		}

		[Test]
		public void Posicion_Bloque_EnLista_DevuelveLaPosicionDelBloque() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 5, n => n) {
				Count = 100
			};
			ArrayBlock<int> bloque;

			for (int i = 0; i < listBloques.BlockCount; i++) {
				bloque = listBloques.GetBlock(i);
				Assert.That(listBloques.Position(bloque), Is.EqualTo(i));
			}
		}

		[Test]
		public void Posicion_Bloque_NoEnLista_DevuelveM1() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 5, n => n) {
				Count = 100
			};
			ArrayBlock<int> bloque = _bloqueVacio;

			Assert.That(listBloques.Position(bloque), Is.EqualTo(-1));
		}

		[Test]
		public void PrimerElemento_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();

			// Act

			// Assert
			Assert.Throws<InvalidOperationException>(() => _ = listBloques.First); 
		}

		[Test]
		public void PrimerElemento_ConElementos_DevuelveElElemento() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();
			listBloques.Insert(_bloqueConElementos, 0);

			// Act
			var result = listBloques.First;

			// Assert
			Assert.That(result, Is.EqualTo(listBloques[0]));
		}

		[Test]
		public void Restar_Elemento_EnLista_BorraTodasLasOcurrenciasEnNuevaLista() {
			// Arrange
			static int funcion(int n) => n % 10;
			int elemento = 5, longitud = 100;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, funcion) {
				Count = longitud
			};
			int ocurrencias = listBloques.Appareances(elemento).Length;

			// Act
			var result = listBloques.Subtract(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(longitud - ocurrencias));
				Assert.That(result.Contains(elemento), Is.False);
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
			});
		}

		[Test]
		public void Restar_Bloque_BorraElBloqueEnNuevaLista() {
			// Arrange
			int longitud = 100, posicion = 5;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			ArrayBlock<int> bloque = listBloques.GetBlock(posicion);

			// Act
			var result = listBloques.Subtract(
				bloque);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.GetBlock(posicion), Is.Not.EqualTo(bloque));
				Assert.That(result.Count, Is.EqualTo(longitud - bloque.Count));
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
			});
		}

		[Test]
		public void SetBloque_PosicionValida_CambiaElBloquePorElNuevo() {
			// Arrange
			int longitud = 100, lBloque = 20, posicion = 3;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			ArrayBlock<int> bloque = new int[lBloque];
			for (int i = 0; i < 20; i++) {
				bloque[i] = i;
			}

			// Act
			var result = listBloques.SetBlock(
				bloque,
				posicion);

			Assert.Multiple(() => {
				// Assert
				Assert.That(listBloques.GetBlock(posicion), Is.Not.EqualTo(result));
				Assert.That(listBloques.Count, Is.EqualTo(longitud + lBloque - result.Count));
			});
		}

		[Test]
		public void SetBloque_PosicionNoValida_LanzaExcepcion() {
			// Arrange
			int longitud = 100, lBloque = 20, posicion = 15;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			ArrayBlock<int> bloque = new int[lBloque];
			
			// Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => listBloques.SetBlock(bloque, posicion));
		}

		[Test]
		public void Sumar_Elemento_InsertaElElementoEnNuevaLista() {
			// Arrange
			int longitud = 15;
			var listBloques = new BlockList<int,ArrayBlock<int>>(n => 10, n => n) {
				Count = longitud
			};
			int elemento = 123;

			// Act
			var result = listBloques.AddNew(
				elemento);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(longitud + 1));
				Assert.That(listBloques.Count, Is.EqualTo(longitud));
				Assert.That(result.Contains(elemento));
			});
		}

		[Test]
		public void Sumar_Bloque_InsertaElBloqueEnNuevaLista() {
			// Arrange
			int longLista = 15, longBloque = 10;
			var listBloques = new BlockList<int, ArrayBlock<int>>(n => 10, n => n) {
				Count = longLista
			};
			ArrayBlock<int> bloque = new int[longBloque];

			// Act
			var result = listBloques.Add(
				bloque);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(longLista + longBloque));
				Assert.That(result.GetBlockEnumerable(), Does.Contain(bloque));
				Assert.That(listBloques.Count, Is.EqualTo(longLista));
			});
		}

		[Test]
		public void UltimoElemento_ConElementos_DevuelveElElemento() {
			// Arrange
			var listBloques = new BlockList<int,ArrayBlock<int>>();
			listBloques.Insert(_bloqueConElementos, 0);
			int valor = 100;

			// Act
			var result = listBloques.Last;

			// Assert
			Assert.That(result, Is.EqualTo(listBloques[listBloques.Count - 1]));
		}

		[Test]
		public void UltimoElemento_SinElementos_LanzaUnaExcepcion() {
			// Arrange
			var listBloques = new BlockList<int, ArrayBlock<int>>();

			// Act

			// Assert
			Assert.Throws<InvalidOperationException>(() => _ = listBloques.Last);
		}

		[Test]
		public void Unir_StateUnderTest_ExpectedBehavior() {
			// Arrange
			int lPrimera = 100, lSegunda = 75;
			BlockList<int,ArrayBlock<int>> primeraLista = new (n => 10, n => n) { Count = lPrimera},
				segundaLista = new(n => 15, n => n * n + n) { Count = lSegunda};

			// Act
			var result = primeraLista.Join(
				segundaLista);

			Assert.Multiple(() => {
				// Assert
				Assert.That(result.Count, Is.EqualTo(lPrimera + lSegunda));
				Assert.That(primeraLista.Count, Is.EqualTo(lPrimera));
				Assert.That(segundaLista.Count, Is.EqualTo(lSegunda));
			});
		}
	}
}
