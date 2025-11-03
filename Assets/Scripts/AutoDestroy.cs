using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        // Try to destroy when the particle system finishes
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            // fallback (if no particle system)
            Destroy(gameObject, 2f);
        }
    }
}
