using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyBossFall : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private EnemyAI ai;
        [SerializeField] private bool initOnAwake;
        [SerializeField] private float initDelay;

        private void Awake()
        {
            if (initOnAwake) Init();
        }

        public void Init()
        {
            IEnumerator wait()
            {
                yield return new WaitForSeconds(initDelay);
                rigidbody = GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                rigidbody.velocity = Vector3.zero;
                ai = GetComponent<EnemyAI>();
            }
            StartCoroutine(wait());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 0 && other.gameObject.CompareTag("Finish"))
            {
                rigidbody.useGravity = false;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                rigidbody.position = new Vector3(rigidbody.position.x, 0, rigidbody.position.z);
                transform.position = rigidbody.position;
                ai.enabled = true;
                ai.spawnRoom = Room.currentRoom;
                ai.Init();
                Destroy(this);
            }
        }
    }
}