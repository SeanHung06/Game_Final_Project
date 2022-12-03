using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    internal string fruitName;
    private void OnCollisionEnter(Collision other) {
        if(other.transform.name == "Player" and ){
            Debug.Log("The player pickup "+fruitName);
            Sean_GameManager1.Score += 1;
            Debug.Log(Sean_GameManager1.Score);
            Object.Destroy(transform.gameObject);

        }
        else{
            Debug.Log(other.transform.name);
        }
    }
    
}
