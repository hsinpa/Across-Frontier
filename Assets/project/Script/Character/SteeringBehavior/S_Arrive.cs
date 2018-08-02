using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteerMovement
{
	public class S_Arrive : SteerBase {
		float rotate_speed = 5, max_angle = 0.1f;

		public override SteeringOutput Execute(AIAgent self, Vector3 target) {
			SteeringOutput steeringOutput = new SteeringOutput();
			steeringOutput.isValid = true;

			Vector3 direction = new Vector3(target.x, self.transform.position.y, target.z) - self.transform.position;
			float dist = direction.normalized.magnitude;


			Quaternion rotation = Quaternion.LookRotation(direction);
			

			Vector3 selfAngle = self.transform.rotation.eulerAngles,
					directionAngle = rotation.eulerAngles;
			
			steeringOutput.orientation = self.transform.rotation;
			if ((directionAngle - selfAngle).magnitude > max_angle) {
				steeringOutput.orientation = Quaternion.Lerp(self.transform.rotation, rotation, 0.15f);
			}

			return steeringOutput;
		}

	}

}