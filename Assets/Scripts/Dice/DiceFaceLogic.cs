using System.Collections.Generic;
using UnityEngine;

public class DiceFaceLogic : MonoBehaviour
{
    public DiceFaceDisplayer diceFaceDisplayer;

    public List<DiceFace> diceFaces;//needs to be an ordered list from 1-6

    public int RollDice(Piece piece)//returns the roll number
    {
        int decidedMove = MovementLogic.DecideRollNumber(piece);
        DiceFace toDisplay = GetDiceFaceFromFaceNumber(decidedMove);
        diceFaceDisplayer.DisplayDiceFace(toDisplay);
        return decidedMove;
    }

    public DiceFace GetDiceFaceFromFaceNumber(int i) => diceFaces[i-1];
}