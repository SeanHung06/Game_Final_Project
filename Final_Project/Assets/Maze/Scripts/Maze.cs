using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public IntVector2 size;
    public float generationStepDelay;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
	public MazeWall[] wallPrefab;
    public MazeDoor doorPrefab;
	[Range(0f, 1f)]
	public float doorProbability;
    public MazeRoomSettings[] roomSettings;
    private MazeCell[,] cells;
    private List<MazeRoom> rooms = new List<MazeRoom>();

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
        MazeCell newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}
    private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room) {
			MazeRoom roomToAssimilate = otherCell.room;
			cell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
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
            else if(currentCell.room.settingIndex == neighbor.room.settingIndex){
                CreatePassageInSameRoom(currentCell, neighbor, direction);
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
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, othercell, direction);
        passage = Instantiate(prefab) as MazePassage;
        if(passage is MazeDoor){
            othercell.Initialize(CreateRoom(cell.room.settingIndex));
        }
        else{
            othercell.Initialize(cell.room);
        }
        passage.Initialize(othercell, cell, direction.GetOpposite());
    }
    private void CreateWall(MazeCell cell, MazeCell othercell, MazeDirection direction){ 
        MazeWall wall = Instantiate(wallPrefab[Random.Range(0, wallPrefab.Length)]) as MazeWall;
        wall.Initialize(cell, othercell, direction);
        if(othercell != null){
            wall = Instantiate(wallPrefab[Random.Range(0, wallPrefab.Length)]) as MazeWall;
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
    private MazeRoom CreateRoom(int indexToExclude){
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingIndex = Random.Range(0, roomSettings.Length);
        if(newRoom.settingIndex == indexToExclude){
            newRoom.settingIndex = (newRoom.settingIndex+1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingIndex];
        rooms.Add(newRoom);
        return newRoom;
    }
}
