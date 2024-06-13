using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//https://www.youtube.com/watch?v=70PcP_uPuUc
public class EventManager : MonoBehaviour
{

    //events?
    public static event Action<int>  MatchFoundEvent;
    public static void StartMatchFoundEvent(int id){
        MatchFoundEvent?.Invoke(id);
    }

    public static event Action WallDestroyed;
    public static void StartWallDestroyedEvent(){
        WallDestroyed?.Invoke();
    }
    public static event Action<int,int> MatchFailed;
    public static void StartMatchFailed(int id1, int id2){
        MatchFailed?.Invoke(id1, id2);
    }

    public static event Action<int,int, int> MatchThreeFoundEvent;
    public static void StartMatchThreeFaileddEvent(int id1, int id2, int id3){
        MatchThreeFoundEvent?.Invoke(id1, id2, id3);
    }
    
    public static event Action LuckyMatchFound;
    public static void StartLuckyMatchFound(){
        LuckyMatchFound?.Invoke();
    }
    public static event Action<float> GameTimer;
    public static void StartGameTimer(float time){
        GameTimer?.Invoke(time);
    }

    public static event Action<Score> InitializeView;
    public static void StartInitializeView(Score score){
        InitializeView?.Invoke(score);
    }
}
