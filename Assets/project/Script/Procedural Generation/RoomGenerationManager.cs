using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Utility;

public class RoomGenerationManager : MonoBehaviour {
	public int maxRoomNum = 10;
	public int roomNum = 10;
	private GameObject mRoomHolder;
	public Sprite[] demoSprite;

	private List<Room> roomsPrefab = new List<Room>();
	public List<Room> mRoomsList = new List<Room>();

	public void GenerateDungeon() {
		GenerateDungeonRoom();
		GenerateBridge();
	}

	public void GenerateDungeonRoom() {
		roomNum = maxRoomNum;
		roomsPrefab = Resources.LoadAll<Room>("Prefab/Room").ToList();
		mRoomsList.Clear();

		List<Room> openRoom = new List<Room>();

		Room startRoom = CreateStartRoom();
		openRoom.Add(startRoom);
		mRoomsList.Add (startRoom);
		roomNum--;

		while (openRoom.Count > 0) {
			if (roomNum <= 0) break; 

			Room currentRoom = openRoom[0];
			int availableRoom = Mathf.Clamp(Random.Range(1, 3), 0, roomNum);
			currentRoom.DecideDoor(availableRoom);

			openRoom.Remove(currentRoom);


			foreach (Door door in currentRoom.expandDoors) {
				//Create Room
				if (roomNum > 0) {
					Room newRoom = CreateRoom(currentRoom, door);
					if (newRoom == null) continue;
					openRoom.Add(newRoom);
					mRoomsList.Add (newRoom);

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
		mRoom.OpenShadowCasting();
		return mRoom;
	}


	public Room CreateRoom(Room previousRoom, Door previousDoor) {
		Room roomPrefab = roomsPrefab[Random.Range(0, roomsPrefab.Count) ];

		Vector3 roomSize = roomPrefab.GetSize();
		float   length = Random.Range(3, 8);
//		float   length = (( previousDoor.head_direction.x == 0f ) ? roomSize.z : roomSize.x);
		Vector3	direction = previousDoor.head_direction * Mathf.RoundToInt( length),
				newPosition = new Vector3( previousDoor.transform.position.x + direction.x, 0,
											previousDoor.transform.position.z + direction.y);
		Door doors = roomPrefab.GetComponentsInChildren<Door>().ToList().Find(x => -x.head_direction == previousDoor.head_direction);
		Vector3 distance = newPosition + ( roomPrefab.transform.position - doors.transform.position);
		//Vector3 distance = newPosition;

		if (!CheckRoomPlaceValid(distance, roomSize )) return null;

		GameObject mRoomObject = Instantiate(roomPrefab.gameObject, distance, transform.rotation  ) as GameObject;
		Room mRoom = mRoomObject.GetComponent<Room>();

		mRoom.transform.SetParent(mRoomHolder.transform);
		Door roomDoor = mRoom.GetComponentsInChildren<Door>().ToList().Find(x => -x.head_direction == previousDoor.head_direction);
		previousDoor.connectedDoor  = roomDoor;
		roomDoor.connectedDoor  = previousDoor;

		return mRoom;
	}


	public void GenerateBridge() {
		foreach (Room room in mRoomsList) {
			foreach (Door door in room.expandDoors ) {
				CreateBridge(door, door.connectedDoor);
			}
		}
	}

	public void CreateBridge(Door p_doorA, Door p_doorB) {
		if (p_doorB == null || p_doorA.isConnected || p_doorB.isConnected ) return;
		int distance = Mathf.RoundToInt( Vector3.Distance(p_doorA.transform.position, p_doorB.transform.position) );

		GameObject bridgeObject = new GameObject();
		bridgeObject.name = "Bridge";
		bridgeObject.transform.SetParent(mRoomHolder.transform);
		bridgeObject.transform.position = p_doorA.transform.position;
		for (int i = 1; i < distance; i++) {
			Vector3 roadLength = p_doorA.head_direction * i,
					roadPosition = p_doorA.transform.position + new Vector3(roadLength.x, 0 , roadLength.y);

			//Main Road
			SpriteRenderer bridgeTile = CreateEmptyTile(bridgeObject.transform, roadPosition, 
				UtilityMethod.LoadSpriteFromMulti( demoSprite, "room_demo_road"));

			//Walls
			SpriteRenderer wallTileL = CreateEmptyTile(bridgeObject.transform, 
				p_doorA.transform.position + new Vector3(roadLength.x + p_doorA.head_direction.y, 0 , roadLength.y + p_doorA.head_direction.x),
				UtilityMethod.LoadSpriteFromMulti( demoSprite, "room_demo_wall") );

			SpriteRenderer wallTileR = CreateEmptyTile(bridgeObject.transform,
				p_doorA.transform.position + new Vector3(roadLength.x - p_doorA.head_direction.y, 0 , roadLength.y - p_doorA.head_direction.x),
				UtilityMethod.LoadSpriteFromMulti( demoSprite, "room_demo_wall"));

		}


		p_doorA.isConnected = true;
		p_doorB.isConnected = true;
	}

	public SpriteRenderer CreateEmptyTile(Transform p_parent, Vector3 p_position, Sprite p_sprite) {
		GameObject tile = new GameObject();
		tile.transform.SetParent(p_parent);
		tile.transform.Rotate(new Vector3(90,0,0));
		tile.transform.position = p_position;

		tile.transform.localScale = new Vector3(0.8f, 0.8f, 0);
		SpriteRenderer spriteRenderer = tile.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = p_sprite;

		return spriteRenderer;
	}

	public bool CheckRoomPlaceValid(Vector3 p_position, Vector3 p_radius) {
		Collider[] colliders = Physics.OverlapBox(p_position, p_radius, transform.rotation, GeneralSetting.gridLayer);
		return (colliders.Length <= 0);
	}
}
