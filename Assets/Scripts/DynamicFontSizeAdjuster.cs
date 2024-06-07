using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DynamicFontSizeAdjuster : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public RectTransform layoutGroupRect;
    public List<TextMeshProUGUI> textObjects;
    public int maxFontSize = 36;
    public int minFontSize = 10;

    //public Rec
    void Start()
    {
        int childCount = transform.childCount;
        // Populate the array with the children
        for (int i = 0; i < childCount; i++)
        {
            textObjects.Add(transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }

    }

    public void AdjustFontSize(float width)
    {
        layoutGroupRect.sizeDelta = new Vector2(width, layoutGroupRect.sizeDelta.y);
        // Get the available width of the layout group
        float availableWidth = layoutGroupRect.rect.width;

        // Calculate the total width of text objects at max font size
        float totalTextWidth = 0f;
        foreach (var textObject in textObjects)
        {
            textObject.fontSize = maxFontSize; // Temporarily set to max size to measure width
            totalTextWidth += LayoutUtility.GetPreferredWidth(textObject.rectTransform);
        }
        // Include spacing and padding in the total width
        float spacing = layoutGroup.spacing * (textObjects.Count - 1);
        float padding = layoutGroup.padding.left + layoutGroup.padding.right;
        totalTextWidth += spacing + padding;



        // Calculate the scale factor to fit all text within the available width
        float scaleFactor = availableWidth / totalTextWidth;

        // Calculate the new font size based on the scale factor
        int newFontSize = Mathf.Clamp(Mathf.FloorToInt(maxFontSize * scaleFactor), minFontSize, maxFontSize);



        // Apply the new font size to all text objects
        foreach (var textObject in textObjects)
        {
            textObject.fontSize = newFontSize;
        }
    }


}