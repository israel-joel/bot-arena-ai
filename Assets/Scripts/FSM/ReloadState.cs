using UnityEngine;

// L’IA recharge ses munitions lorsqu’elle n’en a plus
public class ReloadState : IBotState
{
    private BotController bot;
    private float reloadTime = 2f;
    private float timer = 0f;

    public ReloadState(BotController bot) { this.bot = bot; }

    public void Enter()
    {
        Debug.Log("Entering RELOAD");
        bot.stateText.text = "Entering RELOAD";
        timer = 0f;

        // Pendant le rechargement, le bot fuit si l’ennemi est proche
        if (bot.enemyTarget != null && bot.IsInShootingRange())
            bot.Flee();
    }

    public void Execute()
    {
        timer += Time.deltaTime;

        if (bot.Health <= bot.fleeThreshold)
        {
            bot.StateMachine.ChangeState(new FleeState(bot));
            return;
        }

        if (timer >= reloadTime)
        {
            bot.Reload();

            if (bot.CanSeeEnemy())
                bot.StateMachine.ChangeState(new ChaseState(bot));
            else
                bot.StateMachine.ChangeState(new PatrolState(bot));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting RELOAD");
    }
}