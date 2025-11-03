using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 50f;
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (explosionPrefab)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (col.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
                asteroid.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
