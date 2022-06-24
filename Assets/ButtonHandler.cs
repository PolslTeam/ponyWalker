using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {
    public void PlayLevel1() {
        SceneManager.LoadScene(1); // index from menu file->build_settings
    }
    public void PlayLevel2() {
        SceneManager.LoadScene(2); // index from menu file->build_settings
    }
    public void PlayLevel3() {
        SceneManager.LoadScene(3); // index from menu file->build_settings
    }

    public void QuitGame() {
        Application.Quit(); // doesnt work in editor!
    }
}
