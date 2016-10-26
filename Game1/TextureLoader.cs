using Bridge.Html5;
using Bridge.WebGL;
using ProductiveRage.Immutable;
using System;

namespace Game1
{
	public sealed class TextureInfo
	{
		public TextureInfo(WebGLTexture texture, Optional<int> width, Optional<int> height)
		{
			Texture = texture;
			Width = width;
			Height = height;
		}

		public WebGLTexture Texture { get; }
		public Optional<int> Width { get; set; }
		public Optional<int> Height { get; set; }

		public event Action OnLoad;

		public static TextureInfo Create(WebGLRenderingContext gl, string fileName)
		{
			var texture = gl.CreateTexture();
			var textureInfo = new TextureInfo(
				texture: texture,
				width: Optional<int>.Missing,
				height: Optional<int>.Missing
			);

			var image = new HTMLImageElement();
			image.Src = fileName;
			image.OnLoad = (ev) =>
			{
				textureInfo.Width = image.Width;
				textureInfo.Height = image.Height;

				gl.BindTexture(gl.TEXTURE_2D, textureInfo.Texture);
				gl.TexImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, image);
				gl.TexParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);

				textureInfo.OnLoad?.Invoke();
			};

			return textureInfo;
		}
	}

	public sealed class TextureLoader : IAmImmutable
	{
		uint texturesToLoad;

		public TextureLoader(Set<TextureInfo> textures)
		{
			this.CtorSet(_ => _.Textures, textures);
		}

		public Set<TextureInfo> Textures { get; }

		public event Action AllLoaded;

		public void ProcessTextures()
		{
			texturesToLoad = Textures.Count;

			for (var i = 0u; i < texturesToLoad; i++)
			{
				Textures[i].OnLoad += HandleTextureLoad;
			}
		}

		void HandleTextureLoad()
		{
			texturesToLoad--;

			if (texturesToLoad == 0)
			{
				for (var i = 0u; i < Textures.Count; i++)
				{
					Textures[i].OnLoad -= HandleTextureLoad;
				}

				AllLoaded?.Invoke();
			}
		}
	}
}
