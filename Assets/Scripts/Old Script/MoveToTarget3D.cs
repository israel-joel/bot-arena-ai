using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveToTarget3D : MonoBehaviour
{
    [Header("Cible et mouvement")]
    public Transform target;
    public float moveSpeed = 3f;           // unités/seconde
    public float stopDistance = 0.5f;      // arrêt à cette distance du waypoint

    [Header("Évitement d'obstacles")]
    public LayerMask obstacleLayer;
    public float obstacleCheckDistance = 1f;  // portée du Raycast

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Verrouille la rotation si tu veux éviter les rotations dues aux collisions
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = rb.position;
        Vector3 targetPos = target.position;
        // On ignore la composante Y
        Vector3 delta = new Vector3(targetPos.x - currentPos.x, 0f, targetPos.z - currentPos.z);

        // Si on est assez près, on n'avance plus
        if (delta.magnitude <= stopDistance) return;

        // Choix de l'axe de déplacement : priorité X ou Z
        Vector3 primaryDir, secondaryDir;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.z))
        {
            primaryDir = new Vector3(Mathf.Sign(delta.x), 0f, 0f);
            secondaryDir = new Vector3(0f, 0f, Mathf.Sign(delta.z));
        }
        else
        {
            primaryDir = new Vector3(0f, 0f, Mathf.Sign(delta.z));
            secondaryDir = new Vector3(Mathf.Sign(delta.x), 0f, 0f);
        }

        // Test obstacle sur la direction prioritaire
        bool blockedPrimary = Physics.Raycast(
            currentPos,
            primaryDir,
            obstacleCheckDistance,
            obstacleLayer
        );

        Vector3 moveDir = Vector3.zero;
        if (!blockedPrimary)
        {
            moveDir = primaryDir;
        }
        else
        {
            // si obstacle, tenter l'axe secondaire
            bool blockedSecondary = Physics.Raycast(
                currentPos,
                secondaryDir,
                obstacleCheckDistance,
                obstacleLayer
            );
            if (!blockedSecondary)
                moveDir = secondaryDir;
            // sinon moveDir reste zéro (bloqué)
        }

        // Applique le mouvement
        if (moveDir != Vector3.zero)
        {
            Vector3 newPos = currentPos + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    // (Optionnel) pour visualiser les Raycasts dans l'éditeur
    void OnDrawGizmosSelected()
    {
        if (rb == null) return;
        Gizmos.color = Color.red;
        Vector3 origin = rb.position;
        if (target != null)
        {
            Vector3 delta = target.position - origin;
            Vector3 primary = Mathf.Abs(delta.x) > Mathf.Abs(delta.z)
                ? new Vector3(Mathf.Sign(delta.x), 0, 0)
                : new Vector3(0, 0, Mathf.Sign(delta.z));
            Gizmos.DrawLine(origin, origin + primary * obstacleCheckDistance);
        }
    }
}
