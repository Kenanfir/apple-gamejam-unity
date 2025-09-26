using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerConfig config;
    [SerializeField] private GroundCheck groundCheck;

    [Header("Input")]
    [Tooltip("Assign Input Action: Gameplay/Jump")]
    [SerializeField] private InputActionReference jumpAction;

    private Rigidbody rb;
    private float targetSpeed;
    private float lastGroundedTime;
    private float lastJumpPressedTime;
    private bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        targetSpeed = config ? config.startSpeed : 6f;
    }

    void OnEnable()
    {
        if (jumpAction) { jumpAction.action.performed += OnJumpPerformed; jumpAction.action.Enable(); }
    }
    void OnDisable()
    {
        if (jumpAction) { jumpAction.action.performed -= OnJumpPerformed; jumpAction.action.Disable(); }
    }

    void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        lastJumpPressedTime = Time.time;
        jumpQueued = true;
    }

    void Update()
    {
        if (groundCheck && groundCheck.IsGrounded) lastGroundedTime = Time.time;
        if (config) targetSpeed = Mathf.Min(config.maxSpeed, targetSpeed + config.acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        var v = rb.linearVelocity;
        // Player stays stationary - environment moves instead
        v.x = 0f; // No forward movement for player

        float extraG = config ? config.extraGravity : 15f;
        float fallMul = config ? config.fallGravityMultiplier : 1.4f;
        float g = Physics.gravity.y + (-extraG * (v.y <= 0f ? fallMul : 1f));
        v.y += g * Time.fixedDeltaTime;

        if (jumpQueued)
        {
            bool withinBuffer = Time.time - lastJumpPressedTime <= (config ? config.jumpBuffer : 0.15f);
            bool withinCoyote = Time.time - lastGroundedTime <= (config ? config.coyoteTime : 0.12f);
            if (withinBuffer && withinCoyote)
            {
                v.y = (config ? config.jumpForce : 7.5f);
                jumpQueued = false;
            }
        }

        rb.linearVelocity = v;
    }
    
    // Get current target speed for environment movement
    public float GetCurrentSpeed()
    {
        return targetSpeed;
    }
}
