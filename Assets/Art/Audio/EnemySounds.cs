using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{

    public void ZombieStep()
    {
        AudioManager.Instance.PlayOneShotString("BareStep", gameObject.transform.position);
    }
    public void Swipe()
    {
        AudioManager.Instance.PlayOneShotString("ZombieSwipe", Vector3.zero);
    }

    public void Tension()
    {
        AudioManager.Instance.PlayOneShotString("Tension",gameObject.transform.position);
    }

    public void Slam()
    {
        AudioManager.Instance.PlayOneShotString("Slam", gameObject.transform.position);

    }

    public void Speak()
    {
        AudioManager.Instance.PlayOneShotString("ZombieSpeak", gameObject.transform.position);
    }

}
