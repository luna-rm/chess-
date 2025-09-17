using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TheDevilScript : FaithScript {

    public PieceScript cultistPrefab;
    public PieceScript devilPrefab;

    public static bool2 enable = new bool2(false, false);

    public enum ListTheDevilPiece {
        pawn,
        tower,
        horse,
        bishop,
        king,
        queen,
        cultist,
        devil
    }

    public void startPieces() {
        if (enable.x) {
            for (int i = 1; i < StaticScript.boardSize.x - 1; i++) {
                SpawnTheDevil(ListTheDevilPiece.pawn, new Vector2Int(i, 2), StaticScript.ListColor.white, true);
            }
            
            SpawnTheDevil(ListTheDevilPiece.tower, new Vector2Int(1, 1), StaticScript.ListColor.white, true);
            SpawnTheDevil(ListTheDevilPiece.tower, new Vector2Int(8, 1), StaticScript.ListColor.white, true);

            SpawnTheDevil(ListTheDevilPiece.horse, new Vector2Int(2, 1), StaticScript.ListColor.white, true);
            SpawnTheDevil(ListTheDevilPiece.horse, new Vector2Int(7, 1), StaticScript.ListColor.white, true);

            SpawnTheDevil(ListTheDevilPiece.bishop, new Vector2Int(3, 1), StaticScript.ListColor.white, true);
            SpawnTheDevil(ListTheDevilPiece.bishop, new Vector2Int(6, 1), StaticScript.ListColor.white, true);

            SpawnTheDevil(ListTheDevilPiece.king, new Vector2Int(4, 1), StaticScript.ListColor.white, true);

            SpawnTheDevil(ListTheDevilPiece.queen, new Vector2Int(5, 1), StaticScript.ListColor.white, true);

        }

        if (enable.y) {
            for (int i = 1; i < StaticScript.boardSize.x - 1; i++) {
                SpawnTheDevil(ListTheDevilPiece.pawn, new Vector2Int(i, StaticScript.boardSize.y - 1), StaticScript.ListColor.black, true);
            }

            SpawnTheDevil(ListTheDevilPiece.tower, new Vector2Int(1, StaticScript.boardSize.y), StaticScript.ListColor.black, true);
            SpawnTheDevil(ListTheDevilPiece.tower, new Vector2Int(8, StaticScript.boardSize.y), StaticScript.ListColor.black, true);

            SpawnTheDevil(ListTheDevilPiece.horse, new Vector2Int(2, StaticScript.boardSize.y), StaticScript.ListColor.black, true);
            SpawnTheDevil(ListTheDevilPiece.horse, new Vector2Int(7, StaticScript.boardSize.y), StaticScript.ListColor.black, true);

            SpawnTheDevil(ListTheDevilPiece.bishop, new Vector2Int(3, StaticScript.boardSize.y), StaticScript.ListColor.black, true);
            SpawnTheDevil(ListTheDevilPiece.bishop, new Vector2Int(6, StaticScript.boardSize.y), StaticScript.ListColor.black, true);

            SpawnTheDevil(ListTheDevilPiece.king, new Vector2Int(4, StaticScript.boardSize.y), StaticScript.ListColor.black, true);

            SpawnTheDevil(ListTheDevilPiece.queen, new Vector2Int(5, StaticScript.boardSize.y), StaticScript.ListColor.black, true);
        }
    }


    private void Update() {
        if(StaticScript.faithWhite == StaticScript.ListFaith.devil) {
            enable.x = true;
        }

        if(StaticScript.faithBlack == StaticScript.ListFaith.devil) {
            enable.y = true;
        }
    }

    public void SpawnTheDevil(ListTheDevilPiece piece, Vector2Int posGrid, StaticScript.ListColor color, bool anim) {
        PieceScript aux = pawnPrefab;
        if(piece == ListTheDevilPiece.pawn) {
            aux = pawnPrefab;
        } else if(piece == ListTheDevilPiece.tower) {
            aux = towerPrefab;
        } else if(piece == ListTheDevilPiece.horse) {
            aux = horsePrefab;
        } else if(piece == ListTheDevilPiece.bishop) {
            aux = bishopPrefab;
        } else if(piece == ListTheDevilPiece.king) {
            aux = kingPrefab;
        } else if(piece == ListTheDevilPiece.queen) {
            aux = queenPrefab;
        } else if(piece == ListTheDevilPiece.cultist) {
            aux = cultistPrefab;
        } else if(piece == ListTheDevilPiece.devil) {
            aux = devilPrefab;
        } 

        Vector3 posWorld = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));

        PieceScript newPiece = Instantiate(aux, posWorld, Quaternion.identity);
        
        if(anim){                 
            Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 start  = target + new Vector3(0, 0.4f, 0);
            newPiece.posGrid = new Vector2Int(posGrid.x, posGrid.y-1);
            newPiece.active = false;

            StartCoroutine(newPiece.MoveToPosition(target, start, false, 0.2f, true));
        }

        GameManagerScript.auxSpawn(newPiece, posGrid, color, false);
    }
}
