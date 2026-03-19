using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PlayerAudio : AudioStreamPlayer3D, IPlayerObserve
{
	[Export] public PlayerController pc;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc.Attach(this);
	}

	public void OnHit()
	{
		PlaySfx("Hit");
	}
	public void OnHurt(int health)
	{
		PlaySfx("Hurt");
	}
	public void OnJump()
	{
		PlaySfx("Jump");
	}

	private void PlaySfx(string name)
	{
		//Audio logic
	}

}
