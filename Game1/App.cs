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
				.Add(TextureInfo.Create(gl, "../Content/Images/sprites.png")));
			var programManager = new ProgramManager(gl, Set<Tuple<string, string, string>>.Empty
				.Add(Tuple.Create("prog-image", "image-vert-shader", "image-frag-shader"))
				.Add(Tuple.Create("prog-line", "line-vert-shader", "line-frag-shader")));

			textureLoader.AllLoaded += () => Start(gl, textureLoader, programManager);
			textureLoader.ProcessTextures();	
		}

		static void Start(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager)
		{
			Global.SetInterval(() => Render(gl, textureLoader, programManager), 200);
		}

		static void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager)
		{
			gl.Viewport(0, 0, gl.DrawingBufferWidth, gl.DrawingBufferHeight);
			gl.ClearColor(0, 0, 0, 1);
			gl.Clear(gl.COLOR_BUFFER_BIT);

			for (var y = 0; y <= 960; y += 64)
			{
				for (var x = 0; x <= 960; x += 64)
				{
					if (((x + y) & 64) != 0)
						WebGLHelpers.DrawImage(gl, textureLoader.Textures.First(), programManager.Programs["prog-image"].Program, 0, 0, 64, 64, x, y, 64, 64);
				}
			}

			WebGLHelpers.DrawLine(gl, programManager.Programs["prog-line"].Program, 
				new RGBAColour(255, 255, 0, 255), 
				300, 200, 100, 50);
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
