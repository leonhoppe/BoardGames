using BoardGames.Board.Math;
using OpenTK.Mathematics;

namespace BoardGames.Board.Graphics; 

public struct Camera {
    public Transform Transform = new();
    public float NearClip = 0.01f;
    public float FarClip = 100.0f;

    public Matrix4 Projection { get; private set; }

    public Camera() {
        UpdateProjection();
    }

    public void UpdateProjection() {
        Projection = Transform.CreateTransformationMatrix();
    }
}
