using Bridge.Html5;
using Bridge.WebGL;
using ProductiveRage.Immutable;
using System;
using System.Collections.Generic;

namespace Game1
{
	public sealed class ProgramInfo : IAmImmutable
	{
		public ProgramInfo(WebGLProgram program)
		{
			this.CtorSet(_ => _.Program, program);
		}

		public WebGLProgram Program { get; }
	}

	public sealed class ProgramManager
	{
		public Dictionary<string, ProgramInfo> Programs { get; private set; }

		public ProgramManager(WebGLRenderingContext gl, Set<Tuple<string, string, string>> shaders)
		{
			Programs = new Dictionary<string, ProgramInfo>();

			foreach (var shader in shaders)
			{
				var vertShaderElement = Document.GetElementById(shader.Item2);
				var fragShaderElement = Document.GetElementById(shader.Item3);

				Programs.Add(shader.Item1,
					new ProgramInfo(program: CreateShaderProgram(gl,
						vertShader: CreateShader(gl, gl.VERTEX_SHADER, vertShaderElement.InnerHTML),
						fragShader: CreateShader(gl, gl.FRAGMENT_SHADER, fragShaderElement.InnerHTML)
					))
				);
			}
		}

		WebGLProgram CreateShaderProgram(WebGLRenderingContext gl, WebGLShader vertShader, WebGLShader fragShader)
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

		WebGLShader CreateShader(WebGLRenderingContext gl, int type, string source)
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
	}
}
