using Godot;
using System;

public class Mob : RigidBody2D
{
    [Export]
    public int MinSpeed = 150;

    [Export]
    public int MaxSpeed = 250;

    private String[] mobTypes = { "walk", "swim", "fly" };

    private AnimatedSprite animatedSprite;
    private Random random = new Random();

    public override void _Ready()
    {
        animatedSprite = (AnimatedSprite)GetNode("AnimatedSprite");
        animatedSprite.Animation = mobTypes[random.Next(0, mobTypes.Length)];
    }

    public void onVisibilityScreenExited()
    {
        QueueFree();
    }
}
