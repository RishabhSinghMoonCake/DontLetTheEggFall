using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManger))]
public class projScript : MonoBehaviour
{

    [SerializeField] private string enemyL;
    [SerializeField] private GameObject[] EnemyDeathEffects;
    [SerializeField] private GameObject[] HitEff;

    [SerializeField] private int score = 1;

    private SoundManger manger;

    private void Start()
    {
        manger = GetComponent<SoundManger>();   
    }
    private void OnCollisionEnter(Collision collision)
    {
        manger.PlaySound(0);
        GetComponent<Collider>().enabled = false;
        if (collision.gameObject.tag == enemyL)
        {
            PlayerScoreManager.instance.UpdateScore(score);
            collision.transform.DOKill();
            foreach (var effect in EnemyDeathEffects)
            {
                Instantiate(effect, collision.transform.position, Quaternion.identity);
            }
            collision.gameObject.GetComponent<GeoundEnemyBase>().KillYourself();
        }
        foreach (var effect in HitEff)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        Invoke("Destruct", 0.1f);
    }

    private void Destruct()
    {
        Destroy(gameObject);
    }
}
