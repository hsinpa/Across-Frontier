using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class WayPoint : BehaviorComponent {
	private Renderer[] barriers;
	private Vector3[] waypoints;
	private float range;

	public WayPoint(BaseUnit p_unit, Renderer[] p_barrier, float p_range) {
		unit = p_unit;
		barriers = p_barrier;
		range = p_range;
		rigidbody = p_unit.GetComponent<Rigidbody>();

		SetWayPoint(barriers);
	}

	public override void Update() {

	}

	public void SetWayPoint(Renderer[] barriers) {
		if (barriers.Length <= 0) return;
		List<Vector3> availableWayPoints = new List<Vector3>();
		Vector3[] pickedWayPoints = new Vector3[2];

		foreach(Renderer render in barriers) {
			//
			float offset = render.bounds.size.z;
			if (render.bounds.size.z > render.bounds.size.x) {
				availableWayPoints.Add(render.transform.position + ( render.transform.forward * offset));
			} else {
				offset = render.bounds.size.x;
				availableWayPoints.Add(render.transform.position + ( render.transform.right * offset));
			}
		}

		int randomIndex = Random.Range(0, availableWayPoints.Count-1 );
		pickedWayPoints[0] = new Vector3(availableWayPoints[randomIndex].x, unit.transform.position.y, availableWayPoints[randomIndex].z);
		availableWayPoints.RemoveAt(randomIndex);
		
		if (availableWayPoints.Count <= 0) return;
		randomIndex = Random.Range(0, availableWayPoints.Count -1);
		pickedWayPoints[1] = new Vector3(availableWayPoints[randomIndex].x, unit.transform.position.y, availableWayPoints[randomIndex].z);
		waypoints = pickedWayPoints;
	}

	public override Vector3 FindTargetLocation() {
		if (waypoints.Length <=  0) return Vector3.zero;

		if (waypoints.Length == 1) return waypoints[0];

		float distance = Vector3.Distance(unit.transform.position, waypoints[0]);
		return (distance > range) ? waypoints[0] : waypoints[1];
	}
}
