using Bridge.WebGL;
using Game1.Maths;
using System;

namespace Game1
{
	public sealed class Level
	{
		Map mapData;

		public void Update()
		{
			var waterTile = new Water("water");
			var grassTile = new Grass("grass");
			var metalPlateTile = new MetalPlate("metalPlate");

			var tileTypeMatcher = new TileTypeMatcher(defaultTileType: waterTile);

			tileTypeMatcher.AddTileType(waterTile);
			tileTypeMatcher.AddTileType(grassTile);
			tileTypeMatcher.AddTileType(metalPlateTile);

			var mapWidth = 10;
			var mapHeight = 10;

			Func<int, int, string> mapPopulator = (x, y) =>
			{
				if (x < 2 || y < 2 || x >= mapWidth - 2 || y >= mapHeight - 2)
					return metalPlateTile.ID;

				if (((x + y) & 1) == 0)
					return grassTile.ID;

				return waterTile.ID;
			};

			mapData = new Map(mapWidth, mapHeight, mapPopulator, tileTypeMatcher);
		}

		public void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager)
		{
			var tileSize = TileType.GetTileSize();
			var tilesInWidth = gl.Canvas.Width / tileSize.X + 1;
			var tilesInHeight = gl.Canvas.Height / tileSize.Y + 1;

			for (var y = 0; y < tilesInHeight; y++)
			{
				for (var x = 0; x < tilesInWidth; x++)
				{
					mapData.GetTile(x, y).Render(gl, textureLoader, programManager, (int)(x * tileSize.X), (int)(y * tileSize.Y));
				}
			}
			
			/*
			var gridLineColour = new RGBAColour(0.1f, 1f, 0.5f, 1.0f);

			for (var i = 0; i <= Math.Max(gl.Canvas.Width, gl.Canvas.Height); i += 64)
			{
				if (i <= gl.Canvas.Height)
					WebGLHelpers.DrawLine(gl, programManager.Programs["prog-line"].Program,
						gridLineColour,
						0, i, gl.Canvas.Width, i);

				if (i <= gl.Canvas.Width)
					WebGLHelpers.DrawLine(gl, programManager.Programs["prog-line"].Program,
						gridLineColour,
						i, 0, i, gl.Canvas.Height);
			}
			*/
		}
	}
}
