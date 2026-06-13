using UnityEngine;

// L’IA attaque si l’ennemi est en vue et qu’il lui reste des munitions
public class AttackState : IBotState
{
    private BotController bot;

    public AttackState(BotController bot) { this.bot = bot; }

    public void Enter()
    {
        Debug.Log("Entering ATTACK");
        bot.SetTarget(bot.enemyTarget); // Suit l'ennemi
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

        // Si en cooldown de reload, ne tire pas
        if (!bot.IsReloadCooldownOver())
        {
            Debug.Log("Cooldown en cours après reload...");
            return;
        }

        if (bot.Ammo > 0)
        {
            // S'il est à distance de tir, tirer ; sinon, s'approcher
            if (bot.IsInShootingRange())
            {
                bot.SetTarget(bot.transform);
                if (bot.CanShoot())
                    bot.Shoot();
                else
                    Debug.Log("Tir en cooldown...");
            }

            else
            {
                bot.SetTarget(bot.enemyTarget); // S'approche
            }
        }
        else
        {
            bot.StateMachine.ChangeState(new ReloadState(bot));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting ATTACK");
    }
}