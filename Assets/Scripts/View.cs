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
    [SerializeField] private Transform wallPrefab;

    [SerializeField] private Transform[,] gridViewItems;
    [Header ("Reveal Settings")]    
    [SerializeField] private float delay;
    [SerializeField] private float revealTime;
    [Header ("Scaling")]    
    [SerializeField] private bool maintainAspectRatio =true;
    [SerializeField] private UnityEngine.Vector2 cardSize = new UnityEngine.Vector2(100f,144f);
    [SerializeField] private RectTransform  pane;
    [SerializeField] private RectTransform  refPane;    //a pane that stretches with the screen size acts as a "size goal" for our grid
    [SerializeField] private Transform buttonParent;
    [SerializeField] private RectTransform topRowHider;

    private float localWidth, localHeight;
    private float refWidth, refHeight;

    private UnityEngine.Vector2 scaleFactor;
    void Start()
    {
        NullChecks();
        localWidth = pane.rect.width;
        localHeight = pane.rect.height;
        refWidth=refPane.rect.width *refPane.localScale.x;
        refHeight=refPane.rect.height *refPane.localScale.y;
    }

    private void NullChecks(){
        if(pane==null){
            pane = GameObject.Find("GridPane").GetComponent<RectTransform>();
        }
        if(refPane==null){
            refPane = GameObject.Find("ScalingGridPaneReference").GetComponent<RectTransform>();
        }
        if(buttonParent ==null){
            buttonParent= GameObject.Find("GridPane").transform;
        }
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
            
            //calculate scale factor based on the the refPane (want local*scalefactor = ref) and scale down the grid if needed
            scaleFactor.x = refWidth/localWidth;
            scaleFactor.y = refHeight/localHeight;
            
            float scaleFactorAverage = Mathf.Min(scaleFactor.x, scaleFactor.y, 1); //gives a max size
            if(maintainAspectRatio){
                pane.localScale = new UnityEngine.Vector3(scaleFactorAverage, scaleFactorAverage, 1f);
            }else{
                pane.localScale = new UnityEngine.Vector3(scaleFactor.x, scaleFactor.y, 1f);
            }
            //center the screen
            // float newPosX = -(pane.rect.width *pane.localScale.x)/2;
            // float newPosY = (model.isHideTopRows()) ? (pane.rect.height * pane.localScale.y +2*(cardSize.y * pane.localScale.y ))/2: (pane.rect.height *pane.localScale.y )/2;
            // pane.localPosition = new UnityEngine.Vector3(newPosX, newPosY);

            // if(model.isHideTopRows()){
            //     RectTransform trh =Instantiate(topRowHider, buttonParent);
            //     UnityEngine.Vector2 newSize = trh.sizeDelta;
            //     newSize.y = 2 * cardSize.y* pane.localScale.y;
            //     trh.sizeDelta = newSize;            
            // }

        }
    }


    private void ClearView(){
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        // Loop through each GameObject and destroy it
        foreach (GameObject card in cards)
        {
            MyDestroy(card);
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
            MyDestroy(gridViewItems[row,col].gameObject); 
        }
        gridViewItems[row, col] = InstantiateCard(row, col, model);
    }

    public Transform InstantiateCard(int i, int j, Model model){
        if(model.getCardAtIndex(i,j)==null){
            return null;
        }
        float xOffset = localWidth/(model.getCol());
        float yOffset = localHeight/(model.getRow());
        switch(model.getCardAtIndex(i,j)) 
        {
        case Card card:
            gridViewItems[i,j] = Instantiate(prefab, buttonParent);        
            Transform gameObject=gridViewItems[i,j];     
            gameObject.GetComponent<Button>().onClick.AddListener(() =>{
            //card flipped function
            gameObject.GetComponent<CardMono>().flipCard();
                Debug.Log("Button " + i + ","+ j+ "clicked!");
            });
            break;
        case Wall wall:
            gridViewItems[i,j] = Instantiate(wallPrefab, buttonParent);
            break;
        default:
            gridViewItems[i,j] = Instantiate(prefab, buttonParent);
            break;
        }
        gridViewItems[i,j].localPosition = new UnityEngine.Vector3(xOffset*(j), -yOffset*(i+1),-5);
        model.getCardAtIndex(i,j).ResetCellsToFall();
        gridViewItems[i,j].GetComponent<GridObjectMono>().setCardBase(model.getCardAtIndex(i,j));
        

        return gridViewItems[i,j];
    }
    public void RemoveCard(int row, int col){
        if(gridViewItems[row,col] == null){
            return;
        }
        MyDestroy(gridViewItems[row,col].gameObject); 
        gridViewItems[row,col]=null;
    }


    public void UpdateColumn(int col, Model model){
        //this is shit
        //handles dropping translation and generating new card
        //must be called after removing a card object
        //card objects check their linked base card to determine how many cells to fall
        //then their position is updated to reflect the models position of the card
        for(int i=gridViewItems.GetLength(0)-1; i>=0; i--){          
            if(gridViewItems[i,col]!=null){
                int myCellsToFall=gridViewItems[i,col].GetComponent<GridObjectMono>().getCardBase().GetCellsToFall();
                if(myCellsToFall>0){
                    //get target vector
                    UnityEngine.Vector3 targetPos= new UnityEngine.Vector3(
                        gridViewItems[i,col].localPosition.x,
                        gridViewItems[i,col].localPosition.y-(myCellsToFall*(localHeight/(model.getRow()))),                            
                        gridViewItems[i,col].localPosition.z
                        ); 
                    Debug.Log(gridViewItems[i,col].GetComponent<GridObjectMono>().getCardBase().GetCellsToFall() + "Sending "+i+","+col+" to "+targetPos);
                    //play the falling sequence
                    gridViewItems[i,col].GetComponent<GridObjectMono>().FallToPos(targetPos);                    
                    gridViewItems[i,col].GetComponent<GridObjectMono>().getCardBase().ResetCellsToFall();
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


    private void MyDestroy(GameObject go){
        go.GetComponent<GridObjectMono>().Die();
    }



    public void RevealRow(int row){
        StartCoroutine(RevealRowCards(row));
    }
    public void RevealCol(int col){
        StartCoroutine(RevealColCards(col));
    }

    IEnumerator RevealRowCards(int row){
        EnableButtons(false);
        for(int i=0; i<gridViewItems.GetLength(1); i++){
            if(gridViewItems[row, i] !=null){
                if(gridViewItems[row, i].gameObject.tag =="Card"){
                    StartCoroutine(RevealCard(gridViewItems[row, i].GetComponent<CardMono>()));
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        yield return new WaitForSeconds(delay+ revealTime);
        EnableButtons(true);
    }
    IEnumerator RevealColCards(int col){
        EnableButtons(false);
        for(int i=0; i<gridViewItems.GetLength(0); i++){
            if(gridViewItems[i, col] !=null){
                if(gridViewItems[i, col].gameObject.tag =="Card"){
                    StartCoroutine(RevealCard(gridViewItems[i, col].GetComponent<CardMono>()));
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        yield return new WaitForSeconds(delay+ revealTime);
        EnableButtons(true);
    }
    IEnumerator RevealCard(CardMono card){
        card.ShowflipCard();
        yield return new WaitForSeconds(revealTime);
        card.ShowUnflipCard();
    }


    private void EnableButtons(bool x){
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject b in buttons){
            b.GetComponent<CardMono>().SetEnabled(x);
        }
    }
}
