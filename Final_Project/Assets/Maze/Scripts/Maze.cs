using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public IntVector2 size;
    public float generationStepDelay;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
	public MazeWall wallPrefab;
    private MazeCell[,] cells;

    public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}
    public IEnumerator Generate(){
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        IntVector2 coordinates = RandomCoordinates;
        DoFirstGenerationStep(activeCells);
        while(activeCells.Count > 0){
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }
    private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
        if(currentCell.IsFullyInitialized){
            activeCells.RemoveAt(currentIndex);
            return;
        }
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
            if(neighbor == null){
                neighbor = CreateCell(coordinates);
                CreatPassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else{
                CreateWall(currentCell, neighbor, direction);
                
            }
		}
		else {
			CreateWall(currentCell, null, direction);
            
		}
	}
    private MazeCell CreateCell(IntVector2 coordinates){
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell "+coordinates.x+","+coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x-size.x*0.5f+0.5f, 0f, coordinates.z-size.z*0.5f+0.5f);
        return newCell;
    }
    private void CreatPassage(MazeCell cell, MazeCell othercell, MazeDirection direction){
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, othercell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(othercell, cell, direction.GetOpposite());
    }
    private void CreateWall(MazeCell cell, MazeCell othercell, MazeDirection direction){
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, othercell, direction);
        if(othercell != null){
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(othercell, cell, direction.GetOpposite());
        }
    }

    public IntVector2 RandomCoordinates{
        get {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }
    public bool ContainsCoordinates(IntVector2 coordinates){
        return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.z;
    }
}
