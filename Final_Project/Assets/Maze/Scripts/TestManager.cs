using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public int TargetNumber = 10;
    public Maze mazePrefab;
    private Maze mazeInstance;
    public GameObject playerPrefab;
	private GameObject playerInstance;
    public FruitManager FruitManagerPrefab;
    private FruitManager FruitManagerInstance;

    private void Start () {
		StartCoroutine(BeginGame());
	}
	
	private void Update () {
		
	}

	private IEnumerator BeginGame () {
        

        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        
        List<MazeCell> randomCells = GetRandomCellArray(TargetNumber+1);
        Debug.Log(randomCells.Count);
        

        //Create Player instance
        playerInstance = Instantiate(playerPrefab);
        Vector3 playerPos = randomCells[randomCells.Count-1].transform.position;
        randomCells.RemoveAt(randomCells.Count-1);
        playerPos.y = 10f;
        playerInstance.transform.localPosition = playerPos;
        playerInstance.transform.localScale = new Vector3(1f, 0.4f, 1f);
        playerInstance.transform.name = "Player";
        CapsuleCollider collider = playerInstance.GetComponent<CapsuleCollider>();
        collider.radius = 0.38f;

        //Create Fruit Manager Instance
        FruitManagerInstance = Instantiate(FruitManagerPrefab) as FruitManager;
        FruitManagerInstance.InitializeTarget();
        //Create Target Fruits
        while(randomCells.Count>0){
            Vector3 pos = randomCells[randomCells.Count-1].transform.position;
            randomCells.RemoveAt(randomCells.Count-1);
            // [Sean] Add to Randomize the maze fruit 
            FruitManagerInstance.InitializeTarget();
            // [Sean]
            FruitManagerInstance.CreateTargetFruit(pos);
        }
    }

	private void RestartGame () {
        StopAllCoroutines();
        Object.Destroy(mazeInstance.gameObject);
        if (playerInstance != null) {
			Object.Destroy(playerInstance.gameObject);
		}
        Object.Destroy(FruitManagerInstance.gameObject);
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
}
