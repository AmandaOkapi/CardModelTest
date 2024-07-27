using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditor : MonoBehaviour
{
    [SerializeField] private Transform deckEditorGrid;
    [SerializeField] private GameObject deckEditorPrefab;    
    [SerializeField] private GameObject saveButton;    

    [SerializeField] private CardData cardData;

    [SerializeField] private GameObject canvas;    
    public List<int> editedDeck = new List<int>();

    private bool firstTimeOpened=true;
    
    void Awake(){
        //create all the icons on the scroll
        for(int i=0; i<cardData.cardImages.Length; i++){
            GameObject newIcon = Instantiate(deckEditorPrefab, deckEditorGrid);
            newIcon.GetComponent<Image>().sprite  = cardData.cardImages[i];
            int index = i; // capture the current index
            newIcon.GetComponent<DeckEditorIcon>().id=index;
            newIcon.GetComponent<Button>().onClick.AddListener(() =>{
                Debug.Log("icon clicked!");
                newIcon.GetComponent<DeckEditorIcon>().Toggle();
            });
        }
    }
    void Start(){

        //hide it
        canvas.SetActive(false);
    }

    public void EnableDeckEditorWindow(){
        canvas.SetActive(true);
        List<int> currentList = PlayerPrefsUtility.GetIntList(PlayerPrefsUtility.keyCurrentLevelDeck);
        editedDeck = new List<int>();
        Debug.Log("enabling wiith this list");
        GeneralToolbox.PrintListToConsole(currentList);        
        //turn on the correct cards
        for (int i = 0; i < deckEditorGrid.transform.childCount; i++){
            // Get the child at index i
            Debug.Log("getting child: "+i);
            DeckEditorIcon childDeckEditorIcon = deckEditorGrid.transform.GetChild(i).GetComponent<DeckEditorIcon>();
            if (currentList.Contains(childDeckEditorIcon.id)){
                childDeckEditorIcon.ForcedIconEnable(true);
            } else {
                childDeckEditorIcon.ForcedIconEnable(false);
            }
        }
        //since there is some weird bug where things arent initialized properly the first time when you open this shit...
        //i present the nuclear option (time to move on)
        // if(firstTimeOpened){
        //     firstTimeOpened=false;
        //     SaveAndExitView();
        //     EnableDeckEditorWindow();
        // }
    }

    private void Update(){
        saveButton.GetComponent<Button>().interactable = (editedDeck.Count == PlayerPrefsUtility.GetIntList(PlayerPrefsUtility.keyCurrentLevelDeck).Count);
    }

    public void ExitView(){
        canvas.SetActive(false);
    }

    public void SaveAndExitView(){
        //update default deck
        List<int> listToUpdate = PlayerPrefsUtility.GetIntList(PlayerPrefsUtility.keyPrefferedDeck);
        List<int> secondList = new List<int>();
        for(int i=0; i<listToUpdate.Count; i++){
            if(editedDeck.IndexOf(listToUpdate[i])==-1){
                secondList.Add(listToUpdate[i]);
            } 
        }
        List<int> finalList = new List<int>();
        editedDeck.Sort();
        finalList.AddRange(editedDeck);
        finalList.AddRange(secondList); 
        Debug.Log("Final list to be saved");
        GeneralToolbox.PrintListToConsole(finalList);
        PlayerPrefsUtility.SaveIntCollection(PlayerPrefsUtility.keyPrefferedDeck,finalList);
        ExitView();
    }
}
