using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float maxLifetime = 5f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Destroy(gameObject, maxLifetime); // Auto-destruction
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(transform.position, target.position);

        if(distance <= 0.1f)
        {

            if (target.TryGetComponent<BotController>(out var bot))
            {
                bot.TakeDamage(damage);
            }

            Destroy(gameObject); // détruire la balle après impact
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la balle touche l'obstacle ou autre chose que la cible
        if (collision.transform != target)
        {
            if (collision.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
                Debug.Log("Balle détruite par obstacle");
            }
        }
    }

}
