using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene Mob;

    public int Score = 0;

    // Note: We're going to use this many times, so instantiating it
    // allows our numbers to consistently be random.
    private Random rand = new Random();

    private Timer mobTimer;
    private Timer scoreTimer;
    private Timer startTimer;
    private Player player;
    private Position2D startPosition;
    private PathFollow2D mobSpawnLocation;
    private HUD hud;
    private AudioStreamPlayer music;
    private AudioStreamPlayer deathSound;

    public override void _Ready()
    {
        mobTimer = (Timer)GetNode("MobTimer");
        scoreTimer = (Timer)GetNode("ScoreTimer");
        startTimer = (Timer)GetNode("StartTimer");
        player = (Player)GetNode("Player");
        startPosition = (Position2D)GetNode("StartPosition");
        mobSpawnLocation = (PathFollow2D)GetNode("MobPath/MobSpawnLocation");
        hud = (HUD)GetNode("HUD");
        music = (AudioStreamPlayer)GetNode("Music");
        deathSound = (AudioStreamPlayer)GetNode("DeathSound");
    }

    // We'll use this later because C# doesn't support GDScript's randi().
    private float RandRand(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }

    public void GameOver()
    {
        scoreTimer.Stop();
        mobTimer.Stop();

        hud.ShowGameOver();

        music.Stop();
        deathSound.Play();
    }

    public void NewGame()
    {
        Score = 0;

        player.Start(startPosition.Position);
        startTimer.Start();

        hud.UpdateScore(Score);
        hud.ShowMessage("Get Ready!");

        music.Play();
    }

    public void OnStartTimerTimeout()
    {
        mobTimer.Start();
        scoreTimer.Start();
    }

    public void OnScoreTimerTimeout()
    {
        Score += 1;

        hud.UpdateScore(Score);
    }

    public void OnMobTimerTimeout()
    {
        // Choose a random location on Path2D.
        mobSpawnLocation.SetOffset(rand.Next());

        // Create a Mob instance and add it to the scene.
        var mobInstance = (RigidBody2D)Mob.Instance();
        AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mobInstance.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRand(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

        // Choose the velocity.
        mobInstance.SetLinearVelocity(new Vector2(RandRand(150f, 250f), 0).Rotated(direction));
    }
}
