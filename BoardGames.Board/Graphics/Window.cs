using System.ComponentModel;
using System.Drawing;
using BoardGames.Board.Math;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace BoardGames.Board.Graphics; 

public class Window : NativeWindow, IWindowEvents {

    public Scene MainScene { get; }
    public bool ShouldClose { get; private set; }
    public (float, float) Aspect { get; }
    public bool Vertical { get; }

    public Window(int size, string title, (float, float) aspect, bool vertical = false) : base(new NativeWindowSettings {
        Title = title,
        Size = vertical ? new Vector2i(size, (int)(size / (aspect.Item2 / aspect.Item1))) : new Vector2i((int)(size * (aspect.Item1 / aspect.Item2)), size),
        WindowBorder = WindowBorder.Hidden,
        StartVisible = true,
        StartFocused = true,
        Vsync = VSyncMode.On,
        
        API = ContextAPI.OpenGL,
        APIVersion = new Version(3, 3),
        Profile = ContextProfile.Core
    }) {
        Aspect = aspect;
        Vertical = vertical;
        MainScene = new Scene();
        CenterWindow();
        StbImage.stbi_set_flip_vertically_on_load(1);

        Closing += IsClosing;
    }

    public void Load() {
        GL.ClearColor(Color.Black);
        
        MainScene.MainCamera.Transform.Rotation = Vertical ? 0 : Single.Pi / 2f;
        MainScene.MainCamera.Transform.Scale = new Vector3(1, Aspect.Item2 / Aspect.Item1, 1);
        MainScene.MainCamera.UpdateProjection();
        
        MainScene.Load();
    }

    public void Update() {
        Time.DeltaTime = (float)GLFW.GetTime() - Time.TimeElappsed;
        Time.TimeElappsed = (float)GLFW.GetTime();
        
        GLFW.PollEvents();
        MainScene.Update();
    }

    public void Render() {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        MainScene.Render();
        
        Context.SwapBuffers();
    }

    public void Destroy() {
        MainScene.Destroy();
    }

    private void IsClosing(CancelEventArgs e) {
        ShouldClose = true;
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        Destroy();
    }
}