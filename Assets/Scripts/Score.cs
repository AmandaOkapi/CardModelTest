using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class ScoreRequirements{
    protected string displayString;
    public string getDisplayString(){
        return displayString;
    }

    public virtual string UpdateDisplayString(int newValue){
        displayString= "Debug:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>Min 0 </size>";
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
        displayString = "Points:\n<size=150%>0</size>\n<size=50%>Min "+ points.ToString() + "</size>";
    }

    public override string UpdateDisplayString(int newValue){
        displayString ="Points:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>Min "+ points.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return points<=finalAmount;
    }
    public override string ToString(){
        return "Get at least" + points + " points";
    }
}

public class DestroyXWalls:ScoreRequirements{
    private int walls;
    public DestroyXWalls(int x){
        walls =x;
        displayString = "Walls:\n<size=150%>0</size>\n<size=50%>Min "+ walls.ToString() + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString ="Walls:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>Min "+ walls.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return walls<=finalAmount;
    }
    public override string ToString(){
        return "Destroy at least" + walls + " walls";
    }
}

public class DestroyAllXWalls:ScoreRequirements{
    private int walls;
    public DestroyAllXWalls(int x){
        walls =x;
        displayString = "Walls:\n<size=150%>0</size>\n<size=50%>0/"+ walls.ToString() + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString = "Walls:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>"+ newValue.ToString()+"/"+ walls.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return walls<=finalAmount;
    }
    public override string ToString(){
        return "Destroy all " + walls + " walls";
    }
}
public class GetXMatches:ScoreRequirements{
    private int matches;
    public GetXMatches(int x){
        matches=x;
        displayString = "Matches:\n<size=150%>0</size>\n<size=50%>Min "+ matches + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString ="Matches:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>Min "+ matches.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return matches<=finalAmount;
    }
    public override string ToString(){
        return "Get  " + matches + " matches";
    }
}

public class GetXCombo:ScoreRequirements{
    int combo;
    public GetXCombo(int x ){
        combo=x;
        displayString = "Combo:\n<size=150%>0</size>\n<size=50%>Min "+ combo.ToString() + "</size>";

    }
    public override string UpdateDisplayString(int newValue){
        displayString ="Combo:\n<size=150%>"+newValue.ToString() +"</size>\n<size=50%>Min "+ combo.ToString() + "</size>";
        return displayString;
    }
    public override bool CheckConditional(int finalAmount){
        return combo<=finalAmount;
    }
    public override string ToString(){
        return "Get a comnbo of a least " + combo + " matches";
    }
}
