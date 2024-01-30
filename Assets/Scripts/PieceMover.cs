using UnityEngine;

public class PieceMover : MonoBehaviour
{
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    [SerializeField] private float moveUpBy = 0.5f;//we will need to shift pawn up to avoid clipping thru spaces

    public void TranslateToSpace(Piece piece,int currentPlayerSpaceNum)
    {
        Vector3 spacePosition = boardCreator.GetSpaceTransformPosition(currentPlayerSpaceNum);
        Vector3 piecePosition = ShiftUpwardsToStandOnSpacesOnBoard(spacePosition);
        piece.gameObject.transform.position=piecePosition;
        piece.gameObject.transform.SetParent(transform,true);
    }

    private Vector3 ShiftUpwardsToStandOnSpacesOnBoard(Vector3 old)
    {
        old.y+=moveUpBy;
        return old;
    }

    public void Move(Piece piece,int spaces)
    {
        piece.AdjustSpaces(spaces);//the player data will store the space number
        TranslateToSpace(piece,piece.GetPlayerData().spaceNumber);//the piece will physically move
        TriggerSpaceEffects(piece);//the player data will now gain or lose money
    }

    private void TriggerSpaceEffects(Piece piece)
    {
        Space spaceLandedOn = spaceDefinitions.GetSpaceFromIndex(piece.GetPlayerData().spaceNumber);
        int money = spaceLandedOn.GetMoney();
        piece.AdjustMoney(money);
    }
}