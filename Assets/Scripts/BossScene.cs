using DG.Tweening;
using Enemies;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossScene : MonoBehaviour
{
    [SerializeField] private GameObject block;
    [SerializeField] private string bossGameobjectName = "Boss";
    [SerializeField] private EnemyAI boss;

    [SerializeField] private EnemyAI[] enemyPrefabs;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float spawnPosXRandomness = 6;
    [SerializeField] private float spawnPosZRandomness = 2;
    [SerializeField] private int targetAmount = 30;
    [SerializeField] private int newMaxAttackingAmount = 15;
    [SerializeField] private float intervalSeconds = 0.9f;
    [SerializeField] private float initIntevalSeconds = 3f;
    private int currentAmount = 0;
    private int defaultMaxAttackingAmount;

    [SerializeField] private CanvasGroup blackscreen;
    float blackScreenDuration = 1.25f;

    private void Start()
    {
        HealthComp.onAnyDiedEvent += TryInit;
        defaultMaxAttackingAmount = World.instance.globalEnemiesSettings.maxAttackingEnemies;
    }

    private void TryInit(HealthComp died)
    {
        if (died.group == HealthComp.HealthGroup.Player || died.name != bossGameobjectName) return;

        FloodAttack();
    }

    private void OnDisable()
    {
        HealthComp.onAnyDiedEvent -= TryInit;
        World.instance.globalEnemiesSettings.maxAttackingEnemies = defaultMaxAttackingAmount;
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
            yield return new WaitForSeconds(intervalSeconds);
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

        IEnumerator startSpawning()
        {
            yield return new WaitForSeconds(initIntevalSeconds);
            yield return spawn();
        }

        StartCoroutine(startSpawning());
        World.instance.globalEnemiesSettings.maxAttackingEnemies = newMaxAttackingAmount;
        PlayerController.instance.onDiedEvent += LoadEnding;
    }

    private void LoadEnding()
    {
        PlayerController.instance.onDiedEvent -= LoadEnding;

        blackscreen.DOFade(1, blackScreenDuration);
        IEnumerator reload()
        {
            yield return new WaitForSeconds(blackScreenDuration);
            SceneManager.LoadScene(1);
            blackscreen.alpha = 1;
            blackscreen.DOFade(0, blackScreenDuration);
        }

        StartCoroutine(reload());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetTransform.position + targetTransform.forward * spawnPosZRandomness / 2.0f, new Vector3(2 * spawnPosXRandomness, 0, spawnPosZRandomness));
    }
}
