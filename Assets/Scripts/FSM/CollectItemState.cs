using UnityEngine;

public class CollectItemState : IBotState
{
    private BotController bot;
    private GameObject nearestItem;

    public CollectItemState(BotController bot)
    {
        this.bot = bot;
    }

    public void Enter()
    {
        Debug.Log("Entering COLLECT ITEM");
        nearestItem = FindClosestItem();

        if (nearestItem != null)
            bot.SetTarget(nearestItem.transform);
        else
            bot.StateMachine.ChangeState(new PatrolState(bot)); // Aucun item trouvé
    }

    public void Execute()
    {
        if (bot.Health >= bot.MaxHealth || nearestItem == null)
        {
            bot.StateMachine.ChangeState(new PatrolState(bot));
            return;
        }

        bot.SetTarget(nearestItem.transform);

        float distance = Vector2.Distance(bot.transform.position, nearestItem.transform.position);
        if (distance < 0.25f) // Bonus ramassé
        {
            Debug.Log("Arrivé sur item.");
            // CollectItem() sera appelé via le HealthItem
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting COLLECT ITEM");
    }

    private GameObject FindClosestItem()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("HealthItem");
        GameObject closest = null;
        float minDist = float.MaxValue;

        foreach (var item in items)
        {
            float dist = Vector2.Distance(bot.transform.position, item.transform.position);
            if (dist < minDist)
            {
                closest = item;
                minDist = dist;
            }
        }

        return closest;
    }
}