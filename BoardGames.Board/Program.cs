using BoardGames.Board.Graphics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BoardGames.Board; 

public class Program {

    public static void Main(string[] args) {
        var window = new BoardWindow("BoardGames", (9, 16), new Vector2i(720, 1280), false);
        window.Load();

        while (window.Exists) {
            GLFW.PollEvents();
            window.Render();
        }
    }
    
}