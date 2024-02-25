using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    private List<Piece> pieces = new();
    private Piece pieceTakingTurn = null;
    private bool canInputToStartTurn = false;
    private GameStates gameStates = new(new());

    private void Start()
    {
        InitializeGame();
        InitializeJsonLog();
        Time.timeScale=timeScale;
        BeginGame();
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

    private void BeginGame()
    {
        canInputToStartTurn=true;
        pieceTakingTurn=pieces[0];
    }

    private void CompleteGame() => JsonLogger.WriteJson(gameStates);

    private void EndRound()
    {
        gameStates.Add(GetGameState());
        JsonLogger.WriteJson(gameStates);
        StartCoroutine(CallGameSequenceFunctionAfterWait());
    }

    private IEnumerator CallGameSequenceFunctionAfterWait()
    {
        yield return new WaitForSeconds(waitBetweenRounds);
        BeginGame();
    }

    public void Select()
    {
        if(canInputToStartTurn&&!IsGameOver())
        {
            if(pieceTakingTurn!=null)
            {
                PlayerTurn(pieceTakingTurn);
                StartCoroutine(NoInputsFor(waitBetweenTurns));
                int pieceIndex = pieces.IndexOf(pieceTakingTurn);
                if(pieceIndex>=pieces.Count-1)
                {
                    pieceTakingTurn=pieces[0];
                    EndRound();
                    TurnNumber++;
                    if(IsGameOver())
                        CompleteGame();
                }
                else
                    pieceTakingTurn=pieces[pieceIndex+1];
            }
            else
                throw new System.ArgumentNullException(nameof(pieceTakingTurn));
        }
    }

    private IEnumerator NoInputsFor(float waitTime)
    {
        canInputToStartTurn=false;
        yield return new WaitForSeconds(waitTime);
        canInputToStartTurn=true;
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