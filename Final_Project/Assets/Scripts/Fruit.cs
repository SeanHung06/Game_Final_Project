using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    internal string fruitName;
    private void OnCollisionEnter(Collision other) {
        if(other.transform.name == "Player" && transform.gameObject.name == Sean_GameManager1.target_fruit_name){
            // [Sean] Add the Score Update if we encounter the correct Fruit
            Debug.Log("The player pickup "  +  transform.gameObject.name);
            Sean_GameManager1.Score += 1;
            Debug.Log(Sean_GameManager1.Score);
            Object.Destroy(transform.gameObject);
            // [Sean]
        }
        else{
            // [Sean] Minus point if we encounter the wrong fruit 
            Debug.Log("The player pickup "  +  transform.gameObject.name);
            Sean_GameManager1.is_wrong_fruit = true;
            Sean_GameManager1.Score -= 1;
            // [Sean] Add the warning when player pick up the wrong fruit
            Object.Destroy(transform.gameObject);
            //Debug.Log(other.transform.name);
        }
    }
    
}
