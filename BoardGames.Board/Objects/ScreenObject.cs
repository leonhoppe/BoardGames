using BoardGames.Board.Graphics;
using BoardGames.Board.Math;
using BoardGames.Board.Objects.Components;
using OpenTK.Mathematics;

namespace BoardGames.Board.Objects; 

public sealed class ScreenObject : IWindowEvents {
    public Transform Transform = new();
    public ScreenObject Parent { get; private set; }
    public Scene Scene { get; private set; }
    public Matrix4 Model { get; set; }
    public bool IsVisible { get; set; } = true;
    
    private List<ScreenComponent> Components { get; } = new();
    private List<ScreenObject> Children { get; } = new();
    private bool _loaded;

    public T AddComponent<T>(bool autoLoad = true) where T : ScreenComponent {
        var component = Activator.CreateInstance<T>();
        component.Object = this;
        if (_loaded && autoLoad) component.Load();
        Components.Add(component);
        return component;
    }

    public void RemoveComponent<T>() where T : ScreenComponent {
        Components.Remove(Components.SingleOrDefault(c => c.GetType() == typeof(T)));
    }

    public T GetComponent<T>() where T : ScreenComponent {
        return (T)Components.SingleOrDefault(c => c.GetType() == typeof(T));
    }

    public ScreenObject AddChild(ScreenObject child) {
        child.Parent = this;
        Children.Add(child);
        return child;
    }
    
    public ScreenObject AddChild() {
        var obj = new ScreenObject();
        obj.SetScene(Scene);
        return AddChild(obj);
    }

    public void RemoveChild(ScreenObject child) {
        child.Parent = null;
        Children.Remove(child);
    }

    public ScreenObject[] GetChildren() => Children.ToArray();

    public void SetScene(Scene scene) {
        Scene = scene;
        Children.ForEach(c => c.SetScene(scene));
    }
    
    public void Load() {
        CompileModel();
        Components.ForEach(c => c.Load());
        Children.ForEach(c => c.Load());
        _loaded = true;
    }

    public void Update() {
        if (!IsVisible) return;
        Components.ForEach(c => c.Update());
        Children.ForEach(c => c.Update());
        CompileModel();
    }

    public void Render() {
        if (!IsVisible) return;
        Components.ForEach(c => c.Render());
        Children.ForEach(c => c.Render());
    }

    public void Destroy() {
        Components.ForEach(c => c.Destroy());
        Children.ForEach(c => c.Destroy());
        _loaded = false;
    }

    private void CompileModel() {
        if (Parent != null)
            Model = Transform.CreateTransformationMatrix() * Parent.Model;
        else Model = Transform.CreateTransformationMatrix();
    }
}