using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{

    public Rigidbody ball;
    public Transform target;
    public float h = 25;
    public float gravity = -18;

	public bool debugPath;

    // Use this for initialization
    void Start()
    {
        ball.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
		if (debugPath)
			DrawPath();
    }

    void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        ball.useGravity = true;
        ball.velocity = CalculateLaunchVelocity().initialVelocity;
    }

	void DrawPath() {
		LaunchData data = CalculateLaunchVelocity();
		Vector3 previousDrawPoint = ball.position;
		int resolution = 30;
		for (int i = 1; i <= resolution; i++) {
			float simulationTime = i / (float)resolution * data.timeToTarget;
			Vector3 displacement = data.initialVelocity * simulationTime + Vector3.up* gravity * simulationTime * simulationTime / 2f;
			Vector2 drawPoint = ball.position + displacement;
			Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
			previousDrawPoint = drawPoint;
		}
	}

    LaunchData CalculateLaunchVelocity()
    {
        float displacementY = target.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);

        float time = (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

		public LaunchData(Vector3 p_velocity, float p_time) {
			this.initialVelocity = p_velocity;
			this.timeToTarget = p_time;
		}

    }

}
