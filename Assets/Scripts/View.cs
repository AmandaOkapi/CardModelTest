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
    [SerializeField] public static UnityEngine.Vector2 cardSize = new UnityEngine.Vector2(150f,216f);

    private Transform[,] gridViewItems;
    private Transform[,] glassView;
    
    [Header ("Prefabs")]    
    [SerializeField] private Transform prefab, wallPrefab, glassPrefab;
    [Header ("Extra Elements")]
    [SerializeField] private GameObject LuckyMatchPrefab;
    [SerializeField] private GameObject PowerUpSlider;
        
    
    [Header ("Adjustable Stats")]

    [Header ("Reveal Settings")]    
    [SerializeField] private float delay;
    [SerializeField] private float revealTime;
    [SerializeField] private float frenzyTime;

    [Header ("Scaling")]    
    [SerializeField] private bool maintainAspectRatio =true;
    [SerializeField] private RectTransform  pane;
    [SerializeField] private RectTransform  refPane;    //a pane that stretches with the screen size acts as a "size goal" for our grid
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Transform glassParent;

    [SerializeField] private RectTransform topRowHider;
    [SerializeField] private DynamicFontSizeAdjuster dynamicFontSizeAdjuster;
    private UnityEngine.Vector2 scaleFactor;
    private float localWidth, localHeight;
    private float refWidth, refHeight;
    
    //gameState
    public static float cardsFalling;
    private bool powerUpPlaying =false;
    private bool frenzy=false;

    public bool IsPowerUpPlaying(){return powerUpPlaying;}
    public bool IsFrenzy(){return frenzy;}

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

        int myRow = model.getRow();
        int myCol = model.getCol();
        gridViewItems = new Transform[myRow, myCol]; 
        if(model.isHasGlass()){
            glassView = new Transform[myRow, myCol];
        }    
        //calculate and set the perfect dimensions for the grid based on the card pixel size
        pane.sizeDelta = new UnityEngine.Vector2(myCol*cardSize.x, myRow*cardSize.y);
        localWidth = pane.rect.width;
        localHeight = pane.rect.height;


        //populate the grid perfect sized grid with cards
        for(int i=0; i<myRow; i++){
            for(int j=0; j<myCol; j++){
                gridViewItems[i,j] = InstantiateCard(i,j,model);
                //glass
                if(model.isHasGlass()){
                    if(model.GetGlassAtIndex(i,j)){
                        float xOffset = localWidth/(model.getCol());
                        float yOffset = localHeight/(model.getRow());
                        glassView[i,j] = Instantiate(glassPrefab, glassParent);
                        glassView[i,j].localPosition = new UnityEngine.Vector3(xOffset*(j), -yOffset*(i+1),-5);
                    }else{
                        glassView[i,j]=null;   
                    }
                }
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
        float newPosX = -(pane.rect.width *pane.localScale.x)/2;
        float newPosY = (model.isHideTopRows()) ? (pane.rect.height * pane.localScale.y +(model.getRowsToHide())*(cardSize.y * pane.localScale.y ))/2: (pane.rect.height *pane.localScale.y )/2;
        pane.localPosition = new UnityEngine.Vector3(newPosX, newPosY);

        if(model.getRowsToHide()>0){
            //UnityEngine.Vector2 newSize = new UnityEngine.Vector2(topRowHider.sizeDelta.x , ((model.getRowsToHide()) * cardSize.y* pane.localScale.y));
            topRowHider.offsetMin = new UnityEngine.Vector2(topRowHider.offsetMin.x, model.getRow()*cardSize.y -((model.getRowsToHide()) * cardSize.y));

            //topRowHider.sizeDelta = newSize;
        }else{
            topRowHider.gameObject.SetActive(false);;
        } 
        dynamicFontSizeAdjuster?.AdjustFontSize(pane.sizeDelta.x);
    }


    private void ClearView(){
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        // Loop through each GameObject and destroy it
        foreach (GameObject card in cards)
        {
            Die(card);
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
            //this might need to be Destroy();
            Die(gridViewItems[row,col].gameObject); 
        }
    }

    public Transform InstantiateCard(int i, int j, Model model){
        if(model.getObjectAtIndex(i,j)==null){
            return null;
        }
        float xOffset = localWidth/(model.getCol());
        float yOffset = localHeight/(model.getRow());
        switch(model.getObjectAtIndex(i,j)) 
        {
        case Card card:
            gridViewItems[i,j] = Instantiate(prefab, buttonParent);        
            Transform gameObject=gridViewItems[i,j];     
            gameObject.GetComponent<Button>().onClick.AddListener(() =>{
            //card flipped function
            gameObject.GetComponent<CardMono>().flipCard();
                //Debug.Log("Button " + i + ","+ j+ "clicked!");
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
        model.getObjectAtIndex(i,j).ResetCellsToFall();
        gridViewItems[i,j].GetComponent<GridObjectMono>().SetCardBase(model.getObjectAtIndex(i,j));
        

        return gridViewItems[i,j];
    }
    public void RemoveCard(int row, int col){
        if(gridViewItems[row,col] == null){
            return;
        }
        Die(gridViewItems[row,col].gameObject); 
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
                    //Debug.Log(gridViewItems[i,col].GetComponent<GridObjectMono>().getCardBase().GetCellsToFall() + "Sending "+i+","+col+" to "+targetPos);
                    //play the falling sequence
                    gridViewItems[i,col].GetComponent<GridObjectMono>().FallToPos(targetPos);
                    gridViewItems[i,col].GetComponent<GridObjectMono>().getCardBase().ResetCellsToFall();
                    //reset local object grid
                    gridViewItems[i+myCellsToFall,col] = gridViewItems[i,col];
                    gridViewItems[i,col]=null;
                    }
                }      
            } 

        int index=0;
        for(int row=gridViewItems.GetLength(0)-1; row>=0; row--){          
            if(gridViewItems[row,col]==null){
                //Debug.Log("Resetting " + row + "," + col);
                InstantiateCard(row, col, model);
                if(gridViewItems[row, col] !=null){
                    //stop the pop-in
                    UnityEngine.Vector3 targetPos= gridViewItems[row, col].localPosition;
                    //send the card to the top
                    UnityEngine.Vector3 tempPos= new UnityEngine.Vector3(
                            gridViewItems[row,col].localPosition.x,
                            ((-1)*(model.getRowsToHide())*localHeight/(model.getRow())) + (index * localHeight/model.getRow()),                            
                            gridViewItems[row,col].localPosition.z
                            ); 
                    gridViewItems[row,col].localPosition = tempPos;
                    //play the falling sequence
                    //Debug.Log("THE TARGET POS IS " + targetPos + " FROM "+ tempPos );
                    gridViewItems[row,col].GetComponent<GridObjectMono>().FallToPos(targetPos);                    
                    index++;
                }
            }
        }      

    }


    private void Die(GameObject go){
        go.GetComponent<GridObjectMono>().Die();
    }

    public void DestroyGlassObject(int row, int col){
        Die(glassView[row, col].gameObject);        
        glassView[row, col]=null;

    }
    public void ResetPowerUpSlider(PowerUp powerUp){
        PowerUpSlider.GetComponent<PowerUpSlider>().ResetSlider(powerUp);
    }
    public void RevealAll( float startDelay){
        StartCoroutine(RevealAllCards( startDelay));
    }

    public void RevealRow(int row, float startDelay){
        StartCoroutine(RevealRowCards(row, startDelay));
    }
    public void RevealCol(int col, Model model, float startDelay){
        StartCoroutine(RevealColCards(col, model , startDelay));
    }

    IEnumerator RevealRowCards(int row, float startDelay){
        powerUpPlaying =true;
        float myRevealTime = AdjustRevealTime(CardsInRow(row));                
        yield return new WaitForSeconds(startDelay);

        PowerUpSlider.GetComponent<PowerUpSlider>().StartDepleteSlider(myRevealTime  + delay*gridViewItems.GetLength(1));

        for(int i=0; i<gridViewItems.GetLength(1); i++){
            Debug.Log("Hi from Reveal Row");
            if(gridViewItems[row, i] !=null){
                if(gridViewItems[row, i].gameObject.tag =="Card"){
                    StartCoroutine(RevealCard(gridViewItems[row, i].GetComponent<CardMono>() , myRevealTime, true));
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        yield return new WaitForSeconds((delay)+ myRevealTime);
        powerUpPlaying = false;
    }
    IEnumerator RevealColCards(int col, Model model, float startDelay){        
        powerUpPlaying=true;
        yield return new WaitForSeconds(startDelay);
        float myRevealTime =AdjustRevealTime(gridViewItems.GetLength(0) - model.getRowsToHide());
        PowerUpSlider.GetComponent<PowerUpSlider>().StartDepleteSlider(myRevealTime  + delay*gridViewItems.GetLength(0));
        for(int i=model.getRowsToHide(); i<gridViewItems.GetLength(0); i++){
            if(gridViewItems[i, col] !=null){
                if(gridViewItems[i, col].gameObject.tag =="Card"){
                    StartCoroutine(RevealCard(gridViewItems[i, col].GetComponent<CardMono>() , myRevealTime, true));
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        yield return new WaitForSeconds((delay) + myRevealTime);
        powerUpPlaying = false;
    }

    IEnumerator RevealAllCards( float startDelay){
        //powerUpPlaying=true;
        frenzy=true;
        yield return new WaitForSeconds(startDelay);
        PowerUpSlider.GetComponent<PowerUpSlider>().StartDepleteSlider(frenzyTime);
        GridObjectMono.fallSpeed = GridObjectMono.frenzyFallspeed;
        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Card")){
            StartCoroutine(RevealCard(card.GetComponent<CardMono>(), frenzyTime, false));
        }
        yield return new WaitForSeconds(frenzyTime);
        GridObjectMono.fallSpeed = GridObjectMono.defaultFallspeed;
        frenzy=false;
        //powerUpPlaying=false;
    }
    IEnumerator RevealCard(CardMono card, float myRevealTime, bool playSound){
        card.ShowflipCard(playSound);
        ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).SetIsFlipped(true);
        yield return new WaitForSeconds(myRevealTime);
        if(card!=null){
            card.ShowUnflipCard(false);
            ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).SetIsFlipped(false);
        }
    }
    private float AdjustRevealTime(int cardsToReveal){
        return Mathf.Max(revealTime, (delay * cardsToReveal) +Mathf.Min((cardsToReveal*0.15f), 1.5f));
    }

    private int CardsInRow(int row){
        int cnt=0;
        for(int i=0; i< gridViewItems.GetLength(1); i++){
            if(gridViewItems[row, i].gameObject.tag =="Card"){
                    cnt++;
            }
        }
        return cnt;
    }
    private void EnableButtons(bool x){
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject b in buttons){
            b.GetComponent<CardMono>().SetEnabled(x);
        }
    }

    public void InstantiateLuckyMatch(UnityEngine.Vector3 pos){
        GameObject luckyMatch = Instantiate(LuckyMatchPrefab, pos, UnityEngine.Quaternion.identity, transform );
        float scaleFactorAverage = Mathf.Min(scaleFactor.x, scaleFactor.y, 1); //gives a max size
        if(maintainAspectRatio){
            luckyMatch.transform.localScale = new UnityEngine.Vector3(scaleFactorAverage, scaleFactorAverage, 1f);
        }else{
            luckyMatch.transform.localScale = new UnityEngine.Vector3(scaleFactor.x, scaleFactor.y, 1f);
        }    
    }

}
