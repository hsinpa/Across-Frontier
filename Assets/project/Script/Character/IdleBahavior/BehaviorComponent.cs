using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteerMovement;

public abstract class BehaviorComponent {
	public abstract Vector3 FindTargetLocation();
	public abstract void Update();

	public void Enter() {

	}
	
	public void Leave() {

	}

	protected BaseUnit unit;
	protected Rigidbody rigidbody;
}
