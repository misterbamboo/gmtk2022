using Assets.DiceGame.Combat.Application;
using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.Combat.Presentation.Animations;
using Assets.DiceGame.Combat.Presentation.Exceptions;
using Assets.DiceGame.Combat.Presentation.Inspector;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Turn.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CombatAnimatorComponent))]
public class CombatManager : MonoBehaviour
{
    public const string Tag = "CombatManager";

    public bool HasTarget => combatController.HasTarget;

    [Header("UI")]
    [SerializeField] float characterDisplayOffsetY = -1.5f;

    [Header("Player")]
    [SerializeField] PlayerPrefabDefinition playerPrefab;

    [Header("Enemies")]
    [SerializeField] int minNumberOfEnemies = 1;
    [SerializeField] int maxNumberOfEnemies = 4;
    [SerializeField] List<EnemyPrefabDefinition> enemyPrefabs;
    [SerializeField] GameStatsManager statsManager;

    List<EnemyComponent> enemiesComponents = new List<EnemyComponent>();
    private PlayerComponent playerComponent;

    private CombatController combatController;

    private CombatAnimatorComponent combatAnimator;

    void Start()
    {
        var enemyStatsPerType = statsManager.GetEnemyStats();

        combatAnimator = GetComponent<CombatAnimatorComponent>();
        combatController = new CombatController(minNumberOfEnemies, maxNumberOfEnemies, enemyStatsPerType, playerPrefab.maxLife);
        SubscribeEvents();
        combatController.NewCombat();
    }

    private void SubscribeEvents()
    {
        GameEvents.Subscribe<NewCombatReadyEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemySelectedEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyUnselectedEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyTakeDamageEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyKilledEvent>(EventsReceiver);
        GameEvents.Subscribe<TurnStartedEvent>((e) => combatController.OnTurnStarted(e));
        GameEvents.Subscribe<EnemyDecisionTakenEvent>(OnEnemyDecisionTaken);
    }

    private void OnEnemyDecisionTaken(EnemyDecisionTakenEvent enemyDecisionTakenEvent)
    {
        var sourceTransform = FindCharacterTransform(enemyDecisionTakenEvent.EnemyDecision.SourceId);
        var targetTransform = FindCharacterTransform(enemyDecisionTakenEvent.EnemyDecision.TargetId);
        combatAnimator.QueueAnimationForEnemyDecision(enemyDecisionTakenEvent.EnemyDecision.Decision, sourceTransform, targetTransform);
    }

    private Transform FindCharacterTransform(int characterId)
    {
        var souceEnemy = enemiesComponents.FirstOrDefault(c => c.GetCharacterID() == characterId);
        if (souceEnemy != null)
        {
            return souceEnemy.transform;
        }

        if(playerComponent.GetCharacterID() == characterId)
        {
            return playerComponent.transform;
        }

        throw new CharacterTransformNotFound(characterId);
    }

    private void Update()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 1f, LayerMask.GetMask(EnemyComponent.LayerMaskName));
        if (hit.collider == null)
        {
            combatController.OnUnfocusEnemy();
        }
        else
        {
            var enemyComponent = hit.collider.GetComponent<EnemyComponent>();
            combatController.OnFocusEnemy(enemyComponent.Enemy);
        }
    }

    public void HitTarget(float damages)
    {
        combatController.HitFocusEnemy(damages);
    }

    private void EventsReceiver(IGameEvent gameEvent)
    {
        if (gameEvent is NewCombatReadyEvent) InitGameObjects();
        if (gameEvent is EnemySelectedEvent) { }
        if (gameEvent is EnemyUnselectedEvent) { }
        if (gameEvent is EnemyTakeDamageEvent) ShakeRelatedEnemyComponent((EnemyTakeDamageEvent)gameEvent);
        if (gameEvent is EnemyKilledEvent) DestroyEnemyComponent((EnemyKilledEvent)gameEvent);
    }

    private void DestroyEnemyComponent(EnemyKilledEvent gameEvent)
    {
        var enemyComponent = enemiesComponents.FirstOrDefault(c => c.Enemy?.Id == gameEvent.Id);
        if (enemyComponent != null)
        {
            enemiesComponents.Remove(enemyComponent);
            Destroy(enemyComponent.gameObject);
        }
    }

    private void ShakeRelatedEnemyComponent(EnemyTakeDamageEvent enemyTakeDamageEvent)
    {
        var enemyComponent = enemiesComponents.FirstOrDefault(c => c.Enemy?.Id == enemyTakeDamageEvent.Id);
        if (enemyComponent != null)
        {
            enemyComponent.Shake();
        }
    }

    private void InitGameObjects()
    {
        ClearEnemiesGameObjects();

        float index = 0;
        foreach (var enemy in combatController.Enemies)
        {
            var prefab = GetEnemyPrefab(enemy.Type);

            var x = index * 1.5f;
            var y = (index % 2) + characterDisplayOffsetY;
            var enemyComponent = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            enemyComponent.Enemy = enemy;
            enemiesComponents.Add(enemyComponent);
            index++;
        }

        playerComponent = Instantiate(playerPrefab.component, new Vector3(-4, characterDisplayOffsetY, 0), Quaternion.identity);
        playerComponent.Player = combatController.Player;
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
