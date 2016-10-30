using System;

namespace Game1
{
	public sealed class Map
	{
		string[] grid;
		int width;
		int height;

		IMatchTileTypes tileTypeMatcher;

		public Map(int width, int height, Func<int, int, string> mapPopulator, IMatchTileTypes tileTypeMatcher)
		{
			this.width = width;
			this.height = height;

			grid = new string[width * height];

			for (var y = 0; y < height; y++)
				for (var x = 0; x < width; x++)
					grid[y * width + x] = mapPopulator(x, y);

			this.tileTypeMatcher = tileTypeMatcher;
		}

		public ITileType GetTile(int x, int y)
		{
			if (x < 0 || x >= width || y < 0 || y >= height)
				return tileTypeMatcher.DefaultTileType;

			var tileTypeId = grid[y * width + x];

			if (string.IsNullOrWhiteSpace(tileTypeId))
				return tileTypeMatcher.DefaultTileType;

			return tileTypeMatcher.GetTileType(tileTypeId);
		}
	}
}
