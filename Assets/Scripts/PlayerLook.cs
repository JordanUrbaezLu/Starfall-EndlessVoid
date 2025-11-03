using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLook : MonoBehaviour
{
    private Vector2 lookInput;
    [SerializeField] private float rotationSpeed = 100f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Calculate pitch (up/down) and yaw (left/right)
        float yaw = lookInput.x * rotationSpeed * Time.fixedDeltaTime;
        float pitch = -lookInput.y * rotationSpeed * Time.fixedDeltaTime;

        // Apply rotation smoothly with physics timestep
        Quaternion deltaRotation = Quaternion.Euler(pitch, yaw, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
