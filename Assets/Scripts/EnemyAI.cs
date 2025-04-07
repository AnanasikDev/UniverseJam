using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;
    [Range(0, 6)] public float movementSpeed = 3;
    public bool doFollowPlayer = true;

    public void Init()
    {
        TryGetComponent<HealthComp>(out health);
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (!doFollowPlayer) return;
        transform.position += (PlayerController.instance.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime;
    }
}
