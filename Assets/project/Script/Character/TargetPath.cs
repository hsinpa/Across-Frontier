using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TargetPath {

	public Vector3[] paths;

	public TargetPath(Vector3[] p_paths) {
		paths = p_paths;
	}

	public TargetPath(Vector3 p_path) {
		paths = new Vector3[1] { p_path };
	}
}
