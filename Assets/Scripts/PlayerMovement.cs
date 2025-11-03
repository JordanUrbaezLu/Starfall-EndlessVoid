using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    [Header("Movement Settings")]
    [SerializeField] private float thrust = 20f;
    [SerializeField] private float maxSpeed = 40f;
    [SerializeField, Range(0f, 1f)] private float strafeSpeedMultiplier = 0.75f;
    [SerializeField, Range(0f, 1f)] private float momentumBlend = 0.1f; // how fast velocity redirects

    [Header("Thruster FX")]
    [SerializeField] private ParticleSystem thrusterMain;

    private bool isStrafing;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

#if UNITY_6_0_OR_NEWER
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.2f;
#else
        rb.drag = 0.1f;
        rb.angularDrag = 0.2f;
#endif

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // --- INPUT CALLBACKS ---
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnStrafe(InputAction.CallbackContext ctx)
    {
        isStrafing = ctx.ReadValueAsButton();
    }

    void FixedUpdate()
    {
        // Input-based desired movement direction
        Vector3 desiredDir =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        if (desiredDir.sqrMagnitude < 0.01f)
            return;

        desiredDir.Normalize();

        float currentThrust = isStrafing ? thrust * strafeSpeedMultiplier : thrust;

#if UNITY_6_0_OR_NEWER
        Vector3 currentVel = rb.linearVelocity;
#else
        Vector3 currentVel = rb.velocity;
#endif

        Vector3 targetVel = desiredDir * currentThrust;

        if (isStrafing)
        {
            // Gradually redirect momentum toward new heading
            Vector3 blendedVel = Vector3.Lerp(currentVel, targetVel, momentumBlend);

#if UNITY_6_0_OR_NEWER
            rb.linearVelocity = blendedVel;
#else
            rb.velocity = blendedVel;
#endif
        }
        else
        {
            // Normal forward thrust physics
            rb.AddForce(desiredDir * currentThrust, ForceMode.Acceleration);
        }

        // Cap top speed
#if UNITY_6_0_OR_NEWER
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
#else
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
#endif

        UpdateThruster(moveInput.y);
    }

    void UpdateThruster(float forwardValue)
    {
        if (!thrusterMain) return;

        var emission = thrusterMain.emission;

        if (forwardValue > 0.05f)
        {
            float intensity = Mathf.Clamp01(forwardValue);
            emission.rateOverTime = Mathf.Lerp(0f, 60f, intensity);
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }
}
