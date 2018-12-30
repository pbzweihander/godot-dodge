using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public int Speed = 400;
    private Vector2 screenSize;

    private AnimatedSprite animatedSprite;
    private CollisionShape2D collisionShape2D;

    public override void _Ready()
    {
        screenSize = GetViewport().GetSize();
        animatedSprite = (AnimatedSprite)GetNode("AnimatedSprite");
        collisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
    }

    public override void _Process(float delta)
    {
        var velocity = new Vector2();
        if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= 1;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        Position += velocity * delta;
        Position = new Vector2(
            Mathf.Clamp(Position.x, 0, screenSize.x),
            Mathf.Clamp(Position.y, 0, screenSize.y)
        );

        if (velocity.x != 0)
        {
            animatedSprite.Animation = "right";
            animatedSprite.FlipH = velocity.x < 0;
            animatedSprite.FlipV = false;
        }
        else if (velocity.y != 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = velocity.y > 0;
        }
    }

    [Signal]
    public delegate void Hit();

    public void OnPlayerBodyEntered(Godot.Object body)
    {
        Hide();
        EmitSignal("Hit");

        collisionShape2D.Disabled = true;
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();

        collisionShape2D.Disabled = false;
    }
}
