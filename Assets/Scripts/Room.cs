using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("Only used to find enemies within the room")][SerializeField] private BoxCollider bounds;

    [InfoBox("The higher the index, the later this room will be opened. 0 for the beginning one.")]
    public int index;
    public MeshRenderer gateRenderer;
    public BoxCollider gateCollider;
    public bool locked = true;
    public static Room currentRoom;
    public static event Action<Room> onUnlockedEvent;

    [SerializeField][ReadOnly] private List<EnemyAI> enemies;

    public void Init()
    {
        enemies = World.instance.enemies.Where(e => bounds.bounds.Contains(e.transform.position)).ToList();
        foreach (var enemy in enemies)
        {
            enemy.spawnRoom = this;
            enemy.Init();
        }

        HealthComp.onAnyDiedEvent += TryUnlock;

        if (index == 0)
        {
            currentRoom = this;
        }
    }

    private void TryUnlock(HealthComp died)
    {
        if (!locked || died.group == HealthComp.HealthGroup.Player) return;

        var enemy = died.GetComponent<EnemyAI>();
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }

        if (enemies.Count == 0)
        {
            Unlock();
        }
    }

    public void Unlock()
    {
        locked = false;
        gateRenderer.material.SetFloat("_TimeWhenFadeAwayStarted", Time.time);

        StartCoroutine(DisableCollider());

        IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(3);
            gateCollider.enabled = false;
            onUnlockedEvent?.Invoke(this);
        }
    }
}
