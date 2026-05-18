using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    // Khi bấm nút Setting
    public void PauseGame()
    {
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    // Khi bấm Resume
    public void ResumeGame()
    {
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}