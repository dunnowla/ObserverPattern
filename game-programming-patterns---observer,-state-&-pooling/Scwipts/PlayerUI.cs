using Godot;
using System;

public partial class PlayerUI : Label3D, IPlayerObserve
{
	[Export] public PlayerController pc;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc.Attach(this);
		Text = "Health: 3";
	}
	public void OnHurt(int currentHealth)
	{
		if(currentHealth > 0)
		{
			Text = $"Health: {currentHealth}";
		}
		else
		{
			Text = $"u dead g";
		}
	}

	public void OnHit(){}
	public void OnJump(){}
	

}
