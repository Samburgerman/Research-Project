using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private PieceGenerator pieceGenerator;
    [SerializeField] private PieceMover pieceMover;
    [SerializeField] private int numSpaces = 12;
    [SerializeField] private float radius = 5;

    private List<Piece> pieces = new();
    private GameStates gameStates = new GameStates(new List<GameState>());

    private int turnNumber = 0;
    [SerializeField] private int totalTurnsInGame = 10;
    [SerializeField] private int startSpace = 0;
    [SerializeField] private int startMoney = 10;

    [SerializeField] private const float waitTimeInTransitions = 1.0f;

    public int GetPlayerPos(int playerIndex)
    { return pieces[playerIndex].GetPlayerData().GetSpaceNumber(); }

    public int GetTurnNumber()
    { return turnNumber; }

    public int GetTotalSpaces()
    { return numSpaces; }

    private void Start()
    {
        boardCreator.GenerateBoard(numSpaces,radius);
        pieces=pieceGenerator.GeneratePieces(startSpace,startMoney);
        LogJson();
        GameLoop();
    }

    private void GameLoop()
    {
        while(!IsGameOver())
        {
            turnNumber++;
            //each player takes a turn moving around the board
            foreach(Piece piece in pieces)
            {
                PlayerTurn(piece);
                StartCoroutine(Wait(waitTimeInTransitions));
            }
            gameStates.Add(GetGameState());
        }
        LogJson();
    }

    private void LogJson()
    {
        string jsonOutput = JsonUtility.ToJson(gameStates);//find how to convert correctly
        Debug.Log(jsonOutput);
        File.WriteAllText(Application.dataPath+"/data.txt",jsonOutput);
    }

    private void PlayerTurn(Piece piece)
    {
        int roll = MovementLogic.DecideMovement(piece);
        //Debug.Log("roll: "+roll);
        pieceMover.Move(piece,roll);
    }

    private GameState GetGameState()
    {
        List<PlayerData> playerDatas = new();//dont grammar me
        foreach(Piece piece in pieces)
            playerDatas.Add(piece.GetPlayerData());
        Debug.Log(playerDatas[0]);
        return new GameState(turnNumber,playerDatas);
    }

    private bool IsGameOver()
    { return turnNumber>=totalTurnsInGame; }

    private IEnumerator Wait(float waitTime)
    {
        //print("before");
        yield return new WaitForSecondsRealtime(waitTime);
        //print("after");
    }
}
[System.Serializable]
public struct GameState
{
    public int turnNumber;
    public List<PlayerData> playerDatas;

    public GameState(int turnNumber,List<PlayerData> playerDatas)
    {
        this.playerDatas=new();
        this.turnNumber=turnNumber;
        foreach(PlayerData playerData in playerDatas)
            this.playerDatas.Add(playerData.Clone());
    }

    public override string ToString()
    {
        string playerDatasMessage = "";
        foreach(PlayerData playerData in playerDatas)
            playerDatasMessage+=playerData.ToString();
        return "Turn: "+turnNumber+" PlayerDatasMessage: "+playerDatasMessage;
    }
}
[System.Serializable]//need a wrapper class for the list so it can be .json-ed
public struct GameStates
{
    public List<GameState> gameStatesList;
    
    public GameStates(List<GameState> gameStateList)
    { this.gameStatesList=gameStateList; }

    public void Add(GameState gameState)
    {
        gameStatesList.Add(gameState);
    }
}