using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject [] FruitPrefab;
    public float height = 0.3f;
    private GameObject targetFruitInstance;
    private string targetFruitName;
    private int targetIndex = 0;

    // [Sean] Add the other Fruit Variable
    private GameObject otherFruitInstance;
    private string otherFruitName;
    private int otherIndex = 0;
    // [Sean] add to retrieve the Fruit name
    public string getFruitName() {
        return targetFruitName;
    }
    public void CreateTargetFruit(Vector3 pos){
        GameObject prefab = FruitPrefab[targetIndex];
        targetFruitName = prefab.transform.name;
        targetFruitInstance = Instantiate(prefab);
        targetFruitInstance.transform.parent = transform;
        pos.y = height;
        targetFruitInstance.transform.position = pos;
        // [Sean] Update the Fruit Scale to fit the maze size
        targetFruitInstance.transform.localScale = new Vector3(40f, 40f, 40f);
        targetFruitInstance.AddComponent<Fruit>();
        targetFruitInstance.AddComponent<SphereCollider>();
    }
    // [Sean] Add to clarify the Target and Other Fruits 
    public void CreateOtherFruit(Vector3 pos){
        GameObject prefab = FruitPrefab[otherIndex];
        otherFruitName = prefab.transform.name;
        otherFruitInstance = Instantiate(prefab);
        otherFruitInstance.transform.parent = transform;
        pos.y = height;
        otherFruitInstance.transform.position = pos;
        // [Sean] Update the Fruit Scale to fit the maze size
        otherFruitInstance.transform.localScale = new Vector3(40f, 40f, 40f);
        otherFruitInstance.AddComponent<Fruit>();
        otherFruitInstance.AddComponent<SphereCollider>();
    }
    // [Sean] Change the Target to other 
    public void InitializeOther(){
        // [Sean] add other Fruit
        otherIndex = Random.Range(0, FruitPrefab.Length);
    }
    // [Sean] Intialize Target 
    public void InitializeTarget(){
        targetIndex = Random.Range(0, FruitPrefab.Length);
    }
}
