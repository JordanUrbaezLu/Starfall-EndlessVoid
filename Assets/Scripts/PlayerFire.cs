using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerFire : MonoBehaviour
{
    [Header("Missiles")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int maxMissiles = 3;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private float fireCooldown = 0.3f; // Delay between auto-fire shots

    [Header("Laser Bolts")]
    [SerializeField] private GameObject laserBoltPrefab;   // new physical bolt prefab
    [SerializeField] private Transform laserFirePoint;     // where bolts spawn
    [SerializeField] private float laserFireRate = 0.1f;   // seconds between bolts

    private int currentMissiles;
    private bool canShoot = true;
    private bool isFiringMissiles;
    private bool isFiringLaser;
    private bool isReloading;

    private float nextLaserTime = 0f;

    void Awake()
    {
        currentMissiles = maxMissiles;
    }

    // --- INPUT CALLBACKS ---
    public void OnFireMissile(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isFiringMissiles = true;
            StartCoroutine(AutoFireLoop());
        }
        else if (ctx.canceled)
        {
            isFiringMissiles = false;
        }
    }

    public void OnLaser(InputAction.CallbackContext ctx)
    {
        isFiringLaser = ctx.ReadValueAsButton();
    }

    void Update()
    {
        HandleLaser();
    }

    // ---------- MISSILE LOGIC ----------
    private IEnumerator AutoFireLoop()
    {
        while (isFiringMissiles)
        {
            if (canShoot && !isReloading && currentMissiles > 0)
            {
                FireMissile();
                yield return new WaitForSeconds(fireCooldown);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void FireMissile()
    {
        if (!missilePrefab || !firePoint) return;

        Instantiate(missilePrefab, firePoint.position + firePoint.forward * 3f, firePoint.rotation);
        currentMissiles--;

        if (currentMissiles <= 0)
        {
            StartCoroutine(ReloadMissiles());
        }
    }

    private IEnumerator ReloadMissiles()
    {
        isReloading = true;
        canShoot = false;

        yield return new WaitForSeconds(reloadTime);

        currentMissiles = maxMissiles;
        canShoot = true;
        isReloading = false;
    }

    // ---------- LASER BOLTS ----------
    private void HandleLaser()
    {
        if (!isFiringLaser) return;
        if (!laserBoltPrefab || !laserFirePoint) return;

        if (Time.time >= nextLaserTime)
        {
            Instantiate(laserBoltPrefab, laserFirePoint.position, laserFirePoint.rotation);
            nextLaserTime = Time.time + laserFireRate;
        }
    }
}
