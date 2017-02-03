using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour {
	private List<Door> templateDoors = new List<Door>(); 
	public List<Door> expandDoors = new List<Door>(); 

	public void DecideDoor(int roomNum) {
		templateDoors = GetComponentsInChildren<Door>().ToList();

		int delete = templateDoors.RemoveAll(x=>x.connectedDoor != null);
		for(int i = 0; i < roomNum; i++) {
			if (templateDoors.Count <= 0 ) break;
			Door selectDoor =templateDoors [ Random.Range(0, templateDoors.Count) ];

			templateDoors.Remove(selectDoor);
			expandDoors.Add(selectDoor);
		}
	}

	public void OpenShadowCasting() {
	 SpriteRenderer[] sprites =	GetComponentsInChildren<SpriteRenderer>();
		 foreach (SpriteRenderer sprite in sprites) {
		 	sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		 	sprite.receiveShadows = true;
		 }
	}

	public Vector3 GetSize() {
		List<Transform> childs = GetComponentsInChildren<Transform>().ToList();
		float xMax = childs.Max(x=>x.transform.position.x),
			 xMin = childs.Min(x=>x.transform.position.x),
			 yMax = childs.Max(x=>x.transform.position.z),
			yMin = childs.Min(x=>x.transform.position.z);
		return new Vector3(xMax - xMin, 1, yMax-yMin);
	}

}
