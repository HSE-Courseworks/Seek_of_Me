using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    [SerializeField]
    string mouseHoverSoundName = "ButtonHover";

    [SerializeField]
    string buttonPressSoundName = "ButtonPress";

    AudioManager audioManager;

    private void Start() {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("FREAK OUT! No AudioManager found in the scene.");
        }
    }

    public void Quit() {
        audioManager.PlaySound(buttonPressSoundName);

        Debug.Log("APPLICATION QUIT!");
        Application.Quit();
    }

    public void Retry() {
        audioManager.PlaySound(buttonPressSoundName);

        Destroy(GameMaster.gm.gameObject);
        SceneManager.LoadScene(1);
    }

    public void OnMouseOver() {
        audioManager.PlaySound(mouseHoverSoundName);
    }

}
