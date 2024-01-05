using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackgroundUI : MonoBehaviour
{
    public float scrollSpeed = 0.02f;

    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rawImage.uvRect = new Rect(offset, 0, 1, 1);
    }
}
