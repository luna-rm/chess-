using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StaticScript : MonoBehaviour {

    [SerializeField] GameObject square;

    public static bool awakeDone = false;

    public static Vector2Int boardSize = new Vector2Int(10, 9); 

    public static float animSelectedTime = 0.05f;
    public static Vector3 animSelectedOffset = new Vector3(0, 0.15f, 0);

    public static List<Vector2Int> moveUpConcequence = new List<Vector2Int>();

    public static Vector2Int slay = new Vector2Int(0, 0);

    public enum ListTags {
        tier1,
        tier2,
        tier3,
        tier4,
        commander
    }

    public enum ListTime {
        slow,
        fast,
        bonus
    }

    public enum ListColor {
        white,
        black,
        gray,
        none
    }

    public enum ListFaith {
        devil,
        heavens
    }

    public enum ListBasePiece {
        pawn,
        tower,
        horse,
        bishop,
        king,
        queen
    }

    public static ListFaith faithWhite;
    public static ListFaith faithBlack;

    public static Grid grid;
    
    public static List<PieceScript> blackPieces = new List<PieceScript>();
    public static List<PieceScript> whitePieces = new List<PieceScript>();
    public static List<PieceScript> grayPieces = new List<PieceScript>();

    public static List<List<PieceScript>> pieceMatrix = new List<List<PieceScript>>();
    public static List<List<BoardScript>> boardMatrix = new List<List<BoardScript>>();


    private void Awake() {
        grid = GameObject.Find("Game Manager").GetComponent<Grid>();
        faithWhite = ListFaith.devil;
        faithBlack = ListFaith.devil;

        ListColor auxColor = ListColor.black;
        for(int i = 0; i < boardSize.x; i++) {
            List<PieceScript> auxP = new List<PieceScript>();
            List<BoardScript> auxB = new List<BoardScript>();

            for(int j = 0; j < boardSize.y; j++) {
                auxP.Add(null);

                Vector3 posWorld = grid.CellToWorld(new Vector3Int(i, j, 0));
                GameObject obj = Instantiate(square, posWorld, Quaternion.identity);
                BoardScript newSquare = obj.GetComponent<BoardScript>();
                newSquare.active = false;
                newSquare.color = auxColor;
                newSquare.posGrid = new Vector2Int(i, j);
                auxB.Add(newSquare);
                
                if(auxColor == ListColor.black) {
                    auxColor = ListColor.white;
                } else if(auxColor == ListColor.white) {
                    auxColor = ListColor.black;
                }
            }

            pieceMatrix.Add(auxP);
            boardMatrix.Add(auxB);
        }

        awakeDone = true;
    }
}
