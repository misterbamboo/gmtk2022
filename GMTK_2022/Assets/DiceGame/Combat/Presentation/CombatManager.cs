using DiceGame.Combat.Application;
using DiceGame.Combat.Entities;
using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Events;
using DiceGame.Combat.Presentation.Animations;
using DiceGame.Combat.Presentation.Exceptions;
using DiceGame.Combat.Presentation.Inspector;
using DiceGame.SharedKernel;
using DiceGame.Turn.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using DiceGame.Assets.DiceGame.DecisionScreen.Events;
using DiceGame.Assets.DiceGame.Screens.MainMenuScreen.Events;

[RequireComponent(typeof(CombatAnimatorComponent))]
public class CombatManager : MonoBehaviour
{
    public const string Tag = "CombatManager";

    [Header("Stats")]
    [SerializeField] GameStatsManager statsManager;

    [Header("UI")]
    [SerializeField] float characterDisplayOffsetY = -1.5f;

    [Header("Player")]
    [SerializeField] PlayerPrefabDefinition playerPrefab;

    [Header("Enemies")]
    [SerializeField] int minNumberOfEnemies = 1;
    [SerializeField] int maxNumberOfEnemies = 4;
    [SerializeField] List<EnemyPrefabDefinition> enemyPrefabs;

    List<EnemyComponent> enemiesComponents = new List<EnemyComponent>();
    private PlayerComponent playerComponent;

    private CombatController combatController;

    private CombatAnimatorComponent combatAnimator;

    void Start()
    {
        combatAnimator = GetComponent<CombatAnimatorComponent>();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        GameEvents.Subscribe<NewGameRequestedEvent>(OnNewGameRequested);
        GameEvents.Subscribe<DecisionCompletedEvent>(OnDecisionCompleted);

        GameEvents.Subscribe<TurnStartedEvent>(OnTurnStarted);
        GameEvents.Subscribe<CombatStartedEvent>(EventsReceiver);
        GameEvents.Subscribe<CombatActionSentEvent>(OnEnemyDecisionTaken);
        GameEvents.Subscribe<CharacterKilledEvent>(OnCharacterKilled);

        // TODO: Implement animations
        GameEvents.Subscribe<CharacterTookDamageEvent>(TakeDamageAnimation);
        GameEvents.Subscribe<CharacterGotHealedEvent>(EventsReceiver);
        GameEvents.Subscribe<CharacterGotShieldedEvent>(EventsReceiver);
    }

    private void OnNewGameRequested(NewGameRequestedEvent obj)
    {
        StartNewCombat();
    }

    private void OnDecisionCompleted(DecisionCompletedEvent decisionCompletedEvent)
    {
        StartNewCombat();
    }

    private void StartNewCombat()
    {
        var enemyStatsPerType = statsManager.GetEnemyStats();
        var playerStats = statsManager.GetPlayerStats();

        combatController = new CombatController(minNumberOfEnemies, maxNumberOfEnemies, enemyStatsPerType, playerStats);
        combatController.StartCombat();
    }

    private void OnEnemyDecisionTaken(CombatActionSentEvent combatEvent)
    {
        var sourceTransform = FindCharacterTransform(combatEvent.CombatAction.SourceId);
        var targetTransform = FindCharacterTransform(combatEvent.CombatAction.TargetIds.First());

        // When we add the logic of hitting X times, we raise X attack event event if the
        // Target could die after 1 attack. So if the transorm is null, if probably because
        // the CharacterComponent was deleted before the Coroutine is done
        if (IsSourceOrTargetIsAlive(sourceTransform, targetTransform))
        {
            combatAnimator.QueueAnimationForEnemyDecision(combatEvent.CombatAction, sourceTransform, targetTransform, () =>
            {
                combatController.DispatchCombatAction(combatEvent.CombatAction);
            });
        }
        else
        {
            var character = FindCharacterComponent(combatEvent.CombatAction.SourceId);
            character.RequestCombatActionCancellation();
        }
    }

    private static bool IsSourceOrTargetIsAlive(Transform sourceTransform, Transform targetTransform)
    {
        return sourceTransform != null && targetTransform != null;
    }

    private void OnTurnStarted(TurnStartedEvent e)
    {
        if (!e.IsEnemyTurn) return;
        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        foreach (var enemy in enemiesComponents)
        {
            combatController.EnemyTakeTurn(enemy.CharacterId);
            yield return new WaitForSeconds(0.5f);
        }

        GameEvents.Raise<TurnEndedEvent>(new TurnEndedEvent(1));
    }

    private Transform FindCharacterTransform(int characterId)
    {
        var component = FindCharacterComponent(characterId);

        // Important to keep it like this. The component can have been Destroy()
        // But the reference is still not null. Unity override the == null and != null opperator
        // event if the C# reference exist, the result of this will be true if Destroy has been called on the GameObject
        // A simple, `component?.transform` isn't enought,
        // because the C# object is not null and don't call the == null operator or != null operator
        if (component != null)
        {
            return component.transform;
        }
        return null;
    }

    private CharacterComponent FindCharacterComponent(int id)
    {
        return id == Player.PlayerId ? playerComponent : enemiesComponents.FirstOrDefault(ec => ec.CharacterId == id);
    }

    private void EventsReceiver(IGameEvent gameEvent)
    {
        if (gameEvent is CombatStartedEvent) InitGameObjects();
    }

    private void TakeDamageAnimation(CharacterTookDamageEvent combatEvent)
    {
        var character = FindCharacterComponent(combatEvent.Id);
        character.UpdateUIs();
        character.Shake();
    }

    private void OnCharacterKilled(CharacterKilledEvent characterKilled)
    {
        DestroyCharacterComponent(characterKilled.Id);
        combatController.CheckWinningCondition();
    }

    private void DestroyCharacterComponent(int id)
    {
        if (id == Player.PlayerId)
        {
            Destroy(playerComponent.gameObject);
            return;
        }

        var enemyComponent = enemiesComponents.First(c => c.CharacterId == id);
        enemiesComponents.Remove(enemyComponent);
        Destroy(enemyComponent.gameObject);
    }

    private void InitGameObjects()
    {
        ClearEnemiesGameObjects();
        ClearPlayerGameObjects(playerComponent);

        float index = 0;
        foreach (var enemy in combatController.Enemies)
        {
            var prefab = GetEnemyPrefab(enemy.Type);

            var x = index * 1.5f;
            var y = (index % 2) + characterDisplayOffsetY;
            var enemyComponent = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            enemyComponent.Character = enemy;
            enemiesComponents.Add(enemyComponent);
            index++;
        }

        playerComponent = Instantiate(playerPrefab.component, new Vector3(-4, characterDisplayOffsetY, 0), Quaternion.identity);
        playerComponent.Character = combatController.Player;
    }

    private void ClearPlayerGameObjects(PlayerComponent playerComponent)
    {
        if (playerComponent != null)
        {
            Destroy(playerComponent.gameObject);
        }
    }

    private void ClearEnemiesGameObjects()
    {
        foreach (var enemiesComponent in enemiesComponents)
        {
            Destroy(enemiesComponent.gameObject);
        }
        enemiesComponents.Clear();
    }

    private EnemyComponent GetEnemyPrefab(EnemyType enemyType)
    {
        var enemyPrefabDef = enemyPrefabs.FirstOrDefault(e => e.type == enemyType);
        if (enemyPrefabDef == null || enemyPrefabDef.component == null)
        {
            throw new EnemyPrefabDefinitionUndefined(enemyType);
        }
        return enemyPrefabDef.component;
    }
}
