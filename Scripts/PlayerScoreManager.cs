using CartoonFX;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]


public class PlayerScoreManager : MonoBehaviour
{

    #region Singleton
    public static PlayerScoreManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int currentScore = 0;
    [SerializeField]
    private GameObject scoreTextEffect;

    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private GameObject[] deathEffects;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI starText;
    [SerializeField]
    private List<GameObject> hearts;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;

    private int stars = 0;
    private int starInRound = 0;
    [SerializeField] private GameObject[] gameOverToggle;
    [SerializeField] private TextMeshProUGUI goScoreText;
    [SerializeField] private TextMeshProUGUI goStarText;
    [SerializeField] private TextMeshProUGUI goHighScoreText;
    [SerializeField] private GameObject trophy;

    private SoundManger soundManger;

    public bool gameOver = false;

    [SerializeField] private int maxRandForInterstitialAd = 5;

    [SerializeField] private LayerMask living;
    [SerializeField] private float radius;

    private void Start()
    {
        stars = PlayerPrefs.GetInt("Stars", 0);
        starText.text = stars.ToString();
        soundManger = GetComponent<SoundManger>();
    }

    public void playerHit()
    {
        if(!PlayerMotor.instance.transform) return;
        Transform playerTrans = PlayerMotor.instance.transform;
        if (lives == 0)
        {

            foreach (var effect in deathEffects)
                Instantiate(effect, playerTrans.position, Quaternion.identity);
            GameOver();
        }
        Destroy(hearts[lives-1]);
        hearts.RemoveAt(lives-1);
        lives-=1;
        
       
        playerTrans.DOShakeScale(duration,strength,vibrato,randomness);
        if (lives == 0)
        {
            
            foreach (var effect in deathEffects)
                Instantiate(effect, playerTrans.position,Quaternion.identity);
            GameOver();
        }
    }

    public void GameOver()
    {
        soundManger.PlaySound(0);
        UIManagwe.instance.PauseButton();
        gameOver = true;
        Destroy(PlayerMotor.instance.gameObject);
        goScoreText.text = currentScore.ToString();
        int HighScore = PlayerPrefs.GetInt("HighScore", 0);
        if(currentScore > HighScore)
        {
            trophy.SetActive(true);
            PlayerPrefs.SetInt("HighScore" , currentScore);
            HighScore = currentScore;
        }
        goHighScoreText.text = HighScore.ToString();
        goStarText.text = " : "+ starInRound.ToString();
        foreach(var i in gameOverToggle)
        {
            i.SetActive(!i.activeInHierarchy);
            
        }

        KillLiving();

        int randInt = Random.Range(1, maxRandForInterstitialAd+1);
        if(randInt == 1)
        {
            Interstitial.Instance.ShowInterstitialAd();
        }
    }

    private void KillLiving()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, radius, living);
        foreach (Collider enemyCollider in enemiesInRange)
        {
            Destroy(enemyCollider.gameObject);
        }
    }


    public void UpdateScore(int score)
    {
        currentScore += score;
        GameObject textIns = Instantiate(scoreTextEffect, PlayerMotor.instance.AwardScoreTextPos.position, Quaternion.identity);
        textIns.GetComponent<CFXR_ParticleText>().UpdateText(score.ToString());
        scoreText.text = currentScore.ToString();

    }

    public void UpdateStar(int star)
    {
        stars += star;
        starInRound += star;
        PlayerPrefs.SetInt("Stars", stars);
        starText.text = stars.ToString();
    }

    public void DoubleStars()
    {
        starInRound *= 2;
        goStarText.text = " : " + starInRound.ToString();
    }

    public void ShowRewardedAds()
    {
        Rewarded.instance.ShowRewardedAd();
    }

    
}
