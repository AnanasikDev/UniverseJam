using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup ui;
    [SerializeField] private CanvasGroup blackscreen;

    private void Start()
    {
        PlayerController.instance.healthComp.onDiedEvent += ShowUI;
    }

    private void ShowUI()
    {
        ui.DOFade(1, 1);
        IEnumerator enable()
        {
            yield return new WaitForSeconds(1);
            ui.interactable = true;
        }

        StartCoroutine(enable());
    }

    public void Restart()
    {
        float duration = 1.25f;
        //blackscreen.DOFade(0, duration);
        IEnumerator reload()
        {
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //blackscreen.DOFade(1, duration);
        }

        StartCoroutine(reload());
    }
}