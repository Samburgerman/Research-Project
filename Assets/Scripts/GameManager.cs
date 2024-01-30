using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private readonly BoardCreator boardCreator;
    [SerializeField] private readonly PieceGenerator pieceGenerator;
    [SerializeField] private readonly PieceMover pieceMover;
    public static int TotalSpaces { get; private set; } = 12;
    [SerializeField] private readonly float radius = 5;

    private List<Piece> pieces = new();
    private GameStates gameStates = new(new());

    [SerializeField] readonly Dice dice;

    public int TurnNumber { private set; get; } = 0;
    [SerializeField] private readonly int totalTurnsInGame = 10;
    [SerializeField] private readonly int startSpace = 0;
    [SerializeField] private readonly int startMoney = 10;

    public static int numStepsInSimulation = 500;

    [SerializeField] private readonly float timeScale = 1.0f;
    [SerializeField] private readonly float waitBetweenTurns = 1.0f;
    public static readonly float waitAfterDiceDisplay = 1.5f;

    public int GetPlayerPos(int playerIndex)
    { return pieces[playerIndex].GetPlayerData().spaceNumber; }

    private void Start()
    {
        InitializeBoard();
        JsonLogger.LogJson(gameStates);
        GameLoop();
        Time.timeScale=timeScale;
        //eventually this method will go inside the game loop in a sensical way
        dice.Roll();
    }

    private void InitializeBoard()
    {
        boardCreator.GenerateBoard(TotalSpaces,radius);
        pieces=pieceGenerator.GeneratePieces(startSpace,startMoney);
    }

    private void GameLoop()
    {
        while(!IsGameOver())
        {
            TurnNumber++;
            //each player takes a turn moving around the board
            foreach(Piece piece in pieces)
            {
                PlayerTurn(piece);
                StartCoroutine(Wait(waitBetweenTurns));
            }
            gameStates.Add(GetGameState());
        }
        JsonLogger.LogJson(gameStates);
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
        return new GameState(TurnNumber,playerDatas);
    }

    private bool IsGameOver()
    { return TurnNumber>=totalTurnsInGame; }

    private IEnumerator Wait(float waitTime)
    {
        //print("before");
        yield return new WaitForSecondsRealtime(waitTime);
        //print("after");
    }
}

public static class JsonLogger
{
    public static void LogJson(object o)
    {
        string jsonOutput = JsonUtility.ToJson(o);
        File.WriteAllText(Application.dataPath+"/JsonLogs/data.txt",jsonOutput);
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
            this.playerDatas.Add(playerData);
    }

    public override readonly string ToString()
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
    { gameStatesList=gameStateList; }

    public readonly void Add(GameState gameState) { gameStatesList.Add(gameState); }
}