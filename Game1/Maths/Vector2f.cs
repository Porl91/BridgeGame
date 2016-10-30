using ProductiveRage.Immutable;

namespace Game1.Maths
{
	public sealed class Vector2f : IAmImmutable
	{
		public Vector2f(float x, float y)
		{
			this.CtorSet(_ => _.X, x);
			this.CtorSet(_ => _.Y, y);
		}

		public float X { get; }
		public float Y { get; }
	}
}
