using UnityEngine;
using System;

public class PartyMember : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Health health;
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private Animator animator;
    
    [Header("Visual")]
    [SerializeField] private GameObject highlight;
    
    public Health Health => health;
    public PlayerMotor Motor => motor;
    public CharacterStats Stats => stats;
    public Animator Animator => animator;
    public CharacterType CharacterType => stats ? stats.characterType : CharacterType.Knight;
    public bool IsAlive => health && !health.IsDead;
    
    public static event Action<PartyMember> OnMemberDied;
    
    void Awake()
    {
        if (!health) health = GetComponent<Health>();
        if (!motor) motor = GetComponent<PlayerMotor>();
        if (!animator) animator = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        if (health)
        {
            Health.OnDied += OnHealthDied;
        }
    }
    
    void OnDisable()
    {
        if (health)
        {
            Health.OnDied -= OnHealthDied;
        }
    }
    
    private void OnHealthDied(Health healthComponent)
    {
        if (healthComponent == health)
        {
            OnMemberDied?.Invoke(this);
        }
    }
    
    public void SetActive(bool active)
    {
        if (motor) motor.enabled = active;
        if (highlight) highlight.SetActive(active);
    }
}
