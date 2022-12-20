using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enimy : MonoBehaviour
{
    public float waitingTime = 3.0f;
    public float speed = 2.0f;
    
    internal bool isAttacking;
    internal bool isWalking;
    internal bool isRunning;

    private float StatusChangeTime;
    
    private Animator animation_controller;
    private MazeDirection curDirection; //using MazeDirection

    //Variables for "Field of View"(fov)
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject PlayerRef;
    public LayerMask TargetMask;
    public LayerMask ObstructionMask;
    public float FOVdelay;
    private bool hasFoundPlayer;

    void Start()
    {
        //Initialize the status variables
        isAttacking = false;
        isWalking = false;
        
        SetStatusChangeTime();

        animation_controller = GetComponent<Animator>();

        //Set direction of enimy
        PickRandomDirection();
        PickRandomStatus();

        //Init fov
        PlayerRef = GameObject.FindGameObjectWithTag("Player");
        
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!hasFoundPlayer && StatusChangeTime <= Time.time){
            PickRandomDirection();
            PickRandomStatus();
            SetStatusChangeTime();
        }
        if(!hasFoundPlayer){
            animation_controller.SetBool("IsRunning", false);
            if(isWalking)
                transform.position += transform.forward*speed*Time.deltaTime;
        }
        else if(hasFoundPlayer){
            animation_controller.SetBool("IsRunning", true);
            Vector3 tmp = PlayerRef.transform.position - transform.position;
            tmp.y = 0;
            transform.position += tmp.normalized*speed*1.5f*Time.deltaTime;
        }
        if(transform.position.y > 0.7){
            Vector3 p = transform.position;
            p.y = 0.5f;
            transform.position = p;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.transform.name != "Player" && other.transform.tag != "Floor"){
            SetStatus(false);
        }
        else{
            GameManager.Score -= 200;
            Debug.Log("attack Player");
            Object.Destroy(transform.gameObject);
        }
    }

    void PickRandomDirection(){
        curDirection = MazeDirections.RandomValue;
        transform.rotation = curDirection.ToRotation();
    }
    void PickRandomStatus(){

        SetStatus(Random.Range(0, 2) > 0? true : false);
    }
    void SetStatusChangeTime(){
        StatusChangeTime = Time.time + Random.Range(1.0f, 5.0f);
    }
    void SetStatus(bool stat){
        isWalking = stat;
        animation_controller.SetBool("IsWalking",isWalking);
    }


    private IEnumerator FOVRoutine(){
        WaitForSeconds wait = new WaitForSeconds(FOVdelay);

        while(true){
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck(){
        Collider [] rangeChecks = Physics.OverlapSphere(transform.position, radius, TargetMask);
        
        if(rangeChecks.Length != 0){
            Transform target = rangeChecks[0].transform;
            Vector3 tmp = target.position - transform.position;
            tmp.y = 0f;
            
            Vector3 dirToTarget = tmp.normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < angle/2){
                float distanceToTarget = Vector3.Distance(target.position, transform.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, ObstructionMask))
                    hasFoundPlayer = true;
                else
                    hasFoundPlayer = false;
            }
            else
                hasFoundPlayer = false;
        }
        else if(hasFoundPlayer)
        {
            hasFoundPlayer = false;
        }
    }
}
