using UnityEngine;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 90;
	
	void Update () {
		// subtracting the position of the player from the mouse position to get the direction vector
		Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; 
		difference.Normalize();     // normalising the vector

		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // finding the angle in degrees
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
	}
}
