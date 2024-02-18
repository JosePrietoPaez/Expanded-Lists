using System.Collections;

namespace Listas {
	/// <summary>
	/// Implementa la interfaz <see cref="ISerie{T}"/> delegando los métodos a una instancia de <see cref="List{T}"/> y una <see cref="Func{T, TResult}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListSerie<T> : ISerie<T>
	{
		private string _nombre;
		private List<T> _serie;
		private Func<int,T?> _generadora = num=>default;

		private T? Generar() {
			return _generadora.Invoke(_serie.Count);
		}

		/// <inheritdoc/>
		public string Nombre
		{
			get => _nombre;

			set => _nombre = value;
		}

		/// <inheritdoc/>
		public bool Vacia
		{
			get => _serie.Count == 0;
			set
			{
				if (value)
					_serie.Clear();
				else if (_serie.Count == 0)
				{
					Longitud++;
				}
			}
		}

		/// <inheritdoc/>
		public int Longitud
		{
			get => _serie.Count;
			set
			{
				Contrato.Requires<ArgumentOutOfRangeException>(value >= 0, Mensajes.LongitudNegativa);
				int siz = _serie.Count;
				if (value == 0) _serie.Clear();
				else if (value < siz)
				{
					_serie = new List<T>(_serie.GetRange(0, value));
				}
				else if (value != siz)
				{
					while (value > siz)
					{
						Contrato.Requires<InvalidOperationException>(ILista<T>.CompatibleEnLista(Generar()),
							Mensajes.GeneracionNula);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
						_serie.Add(Generar()); //Ignorar el warning
#pragma warning restore CS8604 // Posible argumento de referencia nulo
						siz++;
					}
				}
			}
		}

		/// <inheritdoc/>
		public Func<int,T?> FuncionDeGeneracion
		{
			get => _generadora;
			set => _generadora = value;
		}

		/// <inheritdoc/>
		public T this[int posicion]
		{
			get => _serie[posicion];
			set => _serie[posicion] = value;
		}

		/// <summary>
		/// Crea una serie a partir de otra lista con el nombre vacío
		/// </summary>
		/// <remarks>
		/// La serie tendrá todos los elementos de la lista y su función generadora
		/// </remarks>
		/// <param name="lista">lista que copiar</param>
		public ListSerie(IListaDinamica<T> lista) : this(lista as ILista<T>){
			_generadora = lista?.FuncionDeGeneracion ?? (num => default);
		}

		/// <summary>
		/// Crea una serie a partir de otra lista con el nombre vacío
		/// </summary>
		/// <remarks>
		/// La serie tendrá todos los elementos de la lista y su función generadora
		/// </remarks>
		/// <param name="lista">lista que copiar</param>
		public ListSerie(ILista<T> lista)
		{
			_serie = [];
			if (lista is not null)
			{
				foreach (T elem in
					lista)
				{
					_serie.Add(elem);
				}
			}
			_nombre = string.Empty;
		}

		public ListSerie(ILista<T> lista, string nombre) : this(lista)
		{
			_nombre = nombre;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con nombre <c>nombre</c> y capacidad inicial <c>capacidad</c>
		/// </summary>
		public ListSerie(string nombre, int capacidad)
		{
			this._nombre = nombre;
			_serie = new List<T>(capacidad);
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con nombre <c>nombre</c> y capacidad inicial <c>capacidad</c>
		/// </summary>
		public ListSerie(string nombre, int capacidad, Func<int,T?> generadora) : this(nombre,capacidad) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/> vacío con nombre <c>nombre</c> y capacidad inicial 10
		/// </summary>
		public ListSerie(string nombre) : this(nombre, 10) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/> vacío con el nombre vacío y capacidad inicial <c>cap</c>
		/// </summary>
		public ListSerie(int cap) : this("", cap) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con el nombre vacío, capacidad inicial <c>cap</c> y la función generadora proporcionada
		/// </summary>
		public ListSerie(int cap, Func<int,T?> generadora) : this("", cap) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/> vacío con el nombre vacío y capacidad inicial 10
		/// </summary>
		public ListSerie() : this("", 10) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con el nombre vacío, capacidad inicial 10 y la función generadora proporcionada
		/// </summary>
		public ListSerie(Func<int,T?> generadora) : this("", 10) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con nombre <c>nombre</c> y los elementos de <c>col</c>
		/// </summary>
		public ListSerie(string nombre, ICollection<T> col)
		{
			this._nombre = nombre;
			_serie = new List<T>(col);
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con el nombre vacío y los elementos de <c>col</c>
		/// </summary>
		public ListSerie(ICollection<T> col) : this("", col) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con el nombre vacío, los elementos de <c>col</c> y la función generadora proporcionada
		/// </summary>
		public ListSerie(ICollection<T> col, Func<int,T?> generadora) : this("", col) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSerie{T}"/>
		/// vacío con el nombre vacío, los elementos de <c>col</c> y la función generadora proporcionada
		/// </summary>
		public ListSerie(ICollection<T> col, string nombre, Func<int, T?> generadora) : this(nombre, col) {
			_generadora = generadora;
		}

		///<inheritdoc/>
		public void InsertarPrimero(T elem)
		{
			_serie.Insert(0,elem);
		}

		///<inheritdoc/>
		public void Insertar(T elem, int pos)
		{
			_serie.Insert(pos, elem);
		}

		///<inheritdoc/>
		public void InsertarUltimo(T elem)
		{
			_serie.Add(elem);
		}

		///<inheritdoc/>
		public void InsertarVarios(T elem, int num, int pos)
		{
			while (num > 0)
			{
				Insertar(elem, pos);
				num--;
			}
		}

		///<inheritdoc/>
		public T BorrarPrimero()
		{
			T primero = _serie[0];
			_serie.RemoveAt(0);
			return primero;
		}

		///<inheritdoc/>
		public int Eliminar(T elem)
		{
			int pos = _serie.IndexOf(elem);
			if (pos != -1)
				_serie.RemoveAt(pos);
			return pos;
		}

		///<inheritdoc/>
		public T Eliminar(int pos)
		{
			T elemento = _serie[pos];
			_serie.RemoveAt(pos);
			return elemento;
		}

		///<inheritdoc/>
		public T BorrarUltimo()
		{
			T ultimo = _serie[^1];
			_serie.RemoveAt(_serie.Count - 1);
			return ultimo;
		}

		///<inheritdoc/>
		public int EliminarVarios(int num, int pos)
		{
			int real = 0;
			while (real < num && pos < _serie.Count)
			{
				_serie.RemoveAt(pos);
				real++;
			}
			return real;
		}

		///<inheritdoc/>
		public int BorrarUltimos(T elem)
		{
			int res = 0;
			Func<T, bool> condicion;
			if (elem == null) {
				condicion = (t => t is null);
			} else {
				condicion = (t => elem.Equals(t));
			}
			while (condicion(_serie[^1]))
			{
				_serie.RemoveAt(_serie.Count - 1);
				res++;
			}
			return res;
		}

		///<inheritdoc/>
		public void BorrarTodos()
		{
			_serie.Clear();
		}

		public int BorrarTodos(T elemento) {
			int res = 0;
			for (int i = _serie.Count - 1; i >= 0; i--)
			{
				if (_serie[i]?.Equals(elemento) ?? elemento is null) { //Si son iguales o los dos son nulos
					res++;
					_serie.RemoveAt(i);
				}
			}
			return res;
		}

		///<inheritdoc/>
		public T PrimerElemento {
			get {
				Contrato.Requires<InvalidOperationException>(_serie.Count > 0, Mensajes.VacioSerie);
				return _serie[0];
			}
		}

		///<inheritdoc/>
		public T UltimoElemento {
			get {
				Contrato.Requires<InvalidOperationException>(_serie.Count > 0, Mensajes.VacioSerie);
				return _serie[^1];
			}
		}

		///<inheritdoc/>
		public int Posicion(T elem)
		{
			return _serie.IndexOf(elem);
		}

		///<inheritdoc/>
		public int[] Ocurrencias(T elem)
		{
			int num = 0, indice = 0;
			int[] ocurrencias = [], cambio;
			foreach (T tipo in _serie) {
				if (Equals(elem, tipo)) {
					cambio = new int[++num];
					Array.Copy(ocurrencias,cambio,ocurrencias.Length);
					cambio[^1] = indice;
					ocurrencias = cambio;
				}
				indice++;
			}
			return ocurrencias;
		}

		///<inheritdoc/>
		public bool Contiene(T elem)
		{
			return _serie.Contains(elem);
		}

		public override string ToString()
		{
			return (_nombre == string.Empty ? string.Empty : _nombre + ": ") +  string.Join(',',_serie);
		}
		public IEnumerator<T> GetEnumerator()
		{
			return _serie.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/**
		 * Compara el objeto llamante con obj
		 * <p>Si ambos son de una clase que implemente {@code Lista}, tienen la misma cantidad de elementos
		 * y estos son iguales, las listas son iguales</p>
		 * <p>Se ignora su nombre al comparar listas</p>
		 * @param obj objeto con el que se compara
		 * @return {@code true} si las lista son iguales
		 */
		public override bool Equals(Object? obj) {
			bool iguales = ReferenceEquals(this,obj);
			if (!iguales && obj is ILista<T> lista) {
				iguales = lista.Longitud == Longitud;
				for (int i = 0; i < Longitud & !iguales; i++) {
					iguales = Equals(lista[i], this[i]);
				}
			}
			return iguales;
		}
		public override int GetHashCode()
		{
			return _serie.GetHashCode() ^ _nombre.GetHashCode();
		}

		string IListaNombrada<T>.ToStringInverso()
		{
			_serie.Reverse();
			string? inverso = ToString();
			_serie.Reverse();
			return inverso??"";
		}

		public IListaArbitraria<T> Multiplicar(int factor) {
			ListSerie<T> nueva;
			if (factor == 0 || _serie.Count == 0) { //Multiplicar por 0 da cero
				nueva = new ListSerie<T>();
			} else {
				nueva = new(this);
				//Si se produce OverflowException no es mi problema, la lista no podría contenerlo
				for (int i = 0; i < _serie.Count*(Math.Abs(factor)-1); i++) { 
					nueva.InsertarUltimo(_serie[i%_serie.Count]);
				}
				if (factor < 0) {
					nueva.Invertir();
				}
			}
			return nueva;
		}

		public void Invertir() {
			_serie.Reverse();
		}

		public int Insertar(T elemento) {
			_serie.Add(elemento);
			return _serie.Count-1;
		}

		public ILista<T> Sumar(T elemento) {
			var nueva = new ListSerie<T>(this);
			nueva.Insertar(elemento);
			return nueva;
		}

		public ILista<T> Unir(ILista<T> segunda) {
			var nueva = new ListSerie<T>(this);
			foreach (var item in segunda) {
				nueva.Insertar(item);
			}
			return nueva;
		}

		public IListaArbitraria<T> Clonar() {
			return Clonar();
		}

		ILista<T> ILista<T>.Clonar() {
			return Clonar();
		}

		public ILista<T> Restar(T elemento) {
			var nueva = Clonar();
			nueva.BorrarTodos(elemento);
			return nueva;
		}

		public ILista<T> Diferencia(ILista<T> lista) {
			var nueva = Clonar();
			foreach (var item in lista) {
				if (nueva.Contiene(item)) {
					nueva.BorrarTodos(item);
				}
			}
			return nueva;
		}

		ISerie<T> ISerie<T>.Clonar() {
			return new ListSerie<T>(this);
		}
	}
}
