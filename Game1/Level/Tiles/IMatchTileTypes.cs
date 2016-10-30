using System.Collections.Generic;

namespace Game1
{
	public interface IMatchTileTypes
	{
		Dictionary<string, ITileType> TileTypes { get; }
		ITileType DefaultTileType { get; }
		void AddTileType(ITileType tileType);
		ITileType GetTileType(string tileTypeID);
	}
}
