using UnityEngine;

public class BotJumpAction : GOAPAction
{
    private bool jumped = false;
    
    private Animator currAnim; // Reference to the Animator component

    private void SetJumpedToTrue()
    {
        jumped = true;
    }

    public BotJumpAction()
    {
        addEffect("evadePlayer", true);
        cost = 500f; // Change later
    }

    void Start()
    {
        // Get a reference to the Animator component
        currAnim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        
    }


    public override void reset()
    {
        jumped = false;
        target = null;
    }

    public override bool isDone()
    {
        return jumped;
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


        return true;

    }


    public override bool perform(GameObject agent)
    {
        Bot currBot = agent.GetComponent<Bot>();

        if (currBot.stamina >= (500 - cost))
        { 

        }

        return true;
           
    }
}
