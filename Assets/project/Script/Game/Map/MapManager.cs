using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	Grid grids;

	void Awake() {
		grids = GetComponent<Grid>();
		GetBarriers();
	}	


	public Renderer[] GetBarriers() {
		Transform barrierHolder = transform.Find("Barrier");
		Renderer[] barrier = new Renderer[barrierHolder.childCount];
		for (int i = 0; i < barrierHolder.childCount; i++) {
			barrier[i] = barrierHolder.GetChild(i).GetComponent<Renderer>(); 
		}
		return barrier; 
	}
}
