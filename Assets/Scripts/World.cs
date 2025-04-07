using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;

public class World : MonoBehaviour
{
    [ReadOnly] public List<EnemyAI> enemies;
    [ReadOnly] public List<HealthComp> healthEntities;

    [SerializeField][DisableInPlayMode] private bool doIncludeInactiveObjects = false;

    public static World instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // populate with enemies added in Editor
        enemies = GameObject.FindObjectsOfType<EnemyAI>(doIncludeInactiveObjects).ToList();
        healthEntities = GameObject.FindObjectsOfType<HealthComp>(doIncludeInactiveObjects).ToList();
    }
}