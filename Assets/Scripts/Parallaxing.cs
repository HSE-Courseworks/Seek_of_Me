using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;         // back- and foregrounds for parallaxing
	private float[] parallaxScales;         // proportion of the camera's movement to move the backgrounds
	public float smoothing = 1f;            // how smooth the parallax is

	private Transform cam;                  // reference to the main cameras transform
	private Vector3 previousCamPos;         // pos of cam in the prev frame

	void Awake () {                         // assigning references between scripts and objects
		cam = Camera.main.transform;        // set up the cam reference
	}

	void Start () {
		previousCamPos = cam.position;      // prev frame had the current frame's pos
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales[i] = backgrounds[i].position.z * -1;  //assigning corresponding parallaxScales
		}
	}
	
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			// parallax is the opposite of the cam move bcs the prev frame multiplied by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
			// set a target x position
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;
			// create a target position - backgrounds cur pos with it's target x pos
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
			// fade between cur pos and the target pos
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

		// at the end of the frame:
		previousCamPos = cam.position;
	}
}
