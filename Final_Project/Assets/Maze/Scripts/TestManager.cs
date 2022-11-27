using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;

    private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}

	private void BeginGame () {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        StartCoroutine(mazeInstance.Generate());
    }

	private void RestartGame () {
        StopAllCoroutines();
        Object.Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
