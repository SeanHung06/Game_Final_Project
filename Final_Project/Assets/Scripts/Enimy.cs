using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enimy : MonoBehaviour
{
    public float waitingTime = 3.0f;
    public float speed = 2.0f;
    
    internal bool isAttacking;
    internal bool isWalking;
    internal bool isWaiting;
    private float StatusChangeTime;
    private bool hasFoundPlayer;

    private Animator animation_controller;
    
    private MazeDirection curDirection; //using MazeDirection

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
    }

    // Update is called once per frame
    void Update()
    {
        if(StatusChangeTime <= Time.time){
            PickRandomDirection();
            PickRandomStatus();
            SetStatusChangeTime();
        }
        if(isWalking){
            transform.position += transform.forward*speed*Time.deltaTime;
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
}
