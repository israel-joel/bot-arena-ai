using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MoveToTarget : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    public float stopDistance = 0.5f;
    public float jumpForce = 7f;
    public float movementForce = 10f; // force horizontale
    public float maxSpeed = 3f;
    public LayerMask obstacleLayer;
    public float obstacleCheckDistance = 1f;
    public float minObstacleHeightToJump = 0.5f; // Hauteur min à franchir

    private Rigidbody2D rb;
    private Collider2D col;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > stopDistance)
        {
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;

            // Détection d'obstacles (BoxCast devant le bot)
            Vector2 boxSize = new Vector2(0.5f, 0.5f);
            Vector2 castOrigin = (Vector2)transform.position + Vector2.up * 0.1f;
            RaycastHit2D hit = Physics2D.BoxCast(castOrigin, boxSize, 0f, Vector2.right * Mathf.Sign(direction.x), obstacleCheckDistance, obstacleLayer);

            if (hit.collider != null && isGrounded)
            {
                float obstacleHeight = hit.collider.bounds.max.y - col.bounds.min.y;

                if (obstacleHeight > minObstacleHeightToJump)
                {
                    Jump();
                }
            }

            // Appliquer une force horizontale si pas encore à la vitesse max
            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                rb.AddForce(new Vector2(direction.x * movementForce, 0f));
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset Y
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    // Pour détecter si le bot touche le sol après un saut
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
