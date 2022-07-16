using Assets.DiceGame.DiceGame.Combat.Application;
using Assets.DiceGame.DiceGame.Combat.Entities.EnemyAggregate;
using Assets.DiceGame.DiceGame.Combat.Entities.Events;
using Assets.DiceGame.DiceGame.Combat.Presentation.Exceptions;
using Assets.DiceGame.DiceGame.Combat.Presentation.Inspector;
using Assets.DiceGame.SharedKernel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] int minNumberOfEnemies = 1;
    [SerializeField] int maxNumberOfEnemies = 4;
    [SerializeField] List<EnemyPrefabDefinition> enemyPrefabs;

    List<EnemyComponent> enemiesComponents = new List<EnemyComponent>();

    private CombatController combatController;

    void Start()
    {
        var lifeConfig = enemyPrefabs.ToDictionary(ep => ep.type, ep => ep.component.GetMaxLife());
        combatController = new CombatController(minNumberOfEnemies, maxNumberOfEnemies, lifeConfig);
        GameEvents.Subscribe<NewCombatReadyEvent>(CombatController_EventsReceiver);
        GameEvents.Subscribe<EnemySelectedEvent>(CombatController_EventsReceiver);
        GameEvents.Subscribe<EnemyUnselectedEvent>(CombatController_EventsReceiver);
        GameEvents.Subscribe<EnemyTakeDamageEvent>(CombatController_EventsReceiver);
    }

    private void Update()
    {
        UpdateTarget();
        CheckHitTarget();
    }

    private void UpdateTarget()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, 1f, LayerMask.GetMask(EnemyComponent.LayerMaskName));
        if (hit.collider == null)
        {
            combatController.Target(null);
        }
        else
        {
            var enemyComponent = hit.collider.GetComponent<EnemyComponent>();
            var enemy = enemyComponent.GetEnemy();
            combatController.Target(enemy);
        }
    }

    private void CheckHitTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            combatController.HitTarget(10);
        }
    }

    public void StartNewCombat()
    {
        combatController.NewCombat();
    }

    private void CombatController_EventsReceiver(IGameEvent gameEvent)
    {
        if (gameEvent is NewCombatReadyEvent) InitGameObjects();
        if (gameEvent is EnemySelectedEvent) { }
        if (gameEvent is EnemyUnselectedEvent) { }
        if (gameEvent is EnemyTakeDamageEvent) ShakeRelatedEnemyComponent((EnemyTakeDamageEvent)gameEvent);
    }

    private void ShakeRelatedEnemyComponent(EnemyTakeDamageEvent enemyTakeDamageEvent)
    {
        var enemyComponent = enemiesComponents.FirstOrDefault(c => c.GetEnemy()?.Id == enemyTakeDamageEvent.Id);
        if (enemyComponent != null)
        {
            enemyComponent.Shake();
        }
    }

    private void InitGameObjects()
    {
        ClearEnemiesGameObjects();

        float index = 0;
        foreach (var enemy in combatController.enemies)
        {
            var prefab = GetEnemyPrefab(enemy.Type);

            var x = index;
            var y = (index % 2) / 2;
            var enemyComponent = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            enemyComponent.SetEnemy(enemy);
            enemiesComponents.Add(enemyComponent);
            index++;
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
