using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SteerMovement;

public class AIAgent : BaseUnit {
	
	public int viewAngle, raycastNum, _speed, _acceleration, arrival_radius;
	
	public Vector3 _velocity, _targetLocation = Vector3.zero;
	private Rigidbody _rigidBody;

	public Transform tempTarget;
	public MapManager map_manager;

	private SteerBase[] steerBehaviors; 
	private BehaviorComponent testBehaviorComponent;

	void Start() {
		// LookAround();
		_rigidBody = GetComponent<Rigidbody>();
		steerBehaviors = new SteerBase[] {
			new S_Seak( _speed, _acceleration, arrival_radius, 0.1f),
			new S_Arrive()
		};

		map_manager.GetBarriers();
		// WayPoint waypoint = new WayPoint(this, map_manager.GetBarriers(), 1f);
		// testBehaviorComponent = waypoint;

		Standing standing = new Standing(this, 60);
		testBehaviorComponent = standing;

		_targetLocation = testBehaviorComponent.FindTargetLocation();
	}

	public void Update() {
		if (testBehaviorComponent == null) return;
			// Vector3 dist = _targetLocation - transform.position;
			
			// if (dist.magnitude < 3f)  {
			// 	_targetLocation = Vector3.zero;
			// 	return;
			// }
			// SteeringOutput steeringOutput = new SteeringOutput();			
			for (int i = 0; i < steerBehaviors.Length; i++) {
				SteeringOutput steeringOutput = steerBehaviors[i].Execute(this, _targetLocation);

				if (!steeringOutput.isValid) {
					_targetLocation = testBehaviorComponent.FindTargetLocation();
					break;
				}
				
				transform.position +=  new Vector3(_velocity.x, 0, _velocity.z) * Time.deltaTime;

				_velocity += steeringOutput.linear * Time.deltaTime;

				if (steeringOutput.orientation != Quaternion.identity)
					transform.rotation = steeringOutput.orientation;
			}
	}

	void OnDrawGizmos()
	{
		Vector3 currentRotation = transform.rotation.eulerAngles;

		float startViewAngle = currentRotation.y - (viewAngle * 0.5f);
		float anglePerRaycast = (float)viewAngle / raycastNum;

		List<Vector3> storedLocation = new List<Vector3>();

		for (int i = 0; i < raycastNum; i++) {
			float angle = startViewAngle + (anglePerRaycast * i);
			Vector3 raycastAngle = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));

			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + (raycastAngle * _view_range));
		}
	}


}
