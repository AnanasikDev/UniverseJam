using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public void PlayerStep()
    {
        AudioManager.Instance.PlayOneShotString("Step", Vector3.zero);
    }
    public void Swipe()
    {
        AudioManager.Instance.PlayOneShotString("DaggerSwipe", Vector3.zero);
    }


}
