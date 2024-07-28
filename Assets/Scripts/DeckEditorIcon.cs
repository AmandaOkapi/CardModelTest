using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditorIcon : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    public bool isEnabled;
    [SerializeField] private GameObject childObject;

    [SerializeField] private DeckEditor deckEditor;
    private GameObject firstChild;    
    private void Awake(){
        deckEditor = GameObject.Find("DeckEditorManager").GetComponent<DeckEditor>();
        if (deckEditor == null)
        {
            Debug.LogError("DeckEditorManager not found");
        }

        // Ensure child object is set
        if (childObject == null && transform.childCount > 0)
        {
            childObject = transform.GetChild(0).gameObject;
        }
        if (childObject != null)
        {
            childObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Child object not found for DeckEditorIcon with id " + id);
        }

        isEnabled = false;
    }

    void Start(){

    }

    public void Toggle(){
        IconEnable(!isEnabled);
    }
    public void IconEnable(bool x){
        if(deckEditor==null){
            Debug.LogError("DeckEditor not found");
            return;
        }
        if(x && deckEditor.editedDeck.Count ==  PlayerPrefsUtility.GetIntList(PlayerPrefsUtility.keyCurrentLevelDeck).Count){
            return;
        }
        isEnabled = x;
        ToggleBlocker();        
        

        AddOrRemoveSelfFromNewDeck();
    }
    public void ForcedIconEnable(bool x){
        isEnabled = x;
        ToggleBlocker();
        AddOrRemoveSelfFromNewDeck();   
    }

    private void AddOrRemoveSelfFromNewDeck(){
        if(isEnabled){
            if(deckEditor.editedDeck.IndexOf(id) == -1){
                deckEditor.editedDeck.Add(id);                  
            }     
        }else{
            if(deckEditor.editedDeck.IndexOf(id) != -1){
                deckEditor.editedDeck.Remove(id);
            } 
        }
    }

    private void ToggleBlocker(){
        if (childObject == null && transform.childCount > 0)
        {
            childObject = transform.GetChild(0).gameObject;
        }

        if (childObject != null)
        {
            childObject.SetActive(!isEnabled);
        }
        else
        {
            Debug.LogError("Child object not found for ToggleBlocker with id " + id);
        }
        } 
}
