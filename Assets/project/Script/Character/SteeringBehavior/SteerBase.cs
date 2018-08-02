using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteerMovement
{
	public abstract class SteerBase {
		public abstract SteeringOutput Execute(AIAgent self, Vector3 target);


	}

	public struct SteeringOutput {
		public Vector3 linear;
		public Quaternion orientation;
		public bool isValid;
	}
}
