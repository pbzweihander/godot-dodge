using Godot;
using System;

public class HUD : CanvasLayer
{
    private Timer messageTimer;
    private Label messageLabel;
    private Label scoreLabel;
    private Button startButton;

    public override void _Ready()
    {
        messageTimer = (Timer)GetNode("MessageTimer");
        messageLabel = (Label)GetNode("MessageLabel");
        scoreLabel = (Label)GetNode("ScoreLabel");
        startButton = (Button)GetNode("StartButton");
    }

    [Signal]
    public delegate void StartGame();

    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");
        await ToSignal(messageTimer, "timeout");
        messageLabel.Text = "Dodge the\nCreeps!";
        messageLabel.Show();
        startButton.Show();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        startButton.Hide();

        EmitSignal("StartGame");
    }

    public void OnMessageTimerTimeout()
    {
        messageLabel.Hide();
    }
}
