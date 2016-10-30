using Bridge.WebGL;
using System.Linq;

namespace Game1
{
	public sealed class Water : TileType, ITileType
	{
		public Water(string id) : base(id) { }

		public override void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, int x, int y)
		{
			WebGLHelpers.DrawImage(gl, textureLoader.Textures.First(), programManager.Programs["prog-image"].Program,
				64, 0, 64, 64,
				x, y, 64, 64);
		}
	}
}
