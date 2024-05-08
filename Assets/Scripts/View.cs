using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
public class View : MonoBehaviour
{
    // Start is called before the first frame update
    public CardData cardDataView;

    [SerializeField] private RectTransform  pane;
    [SerializeField] private Transform prefab;
    [SerializeField] private GameObject canvas;

    [SerializeField] private Transform buttonParent;

    private Transform[,] gridViewItems;

    float width, height;
    void Start()
    {
        float width = pane.rect.width;
        float height = pane.rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeView(Model model){
        if(model!=null){
            gridViewItems = new Transform[model.getRow(), model.getCol()]; 
            width = pane.rect.width;
            height = pane.rect.height;

            for(int i=0; i<model.getRow(); i++){
                for(int j=0; j<model.getCol(); j++){
                    gridViewItems[i,j] = InstantiateCard(i,j,model);
                    //Debug.Log("Button " + i + ","+ j+ " is at" + gameObject.position);
                        int x=i;
                        int y=j;
                    Transform gameObject=gridViewItems[i,j];     
                    gameObject.GetComponent<Button>().onClick.AddListener(() =>{

                        gameObject.GetComponent<CardMono>().flipCard();
                        Debug.Log("Button " + i + ","+ j+ "clicked!");
                    });
                }
            }
        }
    }

    public void RemakeView(Model model){
            Debug.Log("updating view");
            ClearView();
            InitializeView(model);
    }

    public void UpdateView(){

    }
    public void UpdateCards(Model model){
        for(int i=0; i< gridViewItems.GetLength(0); i++){
            for(int j=0; j< gridViewItems.GetLength(1); j++){
                if(gridViewItems[i,j].gameObject==null){
                    gridViewItems[i,j]=InstantiateCard(i,j,model);
                    return;
                }
                if(gridViewItems[i,j].GetComponent<CardMono>().getCardBase() != ((Card)model.getCardAtIndex(i,j))){
                    Destroy(gridViewItems[i,j].gameObject);
                    gridViewItems[i,j]=InstantiateCard(i,j,model);
                    return;
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


    public void UpdateCollumn(int col, Model model){
        //this is shit. gotta fix it
        for(int i=gridViewItems.GetLength(0)-1; i>=0; i--){
            int myCellsToFall=((Card)model.getCardAtIndex(i,col)).GetCellsToFall();
            if(myCellsToFall>0 && gridViewItems[i,col]!=null){
                UnityEngine.Vector3 targetPos= new UnityEngine.Vector3(
                    gridViewItems[i,col].localPosition.x,
                    gridViewItems[i,col].localPosition.y-(myCellsToFall*(height/(model.getCol()+1))),
                    gridViewItems[i,col].localPosition.z
                ); 
                Debug.Log("Sending "+i+","+col+" to "+targetPos);
                gridViewItems[i,col].GetComponent<CardMono>().FallToPos(targetPos);
                gridViewItems[i+myCellsToFall,col] = gridViewItems[i,col];
                ((Card)model.getCardAtIndex(i,col)).ResetCellsToFall();

            }
        }
        
    } 

    public void ResetCard(int row, int col, Model model){
        gridViewItems[row, col] = InstantiateCard(row, col, model);
    }

    public Transform InstantiateCard(int i, int j, Model model){
        float xOffset = width/(model.getRow() +1);
        float yOffset = height/(model.getCol()+1);
        gridViewItems[i,j] = Instantiate(prefab, buttonParent);
        gridViewItems[i,j].localPosition = new UnityEngine.Vector3(xOffset*(j+1), -yOffset*(i+1),-5);
        gridViewItems[i,j].GetComponent<CardMono>().setCardBase(((Card)model.getCardAtIndex(i,j)));
        return gridViewItems[i,j];
    }
    public void RemoveCard(int row, int col){
        Destroy(gridViewItems[row,col].gameObject); 
    }


}
