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

    protected string displayString;
    public string getDisplayString(){
        return displayString;
    }

    public virtual string UpdateDisplayString(int newValue){
        displayString= "Debug:\n<size=150%>"+newValue.ToString() +"</size><size=50%>Min 0 </size>";
        return displayString;
    }
    public virtual bool CheckConditional(int finalAmount){
        return true;
    }
}

public class GetXPoints :ScoreRequirements{
    private int points;
    public GetXPoints(int x){
        points =x;
        iconType = IconType.TextRequirement;
        displayString = "Points:\n<size=150%>0</size><size=50%>Min "+ points.ToString() + "</size>";
    }

    public override string UpdateDisplayString(int newValue){
        displayString ="Points:\n<size=150%>"+newValue.ToString() +"</size><size=50%>Min "+ points.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return points>=finalAmount;
    }
    public override string ToString(){
        return "Get at least" + points + " points";
    }
}

public class DestroyXWalls:ScoreRequirements{
    private int walls;
    public DestroyXWalls(int x){
        walls =x;
        iconType = IconType.IconRequirement;
        //displayString = "Walls:\n<size=150%>0</size><size=50%>Min "+ walls.ToString() + "</size>";
        displayString = walls.ToString();
    }
    public override string UpdateDisplayString(int newValue){
        //displayString ="Walls:\n<size=150%>"+newValue.ToString() +"</size><size=50%>Min "+ walls.ToString() + "</size>";
        displayString = walls.ToString();
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return walls>=finalAmount;
    }
    public override string ToString(){
        return "Destroy at least" + walls + " walls";
    }
}

public class DestroyXGlass:ScoreRequirements{
    private int glass;
    public DestroyXGlass(int x){
        glass =x;
        iconType = IconType.IconRequirement;
        //displayString = "Glass:\n<size=150%>0</size><size=50%>Min "+ glass.ToString() + "</size>";
        displayString = glass.ToString(); 
    }
    public override string UpdateDisplayString(int newValue){
        //displayString ="Glass:\n<size=150%>"+newValue.ToString() +"</size><size=50%>Min "+ glass.ToString() + "</size>";
        displayString = glass.ToString();
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return glass>=finalAmount;
    }
    public override string ToString(){
        return "Destroy at least" + glass + " glass";
    }
}
public class DestroyAllXWalls:ScoreRequirements{
    private int walls;
    public DestroyAllXWalls(int x){
        walls =x;
        iconType = IconType.IconRequirement;
        //displayString = "Walls:\n<size=150%>0</size><size=50%>0/"+ walls.ToString() + "</size>";
        displayString =walls.ToString();
    }
    public override string UpdateDisplayString(int newValue){
        //displayString = "Walls:\n<size=150%>"+newValue.ToString() +"</size><size=50%>"+ newValue.ToString()+"/"+ walls.ToString() + "</size>";
        displayString =walls.ToString();
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return walls>=finalAmount;
    }
    public override string ToString(){
        return "Destroy all " + walls + " walls";
    }
}
public class GetXMatches:ScoreRequirements{
    private int matches;
    public GetXMatches(int x){
        matches=x;
        iconType = IconType.TextRequirement;
        displayString = "Matches:\n<size=150%>0</size><size=50%>\nMin "+ matches + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString ="Matches:\n<size=150%>"+newValue.ToString() +"</size><size=50%>\nMin "+ matches.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return matches>=finalAmount;
    }
    public override string ToString(){
        return "Get  " + matches + " matches";
    }
}

public class GetXCombo:ScoreRequirements{
    int combo;
    public GetXCombo(int x ){
        combo=x;
        iconType = IconType.TextRequirement;
        displayString = "Combo:\n<size=150%>0</size><size=50%>\nMin "+ combo.ToString() + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString ="Combo:\n<size=150%>"+newValue.ToString() +"</size><size=50%>\nMin "+ combo.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return combo>=finalAmount;
    }
    public override string ToString(){
        return "Get a match combo of a least " + combo ;
    }
}


public class GetLessThanXMoves:ScoreRequirements{
    int maxMoves;
    public GetLessThanXMoves(int x ){
        maxMoves=x;
        iconType = IconType.TextRequirement;
        displayString = "Moves Remaining:\n<size=150%>0</size><size=50%>\nMin "+ maxMoves.ToString() + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        int movesLeft = maxMoves - newValue;
        displayString ="Moves:\n<size=150%>"+newValue.ToString() +"</size><size=50%>\nMin "+ movesLeft.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return maxMoves<finalAmount;
    }
    public override string ToString(){
        return "Complete the goal is in less than " + maxMoves + " moves";
    }
}
