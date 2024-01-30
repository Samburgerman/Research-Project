using System.Collections.Generic;
using UnityEngine;
using static PlayerData;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField] readonly GameManager gameManager;
    [SerializeField] readonly BoardCreator boardCreator;
    [SerializeField] readonly SpaceDefinitions spaceDefinitions;
    [SerializeField] readonly PieceMover pieceMover;
    [SerializeField] readonly GameObject piecePrefab;
    [SerializeField] readonly List<Material> materials = new();
    [SerializeField] Vector3 pieceDisplacement;//todo make it so that pieces can share a space

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
        pieceMover.Move(piece,0);//this initializes the piece to the correct starting position
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
        PlayerData playerData = new(playerIndex,startSpaceNumber,startMoney,ExperimentalCondition.Fair);
        //todo will need to overide .fair once the rigged works
        piece.InitializePieceFields(playerData,references);
        return piece;
    }
}