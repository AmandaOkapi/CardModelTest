using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public static class PlayerPrefsUtility
{
    public static string keyPrefferedDeck ="PrefferedDeck";
    public static string keyCurrentLevelDeck ="CurrentLevelDeck";

    public static void SaveIntCollection(string key, int[] intArr)
    {
        // Convert the list to a comma-separated string
        string value = string.Join(",", intArr);
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    public static void SaveIntCollection(string key, List<int> intList)
    {
        // Convert the list to a comma-separated string
        string value = string.Join(",", intList);
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static List<int> GetIntList(string key)
    {
        // Get the string from PlayerPrefs
        string value = PlayerPrefs.GetString(key, string.Empty);

        // Convert the comma-separated string back to a list of integers
        if (!string.IsNullOrEmpty(value))
        {
            return value.Split(',').Select(int.Parse).ToList();
        }

        // Return an empty list if the key does not exist
        return new List<int>();
    }
    public static int[] GetIntArr(string key)
    {
        // Get the string from PlayerPrefs
        string value = PlayerPrefs.GetString(key, string.Empty);

        // Convert the comma-separated string back to an array of integers
        if (!string.IsNullOrEmpty(value))
        {
            return value.Split(',').Select(int.Parse).ToArray();
        }

        // Return an empty array if the key does not exist
        return new int[0];
    }
}
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelInformationScreen;
    [SerializeField] private GameObject deckPreview;

    [SerializeField] private TextMeshProUGUI deckPreviewText;

    [SerializeField] private GameObject deckPreviewEditButton;
    [SerializeField] private GameObject cardPreviewPrefab;

    [SerializeField] private CardData cardData;
    [SerializeField] private TextMeshProUGUI scoreRequirmentsText;
    [SerializeField] private Button button;
    [SerializeField] private MySceneManager mySceneManager;

    public static int selectedLevel;

    [SerializeField] private bool resetDefaultPrefferedDeck;
    private int myPrefferedDeckSize = 30;
    // Start is called before the first frame update
    void Start()
    {
        if((!(PlayerPrefs.HasKey(PlayerPrefsUtility.keyPrefferedDeck))) || resetDefaultPrefferedDeck){
            //set up default starting deck
            int[] defaultDeck = new int[myPrefferedDeckSize];
            for(int i=0; i<defaultDeck.Length; i++){
                defaultDeck[i]=i;
            }
            PlayerPrefsUtility.SaveIntCollection(PlayerPrefsUtility.keyPrefferedDeck, defaultDeck);
        }        
        levelInformationScreen.SetActive(false);

    }
    public void DisplayLevelStartScreen(){
        DisplayLevelStartScreen(selectedLevel);
    }

    public void DisplayLevelStartScreen(int level){
        selectedLevel = level;
        levelInformationScreen.SetActive(true);
        GenerateDeckPreview(level);
        GenerateScoreRequirmentsText(level);
        //ShowModifiers(level);
        //SetSizeText(level);
        //set up the button to send you to the right level
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
        //set Deck [x] text
        deckPreviewText.text = "Deck ["+deck.Length+"]";
        RemoveAllChildren();
        if(model.isHasCustomDeck()){
            //spawn mystery deck preview
            for(int i=0; i< deck.Length; i++){
                GameObject newGridItem = Instantiate(cardPreviewPrefab, deckPreview.transform);
                newGridItem.GetComponent<Image>().sprite = (cardData.mysteryCardIcon);    
            }
            deckPreviewEditButton.SetActive(false);
            return;
        }
        //spawn deck from prefs
        List<int> newDeckFromPrefs = new List<int>();
        int[] myPrefferedDeck = PlayerPrefsUtility.GetIntArr(PlayerPrefsUtility.keyPrefferedDeck);
        //set the display deck using the players fav deck
        for(int i=0; i< deck.Length; i++){
            GameObject newGridItem = Instantiate(cardPreviewPrefab, deckPreview.transform);
            newGridItem.GetComponent<Image>().sprite = (cardData.cardImages[myPrefferedDeck[i]]);    
            newDeckFromPrefs.Add(myPrefferedDeck[i]);
        }
        //Update the model with the edited deck
        model.SetPossibleCards(newDeckFromPrefs, false);
        PlayerPrefsUtility.SaveIntCollection(PlayerPrefsUtility.keyCurrentLevelDeck, newDeckFromPrefs);
        deckPreviewEditButton.SetActive(true);
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

        // string gameTimeText = (score.GetGameTime() > 0) 
        //     ? $"<size=150%><b><align=\"center\">Game Time: {FormatMinutes(score.GetGameTime())} minutes </align></b></size> \n" 
        //     : "";

        // newText += gameTimeText;
        newText += "<size=150%><align=\"left\">";

        foreach (ScoreRequirements sr in scoreRequirements)
        {
            newText += "    -"+sr.ToString()+"\n";
        }

        newText += "</align></size>";
        string starUnicode = "\u2605";
        string gameTimeText = (score.GetGameTime() > 0) 
            ? "\n" +$"<size=200%><b><align=\"center\">{starUnicode} Finish before time runs out {starUnicode}</align></b></size>".Replace(" ", "\u00A0")
            : "";

        newText+=gameTimeText;
        scoreRequirmentsText.text = newText; // Set the new text
        if(score.GetGameTime() > 0){
            scoreRequirmentsText.alignment = TextAlignmentOptions.Bottom;
        }else{
            scoreRequirmentsText.alignment = TextAlignmentOptions.Top;
        }
    }

    private string FormatMinutes(float totalSeconds)
    {
        float minutes = totalSeconds / 60;
        return minutes.ToString("F2");
    }

    //modifiers preview
    // private void ShowModifiers(int level){
    //     ShowModifiers(LevelDataBase.levels[level].model);
    // }

    // private void ShowModifiers(Model model){
    //     //reset
    //     glassPreviewBlocker.SetActive(true);
    //     match3PreviewBlocker.SetActive(true);
    //     wallsPreviewBlocker.SetActive(true);
    //     eliminationPreviewBlocker.SetActive(true);
    //     if(model.isHasGlass()){
    //         glassPreviewBlocker.SetActive(false);
    //     }
    //     if(model.isMatchThreeMode()){
    //         match3PreviewBlocker.SetActive(false);
    //     }
    //     if(model is WallModel){
    //         wallsPreviewBlocker.SetActive(false);
    //     }

    //     if((model is EliminationModel) ||(model is WallModelElimination)){
    //         eliminationPreviewBlocker.SetActive(false);
    //     }
    // }

    // //show size
    // private void SetSizeText(int level){
    //     SetSizeText(LevelDataBase.levels[level].model);
    // }
    // private void SetSizeText(Model model){
    //     sizePreviewText.text = (model.getRow() -model.getRowsToHide()).ToString() +"x"+ model.getCol();
    // }
    
}
