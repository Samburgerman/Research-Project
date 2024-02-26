using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static PlayerData;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] BoardCreator boardCreator;
    [SerializeField] SpaceDefinitions spaceDefinitions;
    [SerializeField] PieceMover pieceMover;
    [SerializeField] GameObject piecePrefab;
    [SerializeField] List<Material> materials = new();
    [SerializeField] Vector3 pieceDisplacement;

    public List<Piece> GeneratePieces(int startPosition,int startMoney)
    {
        List<Piece> pieces = new();
        for(int i = 0; i<materials.Count; i++)
        {
            Piece piece = InstansiatePiece(i,startPosition,startMoney).GetComponent<Piece>();
            pieces.Add(piece);
        }
        return pieces;
    }

    private GameObject InstansiatePiece(int playerIndex,int startSpaceNumber,int startMoney)
    {
        Material material = materials[playerIndex];
        GameObject pieceGameObject = Instantiate(piecePrefab);
        Piece piece = InitializePiece(playerIndex,startSpaceNumber,startMoney,material,pieceGameObject);
        return pieceGameObject;
    }

    private Piece InitializePiece(int playerIndex,
                                  int startSpaceNumber,
                                  int startMoney,
                                  Material material,
                                  GameObject pieceGameObject)
    {
        pieceGameObject.GetComponent<Renderer>().material=material;
        Piece piece = pieceGameObject.GetComponent<Piece>();
        PieceComponentRefrences references = new() { spaceDefinitions=spaceDefinitions };
        PlayerData playerData = new(playerIndex,
                                    startSpaceNumber,
                                    startMoney,
                                    GetExperimentalConditionFromIndex(playerIndex));
        piece.InitializePieceFields(playerData,references);
        //pieceMover.Move(piece,0);//this initializes the piece to the correct starting position
        pieceMover.InitializePiecePostion(piece);
        piece.transform.position+=pieceMover.OffsetPiecePositionForSharing(piece);
        pieceMover.ShiftUpwardsToStandOnSpacesOnBoard(piece.transform.position);
        return piece;
    }

    public List<Material> GetMaterials() => materials;
}