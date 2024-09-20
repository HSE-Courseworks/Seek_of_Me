using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BossAI))]
public class Boss : MonoBehaviour {

    [System.Serializable]
    public class BossStats {
        public int maxHealth = 3000;

        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 50;

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public BossStats stats = new BossStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 100;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start () {
        stats.Init();
        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if (deathParticles == null) {
            Debug.LogError("No death particles referenced on enemy");
        }
    }

    void OnUpgradeMenuToggle(bool active) {
        GetComponent<BossAI>().enabled = !active;
        BossWeaponNRotation _bossWeapon = GetComponentInChildren<BossWeaponNRotation>();
        if (_bossWeapon != null)
        {
            _bossWeapon.enabled = !active;
        }
    }

    public void DamageBoss(int damage) {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0) {
            GameMaster.KillBoss(this);
        }

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null) {
            _player.DamagePlayer(PlayerStats.instance.maxHealth);
        }
    }

    void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
