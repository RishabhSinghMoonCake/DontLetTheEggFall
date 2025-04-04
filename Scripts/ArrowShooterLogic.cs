using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooterLogic : MonoBehaviour
{

    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;

    [SerializeField]
    private GameObject launchPrefab;
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;

    private float waitTime = 0;
    private float curTime = 0;
    private bool leftRight = false;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(curTime > waitTime)
        {
            if(leftRight)
            {
                Instantiate(launchPrefab , leftHand.position, Quaternion.identity); 
            }
            else
            {
                Instantiate(launchPrefab, rightHand.position, Quaternion.identity);
            }
            leftRight = !leftRight;
            curTime = 0;
            waitTime = Random.Range(minWaitTime, maxWaitTime);
        }
        curTime += Time.deltaTime;
    }
}
