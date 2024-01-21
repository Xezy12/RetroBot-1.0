using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI levelText;
    public Animator transition;

    void Start()
    {
        levelText.text = level.ToString();
    }

    public void OpenScene()
    {
        string currentLevelName = "Level " + level.ToString();

        if (IsPreviousLevelCleared(level+3))
        {
            // Set the current level as cleared
            PlayerPrefs.SetInt(currentLevelName + "_Cleared", 1);
            PlayerPrefs.Save(); // Save PlayerPrefs to make changes permanent

            int targetSceneIndex = level+3;
            StartCoroutine(LoadLevel(targetSceneIndex));
        }
        else
        {
            Debug.Log("Complete the previous level before accessing " + currentLevelName);
            // You can add additional feedback to the player, like a message on the screen.
        }
    }

    bool IsPreviousLevelCleared(int currentLevel)
    {
        if (currentLevel > 1)
        {
            string previousLevelName = "Level " + (currentLevel - 1).ToString();
            return PlayerPrefs.HasKey(previousLevelName + "_Cleared");
        }
        else
        {
            return true;
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        float transitionTime = 2f;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        // Check if the scene index is valid
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogError("Invalid scene index: " + levelIndex);
        }
    }
}
