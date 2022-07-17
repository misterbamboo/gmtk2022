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

        var enemyStatsPerType = statsManager.GetEnemyStats();
        var playerStats = statsManager.GetPlayerStats();
        SubscribeEvents();
        combatController = new CombatController(minNumberOfEnemies, maxNumberOfEnemies, enemyStatsPerType, playerStats);
        combatController.StartCombat();
    }

    private void SubscribeEvents()
    {
        GameEvents.Subscribe<NewCombatReadyEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyTakeDamageEvent>(EventsReceiver);
        GameEvents.Subscribe<CharacterKilledEvent>(OnCharacterKilled);
        GameEvents.Subscribe<TurnStartedEvent>(OnTurnStarted);
        GameEvents.Subscribe<CombatActionSentEvent>(OnEnemyDecisionTaken);

        // TODO: Implement animations
        GameEvents.Subscribe<CharacterTookDamageEvent>(TakeDamageAnimation);
        GameEvents.Subscribe<CharacterGotHealedEvent>(EventsReceiver);
        GameEvents.Subscribe<CharacterGotShieldedEvent>(EventsReceiver);
    }

    private void OnEnemyDecisionTaken(CombatActionSentEvent combatEvent)
    {
        var sourceTransform = FindCharacterTransform(combatEvent.CombatAction.SourceId);
        var targetTransform = FindCharacterTransform(combatEvent.CombatAction.TargetIds.First());
        combatAnimator.QueueAnimationForEnemyDecision(combatEvent.CombatAction, sourceTransform, targetTransform, () =>
        {
            combatController.DispatchCombatAction(combatEvent.CombatAction);
        });
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

    private CharacterComponent FindCharacterComponent(int id)
    {
        return id == Player.PlayerId ? playerComponent : enemiesComponents.First(ec => ec.CharacterId == id);
    }

    private Transform FindCharacterTransform(int characterId)
    {
        if (playerComponent.CharacterId == characterId)
        {
            return playerComponent.transform;
        }

        var souceEnemy = enemiesComponents.FirstOrDefault(c => c.CharacterId == characterId);
        if (souceEnemy != null)
        {
            return souceEnemy.transform;
        }

        throw new CharacterTransformNotFound(characterId);
    }

    private void EventsReceiver(IGameEvent gameEvent)
    {
        if (gameEvent is NewCombatReadyEvent) InitGameObjects();
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
