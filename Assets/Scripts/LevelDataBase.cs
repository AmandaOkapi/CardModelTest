using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataBase : MonoBehaviour
{
    public static List<Level> levels;

    private void Awake(){
        InitializeDatabase();
    }
    private void InitializeDatabase()
    {
        levels = new List<Level>();
        levels.AddRange(GetInitialLevels());
    }
    private List<Level> GetInitialLevels()
    {
        List<Level> returnList = new List<Level>();
        
        WallModelDestroyWalls model0 = new WallModelDestroyWalls(15, 6, false);
        model0.SetPossibleCards(new List<int>{0,1,2,3,4,5,6,7,8,9,10});
        int wallCount = model0.GetWallCount();
        DestroyAllXWalls destroyAllXWalls = new DestroyAllXWalls(wallCount);
        GetXMatches getXMatches = new GetXMatches(20);
        returnList.Add(new Level{
            model = model0,
            score = new Score(0, destroyAllXWalls, getXMatches)
        });
        
        OriginalModel model1 = new OriginalModel(12, 6, false);
        model1.SetPossibleCards(new List<int>{0,1,2,3,4,5});
        returnList.Add(new Level{
            model = model1,
            score = new Score(90, new GetXMatches(30), new GetXCombo(2))
        });
        
        EliminationModel model2= new EliminationModel(9,7,false);
        model2.SetPossibleCards(new List<int>{0,1,2,3,4,5,6,7,8,9,10});
        returnList.Add(new Level{
            model = model2,
            score = new Score(0, new GetXMatches(31))
        });

        WallModelOriginal model3= new WallModelOriginal(16,6,false);
        model3.SetPossibleCards(new List<int>{0,1,2,3,4,5,6,7,8});
        returnList.Add(new Level{
            model = model3,
            score = new Score(153, new DestroyXWalls(13), new GetXMatches(20))
        });

        WallModelElimination model4= new WallModelElimination(13,6,false);
        model4.SetPossibleCards(new List<int>{0,1,2,3,4,5,6,7,8});
        returnList.Add(new Level{
            model = model4,
            score = new Score(0,new GetXMatches(25))
        });

        EliminationModel model5= new EliminationModel(13,5,true);
        model5.SetPossibleCards(new List<int>{0,1,2,3,4,5,6,7,8,9,10,11});
        returnList.Add(new Level{
            model = model5,
            score = new Score(200,new GetXMatches(21))
        });
        return returnList;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
