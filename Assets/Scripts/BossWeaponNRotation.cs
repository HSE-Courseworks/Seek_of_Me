using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponNRotation : MonoBehaviour {

	public int rotationOffset = 90;
	public Transform target;
	public float updateRate = 2f;
	private bool searchingForPlayer = false;

	public float fireRate = 0;
	public int Damage = 20;
	public LayerMask whatToHit;

	public Transform BulletTrailPrefab;
	public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;
	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	public float camShakeAmt = 0.05f;
	public float camShakeLength = 0.1f;
	CameraShake camShake;

	public string weaponShootSound = "BossShot";

	float timeToFire = 0;
	Transform firePoint;

	AudioManager audioManager;

	void Awake()
	{
		firePoint = transform.Find("BossFirePoint");
		if (firePoint == null)
		{
			Debug.LogError("No BossFirePoint? WHAT?!");
		}
	}

	void Start()
	{
		camShake = GameMaster.gm.GetComponent<CameraShake>();
		if (camShake == null)
		{
			Debug.LogError("No camera shake script found on gm object!");
		}

		audioManager = AudioManager.instance;
		if (audioManager == null)
		{
			Debug.LogError("FREAK OUT! No AudioManager found in scene.");
		}

		if (target == null)
		{
			if (!searchingForPlayer)
			{
				searchingForPlayer = true;
				StartCoroutine(SearchForPlayer());
			}
			return;
		}

		StartCoroutine(PointWeapon());
	}

	// Update is called once per frame
	void Update()
	{
		if (fireRate == 0)
		{
			Shoot();
		}
		else
		{
			if (Time.time > timeToFire)
			{
				timeToFire = Time.time + 1 / fireRate;
				Shoot();
			}
		}
	}

	void Shoot()
	{
		// StartCoroutine(PointWeapon());
		Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, targetPosition - firePointPosition, 100, whatToHit);

		Debug.DrawLine(firePointPosition, (targetPosition - firePointPosition) * 100, Color.cyan);

		if (hit.collider != null)
		{
			Debug.DrawLine(firePointPosition, hit.point, Color.red);
			Player player = hit.collider.GetComponent<Player>();
			if (player != null)
			{
				player.DamagePlayer(Damage);
			}
		}

		if (Time.time >= timeToSpawnEffect)
		{
			Vector3 hitPos;
			Vector3 hitNormal;

			if (hit.collider == null)
			{
				hitPos = (targetPosition - firePointPosition) * 50;
				hitNormal = new Vector3(9999, 9999, 9999);
			}
			else
			{
				hitPos = hit.point;
				hitNormal = hit.normal;
			}

			Effect(hitPos, hitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
	}

	void Effect(Vector3 hitPos, Vector3 hitNormal)
	{
		Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer lr = trail.GetComponent<LineRenderer>();

		if (lr != null)
		{
			lr.SetPosition(0, firePoint.position);
			lr.SetPosition(1, hitPos);
		}

		Destroy(trail.gameObject, 0.04f);

		if (hitNormal != new Vector3(9999, 9999, 9999))
		{
			Transform hitParticle = Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
			Destroy(hitParticle.gameObject, 1f);
		}

		Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range(0.6f, 0.9f);
		clone.localScale = new Vector3(size, size, size);
		Destroy(clone.gameObject, 0.02f);

		//Shake the camera
		camShake.Shake(camShakeAmt, camShakeLength);

		//Play shoot sound
		audioManager.PlaySound(weaponShootSound);
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
