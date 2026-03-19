using Godot;
using System;

public interface IPlayerObserve
{
	void OnJump(); // When the player jumps
	void OnHit(); // When the players hits the enemy
	void OnHurt(int currentHealth); // When the enemy hits the player
}
