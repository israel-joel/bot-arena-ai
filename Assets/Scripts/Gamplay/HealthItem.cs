using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 50;
    public float pickupRadius = 0.25f;

    private void Update()
    {
        GameObject[] bots = GameObject.FindGameObjectsWithTag("Bot");   // Création d'un array de bot
        foreach (GameObject bot in bots)
        {
            float distance = Vector2.Distance(transform.position, bot.GetComponent<Transform>().position);  // Calcule de la distance qui separe les bot du bonus

            if (distance <= pickupRadius)
            {
                Debug.Log("Bonus Touché par le Bot");
                bot.GetComponent<BotController>().CollectItem(healAmount);  // Lancement de la fontion de collect de bonus du bot
                Destroy(gameObject);
                break;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

}
