using OpenTK.Mathematics;

namespace BoardGames.Board.Math; 

public struct Transform {
    public Vector2 Position { get; set; }
    public Vector3 Scale { get; set; }
    public float Rotation { get; set; }

    public Transform() {
        Position = Vector2.Zero;
        Scale = Vector3.One;
        Rotation = 0.0f;
    }

    public void Translate(Vector2 translation) {
        Position += translation;
    }

    public void Rotate(float angle) {
        Rotation += angle;
    }

    public Matrix4 CreateTransformationMatrix() {
        var pos = Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0.0f));
        var scale = Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z);
        var rot = Matrix4.CreateRotationZ(-Rotation);

        return scale * rot * pos;
    }
}