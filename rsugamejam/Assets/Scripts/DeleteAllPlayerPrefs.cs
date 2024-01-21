using UnityEngine;

public class DeleteAllPlayerPrefs : MonoBehaviour
{
    public void OnDeleteAllButtonPress()
    {
        // Delete all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs data deleted.");
    }
}
