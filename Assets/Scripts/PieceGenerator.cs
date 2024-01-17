using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] BoardCreator boardCreator;
    [SerializeField] SpaceDefinitions spaceDefinitions;
    [SerializeField] PieceMover pieceMover;
    [SerializeField] GameObject piecePrefab;
    [SerializeField] List<Material> materials = new();
    [SerializeField] Vector3 pieceDisplacement;//todo make it so that pieces can share a space

    public List<Piece> GeneratePieces(int startPosition, int startMoney)
    {
        List<Piece> pieces = new();
        int playerIndex = 0;
        foreach(Material m in materials)
        {
            Piece piece = InstansiatePiece(playerIndex, startPosition, startMoney).GetComponent<Piece>();
            pieces.Add(piece);
            playerIndex++;
        }
        return pieces;
    }

    private Vector3 GetPiecePosition(int playerIndex)
    {
        if(gameManager.GetTurnNumber()==0)
            return boardCreator.GetSpaceTransformPosition(0);
        return boardCreator.GetSpaceTransformPosition(gameManager.GetPlayerPos(playerIndex));
    }

    private GameObject InstansiatePiece(int playerIndex, int startPositon, int startMoney)
    {
        Material material = materials[playerIndex];
        GameObject pieceGameObject = GameObject.Instantiate(piecePrefab);
        pieceGameObject.GetComponent<Renderer>().material=material;
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.InitializePiece(gameManager,
                              spaceDefinitions,
                              playerIndex,
                              PlayerData.ExperimentalCondition.Fair,
                              startPositon,
                              startMoney);
        pieceMover.Move(piece,0);//this initializes the piece to the correct starting position
        return pieceGameObject;
    }
}