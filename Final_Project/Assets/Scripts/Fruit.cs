using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    internal string fruitName;
    private void OnCollisionEnter(Collision other) {
        //Debug.Log(GameManager.target_fruit_name);
        //Debug.Log(transform.gameObject.name);

        if(other.transform.name == "Player" && transform.gameObject.name == (GameManager.target_fruit_name+"(Clone)")){
            // [Sean] Add the Score Update if we encounter the correct Fruit
            Debug.Log("The player pickup "  +  transform.gameObject.name);
            GameManager.is_target_fruit = true;
            GameManager.Score += 300;
            Debug.Log(GameManager.Score);
            Object.Destroy(transform.gameObject);
            // [Sean]
        }
        else{
            // [Sean] Minus point if we encounter the wrong fruit 
            // [Sean] Add the warning when player pick up the wrong fruit
            GameManager.is_wrong_fruit = true;
            GameManager.Score -= 100;
            Object.Destroy(transform.gameObject);
            //Debug.Log(other.transform.name);
        }
    }
    
}
