using Bridge.Html5;
using System;
using System.Linq;

namespace Game1
{
	public sealed class Matrix4f
	{
		public float[] Values { get; }

		const uint ROW_COUNT = 4;
		const uint COL_COUNT = 4;
		const uint ELEMENT_COUNT = ROW_COUNT * COL_COUNT;

		public Matrix4f()
		{
			Values = new float[ELEMENT_COUNT];
		}

		public Matrix4f(float[] values)
		{
			if (values.Length != ELEMENT_COUNT)
				throw new ArgumentException($"Array provided contains {values.Length} elements(s), although {ELEMENT_COUNT} is expected");

			Values = values;
		}

		public Matrix4f(Float32Array array)
		{
			Values = array.ToArray();
		}

		public Matrix4f Translate(float tx, float ty, float tz)
		{
			return new Matrix4f(new float[]
			{
				Values[0], Values[1], Values[2], Values[3],
				Values[4], Values[5], Values[6], Values[7],
				Values[8], Values[9], Values[10], Values[11],
				Values[12] + tx, Values[13] + ty, Values[14] + tz, Values[15]
			});
		}

		public Matrix4f Scale(float sx, float sy, float sz)
		{
			return new Matrix4f(new float[]
			{
				Values[0] * sx, Values[1] * sx, Values[2] * sx, Values[3] * sx,
				Values[4] * sy, Values[5] * sy, Values[6] * sy, Values[7] * sy,
				Values[8] * sz, Values[9] * sz, Values[10] * sz, Values[11] * sz,
				Values[12], Values[13], Values[14], Values[15]
			});
		}

		public Matrix4f Multiply(Matrix4f other)
		{
			return new Matrix4f(new float[]
			{
				Values[0] * other.Values[0] + Values[1] * other.Values[4] + Values[2] * other.Values[8] + Values[3] * other.Values[12],
				Values[0] * other.Values[1] + Values[1] * other.Values[5] + Values[2] * other.Values[9] + Values[3] * other.Values[13],
				Values[0] * other.Values[2] + Values[1] * other.Values[6] + Values[2] * other.Values[10] + Values[3] * other.Values[14],
				Values[0] * other.Values[3] + Values[1] * other.Values[7] + Values[2] * other.Values[11] + Values[3] * other.Values[15],

				Values[4] * other.Values[0] + Values[5] * other.Values[4] + Values[6] * other.Values[8] + Values[7] * other.Values[12],
				Values[4] * other.Values[1] + Values[5] * other.Values[5] + Values[6] * other.Values[9] + Values[7] * other.Values[13],
				Values[4] * other.Values[2] + Values[5] * other.Values[6] + Values[6] * other.Values[10] + Values[7] * other.Values[14],
				Values[4] * other.Values[3] + Values[5] * other.Values[7] + Values[6] * other.Values[11] + Values[7] * other.Values[15],

				Values[8] * other.Values[0] + Values[9] * other.Values[4] + Values[10] * other.Values[8] + Values[11] * other.Values[12],
				Values[8] * other.Values[1] + Values[9] * other.Values[5] + Values[10] * other.Values[9] + Values[11] * other.Values[13],
				Values[8] * other.Values[2] + Values[9] * other.Values[6] + Values[10] * other.Values[10] + Values[11] * other.Values[14],
				Values[8] * other.Values[3] + Values[9] * other.Values[7] + Values[10] * other.Values[11] + Values[11] * other.Values[15],

				Values[12] * other.Values[0] + Values[13] * other.Values[4] + Values[14] * other.Values[8] + Values[15] * other.Values[12],
				Values[12] * other.Values[1] + Values[13] * other.Values[5] + Values[14] * other.Values[9] + Values[15] * other.Values[13],
				Values[12] * other.Values[2] + Values[13] * other.Values[6] + Values[14] * other.Values[10] + Values[15] * other.Values[14],
				Values[12] * other.Values[3] + Values[13] * other.Values[7] + Values[14] * other.Values[11] + Values[15] * other.Values[15]
			});
		}

		public Matrix4f Transpose()
		{
			var output = new float[ELEMENT_COUNT];

			for (var i = 0; i < COL_COUNT; i++)
			{
				for (var j = 0; j < ROW_COUNT; j++)
				{
					output[i * ROW_COUNT + j] = Values[j * COL_COUNT + i];
				}
			}

			return new Matrix4f(output);
		}

		public static Matrix4f Identity
		{
			get
			{
				var arr = new float[ELEMENT_COUNT];

				for (var i = 0; i < COL_COUNT; i++)
				{
					for (var j = 0; j < ROW_COUNT; j++)
					{
						arr[i * ROW_COUNT + j] = (i == j) ? 1 : 0;
					}
				}

				return new Matrix4f(arr);
			}
		}

		public float[] ToArray()
		{
			return Values;
		}

		public static Matrix4f operator *(Matrix4f mat1, Matrix4f mat2)
		{
			return mat1.Multiply(mat2);
		}
	}
}
