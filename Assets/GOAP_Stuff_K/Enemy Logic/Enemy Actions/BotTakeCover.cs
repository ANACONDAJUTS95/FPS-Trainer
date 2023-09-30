using UnityEngine;

public class BotTakeCover : GOAPAction
{
    private bool moved = false;
    private bool isStrafing = false;
    private float strafeSpeed = 3f;
    private Vector3 strafeDirection;
    private float strafeDuration = 2.0f;
    private float strafeTimer = 0f;

    private void SetMovedToTrue()
    {
        moved = true;
    }

    public BotTakeCover()
    {
        addEffect("evadePlayer", true);
        cost = 400f; //change later
    }

    void Update()
    {
        if (isStrafing)
        {
            // Move the bot left or right based on strafeDirection
            transform.Translate(strafeDirection * strafeSpeed * Time.deltaTime);

            // Update the strafe timer
            strafeTimer += Time.deltaTime;

            if (strafeTimer >= strafeDuration)
            {
                isStrafing = false;
            }
        }
    }

    public override void reset()
    {
        moved = false;
        target = null;
    }

    public override bool isDone()
    {
        return moved;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = GameObject.Find("Player");
        Bot currBot = agent.GetComponent<Bot>();
        bool isPlayerShooting = target.GetComponent<PlayerMovement>().isPlayerShooting;

        if (target != null && currBot.stamina >= (500 - cost) && isPlayerShooting)
        {
            Debug.Log("BotTakeCover precondition is a go");
            Debug.Log("Bot is STRAFING");
            return true;
        }
        else
        {
            Debug.Log("BotTakeCover precondition is a no no");
            return false;
        }
    }

    private bool CanStrafe(Vector3 direction)
    {
        // Perform a raycast to check if there's an obstacle in the strafe direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 2.0f))
        {
            return false; // There's an obstacle, cannot strafe in this direction
        }
        return true; // No obstacle, can strafe in this direction
    }

    public override bool perform(GameObject agent)
    {
        Bot currBot = agent.GetComponent<Bot>();

        if (currBot.stamina >= (500 - cost) && !isStrafing)
        {
            currBot.stamina -= (500 - cost);

            Animator currAnim = GetComponentInParent<Animator>();
            currAnim.SetBool("isMoving", true);

            // Determine the strafe direction (left or right)
            int strafeDirectionIndex = Random.Range(0, 2);
            if (strafeDirectionIndex == 0)
            {
                strafeDirection = Vector3.left;
            }
            else
            {
                strafeDirection = Vector3.right;
            }

            // Check if the bot can strafe in the chosen direction
            if (!CanStrafe(strafeDirection))
            {
                // If there's an obstacle, strafe in the opposite direction
                strafeDirection = -strafeDirection;
            }

            // Reset the strafe timer
            strafeTimer = 0f;

            isStrafing = true;

            Debug.Log("BOT is strafing!");
            SetMovedToTrue();
            return true;
        }
        else
        {
            return false;
        }
    }
}
