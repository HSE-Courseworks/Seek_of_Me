using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour {

    public Transform leftTarget;
    public Transform rightTarget;

    private Rigidbody2D rb;

    public float speed = 300f;
    public ForceMode2D fMode;

    private int direction = 0;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        direction = Random.Range(0, 2) * 2 - 1;
    }
	
	void Update () {
        if (leftTarget.position.x >= transform.position.x)
            direction = 1;
        if (transform.position.x >= rightTarget.position.x)
            direction = -1;
	}

    void FixedUpdate() {
        rb.AddForce(new Vector2(direction* speed *Time.fixedDeltaTime, 0), fMode);
    }
}
