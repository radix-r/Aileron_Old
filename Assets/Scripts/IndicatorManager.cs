using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : MonoBehaviour
{

    public Transform playerReference; // Playership Transform
    public Image indicatorReference; // Indicator image
    public List<Transform> indicatorList; // List of instantiated indicators

    private Vector3 indicatorVector; // Position of indicator on screen
    private Text textReference; // Distance marker (text)
    private int indicatorRange; // Distance marker (int)
    private int indicatorIndex;
    private bool indicatorValid; // Indicators are enabled if there are more than 0
    

    void Awake () {
        textReference = indicatorReference.GetComponentInChildren<Text>();
    }
    void Start()
    {
        indicatorIndex = 0; // Start index at 0
        indicatorRange = 5;

        indicatorValid = indicatorList.Count > 0; // Insurance
    }

     void Update() {
        // If there is a displayable amount of indicators (>0) Keep em' Updated
        if(indicatorValid && playerReference!=null) {
            // If the distance between the player and the indicator is less than a specified range, update the next indicator instead
            if (Vector3.Distance(playerReference.position, indicatorList[indicatorIndex].position) < indicatorRange) {
                indicatorIndex = (indicatorIndex + 1) % indicatorList.Count;
            }
            UpdateIndicators(indicatorIndex);
        }
    }
    
    public void UpdateIndicators(int index) {
        if (indicatorValid) {
            // Enables gameObject of the indicator if the dot product between the indicator's Z axis and the player's position is greater than 0
            indicatorReference.gameObject.SetActive(Vector3.Dot(Vector3.forward, playerReference.InverseTransformDirection(indicatorList[index].position).normalized) > 0);
        }

        if (indicatorList[index].gameObject.activeInHierarchy) {
            // Updates the text portion of the indicator  e.g (50.00m)
            textReference.text = Vector3.Distance(playerReference.position, indicatorList[index].position) + "m";

            indicatorVector = indicatorReference.rectTransform.anchorMin;
            indicatorVector.x = Camera.main.WorldToViewportPoint(indicatorList[index].position).x;
            indicatorReference.rectTransform.anchorMin = indicatorVector;
            indicatorReference.rectTransform.anchorMax = indicatorVector;
        }
    }
}
