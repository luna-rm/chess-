using Unity.Mathematics;
using UnityEngine;
using static TheDevilScript;

public class TheHeavensScript : FaithScript {

    public PieceScript paladinPrefab;
    public PieceScript wallPrefab;
    public PieceScript angelPrefab;

    public static bool2 enable = new bool2(false, false);

    public enum ListTheHeavensPiece {
        pawn,
        tower,
        horse,
        bishop,
        king,
        queen,
        paladin,
        wall,
        angel
    }

    public void Start() {
        
    }

    private void Update() {
        if(StaticScript.faithWhite == StaticScript.ListFaith.heavens) {
            enable.x = true;
        }

        if(StaticScript.faithBlack == StaticScript.ListFaith.heavens) {
            enable.y = true;
        }
    }

    public void SpawnTheHeavens(ListTheHeavensPiece piece, Vector2Int posGrid, StaticScript.ListColor color, bool anim) {
        PieceScript aux = pawnPrefab;
        if(piece == ListTheHeavensPiece.pawn) {
            aux = pawnPrefab;
        } else if(piece == ListTheHeavensPiece.tower) {
            aux = towerPrefab;
        } else if(piece == ListTheHeavensPiece.horse) {
            aux = horsePrefab;
        } else if(piece == ListTheHeavensPiece.bishop) {
            aux = bishopPrefab;
        } else if(piece == ListTheHeavensPiece.king) {
            aux = kingPrefab;
        } else if(piece == ListTheHeavensPiece.queen) {
            aux = queenPrefab;
        } else if(piece == ListTheHeavensPiece.paladin) {
            aux = paladinPrefab;
        } else if(piece == ListTheHeavensPiece.wall) {
            aux = wallPrefab;
        } else if(piece == ListTheHeavensPiece.angel) {
            aux = angelPrefab;
        }
        
        Vector3 posWorld = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));

        PieceScript newPiece = Instantiate(aux, posWorld, Quaternion.identity);
        
        if(anim){                 
            Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 start  = target + new Vector3(0, 0.4f, 0);
            newPiece.posGrid = posGrid;
            newPiece.active = false;

            StartCoroutine(newPiece.MoveToPosition(target, start, false, 0.2f, true));
        }

        GameManagerScript.auxSpawn(newPiece, posGrid, color, false);
    }
}
