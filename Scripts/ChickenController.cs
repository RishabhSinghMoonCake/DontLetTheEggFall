using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]

public class ChickenController : MonoBehaviour
{
    private float currentHeight;

    [SerializeField]
    private float fieldBreadth;
    [SerializeField]
    private float fieldLength;
    [SerializeField]
    private float speed;

    private Vector3 nextPos;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private GameObject shadow;
    [SerializeField]
    private float heightGround;

    [SerializeField]
    private GameObject egg;
    [SerializeField]
    private Transform eggSpawnPoint;
    [SerializeField]
    private float minTimeEggSpawn;
    [SerializeField]
    private float maxTimeEggSpawn;

    private float timer = 0f;
    private float eggSpawnTime;

    [SerializeField]
    private int score = 10;

    [SerializeField]
    private GameObject[] DeathEffects;

    [SerializeField] private GameObject star;
    [SerializeField] private int minStars;
    [SerializeField] private int maxStars;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;

    private SoundManger soundManger;
    // Start is called before the first frame update
    void Start()
    {
        currentHeight = transform.position.y;
        nextPos = new Vector3(Random.Range(-fieldBreadth, fieldBreadth), currentHeight, Random.Range(-fieldLength, fieldLength));
        eggSpawnTime = Random.Range(minTimeEggSpawn, maxTimeEggSpawn);
        soundManger = GetComponent<SoundManger>();
    }

    // Update is called once per frame
    void Update()
    {
        shadow.transform.position = new Vector3(transform.position.x, heightGround, transform.position.z);
        if(Vector3.Distance(transform.position , nextPos) > 1f)
        {
            Mover();
        }
        else
        {
            nextPos = new Vector3(Random.Range(-fieldBreadth, fieldBreadth), currentHeight, Random.Range(-fieldLength, fieldLength));
        }
        //Egg Spawn
        timer += Time.deltaTime;
        if(timer > eggSpawnTime)
        {
            timer = 0;
            SpawnEgg(); 
            eggSpawnTime = Random.Range(minTimeEggSpawn, maxTimeEggSpawn);
        }
        //EggSpawn
    }

    private void Mover()
    {

        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        Vector3 movementDirection = -(nextPos - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);


        Vector3 eulerRotation = targetRotation.eulerAngles;
        eulerRotation.x = -90; 
        eulerRotation.z = 0;

        Quaternion smoothRotation = Quaternion.Euler(eulerRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.deltaTime * rotationSpeed);
    }

    private void SpawnEgg()
    {
        soundManger.PlaySound(1);
        Instantiate(egg , eggSpawnPoint.position , Quaternion.identity);    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GamePlayLogic.instance.isQuitting)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            soundManger.PlaySound(0);
            PlayerScoreManager.instance.UpdateScore(score);
            int stars = Random.Range(minStars, maxStars+1);
            for(int i = 0;i < stars; i++)
            {
                GameObject s = Instantiate(star,transform.position, Quaternion.identity);
                s.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * Time.deltaTime, transform.position, explosionRadius);
            }
            foreach(var t in DeathEffects)
            {
                Instantiate(t, transform.position, Quaternion.identity);
            }
            Invoke("Destruct", 0.1f);
            
        }
    }

    private void Destruct()
    {
        Destroy(gameObject);
    }
}
