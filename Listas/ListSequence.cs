using System;
using System.Collections;
using System.Collections.Generic;

namespace ExpandedLists {
	/// <summary>
	/// Implementa la interfaz <see cref="ISequence{T}"/> delegando los métodos a una instancia de <see cref="List{T}"/> y una <see cref="Func{T, TResult}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListSequence<T> : ISequence<T>
	{
		private string _nombre;
		private List<T> _serie;
		private Func<int,T?> _generadora = num=>default;

		private T? Generar() {
			return _generadora.Invoke(_serie.Count);
		}

		/// <inheritdoc/>
		public string Name
		{
			get => _nombre;

			set => _nombre = value;
		}

		/// <inheritdoc/>
		public bool IsEmpty
		{
			get => _serie.Count == 0;
			set
			{
				if (value)
					_serie.Clear();
				else if (_serie.Count == 0)
				{
					Count++;
				}
			}
		}

		/// <inheritdoc/>
		public int Count
		{
			get => _serie.Count;
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0, Messages.NegativeLength);
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
						Contract.Requires<InvalidOperationException>(IExpandedList<T>.CompatibleEnLista(Generar()),
							Messages.NullGeneration);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
						_serie.Add(Generar()); //Ignorar el warning
#pragma warning restore CS8604 // Posible argumento de referencia nulo
						siz++;
					}
				}
			}
		}

		/// <inheritdoc/>
		public Func<int,T?> GeneratorFunction
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
		public ListSequence(IDynamicList<T> lista) : this(lista as IExpandedList<T>){
			_generadora = lista?.GeneratorFunction ?? (num => default);
		}

		/// <summary>
		/// Crea una serie a partir de otra lista con el nombre vacío
		/// </summary>
		/// <remarks>
		/// La serie tendrá todos los elementos de la lista y su función generadora
		/// </remarks>
		/// <param name="lista">lista que copiar</param>
		public ListSequence(IExpandedList<T> lista)
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

		public ListSequence(IExpandedList<T> lista, string nombre) : this(lista)
		{
			_nombre = nombre;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con nombre <c>nombre</c> y capacidad inicial <c>capacidad</c>
		/// </summary>
		public ListSequence(string nombre, int capacidad)
		{
			this._nombre = nombre;
			_serie = new List<T>(capacidad);
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con nombre <c>nombre</c> y capacidad inicial <c>capacidad</c>
		/// </summary>
		public ListSequence(string nombre, int capacidad, Func<int,T?> generadora) : this(nombre,capacidad) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/> vacío con nombre <c>nombre</c> y capacidad inicial 10
		/// </summary>
		public ListSequence(string nombre) : this(nombre, 10) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/> vacío con el nombre vacío y capacidad inicial <c>cap</c>
		/// </summary>
		public ListSequence(int cap) : this("", cap) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con el nombre vacío, capacidad inicial <c>cap</c> y la función generadora proporcionada
		/// </summary>
		public ListSequence(int cap, Func<int,T?> generadora) : this("", cap) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/> vacío con el nombre vacío y capacidad inicial 10
		/// </summary>
		public ListSequence() : this("", 10) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con el nombre vacío, capacidad inicial 10 y la función generadora proporcionada
		/// </summary>
		public ListSequence(Func<int,T?> generadora) : this("", 10) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con nombre <c>nombre</c> y los elementos de <c>col</c>
		/// </summary>
		public ListSequence(string nombre, ICollection<T> col)
		{
			this._nombre = nombre;
			_serie = new List<T>(col);
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con el nombre vacío y los elementos de <c>col</c>
		/// </summary>
		public ListSequence(ICollection<T> col) : this("", col) { }

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con el nombre vacío, los elementos de <c>col</c> y la función generadora proporcionada
		/// </summary>
		public ListSequence(ICollection<T> col, Func<int,T?> generadora) : this("", col) {
			_generadora = generadora;
		}

		/// <summary>
		/// Crea un objeto <see cref="ListSequence{T}"/>
		/// vacío con el nombre vacío, los elementos de <c>col</c> y la función generadora proporcionada
		/// </summary>
		public ListSequence(ICollection<T> col, string nombre, Func<int, T?> generadora) : this(nombre, col) {
			_generadora = generadora;
		}

		///<inheritdoc/>
		public void InsertFirst(T elem)
		{
			_serie.Insert(0,elem);
		}

		///<inheritdoc/>
		public void InsertAt(T elem, int pos)
		{
			_serie.Insert(pos, elem);
		}

		///<inheritdoc/>
		public void InsertLast(T elem)
		{
			_serie.Add(elem);
		}

		///<inheritdoc/>
		public void InsertMultiple(T elem, int num, int pos)
		{
			while (num > 0)
			{
				InsertAt(elem, pos);
				num--;
			}
		}

		///<inheritdoc/>
		public T RemoveFirst()
		{
			T primero = _serie[0];
			_serie.RemoveAt(0);
			return primero;
		}

		///<inheritdoc/>
		public int Remove(T elem)
		{
			int pos = _serie.IndexOf(elem);
			if (pos != -1)
				_serie.RemoveAt(pos);
			return pos;
		}

		///<inheritdoc/>
		public T RemoveAt(int pos)
		{
			T elemento = _serie[pos];
			_serie.RemoveAt(pos);
			return elemento;
		}

		///<inheritdoc/>
		public T RemoveLast()
		{
			T ultimo = _serie[^1];
			_serie.RemoveAt(_serie.Count - 1);
			return ultimo;
		}

		///<inheritdoc/>
		public int RemoveMultiple(int num, int pos)
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
		public int RemoveLast(T elem)
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
		public void Clear()
		{
			_serie.Clear();
		}

		public int Clear(T elemento) {
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
		public T First {
			get {
				Contract.Requires<InvalidOperationException>(_serie.Count > 0, Messages.EmptySequence);
				return _serie[0];
			}
		}

		///<inheritdoc/>
		public T Last {
			get {
				Contract.Requires<InvalidOperationException>(_serie.Count > 0, Messages.EmptySequence);
				return _serie[^1];
			}
		}

		///<inheritdoc/>
		public int Position(T elem)
		{
			return _serie.IndexOf(elem);
		}

		///<inheritdoc/>
		public int[] Appareances(T elem)
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
		public bool Contains(T elem)
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
			if (!iguales && obj is IExpandedList<T> lista) {
				iguales = lista.Count == Count;
				for (int i = 0; i < Count & !iguales; i++) {
					iguales = Equals(lista[i], this[i]);
				}
			}
			return iguales;
		}
		public override int GetHashCode()
		{
			return _serie.GetHashCode() ^ _nombre.GetHashCode();
		}

		string INamedList<T>.ReverseToString()
		{
			_serie.Reverse();
			string? inverso = ToString();
			_serie.Reverse();
			return inverso??"";
		}

		public IUnsortedList<T> Multiply(int factor) {
			ListSequence<T> nueva;
			if (factor == 0 || _serie.Count == 0) { //Multiplicar por 0 da cero
				nueva = new ListSequence<T>();
			} else {
				nueva = new(this);
				//Si se produce OverflowException no es mi problema, la lista no podría contenerlo
				for (int i = 0; i < _serie.Count*(Math.Abs(factor)-1); i++) { 
					nueva.InsertLast(_serie[i%_serie.Count]);
				}
				if (factor < 0) {
					nueva.Reverse();
				}
			}
			return nueva;
		}

		public void Reverse() {
			_serie.Reverse();
		}

		public int Add(T elemento) {
			_serie.Add(elemento);
			return _serie.Count-1;
		}

		public IExpandedList<T> AddNew(T elemento) {
			var nueva = new ListSequence<T>(this);
			nueva.AddNew(elemento);
			return nueva;
		}

		public IExpandedList<T> Join(IExpandedList<T> segunda) {
			var nueva = new ListSequence<T>(this);
			foreach (var item in segunda) {
				nueva.Add(item);
			}
			return nueva;
		}

		public IUnsortedList<T> CloneUnsorted() {
			return CloneSequence();
		}

		public IExpandedList<T> Clone() {
			return CloneSequence();
		}

		public IExpandedList<T> Subtract(T elemento) {
			var nueva = CloneUnsorted();
			nueva.Clear(elemento);
			return nueva;
		}

		public IExpandedList<T> Difference(IExpandedList<T> lista) {
			var nueva = CloneUnsorted();
			foreach (var item in lista) {
				if (nueva.Contains(item)) {
					nueva.Clear(item);
				}
			}
			return nueva;
		}

		public ISequence<T> CloneSequence() {
			return new ListSequence<T>(this);
		}

		public IDynamicList<T> CloneDynamic() {
			return CloneSequence();
		}
	}
}
