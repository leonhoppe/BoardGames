using BoardGames.Board.Objects;

namespace BoardGames.Board.Graphics; 

public class Scene : IWindowEvents {
    public Camera MainCamera = new();
    
    private List<ScreenObject> _objects = new();

    public ScreenObject Instantiate(ScreenObject obj) {
        obj.SetScene(this);
        _objects.Add(obj);
        return obj;
    }
    
    public ScreenObject Instantiate() {
        return Instantiate(new ScreenObject());
    }

    public void Destroy(ScreenObject obj) {
        obj.Destroy();
        obj.SetScene(null);
        _objects.Remove(obj);
    }
    
    public void Load() {
        _objects.ForEach(o => o.Load());
    }

    public void Update() {
        MainCamera.UpdateProjection();
        _objects.ForEach(o => o.Update());
    }

    public void Render() {
        _objects.ForEach(o => o.Render());
    }

    public void Destroy() {
        _objects.ForEach(o => o.Destroy());
    }
}