using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities {

	public static Vector3 GetLocalPosition(Matrix4x4 transform) => transform.GetColumn(3);
	public static Quaternion GetLocalRotation(Matrix4x4 transform) => Quaternion.LookRotation(transform.GetColumn(2), transform.GetColumn(1));
	public static Vector3 GetLocalScale(Matrix4x4 transform) => new Vector3(transform.GetColumn(0).magnitude, transform.GetColumn(1).magnitude, transform.GetColumn(2).magnitude);

	public static (Vector3, Quaternion, Vector3) GetLocalTransform(Matrix4x4 transform) {
		Vector4 col1 = transform.GetColumn(1);
		Vector4 col2 = transform.GetColumn(2);
		Vector3 position = transform.GetColumn(3);
		Quaternion rotation = Quaternion.LookRotation(col2, col1);
		Vector3 scale = new Vector3(transform.GetColumn(0).magnitude, col1.magnitude, col2.magnitude);
		return (position, rotation, scale);
	}

	public static RaycastHit[] ConeCastAll(Vector3 origin, Vector3 direction, float coneAngle, float maxDistance, int layerMask) {
		var radius = Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance;

		maxDistance += radius;
		
		var potentialHits = Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask);
		var actualHits = new List<RaycastHit>(potentialHits.Length);
		
		actualHits.AddRange(
			from hit in potentialHits 
			let hitDir = hit.point - origin 
			let hitAngle = Vector3.Angle(direction, hitDir) 
			where hitAngle <= coneAngle && hitDir.magnitude <= maxDistance 
			select hit);

		return actualHits.ToArray();
	}
	
	public static bool ConeCast(Vector3 origin, Vector3 direction, float coneAngle, out RaycastHit hit, float maxDistance, int layerMask = int.MaxValue) {
		var hits = ConeCastAll(origin, direction, coneAngle, maxDistance, layerMask);
		hit = new RaycastHit();
		if (hits.Length == 0) return false;
		
		var radius = Mathf.Tan(coneAngle * Mathf.Deg2Rad) * maxDistance;
		maxDistance += radius;
		
		hit.distance = maxDistance + 1.0f;

		foreach (var potential in hits) {
			if (potential.distance < hit.distance) hit = potential;
		}

		return true;
	}
}