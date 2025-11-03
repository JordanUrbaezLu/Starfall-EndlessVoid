using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float damagePerSecond = 20f;

    private bool isFiring;

    void Update()
    {
        if (!isFiring)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, maxDistance))
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Asteroid"))
            {
                var asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                    asteroid.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, firePoint.position + firePoint.forward * maxDistance);
        }
    }

    public void StartFiring() => isFiring = true;
    public void StopFiring() => isFiring = false;
}
