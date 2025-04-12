using Enemies;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class World : MonoBehaviour
{
    public static int totalKills = 0;

    [ReadOnly] public List<EnemyAI> enemies;
    [ReadOnly] public List<HealthComp> healthEntities;

    public GlobalEnemiesSettings globalEnemiesSettings;

    [SerializeField][DisableInPlayMode] private bool doIncludeInactiveObjects = false;

    [SerializeField][ReadOnly] public List<Room> rooms;
    [SerializeField][ReadOnly] private int maxRoomIndex;

    public static World instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("World init");

        // populate with enemies added in Editor
        enemies = GameObject.FindObjectsOfType<EnemyAI>(doIncludeInactiveObjects).ToList();
        healthEntities = GameObject.FindObjectsOfType<HealthComp>(doIncludeInactiveObjects).ToList();

        rooms = GameObject.FindObjectsOfType<Room>().ToList();
        maxRoomIndex = rooms.Max(r => r.index);
        foreach (var room in rooms)
        {
            room.Init();
        }
        Room.onUnlockedEvent += (Room opened) =>
        {
            Room nextRoom = rooms.Find(r => r.index == Room.currentRoom.index + 1);
            if (nextRoom != null)
            {
                Room.currentRoom = nextRoom;
            }
        };
    }
}