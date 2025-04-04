using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{

    [SerializeField] private Joystick joystick; 

    private float gravity = -9.8f;
    [SerializeField] private float incGravity;
    [SerializeField]
    private float gravityMultiplier = 3.0f;
    private float velocityGravity;

    private CharacterController controller;

    private Transform camTransform;

    private float currentVelocity;
    [SerializeField]
    private float speed = 5.0f;
    private Vector3 MoveDirection;
    [SerializeField]
    private float RotationTime = 0.1f;

    public Transform AwardScoreTextPos;


    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpHeight;
    [SerializeField] private LayerMask groundEnemies;
    [SerializeField] private Transform feetPosition;
    [SerializeField] private float detectionRadius;

    [Header("ParticleEffects")]
    [SerializeField] private GameObject landEffect;

    [SerializeField] private List<GameObject> guns;
    [SerializeField] private List<GameObject> projectiles;
    [SerializeField] private GameObject shootBtn;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootForce;
    [SerializeField] private float shootForceGun;
    [SerializeField] private int gunStartInd;
    private int gunInd = 0;

    [SerializeField] private GameObject shadow;
    [SerializeField] private float heightGround;


    [SerializeField] private GameObject[] EnemyDeathEffects;

    [SerializeField] private Animator animator;

    [SerializeField] private SoundManger soundManager;
    #region Singleton
    public static PlayerMotor instance;
    private Vector3 refVel;
    private Vector3 jumpDes;
    private bool jump;


    private void Awake()
    {
        instance = this;
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camTransform = Camera.main.transform;

        gunInd = PlayerPrefs.GetInt("EquipIndex", -1)+1;
        if (gunInd == 0) { shootBtn.SetActive(false); }
        else { shootBtn.SetActive(true); }
        GunOn();
        
    }

    void GunOn()
    {
        for (int i = 0; i < guns.Count; i++)
        {
            if (i != gunInd)
            {
                guns[i].SetActive(false);
            }
            else
            {
                guns[i].SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        ApplyGravity();

        ProcessMovement();
        FormEnemyCheckSphere();

        Shadow();
    }

    private void ApplyGravity()
    {
        if(transform.position.y >= jumpHeight)
        {
            gravity = incGravity;
            jump = true;    
        }
        if(transform.position.y <= 1.2f && velocityGravity <= 0f)
        {
            jump = false;
        }

        if (transform.position.y <= 1.2f && velocityGravity < 0f)//controller.isGrounded
        {
            if (velocityGravity <= -20f)
            {
                soundManager.PlaySound(4);
                Instantiate(landEffect, feetPosition.position, Quaternion.Euler(-90f, 0, 0));
            }
            velocityGravity = -1f;
            gravity = -9.8f;
            
        }
        else
        {
            velocityGravity += gravity * gravityMultiplier * Time.deltaTime;
        }
        controller.Move(new Vector3(0, velocityGravity * Time.deltaTime, 0));

    }

    
    public void ProcessMovement()
    {
        MoveDirection.x = joystick.Horizontal;
        MoveDirection.y = 0.0f;
        MoveDirection.z = joystick.Vertical;
        MoveDirection.Normalize();

        if (MoveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, RotationTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 PlayerDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(PlayerDirection * speed * Time.deltaTime);
            animator.SetBool("isWalking", true);
            //soundManager.StartLoop(6);
        }
        else
        {
            //soundManager.StopLoop();
            animator.SetBool("isWalking", false);
        }
    }

    public void Jump()
    {
        
        if (!jump)
        {   
            soundManager.PlaySound(3);
            velocityGravity += jumpForce;
        }
    }

    private void FormEnemyCheckSphere()
    {
        if (!jump) return;
        Collider[] enemiesInRange = Physics.OverlapSphere(feetPosition.position, detectionRadius, groundEnemies);

        if (enemiesInRange.Length > 0)
        {
            foreach (Collider enemy in enemiesInRange)
            {
                soundManager.PlaySound(2);
                PlayerScoreManager.instance.UpdateScore(1);
                DOTween.Kill(enemy.transform);
                foreach (var effect in EnemyDeathEffects)
                {
                    Instantiate(effect, enemy.transform.position, Quaternion.identity);
                }
                Destroy(enemy.gameObject);
            }
        }
    }

    public void Shoot()
    {
        if (gunInd == 0) return;
        if (gunInd == 1) soundManager.PlaySound(5);
        else  soundManager.PlaySound(1);
        GameObject proj = Instantiate(projectiles[gunInd] , shootPoint);
        proj.transform.parent = null;
        if(gunInd < gunStartInd)
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce, ForceMode.Impulse);
        else
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * shootForceGun , ForceMode.Impulse);
    }

    private void Shadow()
    {
        shadow.transform.position = new Vector3(transform.position.x, heightGround, transform.position.z);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

}
