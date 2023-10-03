using System.Text.RegularExpressions;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace BoardGames.Board.Graphics.Rendering; 

public class Shader : IRenderEvents {
    public const string Default = "Graphics/Shaders/default.glsl";
    
    public Mesh Mesh { get; set; }
    public Texture Texture { get; set; }

    private readonly string _vertexCode;
    private readonly string _fragmentCode;
    private bool _loaded;
    private int _handle;

    public Shader(string file, string mesh = Mesh.Default, string texture = Texture.Default, bool isFile = true) {
        var source = isFile ? File.ReadAllText(ResourceLoader<IResource>.ResourceDirectory + file) : file;
        
        string[] splitString = Regex.Split(source, "(#type [a-zA-Z]+)");
        string firstPattern = splitString[1].Replace("#type ", "").Trim();
        string secondPattern = splitString[3].Replace("#type ", "").Trim();

        if (firstPattern.ToLower().Equals("vertex"))
            _vertexCode = splitString[2];
        else if (firstPattern.ToLower().Equals("fragment"))
            _fragmentCode = splitString[2];
        else throw new IOException("Unexpected token '" + firstPattern);
            
        if (secondPattern.ToLower().Equals("vertex"))
            _vertexCode = splitString[4];
        else if (secondPattern.ToLower().Equals("fragment"))
            _fragmentCode = splitString[4];
        else throw new IOException("Unexpected token '" + secondPattern);

        Mesh = ResourceLoader<Mesh>.Get(mesh);
        Texture = ResourceLoader<Texture>.Get(texture);
    }
    
    public void Load() {
        if (_loaded) return;
        
        int vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        int fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        
        GL.ShaderSource(vertexHandle, _vertexCode);
        GL.ShaderSource(fragmentHandle, _fragmentCode);

        GL.CompileShader(vertexHandle);
        GL.CompileShader(fragmentHandle);

        string vertexError = GL.GetShaderInfoLog(vertexHandle);
        if (vertexError != String.Empty) Console.Error.WriteLine("Vertex Error: " + vertexError);

        string fragmentError = GL.GetShaderInfoLog(fragmentHandle);
        if (fragmentError != String.Empty) Console.Error.WriteLine("Fragment Error: " + fragmentError);
        
        _handle = GL.CreateProgram();
        GL.AttachShader(_handle, vertexHandle);
        GL.AttachShader(_handle, fragmentHandle);
        GL.LinkProgram(_handle);

        GL.DetachShader(_handle, vertexHandle);
        GL.DetachShader(_handle, fragmentHandle);

        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);
        
        Mesh.Load();
        Texture.Load();

        _loaded = true;
    }

    public void Update() {
        Mesh.Update();
    }

    public void Render() {
        Texture.Bind();
        Bind();
        Mesh.Bind();
        Mesh.Render();
    }

    public void Destroy() {
        Unbind();
        GL.DeleteProgram(_handle);
        Mesh.Destroy();
        Texture.Destroy();
        _loaded = false;
    }

    public void Bind() {
        GL.UseProgram(_handle);
    }

    public void Unbind() {
        GL.UseProgram(0);
    }
    
    public void SetMatrix4(string uniformName, Matrix4 matrix) {
        Bind();
        int location = GL.GetUniformLocation(_handle, uniformName);
        GL.UniformMatrix4(location, 1, false, GetMatrix4X4Values(matrix));
        Unbind();
    }

    private float[] GetMatrix4X4Values(Matrix4 m) {
        return new [] {
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44
        };
    }
}