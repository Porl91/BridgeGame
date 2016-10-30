using Bridge.Html5;
using Bridge.WebGL;
using ProductiveRage.Immutable;
using System;

namespace Game1
{
    public class App
    {
		[Ready]
		public static void Main()
		{
			var canvas = Document.GetElementById<HTMLCanvasElement>("main");
			var gl = WebGLHelpers.GetWebGLRenderingContext(canvas);
			if (gl == null)
				throw new InvalidOperationException("Your browser doesn't support WebGL rendering contexts");

			Action onWindowLoadOrResize = () =>
			{
				canvas.Width = Window.InnerWidth;
				canvas.Height = Window.InnerHeight;
			};

			Window.OnLoad = (e) => onWindowLoadOrResize();
			Window.OnResize = (e) => onWindowLoadOrResize();

			var textureLoader = new TextureLoader(Set<TextureInfo>.Empty
				.Add(TextureInfo.Create(gl, "../Content/Images/sprites.png")));

			/* Tuples of 'program ID', 'vertex shader ID' and 'fragment shader ID' */

			var programManager = new ProgramManager(gl, Set<Tuple<string, string, string>>.Empty
				.Add(Tuple.Create("prog-image", "image-vert-shader", "image-frag-shader"))
				.Add(Tuple.Create("prog-line", "line-vert-shader", "line-frag-shader")));

			var level = new Level();

			textureLoader.AllLoaded += () => Start(gl, textureLoader, programManager, level);
			textureLoader.ProcessTextures();	
		}

		static void Start(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, Level level)
		{
			Action tick = () => Tick(gl, textureLoader, programManager, level);
			tick();
			Global.SetInterval(tick, 200);
		}

		static void Tick(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, Level level)
		{
			Update(level);
			Render(gl, textureLoader, programManager, level);
		}

		static void Update(Level level)
		{
			level.Update();
		}

		static void Render(WebGLRenderingContext gl, TextureLoader textureLoader, ProgramManager programManager, Level level)
		{
			gl.Viewport(0, 0, gl.DrawingBufferWidth, gl.DrawingBufferHeight);
			gl.ClearColor(0, 0, 0, 1);
			gl.Clear(gl.COLOR_BUFFER_BIT);

			level.Render(gl, textureLoader, programManager);
		}
	}
}
