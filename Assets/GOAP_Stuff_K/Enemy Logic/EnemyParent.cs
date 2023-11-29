using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyParent : MonoBehaviour, IGOAP
{

	public Animator animator;
	public BoxCollider boxCollider;
	public SphereCollider sphereCollider;
	public PlayerMovement player;

	public int health;
	public int movementSpeed;
	public float stamina;
	public float regenRate;
	protected float terminalSpeed;
	protected float initialSpeed;
	protected float acceleration;
	protected float minDist = 1.5f; 
	protected float aggroDist = 50f; 
	protected bool loop = false;
	protected float maxStamina;

	// Add an event to notify when the enemy dies
	public delegate void EnemyDeathAction();
	public event EnemyDeathAction OnEnemyDeath;

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Die()
	{
		// Find the enemy GameObject by its name
		GameObject enemyGameObjectHS = GameObject.Find("Enemy_GOAP_HS Prefab(Clone)"); //add (Clone)
		GameObject enemyGameObjectBS = GameObject.Find("Enemy_GOAP_BS Prefab(Clone)"); //add (Clone)

		// Deactivate the enemy GameObject
		if (enemyGameObjectBS == null)
        {
			enemyGameObjectHS.SetActive(false);
		}
		else
        {
			enemyGameObjectBS.SetActive(false);
		}
		

		// Trigger the respawn event
		if (OnEnemyDeath != null)
		{
			OnEnemyDeath();
		}
	}

	// Update is called once per frame
	public virtual void Update()
	{
		if (stamina <= maxStamina)
		{
			Invoke("passiveRegen", 1.0f);
		}
		else
		{
			stamina = maxStamina;
		}

		if (health == 0)
		{
			Die();
		}
	}

	public abstract void passiveRegen();

	public HashSet<KeyValuePair<string, object>> getWorldState()
	{
		HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
		worldData.Add(new KeyValuePair<string, object>("damagePlayer", false));
		worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
		return worldData;
	}

	public abstract HashSet<KeyValuePair<string, object>> createGoalState();

	public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
	{

	}

	public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> action)
	{

	}

	public void actionsFinished()
	{

	}

	public void planAborted(GOAPAction aborter)
	{

	}

	public void setSpeed(float val)
	{
		terminalSpeed = val / 10;
		initialSpeed = (val / 10) / 10;
		acceleration = (val / 10) / 4;
		return;
	}

	public virtual bool moveAgent(GOAPAction nextAction)
	{
		float dist = Vector3.Distance(transform.position, nextAction.target.transform.position);

		if (dist < aggroDist)
		{
			// Calculate the move direction towards the player
			Vector3 moveDirection = player.transform.position - transform.position;

			// Ensure the enemy looks at the player's position
			transform.LookAt(player.transform.position);

			setSpeed(movementSpeed);

			Vector3 newPosition = moveDirection.normalized * movementSpeed * Time.deltaTime;
			transform.position += newPosition;

			// Set the "isMoving" parameter to true to trigger the movement animation
			animator.SetBool("isMoving", true);

			Debug.Log("EnemyParent - agent is moving???");
		}
		else
		{
			// Set the "isMoving" parameter to false to stop the movement animation
			animator.SetBool("isMoving", false);
		}

		if (dist <= minDist)
		{
			animator.SetBool("isMoving", false);
			nextAction.setInRange(true);
			return true;
		}
		else
		{
			return false;
		}
	}

}