using Game1.Maths;
using System;

namespace Game1
{
	public static class CameraHelpers
	{
		public static Matrix4f Perspective(float fovInRadians, float aspectRatio, float near, float far)
		{
			var f = (float)Math.Tan(Math.PI * 0.5 - 0.5 * fovInRadians);
			var rangeInverted = 1.0f / (near - far);

			return new Matrix4f(new float[]
			{
				f / aspectRatio, 
				0, 
				0, 
				0,

				0, 
				f, 
				0, 
				0,
				 
				0, 
				0, 
				(near + far) * rangeInverted,
				-1, 

				0, 
				0, 
				near * far * rangeInverted * 2,
				0
			});
		}

		public static Matrix4f Orthographic(float left, float right, float bottom, float top, float near, float far)
		{
			return new Matrix4f(new float[]
			{
				2.0f / (right - left), 
				0, 
				0, 
				0,
				 
				0, 
				2.0f / (top - bottom), 
				0, 
				0, 

				0, 
				0, 
				2.0f / (near - far), 
				0, 

				(left + right) / (left - right), 
				(bottom + top) / (bottom - top), 
				(near + far) / (near - far), 
				1
			});
		}
	}
}
