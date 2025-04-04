using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]
public class UIManagwe : MonoBehaviour
{
    [SerializeField] private List<GameObject> pauseButtonLists;
    [SerializeField] private string mainMenu;
    [SerializeField] private string currentLevel;


    #region Singleton

    public static UIManagwe instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void PauseButton()
    {
        if (PlayerScoreManager.instance.gameOver) return;

        if(Time.timeScale == 0) Time.timeScale = 1;
        else Time.timeScale = 0;
        foreach (GameObject go in pauseButtonLists)
        {
            go.SetActive(!go.activeSelf);
        }
    }

    public void HomeButton()
    {
        DOTween.KillAll();
        Time.timeScale = 1.0f;
        Invoke("DelayLoadMain", 0.1f);
    }

    public void RetryButton()
    {
        DOTween.KillAll();
        Time.timeScale = 1.0f;
        Invoke("DelayLoadCurrent", 0.1f);
    }

    private void DelayLoadMain()
    {
        SceneManager.LoadScene(mainMenu);
    }

    private void DelayLoadCurrent()
    {
        SceneManager.LoadScene(currentLevel);
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
