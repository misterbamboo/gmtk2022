using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using System;
using UnityEngine;

public class DecisionScreenLoader : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        GameEvents.Subscribe<CombatEndedEvent>(OnCombatEnded);
    }

    private void OnCombatEnded(CombatEndedEvent endedEvent)
    {
        if (endedEvent.PlayerWon)
        {
            Load();
        }
    }

    public void Load()
    {
        Instantiate(prefab, GameObject.Find("Canvas").transform);
    }
}
