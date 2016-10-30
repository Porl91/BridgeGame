using Bridge.WebGL;
using System.Linq;

namespace Game1
{
	public sealed class Grass : TileType, ITileType
	{
		public Grass(string id) : base(id) { }

		public override void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, int x, int y)
		{
			WebGLHelpers.DrawImage(gl, textureLoader.Textures.First(), programManager.Programs["prog-image"].Program,
				0, 64, 64, 64,
				x, y, 64, 64);
		}
	}
}
