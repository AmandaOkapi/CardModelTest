using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IconType
{
    Timer,
    Score,
    TextRequirement,
    IconRequirement,
    Moves
}
public class Score 
{
    private ScoreRequirements[] scoreRequirments;  
    private float gameTime;
    public ScoreRequirements[] GetScoreRequirments(){
        return scoreRequirments;
    }
    public float GetGameTime(){
        return gameTime;
    }
    public void SetGameTime(float time){
        gameTime=time;
    }
    public Score(float time, params ScoreRequirements[] scoreRequirments){
        gameTime=time;
        this.scoreRequirments = scoreRequirments;
    }
}

public abstract class ScoreRequirements{
    public IconType iconType; 
    public int photoID;

    public int gameValue;
    public int myGoal;

    protected string displayString;
    protected string name;
    public string getDisplayString(){
        return displayString;
    }

    public virtual string UpdateDisplayString(int newValue){
        gameValue =newValue;
        displayString= SetNewValueToDisplayString(newValue);
        return displayString;
    }
    public virtual string SetNewValueToDisplayString(int newValue){
        if(iconType == IconType.TextRequirement){
            displayString= name +":\n<size=150%>"+newValue.ToString() +"</size><size=50%>\nMin "+ myGoal.ToString() + "</size>";
        }else if(iconType == IconType.IconRequirement){
            displayString = (Mathf.Max(myGoal - newValue, 0)).ToString();
        }
        return displayString;
    }
    public virtual bool CheckConditional(int finalAmount){
        gameValue=finalAmount;
        if(iconType == IconType.IconRequirement){
            return (myGoal-finalAmount)<=0;
        }
        return myGoal <= finalAmount;
    }

    public virtual void Reset(){
        gameValue =0;
        if(iconType == IconType.TextRequirement){
            displayString= name +":\n<size=150%>"+gameValue.ToString() +"</size><size=50%>\nMin "+ myGoal.ToString() + "</size>";
        }else if(iconType == IconType.IconRequirement){
            displayString = gameValue.ToString();
        }

    }
}

public class GetXPoints :ScoreRequirements{

    public GetXPoints(int x){
        myGoal =x;
        iconType = IconType.TextRequirement;
        displayString = "Points:\n<size=150%>0</size><size=50%>Min "+ myGoal.ToString() + "</size>";
        name = "Points";
        gameValue =0;
    }
    public override string ToString(){
        return "Get at least " + myGoal + " points";
    }
}

public class DestroyXWalls:ScoreRequirements{
    public DestroyXWalls(int x){
        myGoal =x;
        iconType = IconType.IconRequirement;
        photoID =0;
        //displayString = "Walls:\n<size=150%>0</size><size=50%>Min "+ walls.ToString() + "</size>";
        displayString = x.ToString();
        gameValue = 0;
    }
    
    public override string ToString(){
        return "Destroy at least " + myGoal + " walls";
    }
}

public class DestroyXGlass:ScoreRequirements{
    public DestroyXGlass(int x){
        myGoal =x;
        iconType = IconType.IconRequirement;
        photoID =1;
        //displayString = "Glass:\n<size=150%>0</size><size=50%>Min "+ glass.ToString() + "</size>";
        displayString = x.ToString(); 
        gameValue =0;
    }
    
    public override string ToString(){
        return "Destroy at least " + myGoal + " glass";
    }
}
public class DestroyAllXWalls:ScoreRequirements{
    public DestroyAllXWalls(int x){
        myGoal =x;
        iconType = IconType.IconRequirement;
        photoID =0;
        //displayString = "Walls:\n<size=150%>0</size><size=50%>0/"+ walls.ToString() + "</size>";
        displayString =x.ToString();
        gameValue =0;
    }
    public override string ToString(){
        return "Destroy all " + myGoal + " walls";
    }
}
public class GetXMatches:ScoreRequirements{
    public GetXMatches(int x){
        myGoal=x;
        iconType = IconType.TextRequirement;
        displayString = "Matches:\n<size=150%>0</size><size=50%>\nMin "+ myGoal + "</size>";
        gameValue =0;
        name = "Matches";
    }
    
    public override string ToString(){
        return "Get " + myGoal + " matches";
    }
}

public class GetXCombo:ScoreRequirements{
    public GetXCombo(int x ){
        myGoal=x;
        iconType = IconType.TextRequirement;
        displayString = "Combo:\n<size=150%>0</size><size=50%>\nMin "+ myGoal.ToString() + "</size>";
        gameValue =0;
        name = "Combo";
    }
    
    public override string ToString(){
        return "Get a match combo of a least " + myGoal ;
    }
}

public class GetLessThanXMoves:ScoreRequirements{
    int maxMoves=0;
    public GetLessThanXMoves(int x){
        maxMoves=x;
        myGoal=0;
        iconType = IconType.Moves;
        displayString = "Moves Remaining:\n<size=150%>0</size><size=50%>\nMax "+ myGoal.ToString() + "</size>";
        gameValue = maxMoves;
    }

    public override bool CheckConditional(int finalAmount){
        return false;
    }
    
    public override void Reset(){
        gameValue =maxMoves;
        displayString = gameValue.ToString();
    }
    public override string ToString(){
        return "Complete the goal is in less than " + maxMoves + " moves";
    }
}
