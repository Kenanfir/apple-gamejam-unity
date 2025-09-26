using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class PartyController : MonoBehaviour
{
    [Header("Party Members")]
    [SerializeField] private List<PartyMember> members = new List<PartyMember>();
    [SerializeField] private int activeIndex = 0;
    
    [Header("Input")]
    [SerializeField] private InputActionReference switchTo1Action;
    [SerializeField] private InputActionReference switchTo2Action;
    [SerializeField] private InputActionReference switchTo3Action;
    
    public PartyMember ActiveMember => (members.Count > 0 && activeIndex >= 0 && activeIndex < members.Count) ? members[activeIndex] : null;
    public List<PartyMember> AllMembers => new List<PartyMember>(members);
    public int ActiveIndex => activeIndex;
    
    public static event Action<PartyMember> OnActiveChanged;
    public static event Action OnPartyWiped;
    
    void Awake()
    {
        // Initialize all members as inactive except the first one
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i])
            {
                members[i].SetActive(i == activeIndex);
            }
        }
        
        // Subscribe to member death events
        PartyMember.OnMemberDied += OnMemberDied;
    }
    
    void OnEnable()
    {
        if (switchTo1Action) switchTo1Action.action.performed += _ => SwitchToMember(0);
        if (switchTo2Action) switchTo2Action.action.performed += _ => SwitchToMember(1);
        if (switchTo3Action) switchTo3Action.action.performed += _ => SwitchToMember(2);
        
        if (switchTo1Action) switchTo1Action.action.Enable();
        if (switchTo2Action) switchTo2Action.action.Enable();
        if (switchTo3Action) switchTo3Action.action.Enable();
    }
    
    void OnDisable()
    {
        if (switchTo1Action) switchTo1Action.action.Disable();
        if (switchTo2Action) switchTo2Action.action.Disable();
        if (switchTo3Action) switchTo3Action.action.Disable();
        
        PartyMember.OnMemberDied -= OnMemberDied;
    }
    
    public void SwitchToMember(int index)
    {
        if (index < 0 || index >= members.Count) return;
        if (members[index] == null || !members[index].IsAlive) return;
        
        // Deactivate current member
        if (ActiveMember)
        {
            ActiveMember.SetActive(false);
        }
        
        // Activate new member
        activeIndex = index;
        if (ActiveMember)
        {
            ActiveMember.SetActive(true);
            OnActiveChanged?.Invoke(ActiveMember);
        }
    }
    
    private void OnMemberDied(PartyMember member)
    {
        // Check if all members are dead
        bool allDead = true;
        foreach (var m in members)
        {
            if (m && m.IsAlive)
            {
                allDead = false;
                break;
            }
        }
        
        if (allDead)
        {
            OnPartyWiped?.Invoke();
        }
        else
        {
            // Switch to next alive member if current one died
            if (ActiveMember == member)
            {
                SwitchToNextAliveMember();
            }
        }
    }
    
    private void SwitchToNextAliveMember()
    {
        for (int i = 0; i < members.Count; i++)
        {
            int index = (activeIndex + 1 + i) % members.Count;
            if (members[index] && members[index].IsAlive)
            {
                SwitchToMember(index);
                return;
            }
        }
    }
}
