using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stars : MonoBehaviour
{
    [SerializeField]private float radius = 5f; // Radius of the sphere
    [SerializeField]private LayerMask playerLayer;

    private int starValue;

    private void Start()
    {
        starValue = PlayerPrefs.GetInt("StarValue", 1);
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, playerLayer);

        if(hitColliders.Length > 0 )
        {
            //SoundManger.instance.PlaySound(7);
            PlayerScoreManager.instance.UpdateStar(starValue);
            Destroy(gameObject);
        }
        
    }
}
