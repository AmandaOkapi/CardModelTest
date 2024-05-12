using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class View : MonoBehaviour
{
    // Start is called before the first frame update
    public CardData cardDataView;
    [SerializeField] private UnityEngine.Vector2 cellSize = new UnityEngine.Vector2(100f,144f);

    [SerializeField] private RectTransform  pane;
    [SerializeField] private Transform prefab;
    [SerializeField] private Transform buttonParent;

    private Transform[,] gridViewItems;

    float width, height;
    void Start()
    {
        width = pane.rect.width;
        height = pane.rect.height;
    }

    public void InitializeView(Model model){
        if(model!=null){
            gridViewItems = new Transform[model.getRow(), model.getCol()]; 
            //width = pane.rect.width;
            //height = pane.rect.height;
            for(int i=0; i<model.getRow(); i++){
                for(int j=0; j<model.getCol(); j++){
                    gridViewItems[i,j] = InstantiateCard(i,j,model);
                    //Debug.Log("Button " + i + ","+ j+ " is at" + gameObject.position);
                }
            }
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
        float xOffset = width/(model.getCol());
        float yOffset = height/(model.getRow());
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
                        gridViewItems[i,col].localPosition.y-(myCellsToFall*(height/(model.getRow()))),                            
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
