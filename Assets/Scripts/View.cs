using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class View : MonoBehaviour
{
    // Start is called before the first frame update
    public CardData cardDataView;    
    [SerializeField] private Transform prefab;
    [SerializeField] private Transform[,] gridViewItems;

    [Header ("Scaling")]    
    [SerializeField] private bool maintainAspectRatio =true;
    [SerializeField] private UnityEngine.Vector2 cardSize = new UnityEngine.Vector2(100f,144f);
    [SerializeField] private RectTransform  pane;
    [SerializeField] private RectTransform  refPane;    //a pane that stretches with the screen size acts as a "size goal" for our grid
    [SerializeField] private Transform buttonParent;


    private float localWidth, localHeight;
    private float refWidth, refHeight;

    private UnityEngine.Vector2  scaleFactor;
    void Start()
    {
        localWidth = pane.rect.width;
        localHeight = pane.rect.height;
        refWidth=refPane.rect.width;
        refHeight=refPane.rect.height;
    }

    public void InitializeView(Model model){
        if(model!=null){
            int myRow = model.getRow();
            int myCol = model.getCol();
            gridViewItems = new Transform[myRow, myCol]; 
            
            //calculate and set the perfect dimensions for the grid based on the card pixel size
            pane.sizeDelta = new UnityEngine.Vector2(myCol*cardSize.x, myRow*cardSize.y);
            localWidth = pane.rect.width;
            localHeight = pane.rect.height;

            //populate the grid perfect sized grid with cards
            for(int i=0; i<myRow; i++){
                for(int j=0; j<myCol; j++){
                    gridViewItems[i,j] = InstantiateCard(i,j,model);
                    //Debug.Log("Button " + i + ","+ j+ " is at" + gameObject.position);
                }
            }
            
            //calculate scale factor based on the the refPane (local*scalefactor = ref) and scale down the grid if needed
            scaleFactor.x = refWidth/localWidth;
            scaleFactor.y = refHeight/localHeight;
            
            float scaleFactorAverage = Mathf.Min(scaleFactor.x, scaleFactor.y, 1);
            if(maintainAspectRatio){
                pane.localScale = new UnityEngine.Vector3(scaleFactorAverage, scaleFactorAverage, 1f);
            }else{
                pane.localScale = new UnityEngine.Vector3(scaleFactor.x, scaleFactor.y, 1f);
            }

            //position code for the grid?
            //Ensure top 2 rows are above?
            //perhaps remove the two rows from the calcualtions? 

        }
    }
    private void ClearView(){
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        // Loop through each GameObject and destroy it
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
    }
    public void RemakeView(Model model){
            Debug.Log("updating view");
            ClearView();
            InitializeView(model);
    }

    public void DeactivateAllButtons(){
        Button[] cards = FindObjectsOfType<Button>();

        // Loop through each GameObject and destroy it
        foreach (Button card in cards)
        {
            if(card.tag== "Card"){
                card.interactable = false;
            }
        }
    }
    
    public void ResetCard(int row, int col, Model model){
        if(gridViewItems[row, col]!=null){
            Destroy(gridViewItems[row,col].gameObject); 
        }
        gridViewItems[row, col] = InstantiateCard(row, col, model);
    }

    public Transform InstantiateCard(int i, int j, Model model){
        if(model.getCardAtIndex(i,j)==null){
            return null;
        }
        float xOffset = localWidth/(model.getCol());
        float yOffset = localHeight/(model.getRow());
        gridViewItems[i,j] = Instantiate(prefab, buttonParent);
        gridViewItems[i,j].localPosition = new UnityEngine.Vector3(xOffset*(j), -yOffset*(i+1),-5);
        ((Card)model.getCardAtIndex(i,j)).ResetCellsToFall();
        gridViewItems[i,j].GetComponent<CardMono>().setCardBase(((Card)model.getCardAtIndex(i,j)));
        
        Transform gameObject=gridViewItems[i,j];     
        gameObject.GetComponent<Button>().onClick.AddListener(() =>{
            gameObject.GetComponent<CardMono>().flipCard();
                Debug.Log("Button " + i + ","+ j+ "clicked!");
            });
        return gridViewItems[i,j];
    }
    public void RemoveCard(int row, int col){
        Destroy(gridViewItems[row,col].gameObject); 
        gridViewItems[row,col]=null;
    }

    public void RemoveCard(int row, int col, Model model) {
        RemoveCard(row,col);
        UpdateCollumn(col, model);
    }
    public void UpdateCollumn(int col, Model model){
        //this is shit
        //handles dropping translation and generating new card
        //must be called after removing a card object
        //card objects check their linked base card to determine how many cells to fall
        //then their position is updated to reflect the models position of the card
        for(int i=gridViewItems.GetLength(0)-1; i>=0; i--){          
            if(gridViewItems[i,col]!=null){
                int myCellsToFall=gridViewItems[i,col].GetComponent<CardMono>().getCardBase().GetCellsToFall();
                if(myCellsToFall>0){
                    //get target vector
                    UnityEngine.Vector3 targetPos= new UnityEngine.Vector3(
                        gridViewItems[i,col].localPosition.x,
                        gridViewItems[i,col].localPosition.y-(myCellsToFall*(localHeight/(model.getRow()))),                            
                        gridViewItems[i,col].localPosition.z
                        ); 
                    Debug.Log(gridViewItems[i,col].GetComponent<CardMono>().getCardBase().GetCellsToFall() + "Sending "+i+","+col+" to "+targetPos);
                    //play the falling sequence
                    gridViewItems[i,col].GetComponent<CardMono>().FallToPos(targetPos);                    
                    gridViewItems[i,col].GetComponent<CardMono>().getCardBase().ResetCellsToFall();
                    //reset local object grid
                    gridViewItems[i+myCellsToFall,col] = gridViewItems[i,col];
                    gridViewItems[i,col]=null;
                    }
                }      
            } 

        for(int x=0; x<gridViewItems.GetLength(0); x++){          
            if(gridViewItems[x,col]==null){
                Debug.Log("Resetting " + x + "," + col);
                ResetCard(x,col, model);
            }
        }      

    }

}
