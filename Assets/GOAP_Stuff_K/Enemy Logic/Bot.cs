using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bot : EnemyParent
{

	// Initialization of the stats of the Bot
	void Start()
	{
		health = 100;
		movementSpeed = 5; // change this for the movement speed of the bot

		regenRate = 5f; //orig: 3f
		stamina = 600f; //orig: 600f
		maxStamina = 1000f; //orig: 500f

		terminalSpeed = movementSpeed / 10;
		initialSpeed = (movementSpeed / 10) / 2;
		acceleration = (movementSpeed / 10) / 4;

		minDist = 11f;

		setSpeed(movementSpeed);

		animator = GetComponent<Animator>();
		player = GameObject.Find("Player").GetComponent<PlayerMovement>(); 

	}

	public override void passiveRegen()
	{
		stamina += regenRate;
	}

	public override HashSet<KeyValuePair<string, object>> createGoalState()
	{
		HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
		goal.Add(new KeyValuePair<string, object>("damagePlayer", true));
		goal.Add(new KeyValuePair<string, object>("evadePlayer", true));
		return goal;
	}
}