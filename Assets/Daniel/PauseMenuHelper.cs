using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHelper : MonoBehaviour
{
    public void Unpause() {
        PauseManager.instance.TogglePause();
    }

    public void QuitToDesktop() {
        Application.Quit(0);
    }

    public void QuitToMainMenu() {
        PlayerMap.Instance.BackToMenu();
    }
}
