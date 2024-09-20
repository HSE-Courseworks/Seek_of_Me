using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponRotation : MonoBehaviour {

	public int rotationOffset = 90;
	// What to point the weapon at
	public Transform target;
	// How many times each second we will update the target
	public float updateRate = 2f;
	private bool searchingForPlayer = false;

	void Start()
	{
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}

		PointWeapon();
	}

	// Update is called once per frame
	void Update()
	{
		StartCoroutine(PointWeapon());
	}

	IEnumerator SearchForPlayer()
	{
		GameObject sResult = GameObject.FindGameObjectWithTag("Player");
		if (sResult == null)
		{
			yield return new WaitForSeconds(0.5f);
			StartCoroutine(SearchForPlayer());
		}
		else
		{
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine(PointWeapon());
			yield break;
		}
	}

	IEnumerator PointWeapon()
    {
		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			yield break;
		}

		Vector3 difference = target.position - transform.position;
		difference.Normalize();

		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);

		yield return new WaitForSeconds(1f / updateRate);
		StartCoroutine(PointWeapon());
	}

}
