using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float time_limitation = 5.0f;
    
    //public GameObject time_text; [comment by Sean to use the Text ]
    public GameObject PlayerPerfab;
    
    internal GameObject player_obj;
    internal List<string> fruit_names = new List<string>{"apple", "avocado", "banana", "cherries", "lemon","peach", "peanut", "pear", "strawberry", "watermelon"};

    private GameObject player_prefab;
    private float rest_time;
    private List<GameObject> fruit_prefabs;

    // [Sean add sound for scroing ]
    private AudioSource source;
    public AudioClip Score_audio;
    public AudioClip Wrong_audio;

    // [Sean add sound for getting points after success]

    // [Sean] to add the feature of Scoring System 
    public static string target_fruit_name ;
    public Text Target_Text;    
    public Text Time_Text;    
    public Text Score_Text;    
    public static int Score;
    // public bool is_wrong_fruit;
    public static bool is_wrong_fruit;    
    public static bool is_target_fruit;    
    public Text Warning_Text;    
    public GameObject text_box;
    private float timestamp_last_msg = 0.0f; // timestamp used to record when last message on GUI happened (after 7 sec, default msg appears)

    //[Joshua] maze and player varialbles
    public int TargetNumber ;
    // [Sean] Add total Fruit number
    public int Total_Fruit_Number ;
    public Maze mazePrefab;
    private Maze mazeInstance;
    public GameObject playerPrefab;
	private GameObject playerInstance;
    public FruitManager FruitManagerPrefab;
    private FruitManager FruitManagerInstance;

    //[Johsua] Enemies' varible
    public GameObject EnemyManagerPrefab;
    private GameObject EnemyManagerInstance;
    public Enimy enimyPrefab;
    private GameObject enimyInstance;
    public int NumberOfEnemy = 5;
    public float SpeedOfEnemy = 1.5f;
    private List<Enimy> EnemyArray;

    //[Sean] End Game Control
    private bool end_game;    
    public Text End_Text;    

    //  [Sean] Firework Object 
    public GameObject Firework;

    // [Sean] Check if the Maze have created
    public bool game_started;
    void Start()
    {
        //Variable initialize
        end_game = false;
        StartCoroutine(BeginGame());
        rest_time = time_limitation;

        // [Sean]Select the target fruit for player to find 
        GameObject target_fruit = GetRamdonFruitId();
        target_fruit_name = target_fruit.name;
        Target_Text.text = "Target: " + target_fruit_name;
        Score = 0;
        Score_Text.text = "Score: " + Score;
        Time_Text.text = "Time: " + rest_time;

        // [Sean] Set up the check target fruit bool 
        is_wrong_fruit = false;
        timestamp_last_msg = 0.0f;
        is_target_fruit = false;

        // [Sean] Audio 
        source = GetComponent<AudioSource>();
        // [Sean] End game setting
        End_Text.GetComponent<Text>().enabled = false;

        // [Sean] Firework Object set false at first and enable it when the games is about to end
        Firework.SetActive(false);
        // [Sean] Maze not started
        game_started = false;

    }

    // Update is called once per frame
    void Update()
    {
        //[Sean] update rest time and change the time text
        if (game_started){
            rest_time -= Time.deltaTime;
        }
        
        // TMP_Text text = time_text.GetComponent<TMP_Text>(); [comment by Sean to use the Text ]
        // text.text = ((int)rest_time).ToString(); [comment by Sean to use the Text ]
        Time_Text.text = "Time: " + ((int)rest_time).ToString();


        // [Sean] Show the warning of the Game that it is picking up the wrong fruit
        if (Score < 0){ Score =0 ;}
        Score_Text.text = "Score: " + Score;
        if (rest_time <= 0){
            rest_time = 0 ;
            // [Sean]End Game !!! 
            end_game = true;
            End_Text.GetComponent<Text>().enabled = true;
            // [Sean] Freeze the Game
            // Time.timeScale = 0;
            // [Sean] Firework enable
            Firework.SetActive(true);

            // [Sean] Press R to Restart the Game
            if (Input.GetKeyDown(KeyCode.R)){
                RestartGame();
            }


        } 
        if (Time.time - timestamp_last_msg > 3.0f) // renew the msg by restating the initial goal
        {
            text_box.GetComponent<Text>().text = "";   
        }
        // [Sean] Check if the Player found the target fruit than we add the point 

        if (is_target_fruit){
            text_box.GetComponent<Text>().text = "Picking up the target fruit!!";
            timestamp_last_msg = Time.time;
            source.clip = Score_audio;
            if (!source.isPlaying){
                source.Play();
            }

            // [Sean] After we pick up the correct fruit we randomly generate another target to balance the total target num
            List<MazeCell> newtargetCells = GetRandomCellArray(TargetNumber+1);
            Vector3 pos_new = newtargetCells[newtargetCells.Count-1].transform.position;
            newtargetCells.RemoveAt(newtargetCells.Count-1);
            FruitManagerInstance.CreateTargetFruit(pos_new);
        }
        if (is_wrong_fruit){
            text_box.GetComponent<Text>().text = "Picking up the wrong fruit!!";
            timestamp_last_msg = Time.time;
            source.clip = Wrong_audio;
            if (!source.isPlaying){
                source.Play();
            }

        }
        is_wrong_fruit = false;
        is_target_fruit = false;

        Target_Text.text = "Target: " + target_fruit_name;
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }

    private IEnumerator BeginGame () {
        
        //[Joshua]Create Maze
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        //[Johsua]Get random maze grid for player and target fruits
        List<MazeCell> randomCells = GetRandomCellArray(Total_Fruit_Number+1);
        // [Sean] Modify the Maze scale 
        mazeInstance.transform.localScale = new Vector3(3f, 3f, 3f);
        // [Sean] Get the Target Fruit List
        List<MazeCell> targetCells = GetRandomCellArray(TargetNumber+1);

        //Create and initialize Player instance
        playerInstance = Instantiate(playerPrefab);
        Vector3 playerPos = randomCells[randomCells.Count-1].transform.position;
        randomCells.RemoveAt(randomCells.Count-1);
        // [Sean] Update the player Y position 
        playerPos.y = 0.1f;
        playerInstance.transform.localPosition = playerPos;
        playerInstance.transform.localScale = new Vector3(1f, 0.4f, 1f);
        playerInstance.transform.name = "Player";
        playerInstance.transform.gameObject.layer = LayerMask.NameToLayer("Player");
        playerInstance.transform.tag = "Player";
        CapsuleCollider collider = playerInstance.GetComponent<CapsuleCollider>();
        collider.radius = 0.38f;

        //Create Fruit Manager Instance
        FruitManagerInstance = Instantiate(FruitManagerPrefab) as FruitManager;
        FruitManagerInstance.InitializeOther();
        FruitManagerInstance.InitializeTarget();

        //Create Target Fruits // [Sean] Should be total Fruits  
        while(randomCells.Count>0){
            Vector3 pos = randomCells[randomCells.Count-1].transform.position;
            randomCells.RemoveAt(randomCells.Count-1);
            // [Sean] Add to Randomize the maze fruit 
            FruitManagerInstance.InitializeOther();
            // [Sean]
            FruitManagerInstance.CreateOtherFruit(pos);
        }
        // [Sean] Create the Begin Game Target Fruit 
        while(targetCells.Count>0 ){
            Vector3 pos_1 = targetCells[targetCells.Count-1].transform.position;
            targetCells.RemoveAt(targetCells.Count-1);
            FruitManagerInstance.CreateTargetFruit(pos_1);
            target_fruit_name = FruitManagerInstance.getFruitName();
        }

        //Create and initialize enemy
        EnemyManagerInstance = Instantiate(EnemyManagerPrefab);
        List<MazeCell> enemyPos = GetRandomCellArray(NumberOfEnemy);
        EnemyArray = new List<Enimy>();
        foreach(MazeCell pos in enemyPos)
        {
            Vector3 p = pos.transform.position;
            Enimy e = Instantiate(enimyPrefab) as Enimy;
            p.y = 0.5f;
            e.transform.position = p;
            e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            e.speed = SpeedOfEnemy;
            e.transform.parent = EnemyManagerInstance.transform;
        }
        game_started = true;
    }

	private void RestartGame () {
        StopAllCoroutines();
        Object.Destroy(mazeInstance.gameObject);
        if (playerInstance != null) {
			Object.Destroy(playerInstance.gameObject);
		}
        Object.Destroy(FruitManagerInstance.gameObject);
        Object.Destroy(EnemyManagerInstance);
        StartCoroutine(BeginGame());
    }
    private List<MazeCell> GetRandomCellArray(int number){
        List<MazeCell> res = new List<MazeCell>();
        int[,] mem = new int [mazeInstance.size.x, mazeInstance.size.z];
        while(res.Count<number){
            IntVector2 coor = mazeInstance.RandomCoordinates;
            if(mem[coor.x, coor.z]==1)continue;
            res.Add(mazeInstance.GetCell(coor));
            mem[coor.x, coor.z] = 1;
        }
        return res;
    }

    
    private GameObject GetRamdonFruitId(){
        int fruit_idx = Random.Range(0, fruit_names.Count);
        string fruitname = fruit_names[fruit_idx];
        GameObject fruit_prefab = (GameObject)Resources.Load("Low Poly Fruits/Prefabs/"+fruitname);
        return fruit_prefab;
    }
}
