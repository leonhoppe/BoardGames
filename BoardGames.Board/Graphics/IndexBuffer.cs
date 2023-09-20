using OpenTK.Graphics.OpenGL;

namespace BoardGames.Board.Graphics; 

public class IndexBuffer {
    public static readonly BufferTarget Target = BufferTarget.ElementArrayBuffer;
    public int[] Indices { get; set; }
    public BufferUsageHint DrawMode { get; set; } = BufferUsageHint.StaticDraw;
    public int Handle { get; private set; }

    public void Load() {
        Handle = GL.GenBuffer();
        Bind();
        Reload();
        GL.BindBuffer(Target, 0);
    }

    public void Bind() {
        GL.BindBuffer(Target, Handle);
    }

    public void Reload() {
        GL.BufferData(Target, Indices.Length * sizeof(int), Indices, DrawMode);
    }

    public void Destroy() {
        GL.DeleteBuffer(Handle);
    }
}