using UnityEngine;

public class Gate : MonoBehaviour
{
    public new MeshRenderer renderer;
    public bool locked = true;

    public void Unlock()
    {
        locked = false;
        renderer.material.SetFloat("_TimeWhenFadeAwayStarted", Time.time);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Unlock();
        }
    }
}
