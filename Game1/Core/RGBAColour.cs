using ProductiveRage.Immutable;

namespace Game1
{
	public sealed class RGBAColour : IAmImmutable
	{
		public RGBAColour(float r, float g, float b, float a)
		{
			this.CtorSet(_ => _.R, r);
			this.CtorSet(_ => _.G, g);
			this.CtorSet(_ => _.B, b);
			this.CtorSet(_ => _.A, a);
		}

		public float R { get; }
		public float G { get; }
		public float B { get; }
		public float A { get; }
	}
}
