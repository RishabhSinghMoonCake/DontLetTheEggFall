using Polycrime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour,IPropelBehavior
{
    private Rigidbody cachedRigidbody3D;
    private Rigidbody2D cachedRigidbody2D;

    [SerializeField]
    private GameObject[] impactEffects;

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

    public void React(Vector3 velocity)
    {
        if (cachedRigidbody3D) cachedRigidbody3D.velocity = velocity;

        if (cachedRigidbody2D) cachedRigidbody2D.velocity = velocity;
    }

    private void Awake()
    {
        cachedRigidbody3D = GetComponent<Rigidbody>();
        cachedRigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        float distance = Vector3.Distance(transform.position , PlayerMotor.instance.transform.position);
        if(collision.gameObject.tag != "Player")
        {
            if(distance < greatDis)
            {
                PlayerScoreManager.instance.UpdateScore(award3);
            }
            else if(distance < goodDis)
            {
                PlayerScoreManager.instance.UpdateScore(award2);
            }
            else if(distance < fineDis)
            {
                PlayerScoreManager.instance.UpdateScore(award1);
            }
            
        }
        else
        {
            PlayerScoreManager.instance.playerHit();
        }

        foreach (var effect in impactEffects) 
            Instantiate(effect , transform.position,Quaternion.identity);
        Destroy(gameObject);    
    }
}
