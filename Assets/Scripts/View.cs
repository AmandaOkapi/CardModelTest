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

            float xOffset = width/(model.getRow() +1);
            float yOffset = height/(model.getCol()+1);
            for(int i=0; i<model.getRow(); i++){
                for(int j=0; j<model.getCol(); j++){
                    gridViewItems[i,j] = Instantiate(prefab, buttonParent);
                    gridViewItems[i,j].localPosition = new UnityEngine.Vector3(xOffset*(j+1), -yOffset*(i+1),-5);
                    gridViewItems[i,j].GetComponent<CardMono>().setCardBase(((Card)model.getCardAtIndex(i,j)));
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

    public void UpdateView(Model model){
            Debug.Log("updating view");
            ClearView();
            InitializeView(model);
    }

    public void UpdateCards(){


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
        for(int i=0; i<gridViewItems.GetLength(0); i++){
            if(((Card)model.getCardAtIndex(i,col)).GetCellsToFall()>0){
                UnityEngine.Vector3 targetPos= new UnityEngine.Vector3(
                    gridViewItems[i,col].localPosition.x,
                    (-1)*(i+2)*(height/(model.getCol()+1)),
                    gridViewItems[i,col].localPosition.z
                ); 
                Debug.Log("Sending "+i+","+col+" to "+targetPos);
                gridViewItems[i,col].GetComponent<CardMono>().FallToPos(targetPos);
                ((Card)model.getCardAtIndex(i,col)).DecreaseCellsToFall();


            }

        }
    } 



}
