using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMissileLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject[] blastEffect;
    [SerializeField]
    private GameObject shadow;

    [SerializeField]
    private float minShadowWidth;
    [SerializeField] 
    private float maxShadowWidth;
    [SerializeField]
    private float shadowHeight;
    private float shadowWidth;

    private Vector3 shadowStartPos;
    private GameObject shadowIns;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float spawnDelay = 1f;

    [SerializeField]
    private int award1 = 1;
    [SerializeField]
    private int award2 = 5;
    [SerializeField]
    private int award3 = 10;
    [SerializeField]
    private float greatDis;
    [SerializeField]
    private float goodDis;
    [SerializeField]
    private float fineDis;
    
    private void Start()
    {
        shadowWidth = Random.Range(minShadowWidth, maxShadowWidth);
        shadowStartPos = transform.position + new Vector3(Random.Range(-shadowWidth , shadowWidth) , 0,Random.Range(shadowWidth , -shadowWidth));
        shadowStartPos.y = shadowHeight;
        
        Invoke("instantiateShadow", spawnDelay);
    }

    private void Update()
    {
        //if(shadowIns!=null) 
            //shadowIns.transform.Translate((new Vector3(transform.position.x,shadowHeight , transform.position.z) - shadowStartPos).normalized * Time.deltaTime * moveSpeed);
    }

    private void instantiateShadow()
    {
        //shadowIns = Instantiate(shadow, shadowStartPos, Quaternion.identity);
        shadowIns = shadowIns = Instantiate(shadow, new Vector3(transform.position.x , shadowHeight , transform.position.z), Quaternion.identity);
    }



    private void OnCollisionEnter(Collision collision)
    {
        float distance = Vector3.Distance(transform.position, PlayerMotor.instance.transform.position);
        if (collision.gameObject.tag != "Player")
        {
            if (distance < greatDis)
            {
                PlayerScoreManager.instance.UpdateScore(award3);
            }
            else if (distance < goodDis)
            {
                PlayerScoreManager.instance.UpdateScore(award2);
            }
            else if (distance < fineDis)
            {
                PlayerScoreManager.instance.UpdateScore(award1);
            }

        }
        else
        {
            PlayerScoreManager.instance.playerHit();
        }
        foreach (var effect in blastEffect) 
            Instantiate(effect , transform.position, Quaternion.identity);
        Destroy(shadowIns);
        Destroy(gameObject);
    }

}
