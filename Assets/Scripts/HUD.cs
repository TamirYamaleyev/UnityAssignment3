using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public int score = 0;
    public GameObject ScoreTxt;
    
    TextMeshProUGUI ScoreTxtComponent;
    Image FillerComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreTxtComponent = ScoreTxt.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreTxtComponent.text = "Score: " + score.ToString();
    }
}
