using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    internal string fruitName;
    private void OnCollisionEnter(Collision other) {
        if(other.transform.name == "Player"){
            Debug.Log("The player pickup "+fruitName);
            Object.Destroy(transform.gameObject);
        }
        else{
            Debug.Log(other.transform.name);
        }
    }
    
}
