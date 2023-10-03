namespace BoardGames.Board.Graphics.Rendering; 

public interface IRenderEvents : IWindowEvents {
    void Bind();
    void Unbind();
}