using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;     // the offset so that avoid errors
    // for check buddy
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;   // used if the object is not tilable

    private float spriteWidth = 0f;     // width of our element

    private Camera camera;
    private Transform myTransform;

    private void Awake() {
        camera = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
        // need buddies? No - do nothing
		if (hasALeftBuddy == false || hasARightBuddy == false) {
            // calculate half of width
            float cameraHorizontalExtend = camera.orthographicSize * Screen.width / Screen.height;

            // calculate x_position where camera can see edge of sprite (element)
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + cameraHorizontalExtend;
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - cameraHorizontalExtend;
        }
	}
}
