using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayer : MonoBehaviour
{
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
			Move(MazeDirection.North);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			Move(MazeDirection.East);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Move(MazeDirection.South);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Move(MazeDirection.West);
		}
    }
}
