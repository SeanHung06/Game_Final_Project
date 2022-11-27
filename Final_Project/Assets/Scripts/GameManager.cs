using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float time_limitation = 60.0f;
    public GameObject time_text;
    
    internal GameObject player_obj;
    internal List<string> fruit_names = new List<string>{"apple", "avocado", "banana", "cherries", "lemon","peach", "peanut", "pear", "strawberry", "watermelon"};

    private GameObject player_prefab;
    private float rest_time;
    private List<GameObject> fruit_prefabs;
    void Start()
    {
        //Variable initialize
        rest_time = time_limitation;

        //Spawn player
        player_prefab = (GameObject)Resources.Load(@"Standard Assets/Characters/FirstPersonCharacter/Prefabs/FPSController");
        if(player_prefab == null) Debug.Log("Wrong Path");
        else SpawnGameobject(player_prefab, "Player", new Vector3(0.0f, 1.0f, 0.0f));

        //Get the random fruit prefab
        GameObject p = GetRamdonFruitId();
        SpawnGameobject(p, "fff", new Vector3(0.0f, 1.0f, 1.0f));
        

    }

    // Update is called once per frame
    void Update()
    {
        //update rest time and change the time text
        rest_time -= Time.deltaTime;
        TMP_Text text = time_text.GetComponent<TMP_Text>();
        text.text = ((int)rest_time).ToString();
    }
    private GameObject SpawnGameobject(GameObject prefab, string objName, Vector3 pos){
        GameObject obj = Instantiate(prefab);
        obj.name = objName;
        obj.transform.position = pos;
        return obj;
    }
    private GameObject GetRamdonFruitId(){
        int fruit_idx = Random.Range(0, fruit_names.Count);
        string fruitname = fruit_names[fruit_idx];
        GameObject fruit_prefab = (GameObject)Resources.Load("Low Poly Fruits/Prefabs/"+fruitname);
        return fruit_prefab;
    }
}
