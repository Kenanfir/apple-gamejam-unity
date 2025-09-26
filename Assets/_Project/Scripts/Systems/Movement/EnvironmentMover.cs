using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private bool usePlayerSpeed = true;

    [Header("References")]
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] private Transform environmentParent;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;

    private float currentSpeed;

    void Start()
    {
        if (!playerMotor)
            playerMotor = FindObjectOfType<PlayerMotor>();

        if (!environmentParent)
            environmentParent = transform;

        currentSpeed = moveSpeed;
    }

    void Update()
    {
        UpdateSpeed();
        MoveEnvironment();

        if (showDebugInfo)
        {
            Debug.Log($"Environment Speed: {currentSpeed}");
        }
    }

    private void UpdateSpeed()
    {
        if (usePlayerSpeed && playerMotor)
        {
            // Get the player's current target speed
            currentSpeed = playerMotor.GetCurrentSpeed();
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    private void MoveEnvironment()
    {
        if (!environmentParent) return;

        // Move the environment parent itself towards the player
        Vector3 moveDirection = Vector3.left * currentSpeed * Time.deltaTime;
        environmentParent.Translate(moveDirection, Space.World);
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        if (!usePlayerSpeed)
        {
            currentSpeed = speed;
        }
    }

    public void SetUsePlayerSpeed(bool usePlayer)
    {
        usePlayerSpeed = usePlayer;
        if (!usePlayerSpeed)
        {
            currentSpeed = moveSpeed;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetEnvironmentParent(Transform parent)
    {
        environmentParent = parent;
    }
}
