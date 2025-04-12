using Enemies;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossScene : MonoBehaviour
{
    [SerializeField] private GameObject block;
    [SerializeField] private string bossGameobjectName = "Boss";

    [SerializeField] private EnemyAI[] enemyPrefabs;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float spawnPosXRandomness = 6;
    [SerializeField] private float spawnPosZRandomness = 2;
    [SerializeField] private int targetAmount = 30;
    private int currentAmount = 0;

    private void Start()
    {
        HealthComp.onAnyDiedEvent += TryInit;
    }

    private void TryInit(HealthComp died)
    {
        if (died.group == HealthComp.HealthGroup.Player || died.name != bossGameobjectName) return;

        FloodAttack();
    }

    private void OnDisable()
    {
        HealthComp.onAnyDiedEvent -= TryInit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            block.SetActive(true);
        }
    }

    public void FloodAttack()
    {
        IEnumerator spawn()
        {
            yield return new WaitForSeconds(1f);
            EnemyAI enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)]);
            enemy.transform.position = targetTransform.position + 
                Vector3.right * Random.Range(-spawnPosXRandomness, spawnPosXRandomness) +
                targetTransform.forward * Random.Range(0, spawnPosZRandomness);
            enemy.enabled = false;
            var bossfall = enemy.AddComponent<EnemyBossFall>();
            bossfall.Init();
            currentAmount++;
            if (currentAmount < targetAmount)
                yield return spawn();
            else
                yield break;
        }

        StartCoroutine(spawn());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetTransform.position + targetTransform.forward * spawnPosZRandomness / 2.0f, new Vector3(2 * spawnPosXRandomness, 0, spawnPosZRandomness));
    }
}
