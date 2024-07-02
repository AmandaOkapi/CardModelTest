using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject deckPreview;

    [SerializeField] private GameObject cardPreviewPrefab;

    [SerializeField] private CardData cardData;
    [SerializeField] private TextMeshProUGUI scoreRequirmentsText;
    [SerializeField] private GameObject wallsPreviewBlocker, match3PreviewBlocker, glassPreviewBlocker, eliminationPreviewBlocker;
    [SerializeField] private TextMeshProUGUI sizePreviewText;
    [SerializeField] private Button button;
    [SerializeField] private MySceneManager mySceneManager;


    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(false);
    }


    public void DisplayStartScreen(int level){
        screen.SetActive(true);
        GenerateDeckPreview(level);
        GenerateScoreRequirmentsText(level);
        ShowModifiers(level);
        SetSizeText(level);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>{
            mySceneManager.GenerateScene(level);
        });
    }

    //deck preview

    private void GenerateDeckPreview(int level){
        GenerateDeckPreview(LevelDataBase.levels[level].model);
    }

    private void GenerateDeckPreview(Model model){
        Card[] deck = model.getPossibleCards();
        RemoveAllChildren();
        for(int i=0; i< deck.Length; i++){
            GameObject newGridItem = Instantiate(cardPreviewPrefab, deckPreview.transform);
            newGridItem.GetComponent<Image>().sprite = (cardData.cardImages[deck[i].getId()]);    
        }
    }
    public void RemoveAllChildren()
    {
        foreach (Transform child in deckPreview.transform)
        {
            Destroy(child.gameObject);
        }
    }
    //score reuirments preview
    private void GenerateScoreRequirmentsText(int level){
        GenerateScoreRequirmentsText(LevelDataBase.levels[level].score);
    }

    private void GenerateScoreRequirmentsText(Score score){
        ScoreRequirements[] scoreRequirements= score.GetScoreRequirments();
        scoreRequirmentsText.text = ""; // Clear existing text
        string newText = "";

        string gameTimeText = (score.GetGameTime() > 0) 
            ? $"<size=150%><b><align=\"center\">Game Time: {FormatMinutes(score.GetGameTime())} minutes </align></b></size> \n" 
            : "";

        newText += gameTimeText;
        newText += "<align=\"left\">";

        foreach (ScoreRequirements sr in scoreRequirements)
        {
            newText += sr.ToString() + "\n";
        }

        newText += "</align>";
        scoreRequirmentsText.text = newText; // Set the new text
    }

    private string FormatMinutes(float totalSeconds)
    {
        float minutes = totalSeconds / 60;
        return minutes.ToString("F2");
    }

    //modifiers preview
    private void ShowModifiers(int level){
        ShowModifiers(LevelDataBase.levels[level].model);
    }

    private void ShowModifiers(Model model){
        //reset
        glassPreviewBlocker.SetActive(true);
        match3PreviewBlocker.SetActive(true);
        wallsPreviewBlocker.SetActive(true);
        eliminationPreviewBlocker.SetActive(true);
        if(model.isHasGlass()){
            glassPreviewBlocker.SetActive(false);
        }
        if(model.isMatchThreeMode()){
            match3PreviewBlocker.SetActive(false);
        }
        if(model is WallModel){
            wallsPreviewBlocker.SetActive(false);
        }

        if((model is EliminationModel) ||(model is WallModelElimination)){
            eliminationPreviewBlocker.SetActive(false);
        }
    }

    //show size
    private void SetSizeText(int level){
        SetSizeText(LevelDataBase.levels[level].model);
    }
    private void SetSizeText(Model model){
        sizePreviewText.text = (model.getRow() -model.getRowsToHide()).ToString() +"x"+ model.getCol();
    }
    
}
