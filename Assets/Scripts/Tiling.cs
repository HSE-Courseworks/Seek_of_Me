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

            // creation criteria for buddies
            if (camera.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (camera.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}

    // creation function for buddy
    void MakeNewBuddy (int rightOrLeft) {
        // calculate position for new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // instantiate newBuddy and storing
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tilable - reverse x_position our element
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x*-1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
