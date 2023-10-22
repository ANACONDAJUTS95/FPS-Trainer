using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotJumpAction : GOAPAction // NOW WORKING ATM!
{
    private bool jumping = false;
    private float jumpHeight = 1.0f; // Adjust this to control the jump height
    private float jumpSpeed = 5.0f; // Adjust this to control the jump speed
    private Vector3 jumpStartPosition;
    private Vector3 jumpEndPosition;
    private float jumpStartTime;

    public BotJumpAction()
    {
        addEffect("evadePlayer", true);
        cost = 100f; // Most prioritized action
    }

    public override void reset()
    {
        jumping = false;
        target = null;
    }

    public override bool isDone()
    {
        return !jumping;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = GameObject.Find("Player");
        Bot currBot = agent.GetComponent<Bot>();

        bool isPlayerZoomedIn = target.GetComponent<PlayerMovement>().isPlayerZoomedIn;

        if (target != null && currBot.stamina >= (500 - cost) && isPlayerZoomedIn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool perform(GameObject agent)
    {
        Bot currBot = agent.GetComponent<Bot>();

        if (!jumping)
        {
            // Record the original position and target jump position
            jumpStartPosition = agent.transform.position;
            jumpEndPosition = jumpStartPosition + Vector3.up * jumpHeight;

            // Start the jump
            jumpStartTime = Time.time;
            jumping = true;
        }

        // Check if the jump has completed
        if (jumping)
        {
            float jumpProgress = (Time.time - jumpStartTime) * jumpSpeed;
            Vector3 newPosition = Vector3.Lerp(jumpStartPosition, jumpEndPosition, jumpProgress);

            // Ensure the bot doesn't overshoot the target position
            if (jumpProgress >= 1.0f)
            {
                newPosition = jumpEndPosition;
                jumping = false;
            }

            agent.transform.position = newPosition;
        }

        return true;
    }
}
