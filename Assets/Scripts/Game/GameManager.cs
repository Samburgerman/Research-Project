using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("ComponentReferences")]
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private DiceFaceLogic diceFaceLogic;
    [SerializeField] private PieceGenerator pieceGenerator;
    [SerializeField] private PieceMover pieceMover;
    [SerializeField] private TextController gameTextController;
    [SerializeField] private List<TextController> playerTextControllers;
    [SerializeField] private ParticleSystemController spaceLandParticleSystemController;
    [SerializeField] private SFXController sfxController;

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
    [SerializeField] private float particleSystemRunLength = 0.5f;

    [Space]
    [Header("FX")]
    [SerializeField] private string turnMessage = "Player #, press spacebar to make your move";
    [SerializeField] private char escapeCharacterForTurnMessage = '#';
    [SerializeField] private string gameEndMessage = "Game over. Please call experimenter";
    [SerializeField] private Color gameTextColor = Color.black;
    [SerializeField] private List<AudioClip> spaceSounds;
    [SerializeField] private AudioClip gameOverSound;

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
        InitializeTexts();
        InitializeParticleSystem();
        InitializeJsonLog();
        Time.timeScale=timeScale;
        BeginGame();
    }


    private void InitializeGame()
    {
        boardCreator.GenerateBoard(NumSpaces);
        pieces=pieceGenerator.GeneratePieces(startSpace,startMoney);
    }

    private void InitializeTexts()
    {
        int i = 0;
        foreach(TextController textController in playerTextControllers)
        {
            textController.SetGameText(pieces[i].GetPlayerData().money+"",GetPieceColor(i));
            i++;
        }
    }
    private void InitializeParticleSystem()
    {
        spaceLandParticleSystemController.RunParticles(new Vector3(0,1000,0),0.0f,Color.white);
    }

    private void InitializeJsonLog()
    {
        JsonLogger.ClearJson();
        JsonLogger.WriteJson(gameStates);
    }

    private void BeginGame()
    {
        canInputToStartTurn=true;
        pieceTakingTurn=pieces[0];
        gameTextController.SetGameText(GetTurnText(),GetPieceColor(0));
    }

    private void CompleteGame()
    {
        gameTextController.SetGameText(gameEndMessage,gameTextColor);
        JsonLogger.WriteJson(gameStates); 
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
                throw new ArgumentNullException(nameof(pieceTakingTurn));
        }
    }

    private IEnumerator NoInputsFor(float waitTime)
    {
        canInputToStartTurn=false;
        yield return new WaitForSeconds(waitTime);
        canInputToStartTurn=true;
        int pieceNum = pieceTakingTurn.GetPlayerData().playerIndex;
        gameTextController.SetGameText(GetTurnText(),GetPieceColor(pieceNum));
    }

    private void PlayerTurn(Piece piece)
    {
        gameTextController.SetGameText(string.Empty,gameTextColor);
        int roll = diceFaceLogic.RollDice(piece);
        pieceMover.Move(piece,roll);
        int pieceNum=piece.GetPlayerData().playerIndex;
        int money = piece.GetPlayerData().money;
        playerTextControllers[pieceNum].SetGameText(money+"",GetPieceColor(piece));
        Vector3 pos = piece.transform.position;
        spaceLandParticleSystemController.RunParticles(pos,particleSystemRunLength,GetSpaceColor(piece.GetSpaceOn()));
        sfxController.PlaySound(spaceSounds[SpaceDefinitions.ConvertSpaceTypeToIndex(piece.GetSpaceOn().SpaceType)]);
    }

    private GameState GetGameState()
    {
        List<PlayerData> playerDatas = new();//dont grammar me
        foreach(Piece piece in pieces)
            playerDatas.Add(piece.GetPlayerData());
        return new GameState(TurnNumber,playerDatas);
    }

    private bool IsGameOver() => TurnNumber>=totalTurnsInGame;

    private string GetTurnText()
    {
        return GetTurnTextFromPlayerIndexAndEscapeChar(pieceTakingTurn.GetPlayerData().playerIndex+1,escapeCharacterForTurnMessage);
    }

    private string GetTurnTextFromPlayerIndexAndEscapeChar(int playerNumberToDisplay, char escapeCharacter)
    {
        int loc = turnMessage.IndexOf(escapeCharacter);
        if(loc==-1)
            throw new ArgumentOutOfRangeException(nameof(loc));
        return turnMessage[..loc]+playerNumberToDisplay+turnMessage[(loc+1)..];
    }
    
    private Color GetPieceColor(Piece piece)
    {
        int pieceNum = piece.GetPlayerData().playerIndex;
        return GetPieceColor(pieceNum);
    }

    private Color GetPieceColor(int pieceNum)
    {
        return pieceGenerator.GetMaterials()[pieceNum].color;
    }

    private Color GetSpaceColor(Space space)
    {
        return space.GetMaterial().color;
    }
}