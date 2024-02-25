using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static int NumSpaces { get; private set; } = 12;
    public static int numStepsInSimulation = 500;
    public static float waitAfterDiceDisplay = 1.5f;

    private List<Piece> pieces = new();
    private GameStates gameStates = new(new());

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
        boardCreator.GenerateBoard(NumSpaces);
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

    public void Select()
    {
        //roll
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