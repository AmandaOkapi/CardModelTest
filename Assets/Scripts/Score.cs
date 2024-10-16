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

    public string MakeDisplayString(string topWord, int collected, int goal){
        return "<color=#222222>"+topWord+":</color>\n<size=200%>" +collected.ToString() + "</size><size=70%><voffset=1em><space=0.2em>/" + goal.ToString() +"</voffset></size>";
    }
    public virtual string UpdateDisplayString(int newValue){
        gameValue =newValue;
        displayString= SetNewValueToDisplayString(newValue);
        return displayString;
    }
    public virtual string SetNewValueToDisplayString(int newValue){
        if(iconType == IconType.TextRequirement){
            displayString= MakeDisplayString(name, newValue, myGoal);
        }else if(iconType == IconType.IconRequirement){
            displayString = (Mathf.Max(myGoal - newValue, 0)).ToString();
        }else if(iconType == IconType.Moves){
            displayString = (newValue).ToString();
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
            displayString= MakeDisplayString(name, gameValue, myGoal);
        }else if(iconType == IconType.IconRequirement){
            displayString = gameValue.ToString();
        }

    }
}

public class GetXPoints :ScoreRequirements{

    public GetXPoints(int x){
        myGoal =x;
        iconType = IconType.TextRequirement;
        name = "Points";
        displayString = MakeDisplayString(name, 0, myGoal);
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
        name = "Matches";
        displayString = MakeDisplayString(name, 0, myGoal);
        gameValue =0;
        
    }
    
    public override string ToString(){
        return "Get " + myGoal + " matches";
    }
}

public class GetXCombo:ScoreRequirements{
    public GetXCombo(int x ){
        myGoal=x;
        iconType = IconType.TextRequirement;
        name = "Combo";
        MakeDisplayString(name, 0, myGoal);
        gameValue =0;
        
    }
    
    public override string ToString(){
        return "Get a match combo of a least " + myGoal ;
    }
}

public class GetLessThanXMoves:ScoreRequirements{
    public int maxMoves=0;
    public GetLessThanXMoves(int x){
        maxMoves=x;
        myGoal=0;
        iconType = IconType.Moves;
        displayString = MakeDisplayString("Moves Remaining", 0, myGoal);
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
