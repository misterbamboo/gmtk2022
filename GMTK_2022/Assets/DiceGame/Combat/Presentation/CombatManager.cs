using Assets.DiceGame.Combat.Application;
using Assets.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.Combat.Events;
using Assets.DiceGame.Combat.Presentation.Exceptions;
using Assets.DiceGame.Combat.Presentation.Inspector;
using Assets.DiceGame.SharedKernel;
using Assets.DiceGame.Turn.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    List<EnemyComponent> enemiesComponents = new List<EnemyComponent>();
    private PlayerComponent playerComponent;

    private CombatController combatController;

    void Start()
    {
        var enemyStatsPerType = enemyPrefabs.ToDictionary(ep => ep.type, ep => (IEnemyStats)ep.stats);
        combatController = new CombatController(minNumberOfEnemies, maxNumberOfEnemies, enemyStatsPerType, playerPrefab.maxLife);
        GameEvents.Subscribe<NewCombatReadyEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemySelectedEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyUnselectedEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyTakeDamageEvent>(EventsReceiver);
        GameEvents.Subscribe<EnemyKilledEvent>(EventsReceiver);
        GameEvents.Subscribe<TurnStartedEvent>((e) => combatController.OnTurnStarted(e));
        combatController.NewCombat();
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
