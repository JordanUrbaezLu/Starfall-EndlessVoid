using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Transform target;

    private float speed;
    private float size;
    private float health;
    private int scoreValue;
    private Rigidbody rb;
    private Vector3 moveDirection;

    // Expose size for other scripts (e.g., LaserBolt)
    public float Size => size;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Size variance
        size = Random.Range(4f, 16f);
        transform.localScale = Vector3.one * size;

        // Speed / HP / Score scale
        speed = Random.Range(10f, 25f) + (2f / size);
        health = Mathf.Round(size * 10f);
        scoreValue = Mathf.RoundToInt(size * 3.5f);

        moveDirection = target
            ? (target.position - transform.position).normalized
            : Vector3.back;

        rb.angularVelocity = Random.insideUnitSphere * 0.3f;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

        if (target && Vector3.Distance(target.position, transform.position) > 500f)
            Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f)
        {
            GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage();
            Destroy(gameObject);
        }
    }
}
