using System;
using System.Collections;
using System.Collections.Generic;

namespace ExpandedLists {
	/// <summary>
	/// Implements <see cref="ISequence{T}"/> using an instance of <see cref="List{T}"/> to store its elements and a <see cref="Func{T, TResult}"/> to generate new elements.
	/// </summary>
	public class ListSequence<T> : ISequence<T>
	{
		private string _name;
		private List<T> _sequence;
		private Func<int,T?> _generator = num=>default;

		private T? Generar() {
			return _generator.Invoke(_sequence.Count);
		}

		/// <inheritdoc/>
		public string Name
		{
			get => _name;

			set => _name = value;
		}

		/// <inheritdoc/>
		public bool IsEmpty
		{
			get => _sequence.Count == 0;
			set
			{
				if (value)
					_sequence.Clear();
				else if (_sequence.Count == 0)
				{
					Count++;
				}
			}
		}

		/// <inheritdoc/>
		public int Count
		{
			get => _sequence.Count;
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 0, Messages.NegativeLength);
				int siz = _sequence.Count;
				if (value == 0) _sequence.Clear();
				else if (value < siz)
				{
					_sequence = new List<T>(_sequence.GetRange(0, value));
				}
				else if (value != siz)
				{
					while (value > siz)
					{
						Contract.Requires<InvalidOperationException>(IExpandedList<T>.CompatibleEnLista(Generar()),
							Messages.NullGenerated);
#pragma warning disable CS8604 // Posible argumento de referencia nulo
						_sequence.Add(Generar()); //Ignorar el warning
#pragma warning restore CS8604 // Posible argumento de referencia nulo
						siz++;
					}
				}
			}
		}

		/// <inheritdoc/>
		public Func<int,T?> GeneratorFunction
		{
			get => _generator;
			set => _generator = value;
		}

		/// <inheritdoc/>
		public T this[int posicion]
		{
			get => _sequence[posicion];
			set => _sequence[posicion] = value;
		}

		/// <summary>
		/// Creates a sequence with the elements from <c>list</c> and its generator function.
		/// </summary>
		public ListSequence(IDynamicList<T> list) : this(list as IExpandedList<T>){
			_generator = list?.GeneratorFunction ?? (num => default);
		}

		/// <summary>
		/// Creates a sequence with the elements from <c>list</c> and an empty name.
		/// </summary>
		public ListSequence(IExpandedList<T> list)
		{
			_sequence = [];
			if (list is not null)
			{
				foreach (T elem in
					list)
				{
					_sequence.Add(elem);
				}
			}
			_name = string.Empty;
		}

		/// <summary>
		/// Creates a sequence with the elements from <c>list</c> with this name.
		/// </summary>
		public ListSequence(IExpandedList<T> list, string name) : this(list)
		{
			_name = name;
		}

		/// <summary>
		/// Creates an empty sequence with this name and initial capacity.
		/// </summary>
		public ListSequence(string name, int capacity)
		{
			_name = name;
			_sequence = new List<T>(capacity);
		}

		/// <summary>
		/// Creates an empty sequence with this name, initial capacity and generator function.
		/// </summary>
		public ListSequence(string nombre, int capacidad, Func<int,T?> generadora) : this(nombre,capacidad) {
			_generator = generadora;
		}

		/// <summary>
		/// Creates an empty sequence with this name and an initial capacity for ten elements.
		/// </summary>
		public ListSequence(string nombre) : this(nombre, 10) { }

		/// <summary>
		/// Creates an empty sequence with this initial capacity and an empty name.
		/// </summary>
		public ListSequence(int cap) : this("", cap) { }

		/// <summary>
		/// Create an empty sequence with this capacity and generator function and an empty name.
		/// </summary>
		public ListSequence(int cap, Func<int,T?> generadora) : this("", cap) {
			_generator = generadora;
		}

		/// <summary>
		/// Create an empty sequence with this capacity, initial capacity for ten elements and an empty name.
		/// </summary>
		public ListSequence() : this("", 10) { }

		/// <summary>
		/// Create an empty sequence with this generator function, an empty name and initial capacity for ten elements.
		/// </summary>
		public ListSequence(Func<int,T?> generadora) : this("", 10) {
			_generator = generadora;
		}

		/// <summary>
		/// Create a sequence with this name and the elements from <c>col</c>.
		/// </summary>
		public ListSequence(string nombre, ICollection<T> col)
		{
			_name = nombre;
			_sequence = new List<T>(col);
		}

		/// <summary>
		/// Create a sequence with the elements from <c>col</c> and an empty name.
		/// </summary>
		public ListSequence(ICollection<T> col) : this("", col) { }

		/// <summary>
		/// Create a sequence with this generator function and the elements from <c>col</c>.
		/// </summary>
		public ListSequence(ICollection<T> col, Func<int,T?> generadora) : this("", col) {
			_generator = generadora;
		}

		/// <summary>
		/// Create a sequence with this name and generator function with the elements from <c>col</c>.
		/// </summary>
		public ListSequence(ICollection<T> col, string nombre, Func<int, T?> generadora) : this(nombre, col) {
			_generator = generadora;
		}

		///<inheritdoc/>
		public void InsertFirst(T elem)
		{
			_sequence.Insert(0,elem);
		}

		///<inheritdoc/>
		public void InsertAt(T elem, int pos)
		{
			_sequence.Insert(pos, elem);
		}

		///<inheritdoc/>
		public void InsertLast(T elem)
		{
			_sequence.Add(elem);
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
			T primero = _sequence[0];
			_sequence.RemoveAt(0);
			return primero;
		}

		///<inheritdoc/>
		public int Remove(T elem)
		{
			int pos = _sequence.IndexOf(elem);
			if (pos != -1)
				_sequence.RemoveAt(pos);
			return pos;
		}

		///<inheritdoc/>
		public T RemoveAt(int pos)
		{
			T elemento = _sequence[pos];
			_sequence.RemoveAt(pos);
			return elemento;
		}

		///<inheritdoc/>
		public T RemoveLast()
		{
			T ultimo = _sequence[^1];
			_sequence.RemoveAt(_sequence.Count - 1);
			return ultimo;
		}

		///<inheritdoc/>
		public int RemoveMultiple(int num, int pos)
		{
			int real = 0;
			while (real < num && pos < _sequence.Count)
			{
				_sequence.RemoveAt(pos);
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
			while (condicion(_sequence[^1]))
			{
				_sequence.RemoveAt(_sequence.Count - 1);
				res++;
			}
			return res;
		}

		///<inheritdoc/>
		public void Clear()
		{
			_sequence.Clear();
		}

		public int Clear(T elemento) {
			int res = 0;
			for (int i = _sequence.Count - 1; i >= 0; i--)
			{
				if (_sequence[i]?.Equals(elemento) ?? elemento is null) { //Si son iguales o los dos son nulos
					res++;
					_sequence.RemoveAt(i);
				}
			}
			return res;
		}

		///<inheritdoc/>
		public T First {
			get {
				Contract.Requires<InvalidOperationException>(_sequence.Count > 0, Messages.EmptySequence);
				return _sequence[0];
			}
		}

		///<inheritdoc/>
		public T Last {
			get {
				Contract.Requires<InvalidOperationException>(_sequence.Count > 0, Messages.EmptySequence);
				return _sequence[^1];
			}
		}

		///<inheritdoc/>
		public int Position(T elem)
		{
			return _sequence.IndexOf(elem);
		}

		///<inheritdoc/>
		public int[] Appareances(T elem)
		{
			int num = 0, indice = 0;
			int[] ocurrencias = [], cambio;
			foreach (T tipo in _sequence) {
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
			return _sequence.Contains(elem);
		}

		public override string ToString()
		{
			return (_name == string.Empty ? string.Empty : _name + ": ") +  string.Join(',',_sequence);
		}
		public IEnumerator<T> GetEnumerator()
		{
			return _sequence.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Determines if this sequence is equal to <c>obj</c>.
		/// </summary>
		/// <remarks>
		/// If <c>obj</c> is an <see cref="IExpandedList{T}"/> and have the same elements in the same order, they are equal.
		/// <para>
		/// Names are ignored when comparing lists.
		/// </para>
		/// </remarks>
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
			return _sequence.GetHashCode() ^ _name.GetHashCode();
		}

		string INamedList<T>.ReverseToString()
		{
			_sequence.Reverse();
			string? inverso = ToString();
			_sequence.Reverse();
			return inverso??"";
		}

		public IUnsortedList<T> Multiply(int factor) {
			ListSequence<T> nueva;
			if (factor == 0 || _sequence.Count == 0) { //Multiplicar por 0 da cero
				nueva = new ListSequence<T>();
			} else {
				nueva = new(this);
				//Si se produce OverflowException no es mi problema, la lista no podría contenerlo
				for (int i = 0; i < _sequence.Count*(Math.Abs(factor)-1); i++) { 
					nueva.InsertLast(_sequence[i%_sequence.Count]);
				}
				if (factor < 0) {
					nueva.Reverse();
				}
			}
			return nueva;
		}

		public void Reverse() {
			_sequence.Reverse();
		}

		public int Add(T elemento) {
			_sequence.Add(elemento);
			return _sequence.Count-1;
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
