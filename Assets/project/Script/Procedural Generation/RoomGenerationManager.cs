using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Utility;

public class RoomGenerationManager : MonoBehaviour {
	public int maxRoomNum = 10;
	public int roomNum = 10;
	private GameObject mRoomHolder;

	public List<Room> roomsPrefab = new List<Room>();



	public void GenerateDungeon() {
		roomNum = maxRoomNum;
		roomsPrefab = Resources.LoadAll<Room>("Prefab/Room").ToList();

		if (transform.FindChild("RoomHolder") != null)	Object.DestroyImmediate(transform.FindChild("RoomHolder").gameObject);
		mRoomHolder = new GameObject();
		mRoomHolder.name = "RoomHolder";
		mRoomHolder.transform.SetParent(transform);

		List<Room> openRoom = new List<Room>();
		HashSet<Room> closeRoom = new HashSet<Room>();

		Room startRoom = CreateStartRoom();
		openRoom.Add(startRoom);


		while (openRoom.Count > 0) {
			if (roomNum <= 0) break; 

			Room currentRoom = openRoom[0];
			int availableRoom = Mathf.Clamp(Random.Range(1, 3), 0, roomNum);
			currentRoom.DecideDoor(availableRoom);

			openRoom.Remove(currentRoom);
			closeRoom.Add (currentRoom);


			foreach (Door door in currentRoom.expandDoors) {
				//Create Room
				if (roomNum > 0) {
					Room newRoom = CreateRoom(currentRoom, door);
					if (newRoom == null) continue;
					openRoom.Add(newRoom);
					roomNum--;
				}
			}
			openRoom.Add(currentRoom);
		}
	}

	public Room CreateStartRoom() {
		GameObject mRoomObject = Instantiate(roomsPrefab[0].gameObject, Vector3.zero, transform.rotation  ) as GameObject;
		Room mRoom = mRoomObject.GetComponent<Room>();
		mRoom.transform.SetParent(mRoomHolder.transform);
		mRoom.name = "Start Room";
		return mRoom;
	}


	public Room CreateRoom(Room previousRoom, Door previousDoor) {
		Room roomPrefab = roomsPrefab[Random.Range(0, roomsPrefab.Count) ];

		Vector3 roomSize = roomPrefab.GetSize();
		float   length = ((previousDoor.head_direction.x == 0) ? roomSize.z : roomSize.x) /1.5f;
		Vector3	direction = previousDoor.head_direction * Mathf.RoundToInt( length),
				newPosition = new Vector3( previousDoor.transform.position.x + direction.x, 0,
											previousDoor.transform.position.z + direction.y);
				

		Door doors = roomPrefab.GetComponentsInChildren<Door>().ToList().Find(x => -x.head_direction == previousDoor.head_direction);
		Vector3 distance = newPosition - doors.transform.position;

		if (!CheckRoomPlaceValid(transform.position + distance, roomSize )) return null;

		GameObject mRoomObject = Instantiate(roomPrefab.gameObject, Vector3.zero, transform.rotation  ) as GameObject;
		Room mRoom = mRoomObject.GetComponent<Room>();

		mRoom.transform.position += distance; 
		mRoom.transform.SetParent(mRoomHolder.transform);
		previousDoor.isConnected  = true;
		mRoom.GetComponentsInChildren<Door>().ToList().Find(x => -x.head_direction == previousDoor.head_direction).isConnected = true;

		return mRoom;
	}

	public bool CheckRoomPlaceValid(Vector3 p_position, Vector3 p_radius) {
		Collider[] colliders = Physics.OverlapBox(p_position, p_radius, transform.rotation, GeneralSetting.gridLayer);
		return (colliders.Length <= 0);
	}
}
