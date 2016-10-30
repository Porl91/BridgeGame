using System;
using System.Collections.Generic;

namespace Game1
{
	public sealed class TileTypeMatcher : IMatchTileTypes
	{
		public Dictionary<string, ITileType> TileTypes { get; }
		public ITileType DefaultTileType { get; }

		public TileTypeMatcher(ITileType defaultTileType)
		{
			if (defaultTileType == null)
				throw new ArgumentNullException(nameof(defaultTileType));

			TileTypes = new Dictionary<string, ITileType>();
			DefaultTileType = defaultTileType;
		}

		public void AddTileType(ITileType tileType)
		{
			if (TileTypes.ContainsKey(tileType.ID))
				throw new ArgumentException($"Attempted to add a tile type with an duplicate tile type ID '{tileType.ID}'");

			TileTypes.Add(tileType.ID, tileType);
		}

		public ITileType GetTileType(string tileTypeID)
		{
			if (!TileTypes.ContainsKey(tileTypeID))
				throw new ArgumentException($"{tileTypeID} is not the ID of a valid tile type");

			return TileTypes[tileTypeID];
		}
	}
}
