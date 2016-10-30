using Bridge.WebGL;

namespace Game1
{
	public interface ITileType
	{
		string ID { get; }

		void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, int x, int y);
	}
}
