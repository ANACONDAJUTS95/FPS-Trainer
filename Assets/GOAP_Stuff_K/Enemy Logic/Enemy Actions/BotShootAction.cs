using UnityEngine;

public class BotShootAction : GOAPAction
{
    private bool attacked = false;
    private GameObject player;

    //Variables for firing
    public GameObject bullet;
    public Transform firePoint;

    public float fireRate;
    private float fireCount;

    public BotShootAction()
    {
        addEffect("damagePlayer", true);
        cost = 100f; // most priority action
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
        Bot currBot = agent.GetComponent<Bot>();
        if (target != null && currBot.stamina >= (500 - cost)) // 500 is a magic num
        {
            // Check if there's an obstacle in the line of sight before shooting
            Vector3 direction = target.transform.position - agent.transform.position;
            float maxDistance = Vector3.Distance(agent.transform.position, target.transform.position);
            bool botCanSee = !Physics.Raycast(agent.transform.position, direction, maxDistance);

            // Check if the player is running (isMakingNoise)
            bool isPlayerRunning = target.GetComponent<PlayerMovement>().isPlayerRunning;

            if (isPlayerRunning || botCanSee)
            {
                Debug.Log("BotShootAction precondition is a go");
                Debug.Log("Bot heard or saw the player.");
                return true;

            }
            else
            {
                Debug.Log("BotShootAction precondition is a no no");
                Debug.Log("Player is not running or there is an obstacle in the line of sight.");
                return false;
            }
        }
        else
        {
            Debug.Log("BotShootAction precondition is a no no");
            return false;
        }
    }

    public override bool perform(GameObject agent)
    {
        Bot currBot = agent.GetComponent<Bot>();
        currBot.stamina -= (500 - cost);

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player != null)
        {
            // Make the bot constantly look at the player's position
            agent.transform.LookAt(player.transform);

            //Making the Enemy Fire
            fireCount = fireRate;
            Debug.Log("BOT is shooting!");
            currBot.animator.SetTrigger("fireShot");

            Instantiate(bullet, firePoint.position, firePoint.rotation); 
        }

        SetAttackedToTrue();

        return true;

    }

    private void SetAttackedToTrue()
    {
        attacked = true;
    }
}
