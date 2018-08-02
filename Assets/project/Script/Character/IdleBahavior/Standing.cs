using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Standing : BehaviorComponent {
	private float angle;
    private Quaternion originalFrontDir;

	public Standing(BaseUnit p_unit, float p_angle) {
		unit = p_unit;
        originalFrontDir = p_unit.transform.rotation;
		angle = p_angle;
        rigidbody = unit.GetComponent<Rigidbody>();
	}
    
    public override void Update() {
        
    }

	public override Vector3 FindTargetLocation() {
        float angleHalf = angle * 0.5f;
        Vector3 currentRotation = unit.transform.rotation.eulerAngles;


		Vector3 lookLeftAngle = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (originalFrontDir.eulerAngles.y - angleHalf)), 0, 
                                            Mathf.Cos(Mathf.Deg2Rad * (originalFrontDir.eulerAngles.y - angleHalf)) ),
                lookRightAngle = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (originalFrontDir.eulerAngles.y + angleHalf)), 0, 
                                            Mathf.Cos(Mathf.Deg2Rad * (originalFrontDir.eulerAngles.y + angleHalf)) );
        
        Debug.Log((unit.transform.forward - lookLeftAngle).magnitude);
        if ((unit.transform.forward - lookLeftAngle).magnitude < 1) {
            return new Vector3(unit.transform.position.x + lookRightAngle.x, 
                            unit.transform.position.y,
                            unit.transform.position.z + lookRightAngle.z);
        } else {
            return new Vector3(unit.transform.position.x + lookLeftAngle.x, 
                            unit.transform.position.y,
                            unit.transform.position.z + lookLeftAngle.z);
        }

	}
}
