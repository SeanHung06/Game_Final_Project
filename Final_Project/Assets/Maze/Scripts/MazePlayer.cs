using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayer : MonoBehaviour
{

    // [Sean add sound for walking]
    private AudioSource source;
    public AudioClip Walk;
    // [Sean add sound for walking]

    private MazeCell currentCell;
    public void SetLocation(MazeCell cell){
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
    }

    private void Move(MazeDirection direcrion){
        MazeCellEdge edge = currentCell.GetEdge(direcrion);
        if(edge is MazePassage){
            SetLocation(edge.otherCell);
        }
    }
   void Start()
    {
        // Audio 
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
			Move(MazeDirection.North);
            source.clip = Walk;
            if (!source.isPlaying){
                source.Play();
            }
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			Move(MazeDirection.East);
            source.clip = Walk;
            Debug.Log (source.isPlaying);

            if (!source.isPlaying){
                source.Play();
            }
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(MazeDirection.South);
            source.clip = Walk;
            Debug.Log (source.isPlaying);

            if (!source.isPlaying){
                source.Play();
            }
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Move(MazeDirection.West);
            source.clip = Walk;
            if (!source.isPlaying){
                source.Play();
            }
		}
    }
}
