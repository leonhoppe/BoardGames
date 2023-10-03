using System.Drawing;
using BoardGames.Board.Graphics;
using BoardGames.Board.Graphics.Rendering;
using OpenTK.Mathematics;

namespace BoardGames.Board; 

public class Program {

    public static void Main(string[] args) {
        LoadDefaultResources();
        
        using var window = new Window(720, "BoardGames", (16, 9), true);
        window.Load();

        while (window.Exists && !window.ShouldClose) {
            window.Update();
            window.Render();
        }
        
        window.Destroy();
    }

    private static void LoadDefaultResources() {
        ResourceLoader<Mesh>.Add(Mesh.Default, new Mesh {
            Vertices = new[] {
                new Vector2(-0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, -0.5f),
                new Vector2(-0.5f, -0.5f)
            },

            Colors = new[] { Color.White },

            UVs = new[] {
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f)
            },

            Triangles = new[] {
                0, 1, 2,
                0, 2, 3
            }
        });
        ResourceLoader<Texture>.Add(Texture.Default, new Texture(null));
    }
    
}