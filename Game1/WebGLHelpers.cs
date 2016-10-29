using Bridge.Html5;
using Bridge.WebGL;
using System.Diagnostics;

namespace Game1
{
	public static class WebGLHelpers
	{
		public static void DrawImage(WebGLRenderingContext gl, TextureInfo textureInfo, WebGLProgram program,
			int xSrc, int ySrc, int srcWidth, int srcHeight,
			int xDest, int yDest, int destWidth, int destHeight)
		{
			var positionLocation = gl.GetAttribLocation(program, "a_position");
			var texCoordLocation = gl.GetAttribLocation(program, "a_texcoord");

			var matrixLocation = gl.GetUniformLocation(program, "u_matrix");
			var textureLocation = gl.GetUniformLocation(program, "u_texture");

			/* Create a buffer to hold the position coordinates. */

			var positionBuffer = gl.CreateBuffer();
			var positionCoords = new float[]
			{
				0, 0,   0, 1,   1, 0,   1, 0,   0, 1,   1, 1
			};

			gl.BindBuffer(gl.ARRAY_BUFFER, positionBuffer);
			gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(positionCoords), gl.STATIC_DRAW);

			/* Create a buffer to hold the texture coordinates. */

			var sx = xSrc / (float)textureInfo.Width.Value;
			var sy = ySrc / (float)textureInfo.Height.Value;

			var sw = srcWidth / (float)textureInfo.Width.Value;
			var sh = srcHeight / (float)textureInfo.Height.Value;

			var texBuffer = gl.CreateBuffer();
			var texCoords = new float[]
			{
				0 + sx, 0 + sy,
				0 + sx, sh + sy,
				sw + sx, 0 + sy,
				sw + sx, 0 + sy,
				0 + sx, sh + sy,
				sw + sx, sh + sy
			};

			gl.BindBuffer(gl.ARRAY_BUFFER, texBuffer);
			gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(texCoords), gl.STATIC_DRAW);

			gl.BindTexture(gl.TEXTURE_2D, textureInfo.Texture);
			gl.UseProgram(program);

			gl.BindBuffer(gl.ARRAY_BUFFER, positionBuffer);
			gl.EnableVertexAttribArray(positionLocation);
			gl.VertexAttribPointer(positionLocation, 2, gl.FLOAT, false, 0, 0);

			gl.BindBuffer(gl.ARRAY_BUFFER, texBuffer);
			gl.EnableVertexAttribArray(texCoordLocation);
			gl.VertexAttribPointer(texCoordLocation, 2, gl.FLOAT, false, 0, 0);

			/* Create an standard orthographic projection to transform our coordinates by and bind to our shader uniform. */

			var matrix = CameraHelpers.Orthographic(0, gl.Canvas.Width, gl.Canvas.Height, 0, -1, 1);
			matrix = matrix.Translate(xDest / (float)gl.Canvas.Width * 2, -yDest / (float)gl.Canvas.Height * 2, 0);
			matrix = matrix.Scale(destWidth, destHeight, 1);

			gl.UniformMatrix4fv(matrixLocation, false, matrix.ToArray());
			gl.Uniform1i(textureLocation, 0);

			/* 6 points per image rendered as two triangles. */

			gl.DrawArrays(gl.TRIANGLES, 0, 6);
		}
	}
}
