using System;
using Bridge.WebGL;
using Game1.Maths;
using ProductiveRage.Immutable;

namespace Game1
{
	public abstract class TileType : ITileType, IAmImmutable
	{
		public TileType(string id)
		{
			this.CtorSet(_ => _.ID, id);
		}

		public string ID { get; }

		public static Vector2f GetTileSize()
		{
			return new Vector2f(64, 64);
		}

		public virtual void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, int x, int y)
		{
			throw new NotImplementedException();
		}
	}
}
