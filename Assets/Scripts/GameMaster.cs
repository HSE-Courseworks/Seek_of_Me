using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private int _remainingLives;
    public int RemainingLives {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public int Money;

    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            DontDestroyOnLoad(this);
            _remainingLives = maxLives;
            Money = startingMoney;
        }
        else {
            if (gm != this) {
                this._remainingLives = gm._remainingLives;
                this.Money = gm.Money;
                Destroy(gm.gameObject);
                gm = GameObject.FindGameObjectsWithTag("GM")[1].GetComponent<GameMaster>();
                DontDestroyOnLoad(this);
            }
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;
    public Transform spawnPrefab;
    public string respawnCountdownSoundName = "RespawnCountdown";
    public string spawnSoundName = "Spawn";
    public string gameOverSoundName = "GameOver";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    //cache
    private AudioManager audioManager;

    void Start() {
        if (cameraShake == null) {
            Debug.LogError("No camera shake referenced in GameMaster");
        }

        //caching
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("FREAK OUT! No AudioManager found in the scene.");
        }
    }

    public void EndGame() {
        audioManager.PlaySound(gameOverSoundName);

        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public IEnumerator _RespawnPlayer() {
        audioManager.PlaySound(respawnCountdownSoundName);
        yield return new WaitForSeconds(spawnDelay); // Ienumerator

        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }

	public void KillPlayer(Player player) {
        Destroy(player.gameObject);
        --_remainingLives;
        if (_remainingLives <= 0) {
            gm.EndGame();
        }
        else {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy (Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy) {
        // Play sound
        audioManager.PlaySound(_enemy.deathSoundName);

        // Gain some money
        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");

        // Add particles
        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 5f);

        // Go camera shake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }

    public static void KillBoss(Boss boss)
    {
        gm._KillBoss(boss);
    }

    public void _KillBoss(Boss _boss)
    {
        // Play sound
        audioManager.PlaySound(_boss.deathSoundName);

        // Gain some money
        Money += _boss.moneyDrop;
        audioManager.PlaySound("Money");

        // Add particles
        Transform _clone = Instantiate(_boss.deathParticles, _boss.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 5f);

        // Go camera shake
        cameraShake.Shake(_boss.shakeAmt, _boss.shakeLength);
        Destroy(_boss.gameObject);
    }
}
