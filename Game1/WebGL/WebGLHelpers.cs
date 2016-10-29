using Bridge.Html5;
using Bridge.WebGL;
using System;

namespace Game1
{
	public static class WebGLHelpers
	{
		public static void DrawImage(WebGLRenderingContext gl, TextureInfo textureInfo, WebGLProgram program,
			int xSrc, int ySrc, int srcWidth, int srcHeight,
			int xDest, int yDest, int destWidth, int destHeight)
		{
			gl.UseProgram(program);

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
			gl.BindBuffer(gl.ARRAY_BUFFER, null);
		}

		public static void DrawLine(WebGLRenderingContext gl, WebGLProgram program, RGBAColour colour,
			int x0, int y0, int x1, int y1)
		{
			gl.UseProgram(program);

			var positionCoords = new float[]
			{
				x0, y0, x1, y1
			};

			/* Add the components of the colour twice (one for each vertex). */

			var colours = new float[]
			{
				colour.R, colour.G, colour.B, colour.A,
				colour.R, colour.G, colour.B, colour.A
			};

			var matrixLocation = gl.GetUniformLocation(program, "u_matrix");

			/* Load the colour into a buffer and bind to shader attribute. */

			var colourBuffer = gl.CreateBuffer();
			var colourLocation = gl.GetAttribLocation(program, "a_color");

			gl.BindBuffer(gl.ARRAY_BUFFER, colourBuffer);
			gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(colours), gl.STATIC_DRAW);
			gl.VertexAttribPointer(colourLocation, 4, gl.FLOAT, false, 0, 0);
			gl.EnableVertexAttribArray(colourLocation);

			/* Load the line endpoints into a buffer and bind to shader attribute. */

			var vertexBuffer = gl.CreateBuffer();
			var positionLocation = gl.GetAttribLocation(program, "a_position");

			gl.BindBuffer(gl.ARRAY_BUFFER, vertexBuffer);
			gl.BufferData(gl.ARRAY_BUFFER, new Float32Array(positionCoords), gl.STATIC_DRAW);
			gl.VertexAttribPointer(positionLocation, 2, gl.FLOAT, false, 0, 0);
			gl.EnableVertexAttribArray(positionLocation);

			/* Set up projection matrix and translate to screen origin. */

			var matrix = CameraHelpers.Orthographic(0, gl.Canvas.Width, gl.Canvas.Height, 0, -1, 1);
			gl.UniformMatrix4fv(matrixLocation, false, matrix.ToArray());

			matrix = matrix.Translate(x0 / (float)gl.Canvas.Width * 2, -y0 / (float)gl.Canvas.Height * 2, 0);

			gl.DrawArrays(gl.LINES, 0, 2);
			gl.BindBuffer(gl.ARRAY_BUFFER, null);
		}

		public static WebGLRenderingContext GetWebGLRenderingContext(HTMLCanvasElement canvas)
		{
			var contextNames = new string[]
			{
				"webgl",
				"experimental-webgl",
				"webkit-3d",
				"moz-webgl"
			};

			WebGLRenderingContext context = null;

			foreach (var name in contextNames)
			{
				try
				{
					context = canvas.GetContext(name).As<WebGLRenderingContext>();
				}
				catch (Exception) { }

				if (context != null)
					break;
			}

			return context;
		}
	}
}
