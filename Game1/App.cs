using Bridge.Html5;
using Bridge.WebGL;
using ProductiveRage.Immutable;
using System;
using System.Linq;

namespace Game1
{
    public class App
    {
		[Ready]
		public static void Main()
		{
			var canvas = Document.GetElementById<HTMLCanvasElement>("main");
			canvas.Width = 1024;
			canvas.Height = 1024;

			var gl = GetWebGLRenderingContext(canvas);
			if (gl == null)
				throw new InvalidOperationException("Your browser doesn't support WebGL rendering contexts");
			
			var textureLoader = new TextureLoader(Set<TextureInfo>.Empty
				.Add(TextureInfo.Create(gl, "../../Content/Images/sprites.png")));

			textureLoader.AllLoaded += () => Start(gl, textureLoader);
			textureLoader.ProcessTextures();	
		}

		static void Start(WebGLRenderingContext gl, TextureLoader textureLoader)
		{
			var vShaderTag = Document.GetElementById("vert-shader");
			var fShaderTag = Document.GetElementById("frag-shader");
			var vShader = CreateShader(gl, gl.VERTEX_SHADER, vShaderTag.InnerHTML);
			var fShader = CreateShader(gl, gl.FRAGMENT_SHADER, fShaderTag.InnerHTML);
			var program = CreateShaderProgram(gl, vShader, fShader);

			Global.SetInterval(() => Render(gl, textureLoader, program), 200);
		}

		static void Render(WebGLRenderingContext gl, TextureLoader textureLoader, WebGLProgram program)
		{
			gl.Viewport(0, 0, gl.DrawingBufferWidth, gl.DrawingBufferHeight);
			gl.ClearColor(0, 0, 0, 1);
			gl.Clear(gl.COLOR_BUFFER_BIT);

			WebGLHelpers.DrawImage(gl, textureLoader.Textures.FirstOrDefault(), program, 
				0, 0, 64, 64,
				0, 0, 64, 64);

			/*
			for (var y = 0; y <= 960; y += 64)
			{
				for (var x = 0; x <= 960; x += 64)
				{
					if (((x + y) & 64) != 0)
						WebGLHelpers.DrawImage(gl, textureLoader.Textures.FirstOrDefault(), program, 64, 64, x, y);
				}
			}
			*/
		}

		static WebGLProgram CreateShaderProgram(WebGLRenderingContext gl, WebGLShader vertShader, WebGLShader fragShader)
		{
			var prog = gl.CreateProgram().As<WebGLProgram>();

			gl.AttachShader(prog, vertShader);
			gl.AttachShader(prog, fragShader);
			gl.LinkProgram(prog);

			var status = gl.GetProgramParameter(prog, gl.LINK_STATUS);
			if (status.As<bool>())
				return prog;

			var info = gl.GetProgramInfoLog(prog);
			gl.DeleteProgram(prog);

			throw new InvalidOperationException($"Unable to link program. Details: {info}");
		}

		static WebGLShader CreateShader(WebGLRenderingContext gl, int type, string source)
		{
			var shader = gl.CreateShader(type);
			gl.ShaderSource(shader, source);
			gl.CompileShader(shader);

			var status = gl.GetShaderParameter(shader, gl.COMPILE_STATUS);
			if (status.As<bool>())
				return shader;

			var info = gl.GetShaderInfoLog(shader);
			gl.DeleteShader(shader);

			throw new InvalidOperationException($"Unable to compile shader. Details: {info}");
		}

		static WebGLRenderingContext GetWebGLRenderingContext(HTMLCanvasElement canvas)
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
