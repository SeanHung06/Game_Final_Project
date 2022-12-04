using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Sean_GameManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float time_limitation = 120.0f;
    public GameObject time_text;
    public GameObject PlayerPerfab;
    
    internal GameObject player_obj;
    internal List<string> fruit_names = new List<string>{"apple", "avocado", "banana", "cherries", "lemon","peach", "peanut", "pear", "strawberry", "watermelon"};

    private GameObject player_prefab;
    private float rest_time;
    private List<GameObject> fruit_prefabs;

    // [Sean] to add the feature of Scoring System 
    public static string target_fruit_name ;
    public Text Target_Text;    
    public Text Score_Text;    
    public static int Score;
    // public bool is_wrong_fruit;
    public static bool is_wrong_fruit;    
    public Text Warning_Text;    
    public GameObject text_box;
    private float timestamp_last_msg = 0.0f; // timestamp used to record when last message on GUI happened (after 7 sec, default msg appears)

    // [Sean]
    void Start()
    {
        //Variable initialize
        rest_time = time_limitation;

        //Spawn player
        //player_prefab = (GameObject)Resources.Load(@"Standard Assets/Characters/FirstPersonCharacter/Prefabs/FPSController");
        player_prefab = PlayerPerfab;
        if(player_prefab == null) Debug.Log("Wrong Path");
        else SpawnGameobject(player_prefab, "Player", new Vector3(0.0f, 1.0f, 0.0f));


        // [Sean]Select the target fruit for player to find 
        GameObject target_fruit = GetRamdonFruitId();
        target_fruit_name = target_fruit.name;
        Target_Text.text = "Target: " + target_fruit_name;
        Score = 0;
        Score_Text.text = "Score: " + Score;

        // [Sean] Set up the wrong fruit bool 
        is_wrong_fruit = false;
        timestamp_last_msg = 0.0f;


    }

    // Update is called once per frame
    void Update()
    {
        //update rest time and change the time text
        rest_time -= Time.deltaTime;
        TMP_Text text = time_text.GetComponent<TMP_Text>();
        text.text = ((int)rest_time).ToString();

        // [Sean] Check if the Player found the target fruit than we add the point 
        Score_Text.text = "Score: " + Score;

        // [Sean] Show the warning of the Game that it is picking up the wrong fruit
        if (Time.time - timestamp_last_msg > 3.0f) // renew the msg by restating the initial goal
        {
            text_box.GetComponent<Text>().text = "";   
        }
        if (is_wrong_fruit){
            text_box.GetComponent<Text>().text = "Picking up the wrong fruit!!";
            timestamp_last_msg = Time.time;
        }

        is_wrong_fruit = false;


        // is_wrong_fruit = false;

        // if (!is_wrong_fruit){
        //     Warning_Text.text = "";
        // }


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
