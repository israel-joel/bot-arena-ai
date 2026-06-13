using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

// Contrôleur principal du bot. Gère l'état courant et les données du bot.
public class BotController : MonoBehaviour
{
    public Text lifeText;
    public Text ammoText;
    public Text stateText;
    public BotStateMachine StateMachine { get; private set; }
    public AIDestinationSetter destinationSetter; // Référence au script du package A*
    public Transform[] patrolPoints; // Points de patrouille possibles
    public Transform enemyTarget; // Référence à l'ennemi

    [Header("Life")]
    public int MaxHealth = 100;
    public int Health;

    [Header("change state variables")]
    public float visionRange = 10f;
    public float fleeThreshold = 25f; // Pourcentage de vie pour fuir

    [Header("Shoot")]
    public int Ammo;
    public int MaxAmmo = 5;
    public float fireCooldown = 1f;       // Durée minimale entre deux tirs
    private float lastFireTime = -10f;     // Temps du dernier tir
    public float minAttackDistance = 3f;     // Distance minimale avant de tirer
    public float reloadCooldown = 1.5f;      // Temps d’attente après rechargement
    public float lastReloadTime = -10f;      // Horodatage du dernier rechargement

    [Header("Shooting System")]
    public GameObject bulletPrefab;
    public Transform firePoint;



    void Awake()
    {
        StateMachine = GetComponent<BotStateMachine>();
    }

    void Start()
    {
        Health = MaxHealth;
        Ammo = MaxAmmo;
        StateMachine.ChangeState(new PatrolState(this)); // État initial
    }

    void Update()
    {
        lifeText.text = $"Bot : {Health}%";
        ammoText.text = $"Ammo : {Ammo}";

        //transform.position.z = 0;
        //transform.position = new Vector3(destinationSetter.target.position.x, destinationSetter.target.position.y, 0);
    }

    public bool IsAtDestination()
    {
        // Vérifie si l'IA est arrivée à sa cible
        return Vector3.Distance(transform.position, destinationSetter.target.position) < 0.5f;
    }

    public void SetRandomPatrolPoint()
    {
        // Choisit un point de patrouille aléatoire
        destinationSetter.target = patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    public bool CanSeeEnemy()
    {
        // Vérifie si l'ennemi est dans la portée de vision
        if (enemyTarget == null) return false;
        return Vector3.Distance(transform.position, enemyTarget.position) <= visionRange;
    }

    public void SetTarget(Transform t)
    {
        destinationSetter.target = t;
    }

    public bool IsInShootingRange()
    {
        return Vector3.Distance(transform.position, enemyTarget.position) <= minAttackDistance;
    }

    public bool CanShoot()
    {
        return (Time.time - lastFireTime) >= fireCooldown;
    }

    public void Shoot()
    {
        if (!CanShoot() || enemyTarget == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.SetTarget(enemyTarget);

        Ammo--;
        lastFireTime = Time.time;

        Debug.Log("Bot SHOOTS");
        stateText.text = "Bot SHOOTS";
    }


    public void Reload()
    {
        Debug.Log("Bot is RELOADING");
        stateText.text = "Bot is RELOADING";
        Ammo = MaxAmmo;
        lastReloadTime = Time.time; // Enregistre l'heure du rechargement
    }

    public bool IsReloadCooldownOver()
    {
        return (Time.time - lastReloadTime) >= reloadCooldown;
    }


    public void Flee()
    {
        Debug.Log("Bot is FLEEING");
        stateText.text = "Bot is FLEEING";

        // Trouve le point le plus éloigné de l'ennemi
        Transform farthest = patrolPoints[0];
        //float maxDist = 0;
        //foreach (var point in patrolPoints)
        //{
        //    float dist = Vector3.Distance(transform.position, point.position);
        //    if (dist > maxDist)
        //    {
        //        maxDist = dist;
        //        farthest = point;
        //    }
        //}
        destinationSetter.target = farthest;
    }

    public void CollectItem(int amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
        Debug.Log($"Bot healed by {amount}. Current HP: {Health}");
        stateText.text = $"Bot healed by {amount}.";
    }


    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"Enemy took {damage} damage (HP left: {Health})");
        stateText.text = $"Enemy took {damage} damage";

        if (Health <= 0)
        {
            lifeText.text = $"Bot : {Health}%";
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        stateText.text = "Enemy died";
        Destroy(gameObject); // Ou animation, effets, etc.
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, visionRange);
    }
    
    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, minAttackDistance);
    }
}