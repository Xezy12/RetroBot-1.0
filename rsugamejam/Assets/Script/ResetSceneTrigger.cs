using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSceneTrigger : MonoBehaviour
{
    public GameObject popupAfterTheEnd;
    public Button playAgainButton;
    public Button quitButton;
    public Animator popupAfterTheEndAnimator;

    void Start()
    {
        popupAfterTheEnd.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject != null && other.CompareTag("Player"))
        {
            // Trigger the "ShowPopup" animation
            popupAfterTheEnd.SetActive(true);
            popupAfterTheEndAnimator.SetTrigger("ShowPopup");

            // Assign functions to buttons
            playAgainButton.onClick.AddListener(PlayAgain);
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    // Function to restart the scene
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Function to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
