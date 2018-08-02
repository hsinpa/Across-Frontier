using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Steering {
	public class Kinematic : MonoBehaviour {
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private float _orientation;

		[SerializeField]
		private float maxSpeed;

		[SerializeField]
		private float _rotation;

		Rigidbody _rigidBody;

		// Use this for initialization
		void Start () {
			_rigidBody = GetComponent<Rigidbody>();
		}

		void Update() {
			KinematicsSteeringOutput newSteering = GetSteering();
			// transform.position += newSteering.velocity;
			_rigidBody.velocity = newSteering.velocity;
		}
		

		KinematicsSteeringOutput GetSteering() {
			var steering = new KinematicsSteeringOutput();
			steering.velocity = _target.position - transform.position;

			steering.velocity.Normalize();
			steering.velocity *= maxSpeed;

			float angle = Mathf.Atan2(-steering.velocity.x, steering.velocity.z);
			// Quaternion.AngleAxis(angle, Vector3.up);
			transform.rotation = Quaternion.LookRotation(steering.velocity);
			steering.angular = 0;
			return steering;
		}
		
	}
}
