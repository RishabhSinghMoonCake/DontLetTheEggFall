using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]
public class EggLogic : MonoBehaviour
{

    [SerializeField] private List<GameObject> groundEnemies;
    [SerializeField] private bool bomb = false;
    [SerializeField] private bool danger = false;
    [SerializeField] private GameObject[] bombEffects;
    [SerializeField] private GameObject[] dangerousEnemies;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask playerL;
    [SerializeField] private LayerMask enemyL;
    [SerializeField] private bool normalFall = false;

    [SerializeField] private float gravity = 9.8f;

    private SoundManger soundManger;

    private Rigidbody rb;
    private void Start()
    {
        if(normalFall)
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(Vector3.down * gravity , ForceMode.Impulse);
        }
        soundManger = GetComponent<SoundManger>();  
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(normalFall)
        {

            Destroy(gameObject);
            return;
        }
        if(danger)
        {
            Instantiate(dangerousEnemies[Random.Range(0,dangerousEnemies.Length)] , transform.position,Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        if(bomb)
        {
            soundManger.PlaySound(0);
            foreach(GameObject go in bombEffects)
            {
                Instantiate(go , transform.position,Quaternion.identity);
           
            }
            AttackCheckSphere();
            Invoke("Destruct", 1f);
            return;
        }

        if(other.gameObject.tag == "Ground")
        {
            GameObject g1 = Instantiate(groundEnemies[Random.Range(0, groundEnemies.Count)] , transform.position , Quaternion.identity);
        }
        
        if(other.gameObject.tag == "Player")
        {
            PlayerScoreManager.instance.playerHit();
        }
        Destroy(gameObject);
    }


    private void Destruct()
    {
        Destroy(gameObject);
    }

    private void AttackCheckSphere()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, detectionRadius, playerL);

        if (playerInRange.Length > 0)
        {
            PlayerScoreManager.instance.playerHit();
        }
        playerInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyL);
        
        foreach (Collider col in playerInRange)
        {
            DOTween.Kill(col.transform);
            Destroy(col.gameObject);
        }
    }
}
