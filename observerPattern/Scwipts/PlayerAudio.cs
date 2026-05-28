using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PlayerAudio : AudioStreamPlayer3D, IPlayerObserve
{
	[Export] public PlayerController pc;
	[Export] public AudioStreamPlayer hurt;
	[Export] public AudioStreamPlayer Jump;

	[Export] public AudioStreamPlayer Hit;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pc.Attach(this); // Makes this an observer to the playercontroller
	}
	// Plays the sounds 
	public void OnHit()
	{
		Hit.Play();
	}
	public void OnHurt(int health)
	{
		hurt.Play();
	}
	public void OnJump()
	{
		Jump.Play();
	}

	

}
