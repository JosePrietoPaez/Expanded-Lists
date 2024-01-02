namespace PruebaListas
{

	internal class StringsFlags
	{
		public const int UNICO_I = 0, AYUDA_I = 1;
		public const string UNICO = "-u", AYUDA = "-h";
		private static readonly string[] _todos = [UNICO,AYUDA];
		internal static ulong StringAIndice(string[] array)
		{
			ulong flags = 0;
			foreach (var item in array)
			{
				foreach(var flag in _todos)
				{	int indice = 0;
					if (flag.Equals(item))
					{
						flags |= (ulong) 1 << indice++;
					}
				}
			}
			return flags;
		}
	}
}
