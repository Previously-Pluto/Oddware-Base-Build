using UnityEngine;
using UnityEngine.UI;

public class UIFloat : MonoBehaviour
{
    public float floatSpeed = 1.0f;  // speed of floating
    public float floatDistance = 10.0f;  // distance of floating
    private float randomFactor;

    private RectTransform rectTransform;
    private Vector2 startPos;
    private float randomX;
    private float randomY;
    
    private void Awake() {
        randomFactor = Random.Range(0f, 1f);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        randomX = Random.Range(-randomFactor, randomFactor);
        randomY = Random.Range(-randomFactor, randomFactor);
    }

    private void Update()
    {
        // calculate the new X and Y positions based on time, speed and random factor
        float newX = Mathf.Sin(Time.time * floatSpeed + randomX) * floatDistance;
        float newY = Mathf.Cos(Time.time * floatSpeed + randomY) * floatDistance;

        // update the position of the RectTransform
        rectTransform.anchoredPosition = new Vector2(startPos.x + newX, startPos.y + newY);
    }
}
