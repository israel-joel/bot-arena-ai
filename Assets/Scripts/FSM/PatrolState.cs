using UnityEngine;
// L’IA patrouille entre plusieurs points tant qu’aucun ennemi n’est en vue
public class PatrolState : IBotState
{
    private BotController bot;

    public PatrolState(BotController bot) { this.bot = bot; }

    public void Enter()
    {
        Debug.Log("Entering PATROL");
        bot.stateText.text = "Entering PATROL";
        bot.SetRandomPatrolPoint();
    }

    public void Execute()
    {
        bot.stateText.text = "PATROL...";
        if (bot.Health <= bot.fleeThreshold)
        {
            bot.StateMachine.ChangeState(new FleeState(bot));
            return;
        }

        if (bot.CanSeeEnemy())
        {
            if (bot.Ammo > 0)
                bot.StateMachine.ChangeState(new AttackState(bot));
            else
                bot.StateMachine.ChangeState(new ReloadState(bot));
            return;
        }

        if (bot.IsAtDestination())
        {
            bot.SetRandomPatrolPoint(); // Continue à patrouiller
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting PATROL");
    }
}
