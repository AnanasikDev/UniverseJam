using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup ui;
    [SerializeField] private CanvasGroup blackscreen;
    float blackScreenDuration = 1.25f;

    private void Start()
    {
        PlayerController.instance.onDiedEvent += ShowUI;
        blackscreen.alpha = 1;
        blackscreen.DOFade(0, blackScreenDuration);
    }

    private void OnDestroy()
    {
        PlayerController.instance.onDiedEvent -= ShowUI;
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
        blackscreen.DOFade(1, blackScreenDuration);
        IEnumerator reload()
        {
            yield return new WaitForSeconds(blackScreenDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        StartCoroutine(reload());
    }
}