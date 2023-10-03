using BoardGames.Board.Graphics;

namespace BoardGames.Board.Objects.Components; 

public abstract class ScreenComponent : IWindowEvents {
    public ScreenObject Object { get; set; }
    
    public abstract void Load();
    public abstract void Update();
    public abstract void Render();
    public abstract void Destroy();
}