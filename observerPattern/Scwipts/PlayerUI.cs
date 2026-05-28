using Godot;
using System;

public partial class PlayerUI : Label3D, IPlayerObserve
{
	[Export] public PlayerController pc;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc.Attach(this);
		Text = "Health: 3"; // Sets the starting hp
	}
	public void OnHurt(int currentHealth)
	{
		if(currentHealth > 0)
		{
			Text = $"Health: {currentHealth}"; // displays the health
		}
		else
		{
			Text = $"u dead g"; // says your dead
		}
	}

	public void OnHit(){}
	public void OnJump(){}
	

}
