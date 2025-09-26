using UnityEngine;

[CreateAssetMenu(menuName = "Endless/Config/PlayerConfig", fileName = "PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Auto-Run")]
    public float startSpeed = 6f;
    public float maxSpeed = 16f;
    public float acceleration = 2.5f;

    [Header("Jump")]
    public float jumpForce = 7.5f;
    public float coyoteTime = 0.12f;
    public float jumpBuffer = 0.15f;

    [Header("Gravity")]
    public float extraGravity = 15f;
    public float fallGravityMultiplier = 1.4f;

    [Header("Ground")]
    public LayerMask groundMask;
    public float groundCheckRadius = 0.25f;
    public Vector3 groundCheckOffset = new Vector3(0f, 0.1f, 0f);
}
