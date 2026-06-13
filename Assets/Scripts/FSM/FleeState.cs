using UnityEngine;

// L’IA fuit si sa vie descend en dessous d’un seuil
public class FleeState : IBotState
{
    private BotController bot;

    public FleeState(BotController bot) { this.bot = bot; }

    public void Enter()
    {
        Debug.Log("Entering FLEE");
        bot.Flee(); // Se dirige vers le point le plus éloigné
    }

    public void Execute()
    {
        if (bot.Health > bot.fleeThreshold)
        {
            if (bot.Ammo > 0 && bot.CanSeeEnemy())
                bot.StateMachine.ChangeState(new ChaseState(bot));
        }
        if (bot.Health <= 25)
        {
            bot.StateMachine.ChangeState(new CollectItemState(bot));
        }
        else
        {
            bot.StateMachine.ChangeState(new PatrolState(bot));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting FLEE");
    }
}

