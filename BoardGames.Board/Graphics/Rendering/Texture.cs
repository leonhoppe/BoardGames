using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace BoardGames.Board.Graphics.Rendering; 

public class Texture : IResource {
    public const string Default = "tex_default";
    
    public int Width { get; private set; }
    public int Height { get; private set; }

    private int _handle;
    private string _path;
    private bool _loaded;

    public Texture(string path) {
        _path = path != null ? ResourceLoader<IResource>.ResourceDirectory + path : null;
    }
    
    public void Load() {
        if (_loaded) return;

        _handle = GL.GenTexture();
        Bind();
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        if (string.IsNullOrEmpty(_path)) {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 1, 1, 0, PixelFormat.Rgb, PixelType.UnsignedByte, new byte[] {0xFF, 0xFF, 0xFF});
            Unbind();

            Width = 1;
            Height = 1;
            _loaded = true;
            return;
        }

        var image = ImageResult.FromStream(File.OpenRead(_path), ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        Unbind();

        Width = image.Width;
        Height = image.Height;
        _loaded = true;
    }

    public void Destroy() {
        _loaded = false;
        Unbind();
        GL.DeleteTexture(_handle);
    }

    public void Bind() {
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }

    public void Unbind() {
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }
}