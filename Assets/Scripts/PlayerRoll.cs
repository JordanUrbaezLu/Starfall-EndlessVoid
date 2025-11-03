using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRoll : MonoBehaviour
{
    private float rollInput;
    [SerializeField] private float rollSpeed = 120f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Input System callback
    public void OnRoll(InputAction.CallbackContext ctx)
    {
        rollInput = ctx.ReadValue<float>();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rollInput) > 0.01f)
        {
            // Negative sign makes right bumper roll right naturally
            float roll = -rollInput * rollSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 0f, roll));
        }
    }
}
