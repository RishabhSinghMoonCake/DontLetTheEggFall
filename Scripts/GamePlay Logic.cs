using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]
public class GamePlayLogic : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> chicks;
    [SerializeField]
    private List<GameObject> chicksDangerousClass;
    [SerializeField]
    private List<Transform> chickSpawnerLocs;

    [SerializeField]
    private float dangerousClassThreshold = 0.25f;
    [SerializeField]
    private float minTimeChickSpawn;
    [SerializeField]
    private float maxTimeChickSpawn;

    private float timer = 0f;
    private float eggSpawnTime;
    [SerializeField] private float dangerHeight;
    public bool isQuitting;

    #region Singleton
    public static GamePlayLogic instance;
    

    private void Awake()
    {
        instance = this;
    }
    #endregion

    void Start()
    {
        isQuitting = false;

        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }


    void Update()
    {
        //Chick Spawn
        timer += Time.deltaTime;
        if (timer > eggSpawnTime)
        {
            timer = 0;
            SpawnChick();
            eggSpawnTime = Random.Range((minTimeChickSpawn - PlayerScoreManager.instance.currentScore / 1000) > 1f ? (minTimeChickSpawn - PlayerScoreManager.instance.currentScore / 1000) : 1f, maxTimeChickSpawn);
        }
        //chick spawn
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }


    private void SpawnChick()
    {
        if(PlayerScoreManager.instance.gameOver)
        {
            return;
        }
        float danger = Random.Range(0f, 1f);
        if(danger < Mathf.Clamp(dangerousClassThreshold + (PlayerScoreManager.instance.currentScore / 1000) * 0.1f, 0f, 1f))
            Instantiate(chicksDangerousClass[Random.Range(0, chicksDangerousClass.Count)], chickSpawnerLocs[Random.Range(0, chickSpawnerLocs.Count)].position + new Vector3(0,dangerHeight,0), Quaternion.identity);
        else
            Instantiate(chicks[Random.Range(0,chicks.Count)] , chickSpawnerLocs[Random.Range(0,chickSpawnerLocs.Count)].position , Quaternion.identity);
    }
}
