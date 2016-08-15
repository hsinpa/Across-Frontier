using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

public class EditorMapGeneration : Editor {
	static Sprite[] MapSprite;

	[MenuItem ("Editor/Reset Grid Pos")]
	static void ResetGridPos () {
		GameObject gameBoard = GameObject.Find("_Map/RoomHolder");
		BoxCollider[] grids = gameBoard.GetComponentsInChildren<BoxCollider>();

		foreach (BoxCollider g in grids) {
			g.transform.position = new Vector3(Mathf.RoundToInt(g.transform.position.x),
											 g.transform.position.y, Mathf.RoundToInt(g.transform.position.z)  );
		}
	}
}
