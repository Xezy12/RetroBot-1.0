using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class clickcutscene : MonoBehaviour
{
    public GameObject[] background;
    int index;
    void Start()
    {
        index = 0;
        background[0].gameObject.SetActive(true);
    }
        

    void Update()
    {
        if(index > background.Length-1){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    public void Next()
     {
        index += 1;
         for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
         }
            Debug.Log(index);
     }
}
