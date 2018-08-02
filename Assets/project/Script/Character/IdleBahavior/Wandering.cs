using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteerMovement;
using System.Linq;

public class Wandering : BehaviorComponent{
	private int raycastNum;
	private float viewAngle;
	private SteerBase[] steerBehaviors; 

	public Wandering(BaseUnit p_unit, float p_viewAngle, int p_raycastNum) {
		unit = p_unit;
		raycastNum = p_raycastNum;
		viewAngle = p_viewAngle;
		rigidbody = unit.GetComponent<Rigidbody>();
	}

	public override void Update( ) {

	}

	public override Vector3 FindTargetLocation() {
		Vector3 _targetLocation,
				currentRotation = unit.transform.rotation.eulerAngles;
				
		float startViewAngle = currentRotation.y - (viewAngle * 0.5f);
		float anglePerRaycast = (float)viewAngle / raycastNum;

		List<Vector3> storedLocation = new List<Vector3>();

		for (int i = 0; i < raycastNum; i++) {
			float angle = startViewAngle + (anglePerRaycast * i);
			Vector3 raycastAngle = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

			RaycastHit hit;
			bool hasHitSomething = Physics.Raycast(unit.transform.position, raycastAngle, out hit, viewAngle, 1 << 9);
			if (hasHitSomething) {
				float dist = (unit.transform.position - hit.point).magnitude;
				Vector3 dir = (unit.transform.position - hit.point).normalized,
						targetLocation = hit.point + (dir * dist * 0.4f);

				storedLocation.Add(targetLocation);
			} else {
				storedLocation.Add(unit.transform.position + (raycastAngle * viewAngle));
			}
		}

		storedLocation = storedLocation.OrderByDescending(x=> {
			return (x - unit.transform.position).sqrMagnitude;
		}).ToList();

		float average = storedLocation.Average( x=> (x - unit.transform.position).magnitude);
		if ( average < 1.5f ) {
			_targetLocation = unit.transform.position + (-unit.transform.forward * 3);
		} else {
			_targetLocation = storedLocation[0];
		}

		return _targetLocation;
	}

}
