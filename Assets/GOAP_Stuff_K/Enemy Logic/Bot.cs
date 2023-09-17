using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bot : EnemyParent
{
	/* 
	Kyle's things to do:
	1. Fix animations and the model rotation of the bot
	2. create logics for actions
	3. create preconditions for those actions
	3. create after effects for those actions
	4. create a stamina system for the bot, to avoid action spamming
	5. Fix the detection range of the bot
	6. 
	*/


	// Use this for initialization
	void Start()
	{
		health = 50;
		movementSpeed = 20;

		terminalSpeed = movementSpeed / 10;
		initialSpeed = (movementSpeed / 10) / 2;
		acceleration = (movementSpeed / 10) / 4;

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
		goal.Add(new KeyValuePair<string, object>("stayAlive", true));
		return goal;
	}
}