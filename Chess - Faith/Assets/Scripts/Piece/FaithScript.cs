using Unity.Mathematics;
using UnityEngine;
using static TheHeavensScript;

public class FaithScript : MonoBehaviour {

    public PieceScript pawnPrefab;
    public PieceScript towerPrefab;
    public PieceScript horsePrefab;
    public PieceScript bishopPrefab;
    public PieceScript kingPrefab;
    public PieceScript queenPrefab;

    public  virtual void killTrigger(PieceScript killer, PieceScript target) {

    }

}
