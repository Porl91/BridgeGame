using Bridge.WebGL;
using System;
using System.Linq;

namespace Game1
{
	public sealed class Level
	{
		Map MapData;

		public void Update()
		{
			MapData = new Map(50, 50);
		}

		public void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, Level level)
		{
			for (var y = 0; y <= gl.Canvas.Height; y += 64)
			{
				for (var x = 0; x <= gl.Canvas.Width; x += 64)
				{
					WebGLHelpers.DrawImage(gl, textureLoader.Textures.First(), programManager.Programs["prog-image"].Program,
						0, 0, 64, 64,
						x, y, 64, 64);
				}
			}

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
		}
	}
}
