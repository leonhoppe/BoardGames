using OpenTK.Graphics.OpenGL;

namespace BoardGames.Board.Graphics; 

public class VertexBuffer {
    public static readonly BufferTarget Target = BufferTarget.ArrayBuffer;
    public float[] Vertices { get; set; }
    public BufferUsageHint DrawMode { get; set; } = BufferUsageHint.StaticDraw;
    public int Handle { get; private set; }
    public int VertexArray { get; private set; }
    public int Stride { get; private set; }

    public VertexBuffer(int stride) {
        Stride = stride;
    }

    public void Load() {
        Handle = GL.GenBuffer();
        VertexArray = GL.GenVertexArray();
        Bind();
        Reload();
        GL.BindBuffer(Target, 0);
    }

    public void Reload() {
        GL.BufferData(Target, Vertices.Length * sizeof(float), Vertices, DrawMode);
    }

    public void Bind() {
        GL.BindBuffer(Target, Handle);
    }

    public void BindVertexArray() {
        GL.BindVertexArray(VertexArray);
    }

    public void AddVertexPointer(int index, int size, int offset) {
        GL.VertexAttribPointer(index, size, VertexAttribPointerType.Float, false, Stride * sizeof(float), offset * sizeof(float));
        GL.EnableVertexAttribArray(index);
    }

    public void Destroy() {
        GL.DeleteBuffer(Handle);
        GL.DeleteVertexArray(VertexArray);
    }
}