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
        
        WallModelDestroyWalls model = new WallModelDestroyWalls(7, 6, false);
        int wallCount = model.GetWallCount();
        DestroyAllXWalls destroyAllXWalls = new DestroyAllXWalls(wallCount);
        GetXMatches getXMatches = new GetXMatches(25);
        returnList.Add(new Level{
            model = model,
            score = new Score(135, destroyAllXWalls, getXMatches)
        });
        
        model = new WallModelDestroyWalls(10, 4, false);
        wallCount = model.GetWallCount();
        destroyAllXWalls = new DestroyAllXWalls(wallCount);
        getXMatches = new GetXMatches(25);
        returnList.Add(new Level{
            model = model,
            score = new Score(60, destroyAllXWalls, getXMatches)
        });
        
        model = new WallModelDestroyWalls(14, 4, false);
        wallCount = model.GetWallCount();
        destroyAllXWalls = new DestroyAllXWalls(wallCount);
        getXMatches = new GetXMatches(25);
        returnList.Add(new Level{
            model = model,
            score = new Score(150, destroyAllXWalls, getXMatches)
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
