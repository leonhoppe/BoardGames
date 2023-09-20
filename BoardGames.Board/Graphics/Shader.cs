using OpenTK.Graphics.OpenGL;

namespace BoardGames.Board.Graphics; 

public class Shader {
    public required string VertexSource { get; init; }
    public required string FragmentSource { get; init; }
    
    public int Handle { get; private set; }

    public void Compile() {
        int vertexHandle = GL.CreateShader(ShaderType.VertexShader);
        int fragmentHandle = GL.CreateShader(ShaderType.FragmentShader);
        
        GL.ShaderSource(vertexHandle, VertexSource);
        GL.ShaderSource(fragmentHandle, FragmentSource);

        GL.CompileShader(vertexHandle);
        GL.CompileShader(fragmentHandle);

        string vertexError = GL.GetShaderInfoLog(vertexHandle);
        if (vertexError != String.Empty) Console.Error.WriteLine(vertexError);

        string fragmentError = GL.GetShaderInfoLog(fragmentHandle);
        if (fragmentError != String.Empty) Console.Error.WriteLine(fragmentError);
        
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexHandle);
        GL.AttachShader(Handle, fragmentHandle);
        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, vertexHandle);
        GL.DetachShader(Handle, fragmentHandle);

        GL.DeleteShader(vertexHandle);
        GL.DeleteShader(fragmentHandle);
    }

    public void Destroy() {
        GL.DeleteProgram(Handle);
    }

    public void Bind() {
        GL.UseProgram(Handle);
    }
}