using UnityEngine;

public class PieceMover : MonoBehaviour
{
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    [SerializeField] private float moveUpBy = 0.5f;//we will need to shift pawn up to avoid clipping thru spaces
    [SerializeField] private Vector3 shareSpaceOffset = Vector3.zero;//shift pieces in x and z to avoid clipping

    public void Move(Piece piece,int spaces)
    {
        piece.AdjustSpaces(spaces);//the player data will store the space number
        TranslateToSpace(piece,piece.GetPlayerData().spaceNumber);//the piece will physically move
        TriggerSpaceEffects(piece);//the player data will now gain or lose money
    }

    private void TranslateToSpace(Piece piece,int currentPlayerSpaceNum)
    {
        Vector3 spacePosition = boardCreator.GetSpaceTransformPosition(currentPlayerSpaceNum);
        Vector3 piecePosition = ShiftUpwardsToStandOnSpacesOnBoard(spacePosition);
        piece.gameObject.transform.position=piecePosition;
        piece.gameObject.transform.SetParent(transform,true);
        OffsetPiecePositionForSharing(piece);
    }

    private void OffsetPiecePositionForSharing(Piece piece)
    {
        Vector3 position = piece.transform.position;
        int pieceNumber = piece.GetPlayerData().playerIndex;
        position=CalculateOffsetPosition(position,pieceNumber);
        piece.gameObject.transform.position=position;
    }

    private Vector3 CalculateOffsetPosition(Vector3 position,int pieceNumber)
    {
        if(pieceNumber%4 is 2 or 3)
            position.x+=shareSpaceOffset.x;
        else
            position.x-=shareSpaceOffset.x;
        if(pieceNumber%4 is 1 or 3)
            position.z+=shareSpaceOffset.z;
        else
            position.z-=shareSpaceOffset.z;
        return position;
    }

    private Vector3 ShiftUpwardsToStandOnSpacesOnBoard(Vector3 old)
    {
        old.y+=moveUpBy;
        return old;
    }

    private void TriggerSpaceEffects(Piece piece)
    {
        Space spaceLandedOn = spaceDefinitions.GetSpaceFromIndex(piece.GetPlayerData().spaceNumber);
        int money = spaceLandedOn.GetMoney();
        piece.AdjustMoney(money);
    }
}