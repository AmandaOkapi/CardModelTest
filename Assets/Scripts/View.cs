using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeView(Model model){
        if(model!=null){
            gridViewItems = new Transform[model.getRow(), model.getCol()]; 
            float width = pane.rect.width;
            float height = pane.rect.height;

            float xOffset = width/(model.getRow() +1);
            float yOffset = height/(model.getCol()+1);
            for(int i=0; i<model.getRow(); i++){
                for(int j=0; j<model.getCol(); j++){
                    Transform gameObject = Instantiate(prefab, buttonParent);
                    gameObject.localPosition = new Vector3(xOffset*(j+1), -yOffset*(i+1),-5);
                    gameObject.GetComponent<CardMono>().setCardBase(((Card)model.getCardAtIndex(i,j)));
                    //Debug.Log("Button " + i + ","+ j+ " is at" + gameObject.position);

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

}
