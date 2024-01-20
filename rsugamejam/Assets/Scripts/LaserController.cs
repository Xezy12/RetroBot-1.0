using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private GameObject[] objectsWithTag;
    private GameObject[] objectsWithTag2;
    void Start(){
        objectsWithTag = GameObject.FindGameObjectsWithTag("Laser");
        objectsWithTag2 = GameObject.FindGameObjectsWithTag("Laser2");
        foreach (GameObject obj in objectsWithTag2)
            {
                // Toggle the GameObject's activation state
                obj.SetActive(!obj.activeSelf);
            }
    }

    public void switching(){
        foreach (GameObject obj in objectsWithTag)
            {
                // Toggle the GameObject's activation state
                obj.SetActive(!obj.activeSelf);
            }
        foreach (GameObject obj in objectsWithTag2)
            {
                // Toggle the GameObject's activation state
                obj.SetActive(!obj.activeSelf);
            }
            
    }
}
