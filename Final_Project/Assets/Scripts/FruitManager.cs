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
    
    public void CreateTargetFruit(Vector3 pos){
        GameObject prefab = FruitPrefab[targetIndex];
        targetFruitName = prefab.transform.name;
        targetFruitInstance = Instantiate(prefab);
        targetFruitInstance.transform.parent = transform;
        pos.y = height;
        targetFruitInstance.transform.position = pos;
        targetFruitInstance.transform.localScale = new Vector3(20f, 20f, 20f);
        targetFruitInstance.AddComponent<Fruit>();
        targetFruitInstance.AddComponent<SphereCollider>();
    }
    public void InitializeTarget(){
        targetIndex = Random.Range(0, FruitPrefab.Length);
    }
}
