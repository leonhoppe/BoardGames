using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace BoardGames.Board.Graphics.Rendering; 

public class Mesh : IRenderEvents, IResource {
    public const string Default = "mesh_default";
    
    public Vector2[] Vertices { get; set; }
    public Color[] Colors { get; set; }
    public Vector2[] UVs { get; set; }
    public int[] Triangles { get; set; }
    public float[] Data { get; private set; }

    private int _vao;
    private int _vbo;
    private int _iao;
    private readonly BufferUsageHint _streamMode;
    private readonly PrimitiveType _renderMode;
    private bool _loaded;

    public Mesh(BufferUsageHint streamMode = BufferUsageHint.StaticDraw, PrimitiveType renderMode = PrimitiveType.Triangles) {
        Vertices = Array.Empty<Vector2>();
        Colors = Array.Empty<Color>();
        UVs = Array.Empty<Vector2>();
        Triangles = Array.Empty<int>();
        _streamMode = streamMode;
        _renderMode = renderMode;
    }
    
    public void Load() {
        if (_loaded || Triangles.Length == 0) return;

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();
        _iao = GL.GenBuffer();
        
        Update();
        
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);

        _loaded = true;
    }

    public void Update() {
        var data = new List<float>();

        for (int i = 0; i < Vertices.Length; i++) {
            var vertex = Vertices[i];
            var color = Colors[i % Colors.Length];
            var uv = UVs[i % UVs.Length];
            
            data.AddRange(new [] {
                vertex.X,
                vertex.Y,
                
                color.R / 255f,
                color.G / 255f,
                color.B / 255f,
                color.A / 255f,
                
                uv.X,
                uv.Y
            });
        }

        Data = data.ToArray();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, Data.Length * sizeof(float), Data, _streamMode);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iao);
        GL.BufferData(BufferTarget.ElementArrayBuffer, Triangles.Length * sizeof(int), Triangles, _streamMode);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public void Render() {
        GL.DrawElements(_renderMode, Triangles.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void Destroy() {
        _loaded = false;
        Unbind();
        GL.DeleteBuffer(_vbo);
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_iao);
    }

    public void Bind() {
        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iao);
    }

    public void Unbind() {
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }
}