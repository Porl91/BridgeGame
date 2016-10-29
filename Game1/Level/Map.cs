namespace Game1
{
	public sealed class Map
	{
		int[] grid;

		public Map(int width, int height)
		{
			grid = new int[width * height];
		}
	}
}
