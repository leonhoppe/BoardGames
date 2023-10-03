using BoardGames.Board.Graphics.Rendering;

namespace BoardGames.Board.Objects.Components; 

public class Renderer : ScreenComponent {
    public Shader Shader { get; set; }
    
    public override void Load() {
        Shader.Load();
    }

    public override void Update() {
        Shader.SetMatrix4("projection", Object.Scene.MainCamera.Projection);
        Shader.SetMatrix4("model", Object.Model);
    }

    public override void Render() {
        Shader.Render();
    }

    public override void Destroy() {
        Shader.Destroy();
    }
}