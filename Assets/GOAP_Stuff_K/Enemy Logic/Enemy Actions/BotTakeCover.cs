using UnityEngine;

public class BotTakeCover : GOAPAction
{
    private bool moved = false;
    private bool isStrafing = false;
    private float strafeSpeed = 4f;
    private Vector3 strafeDirection;
    private float strafeDuration = 2.0f;
    private float strafeTimer = 0f;

    private Animator currAnim; // Reference to the Animator component

    private void SetMovedToTrue()
    {
        moved = true;
    }

    public BotTakeCover()
    {
        addEffect("evadePlayer", true);
        cost = 400f; // Change later
    }

    void Start()
    {
        // Get a reference to the Animator component
        currAnim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        // Get a reference to the Bot component
        Bot currBot = GetComponentInParent<Bot>();

        if (isStrafing)
        {
            // Move the bot left or right based on strafeDirection
            transform.Translate(strafeDirection * strafeSpeed * Time.deltaTime);

            // Update the strafe timer
            strafeTimer += Time.deltaTime;

            if (strafeTimer >= strafeDuration)
            {
                isStrafing = false;
                strafeTimer = 0f; // Reset the timer
            }
        }

        // Set isMoving based on whether strafing or moving forward
        bool isMoving = isStrafing || (currBot.stamina >= (500 - cost) && !isStrafing);
        currAnim.SetBool("isMoving", isMoving);
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
            // There's an obstacle, reverse the direction
            ReverseStrafeDirection();
            return false;
        }
        return true; // No obstacle, can strafe in this direction
    }

    private void ReverseStrafeDirection()
    {
        // Reverse the strafe direction
        strafeDirection = -strafeDirection;
    }

    public override bool perform(GameObject agent)
    {
        Bot currBot = agent.GetComponent<Bot>();

        if (currBot.stamina >= (500 - cost) && !isStrafing)
        {
            currBot.stamina -= (500 - cost);

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
                // If there's an obstacle, reverse the direction
                ReverseStrafeDirection();
            }

            // Reset the strafe timer
            strafeTimer = 0f;

            isStrafing = true;
            currAnim.SetBool("isMoving", true); // Set isMoving to true when strafing starts

            Debug.Log("BOT is strafing!");
            SetMovedToTrue();
            return true;
        }
        else if (isStrafing)
        {
            // Check if the bot can continue strafing in the current direction
            if (!CanStrafe(strafeDirection))
            {
                // If there's an obstacle, reverse the direction
                ReverseStrafeDirection();
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
