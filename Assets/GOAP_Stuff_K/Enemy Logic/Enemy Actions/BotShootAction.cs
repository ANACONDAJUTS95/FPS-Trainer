using UnityEngine;

public class BotShootAction : GOAPAction
{
    private bool attacked = false;
    private GameObject player;

    public BotShootAction()
    {
        addEffect("damagePlayer", true);
        cost = 300f;
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
        if (target != null && currBot.stamina >= (500 - cost))
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

        // Create logic here for damaging the player, and shooting sound and vfx

        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (player != null)
        {
            // Make the bot constantly look at the player's position
            agent.transform.LookAt(player.transform);
        }
        currBot.animator.SetTrigger("fireShot");
        Debug.Log("BOT is shooting!");

        SetAttackedToTrue();

        return true;
    }

    private void SetAttackedToTrue()
    {
        attacked = true;
    }
}
