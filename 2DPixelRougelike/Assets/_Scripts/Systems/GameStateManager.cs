using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameLoop,
    Shop,
    MidRunUpgrades,
    WeaponChoice,
    PassiveUpgrades,
    Paused,
    Cutscene,
    Dead
}
public class GameStateManager : Singleton<GameStateManager>
{
    private GameState currentGameState = GameState.GameLoop;
    private GameState lastState = GameState.GameLoop;
    public event Action<GameState> OnGameStateChanged;

    public void SetState(GameState _newState)
    {
        lastState = currentGameState;
        currentGameState = _newState;

        //tell
        OnGameStateChanged?.Invoke(currentGameState);
    }



    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public void ReturnToBaseState()
    {
        SetState(GameState.GameLoop);
    }
    public void ReturnToLastState()
    {
        SetState(lastState);
    }



}
