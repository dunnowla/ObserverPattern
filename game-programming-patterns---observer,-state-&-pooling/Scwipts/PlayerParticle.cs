using Godot;
using System;

public partial class PlayerParticle : GpuParticles3D, IPlayerObserve
{
	[Export] public PlayerController pc;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc.Attach(this);
	}

	public void OnHurt(int health)
	{
		Emitting = true;
	}

	public void OnHit(){}

	public void OnJump(){}


}
