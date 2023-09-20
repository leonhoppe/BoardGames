using System.ComponentModel;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BoardGames.Board.Graphics;

public class BoardWindow : NativeWindow {
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;
    private Shader _shader;

    public BoardWindow(string title, (int, int) aspectRatio, Vector2i dimensions, bool fullscreen) : base(new NativeWindowSettings {
        Title = title,
        AspectRatio = aspectRatio,
        Size = dimensions,
        WindowBorder = fullscreen ? WindowBorder.Hidden : WindowBorder.Fixed,
        StartVisible = true,
        StartFocused = true,
        Vsync = VSyncMode.On,
        
        API = ContextAPI.OpenGL,
        APIVersion = new Version(3, 3),
        Profile = ContextProfile.Core
    }) {
        CenterWindow();
    }

    public void Load() {
        GL.ClearColor(Color.White);

        _vertexBuffer = new VertexBuffer(7) {
            Vertices = new [] {
                // X,   Y,    Z;    R,    G,    B,    A
                -0.5f,  0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
                 0.5f,  0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
                 0.5f, -0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f,
                -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f,
            }
        };

        _indexBuffer = new IndexBuffer {
            Indices = new[] {
                0, 1, 2,
                0, 2, 3
            }
        };
        
        _vertexBuffer.Load();
        _indexBuffer.Load();
        
        _vertexBuffer.BindVertexArray();
        _vertexBuffer.Bind();
        _vertexBuffer.AddVertexPointer(0, 3, 0);
        _vertexBuffer.AddVertexPointer(1, 4, 3);
        GL.BindBuffer(VertexBuffer.Target, 0);
        GL.BindVertexArray(0);

        _shader = new Shader {
            VertexSource = @"
                #version 330 core
                layout (location = 0) in vec3 aPosition;
                layout (location = 1) in vec4 aColor;

                out vec4 fColor;

                void main() {
                    fColor = aColor;
                    gl_Position = vec4(aPosition, 1.0f);
                }
            ",

            FragmentSource = @"
                #version 330 core
                in vec4 fColor;
                out vec4 color;

                void main() {
                    color = fColor;
                }
            "
        };
        _shader.Compile();
    }
    
    public void Destroy() {
        _vertexBuffer.Destroy();
        _indexBuffer.Destroy();
        _shader.Destroy();
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit);
            
        _shader.Bind();
        _vertexBuffer.BindVertexArray();
        _indexBuffer.Bind();
            
        GL.DrawElements(PrimitiveType.Triangles, _indexBuffer.Indices.Length, DrawElementsType.UnsignedInt, 0);
        
        Context.SwapBuffers();
    }

    protected override void OnClosing(CancelEventArgs e) {
        Destroy();
        Environment.Exit(-1);
    }
}