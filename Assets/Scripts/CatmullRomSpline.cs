using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CatmullRomSpline : MonoBehaviour {

	public float sphereRadius = 0.3f;
	public bool isLooping = true;
	public Transform[] controlPoints;


	private void Start () {

	}

	// Update is called once per frame
	private void Update () {

	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.white;
		for (var index = 0; index < controlPoints.Length; index++) {
			Gizmos.DrawWireSphere (controlPoints [index].position, sphereRadius);
		}

		for (var i = 0; i < controlPoints.Length; i++) {
			if ((i == 0 || i == controlPoints.Length - 2 || i == controlPoints.Length - 1) && !isLooping) {
				continue;
			}
			DisplayCatmullRollSpline (i);
		}
	}

	private Vector3 GetCatmullRomPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		float half = 0.5f;
		var point1 = half * (2f * p1);
		var point2 = half * (p2 - p0);
		var point3 = half * (2f * p0 - 5f * p1 + 4f * p2 - p3);
		var point4 = half * (-p0 + 3f * p1 - 3f * p2 + p3);

		var position = point1 + (point2 * t) + (point3 * t * t) + (point4 * t * t * t);

		return position;
	}

	private void DisplayCatmullRollSpline(int index) {
		var p0 = controlPoints[ClampListPos(index - 1)].position;
		var p1 = controlPoints[index].position;
		var p2 = controlPoints[ClampListPos(index + 1)].position;
		var p3 = controlPoints[ClampListPos(index + 2)].position;


		var lastPosition = Vector3.zero;

		for (float t = 0; t < 1; t += 0.1f) {
			var newPosition = GetCatmullRomPoint(t, p0, p1, p2, p3);

			if (t == 0) {
				lastPosition = newPosition;
				continue;
			}

			Gizmos.DrawLine(lastPosition, newPosition);
			lastPosition = newPosition;
		}

		Gizmos.DrawLine(lastPosition, p2);
	}

	private int ClampListPos(int position) {
		if (position < 0) {
			position = controlPoints.Length - 1;
		}
		if (position > controlPoints.Length) {
			position = 1;
		} 
		else if (position > controlPoints.Length - 1) {
			position = 0;
		}
		return position;
	}
}
