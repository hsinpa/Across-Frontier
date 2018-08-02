using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteerMovement
{
	public class S_Seak : SteerBase {
		int slow_radius;
		float timeToTarget, acceleration, speed, target_radius = 0.5f;

		public S_Seak(float p_acceleration, float p_speed, int p_radius, float p_time_to_target) {
			slow_radius = p_radius;
			acceleration = p_acceleration;
			speed = p_speed;
			timeToTarget = p_time_to_target;
		}

		public override SteeringOutput Execute(AIAgent self, Vector3 target) {
			SteeringOutput steeringOutput = new SteeringOutput();
			steeringOutput.isValid = true;
			Vector3 direction = target - self.transform.position;
			float dist = direction.magnitude;
			
			if(dist < target_radius) {
				steeringOutput.linear = Vector3.zero;
				steeringOutput.isValid = false;
				return steeringOutput;
			};

			float targetSpeed = speed;
			if (dist < slow_radius) {
				targetSpeed = speed * dist / slow_radius;
			}

			Vector3 targetVelocity = direction;
			targetVelocity.Normalize();
			targetVelocity *= targetSpeed;

			steeringOutput.linear = targetVelocity - self._velocity;
			steeringOutput.linear /= timeToTarget;

			if (steeringOutput.linear.magnitude > acceleration) {
				steeringOutput.linear.Normalize();
				steeringOutput.linear *= acceleration;
			}

			steeringOutput.orientation = Quaternion.identity;
			return steeringOutput;
		}

	}

}