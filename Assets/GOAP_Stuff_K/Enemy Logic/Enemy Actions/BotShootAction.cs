using UnityEngine;
using System.Collections;

public class BotShootAction : GOAPAction
{

	private bool attacked = false;

	public BotShootAction()
	{
		addEffect("damagePlayer", true);
		cost = 100f;
	}

	public override void reset()
	{
		attacked = false;
		target = null;
	}

	public override bool isDone()
	{
		return attacked;
	}

	public override bool requiresInRange()
	{
		return true;
	}

	public override bool checkProceduralPrecondition(GameObject agent)
	{
		target = GameObject.Find("Player");
		if (target != null && target.GetComponent<PlayerMovement>().isPlayerShooting)
		{
			Debug.Log("BotShootAction precondition is a go");
			Debug.Log("Player is shooting!");
			return true;
		}
        else
        {
			Debug.Log("BotShootAction precondition is a no no");
			Debug.Log("Player is not shooting!");
			return false;
		}
		
	}

	public override bool perform(GameObject agent)
	{
		Bot currBot = agent.GetComponent<Bot>();
		//currBot.animator.SetTrigger("fireshot");


		Debug.Log("BotShootAction - Bot is shooting!");
		attacked = true;
		return true;
		
	}
}