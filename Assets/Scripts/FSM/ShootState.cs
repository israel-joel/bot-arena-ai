using UnityEngine;

// L’IA tire si l’ennemi est à portée, sinon retourne en chasse
public class ShootState : IBotState
{
    private BotController bot;

    public ShootState(BotController bot) { this.bot = bot; }

    public void Enter()
    {
        Debug.Log("Entering SHOOT");
        bot.SetTarget(bot.transform); // Stopper mouvement
    }

    public void Execute()
    {
        if (bot.Health <= bot.fleeThreshold)
        {
            bot.StateMachine.ChangeState(new FleeState(bot));
            return;
        }

        if (!bot.CanSeeEnemy())
        {
            bot.StateMachine.ChangeState(new PatrolState(bot));
            return;
        }

        if (bot.Ammo <= 0)
        {
            bot.StateMachine.ChangeState(new ReloadState(bot));
            return;
        }

        if (!bot.IsInShootingRange())
        {
            bot.StateMachine.ChangeState(new ChaseState(bot)); // Trop loin -> Chase
            return;
        }

        // Si possible, tirer
        if (bot.CanShoot())
        {
            bot.Shoot();
        }
        else
        {
            Debug.Log("Tir en cooldown...");
            bot.stateText.text = ("Tir en cooldown...");
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting SHOOT");
    }
}
