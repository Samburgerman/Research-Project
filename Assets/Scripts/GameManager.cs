using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class GameManager : MonoBehaviour
{
    [Header("ComponentReferences")]
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private DiceFaceLogic diceFaceLogic;
    [SerializeField] private PieceGenerator pieceGenerator;
    [SerializeField] private PieceMover pieceMover;

    [Space]
    [Header("GameRules")]
    [SerializeField] private int totalTurnsInGame = 10;
    [SerializeField] private int startSpace = 0;
    [SerializeField] private int startMoney = 10;
    
    [Space]
    [Header("Time")]
    [SerializeField] private float timeScale = 1.0f;
    [SerializeField] private float waitBetweenTurns = 1.0f;
    [SerializeField] private float waitBetweenRounds = 4.0f;


    public int TurnNumber { private set; get; } = 0;
    public static int TotalSpaces { get; private set; } = 12;
    public static int numStepsInSimulation = 500;
    public static float waitAfterDiceDisplay = 1.5f;

    private List<Piece> pieces = new();
    private GameStates gameStates = new(new());

    public int GetPlayerPos(int playerIndex) => pieces[playerIndex].GetPlayerData().spaceNumber;

    private void Start()
    {
        InitializeGame();
        InitializeJsonLog();
        Time.timeScale=timeScale;
        GameRecursiveSequence();
    }

    private void InitializeJsonLog()
    {
        JsonLogger.ClearJson();
        JsonLogger.WriteJson(gameStates);
    }

    private void InitializeGame()
    {
        boardCreator.GenerateBoard(TotalSpaces);
        pieces=pieceGenerator.GeneratePieces(startSpace,startMoney);
    }

    private void GameRecursiveSequence()
    {
        if(!IsGameOver())
        {
            TurnNumber++;
            //each player takes a turn moving around the board
            StartCoroutine(RecursiveTurns(0));
        }
        else
            CompleteGame();
    }

    private void CompleteGame() => JsonLogger.WriteJson(gameStates);

    private IEnumerator RecursiveTurns(int i)
    {
        if(i<pieces.Count)
        {
            yield return new WaitForSeconds(waitBetweenTurns);
            PlayerTurn(pieces[i]);
            StartCoroutine(RecursiveTurns(i+1));
        }
        else
            EndRound();
    }

    private void EndRound()
    {
        gameStates.Add(GetGameState());
        JsonLogger.WriteJson(gameStates);
        StartCoroutine(CallGameSequenceFunctionAfterWait());
    }

    private IEnumerator CallGameSequenceFunctionAfterWait()
    {
        yield return new WaitForSeconds(waitBetweenRounds);
        GameRecursiveSequence();
    }

    private void PlayerTurn(Piece piece)
    {
        int roll = diceFaceLogic.RollDice(piece);
        pieceMover.Move(piece,roll);
    }

    private GameState GetGameState()
    {
        List<PlayerData> playerDatas = new();//dont grammar me
        foreach(Piece piece in pieces)
            playerDatas.Add(piece.GetPlayerData());
        return new GameState(TurnNumber,playerDatas);
    }

    private bool IsGameOver() => TurnNumber>=totalTurnsInGame;
}

public static class JsonLogger
{
    private static string dataPath = Application.dataPath+"/JsonLogs/data.txt";
    public static void WriteJson(object o)
    {
        string jsonOutput = JsonUtility.ToJson(o,true);
        File.WriteAllText(dataPath,jsonOutput);
    }

    public static void ClearJson() => File.Delete(dataPath);
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

    public override string ToString()
    {
        string playerDatasMessage = "";
        foreach(PlayerData playerData in playerDatas)
            playerDatasMessage+=playerData.ToString();
        return "Turn: "+turnNumber+" PlayerDatasMessage: "+playerDatasMessage;
    }
}

[System.Serializable]//need a wrapper class for the List<GameState> so it can be .json-ed
public struct GameStates
{
    public List<GameState> gameStatesList;

    public GameStates(List<GameState> gameStateList)
    { gameStatesList=gameStateList; }

    public void Add(GameState gameState) => gameStatesList.Add(gameState);
}