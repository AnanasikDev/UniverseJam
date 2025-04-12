using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyBossFall : MonoBehaviour
    {
        private new Rigidbody rigidbody;
        private EnemyAI ai;
        public void Init()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.velocity = Vector3.zero;
            ai = GetComponent<EnemyAI>();
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
                //Destroy(this);
            }
        }
    }
}