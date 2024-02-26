using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class PieceMover : MonoBehaviour
{
    [SerializeField] private BoardCreator boardCreator;
    [SerializeField] private SpaceDefinitions spaceDefinitions;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float moveUpByToSitOnSpace = 0.5f;
    [SerializeField] private float moveUpByForSpaceAnim = 1.5f;
    [SerializeField] private Vector3 shareSpaceOffset = Vector3.zero;
    private static int steps = 20;

    public void Move(Piece piece,int spaces)
    {
        //first we update player data for the correct space number
        //then we jump from space to space until we get to the final space
        //to do that, we raise the piece slowly, move it, and then drop it
        //after the final piece drop, call TriggerSpaceEffects(piece)
        int fromIndex = piece.GetPlayerData().spaceNumber;
        piece.AdjustSpaces(spaces);
        GameObject pieceGameObject = piece.gameObject;
        StartCoroutine(MoveSpaces(pieceGameObject,fromIndex,spaces));
    }

    private IEnumerator MoveSpaces(GameObject pieceGameObject, int fromIndex, int spaces)
    {
        for(int i = 0; i<spaces; i++)
        {
            yield return StartCoroutine(OneSpaceMove(pieceGameObject,fromIndex+i));
        }
        StartCoroutine(PlayMoveSound());
    }

    private IEnumerator PlayMoveSound()
    {
        SFXController sfxController = gameManager.sfxController;
        sfxController.PlaySound(gameManager.moveSound);
        yield return new WaitForSeconds(0.1f);
        sfxController.StopSound();
    }

    private IEnumerator OneSpaceMove(GameObject pieceGameObject, int spaceWasOn)
    {
        yield return StartCoroutine(UpAnimation(pieceGameObject));
        yield return StartCoroutine(MoveForwardOneSpace(pieceGameObject,spaceWasOn));//todo replace transfrom with space trans
        yield return StartCoroutine(DownAnimation(pieceGameObject));
        //call gamemanager.start sound for clink
    }

    private IEnumerator MoveForwardOneSpace(GameObject pieceGameObject, int spaceWasOn)
    {
        Vector3 fromV = pieceGameObject.transform.position;
        int spaceMovedTo = (spaceWasOn+1)%GameManager.NumSpaces;
        Vector3 toV = boardCreator.SpaceGameObjects[spaceMovedTo].transform.position;
        toV.y+=moveUpByForSpaceAnim;
        Debug.Log("spaceWasOn:"+spaceWasOn+" spaceMovedTo:"+spaceMovedTo+" fromV:"+fromV+" toV:"+toV);
        yield return StartCoroutine(MoveThrough(pieceGameObject.transform,fromV,toV));
    }

    private IEnumerator UpAnimation(GameObject pieceGameObject)
    {
        Vector3 current = pieceGameObject.transform.position;
        Vector3 above = new Vector3(current.x,current.y+moveUpByForSpaceAnim,current.z);
        yield return StartCoroutine(MoveThrough(pieceGameObject.transform,current,above));
    }

    private IEnumerator DownAnimation(GameObject pieceGameObject)
    {
        Vector3 current = pieceGameObject.transform.position;
        Vector3 above = new Vector3(current.x,current.y-moveUpByForSpaceAnim,current.z);
        yield return StartCoroutine(MoveThrough(pieceGameObject.transform,current,above));
    }

    private IEnumerator MoveThrough(Transform t, Vector3 from, Vector3 to)
    {
        float totalWait = GameManager.pieceMoveTime;
        float wait = totalWait/steps;
        for(int i=0;i<steps;i++)
        {
            yield return new WaitForSeconds(wait);
            t.position=Vector3.Lerp(from,to,(i+1.0f)/steps);
        }
    }

    //private void LerpToSpace(Piece piece,int currentPlayerSpaceNum,float t)
    //{
    //    Vector3 piecePosition = ShiftUpwardsToStandOnSpacesOnBoard(spacePosition);
    //    piece.gameObject.transform.position=Vector3.Lerp(spacePosition,piecePosition,t);
    //    piece.gameObject.transform.SetParent(transform,true);
    //    OffsetPiecePositionForSharing(piece);
    //}

    public void OffsetPiecePositionForSharing(Piece piece)
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

    public Vector3 ShiftUpwardsToStandOnSpacesOnBoard(Vector3 old)
    {
        old.y+=moveUpByToSitOnSpace;
        return old;
    }

    private void TriggerSpaceEffects(Piece piece)
    {
        Space spaceLandedOn = spaceDefinitions.GetSpaceFromIndex(piece.GetPlayerData().spaceNumber);
        int money = spaceLandedOn.GetMoney();
        piece.AdjustMoney(money);
    }
}