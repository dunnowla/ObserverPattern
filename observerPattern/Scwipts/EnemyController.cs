using Godot;
using System;

public partial class EnemyController : CharacterBody3D
{
	public enum States {Idle,Chasing,Waiting,Dead} // The different states

	[Export] public Node3D player;
	[Export] public float speed = 3f;
	[Export] public float detectRange = 10f;
	[Export] public float waitTime = 2f;

	private States currentState = States.Idle;
	private float waitTimer = 0f;
	private float timePassed = 0f;

    public override void _PhysicsProcess(double delta)
    {
        timePassed += (float)delta;
		switch(currentState) // The "Brain"
		{
			case States.Idle:
				IdleState();
				break;
			case States.Chasing:
				ChaseState(delta);
				break;
			case States.Waiting:
				WaitState(delta);
				break;
			case States.Dead:
				Die();
				break;
		}
		// Makes the enemy move up and down if its alive
		if(currentState != States.Dead)
		{
			float newY = (float)Math.Sin(timePassed * 5f) * 0.1f;
			GetNode<Node3D>("Visuals").Position = new Vector3(0,newY,0);
		}
    }

	private void IdleState()
	{
		if(GlobalPosition.DistanceTo(player.GlobalPosition) < detectRange)
		{
			currentState = States.Chasing; // Chases the player if it gets too close
		}
	}

	private void ChaseState(double delta)
	{
		Vector3 direction = (player.GlobalPosition - GlobalPosition).Normalized();
		direction.Y = 0; // Makes the enemy stay on the ground
		Velocity = direction * speed;
		MoveAndSlide();
		// If player gets far enough away it goes back to idle
		if(GlobalPosition.DistanceTo(player.GlobalPosition)> detectRange +2f)
		{
			currentState = States.Idle;
		}
	}

	private void WaitState(double delta)
	{
		// Starts the timer for the waiting
		waitTimer -= (float) delta;
		if(waitTimer <= 0)
		{
			currentState = States.Chasing;
		}
	}

	public void Die()
	{
		currentState = States.Dead;
		GetNode<Node3D>("Visuals/Button").Position = new Vector3(0,0.125f,0); // Visual change to enemy to show its dead
		SetPhysicsProcess(false); // Stop it from moving
	}

	public void HeadAreaHit(Node3D body)
	{
		// If the player collides with the enemies head the enemy dies
		if(body is PlayerController player)
		{
			player.HitEnemy(); 
			Die();
		}
	}
	public void BodyAreaHit(Node3D body)
	{
		// If the enemy collides with the player with its body
		// It damages the player and makes the enemy wait before keeping up teh chase
		if(currentState != States.Dead && body is PlayerController player)
		{
			player.TakeDamage();

			currentState = States.Waiting;
			waitTimer = waitTime;
		}
	}

}
