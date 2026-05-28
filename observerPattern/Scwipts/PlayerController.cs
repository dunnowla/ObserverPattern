using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Threading;

public partial class PlayerController : CharacterBody3D
{
	// Observer variables
	public List<IPlayerObserve> observers = new List<IPlayerObserve>();
	private int health = 3;

	//Movement Variables
	[Export] public float speed = 10f;
	[Export] public float jumpHeight = 8f;
	[Export] public Camera3D cam;

	private float customGravity;
	private Node3D point; // The pivot point for the rotation 

    public override void _Ready()
    {
        point = GetNode<Node3D>("PivotPoint");
		customGravity = GetGravity().Y;
    }


    public override void _PhysicsProcess(double delta)
    {
		Move(delta);
		Jump(delta);
		Rotate(delta);
		MoveAndSlide();
	}



	public void Attach(IPlayerObserve observer)
	{
		observers.Add(observer); // Adds observer
	}
	public void Detach(IPlayerObserve observer)
	{
		observers.Remove(observer); // removes observer
	}

	public void TakeDamage()
	{
		// Reduces the player health
		health --;
		foreach(var observer in observers)
		{
			observer.OnHurt(health); // calls the hurt function in the observers
		}
	}

	public void Jump(double delta)
	{
		Vector3 velocity = Velocity;

		if(!IsOnFloor())
		{
			velocity.Y -= 15f * (float)delta; // Makes the player fall in the air
		}
		if(Input.IsActionJustPressed("jump")&& IsOnFloor())
		{
			velocity.Y = jumpHeight;
			foreach(var observer in observers)
			{
				observer.OnJump(); // Calls all the jump fucntions in the observers
			}
		}
		Velocity = velocity;

	}

	public void HitEnemy()
	{
		foreach(var observer in observers)
		{
			observer.OnHit(); // calls the hit function in the observers
		}

		Velocity = new Vector3(Velocity.X,jumpHeight * 0.8f,Velocity.Z); // Makes the player jump a bit if it hits the enemy head
	}

	private void Move(double delta)
	{
		// simplified version of the movescript from assignment 1
		Vector3 velocity = Velocity;
		float xMove = Input.GetAxis("moveLeft","moveRight");
		float zMove = Input.GetAxis("moveUp","moveDown");

		Vector3 camForward = cam.GlobalTransform.Basis.Z;
		Vector3 camRight = cam.GlobalTransform.Basis.X;
		camForward.Y = 0;
		camRight.Y = 0;
		camForward = camForward.Normalized();
		camRight = camRight.Normalized();

		Vector3 norm = (camRight * xMove + camForward * zMove).Normalized();

		if (norm != Vector3.Zero)
        {
            velocity.X = norm.X * speed;
            velocity.Z = norm.Z * speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(velocity.Z, 0, speed);
        }
		Velocity = velocity;
	}

	private void Rotate(double delta)
	{
		// rotates the player 
		Vector3 norm = new Vector3(Velocity.X,0,Velocity.Z);
		if(norm != Vector3.Zero)
		{
			Basis targetBasis = Basis.LookingAt(norm, Vector3.Up);
            point.Basis = point.Basis.Slerp(targetBasis, 12 * (float)delta);
		}	
	}
}
