using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileLogic : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float desTime;

    private float curTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        curTime += Time.deltaTime;
        if(curTime > desTime)
        {
            Destroy(gameObject);
        }
    }
}
