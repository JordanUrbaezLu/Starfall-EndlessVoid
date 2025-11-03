using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    [SerializeField] private float speed = 120f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject impactEffectPrefab;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
#if UNITY_6_0_OR_NEWER
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
#else
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
#endif
    }

    void OnCollisionEnter(Collision col)
    {
        if (impactEffectPrefab != null && col.contacts.Length > 0)
        {
            ContactPoint contact = col.contacts[0];
            Vector3 spawnPos = contact.point;
            float offset = 0.25f; // fallback offset

            // Dynamically compute based on asteroid size if applicable
            if (col.gameObject.CompareTag("Asteroid"))
            {
                Asteroid asteroid = col.gameObject.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    // Offset proportional to true asteroid size
                    offset = asteroid.Size * 0.03f; // ~3% of asteroid diameter
                }
                else
                {
                    // Fallback: infer from scale magnitude
                    offset = col.transform.localScale.magnitude * 0.03f;
                }
            }

            spawnPos += contact.normal * offset;
            Quaternion spawnRot = Quaternion.LookRotation(contact.normal);

            Instantiate(impactEffectPrefab, spawnPos, spawnRot);
        }

        // Damage asteroid if hit
        if (col.gameObject.CompareTag("Asteroid"))
        {
            var asteroid = col.gameObject.GetComponent<Asteroid>();
            if (asteroid != null)
                asteroid.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
