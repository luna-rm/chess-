using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static StaticScript;

public class GameManagerScript : MonoBehaviour {
    public BoardScript boardPrefab;

    public PieceScript pawnPrefab;
    public PieceScript towerPrefab;
    public PieceScript horsePrefab;
    public PieceScript bishopPrefab;
    public PieceScript kingPrefab;
    public PieceScript queenPrefab;

    [SerializeField] TheDevilScript theDevilScript;
    [SerializeField] TheHeavensScript theHeavensScript;

    [SerializeField] float animBoardDur = 0.25f;
    [SerializeField] Vector3 animBoardOffset = new Vector3(0, 0.4f, 0);

    int boardAppear = 0;

    public ListColor player = ListColor.white;
    public static PieceScript selectedPiece = null;

    public static int selectedMove = 0;
    public static int auxCanMove = 0;

    public static Vector2Int stamina = new Vector2Int(1, 1);

    void Start() {
        if(awakeDone){     
            for(int i = 1; i < boardSize.x-1; i++) {
                for(int j = 0; j < boardSize.y; j++) {
                    SpawnBoard(new Vector2Int(i, j));
                    boardAppear++;
                }
            }
        }
    }

    private void Update() {
        if(boardAppear != 0) {
            int auxBoardAppear = 0;
            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {  
                    if(b.active){ 
                        auxBoardAppear++;    
                    }
                }
            }

            if(auxBoardAppear == boardAppear) {
                boardAppear = 0;
                appearPieces();
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            leftClick();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame) {
            rightClick();
        }
    }

    void appearPieces() {
        if(TheDevilScript.enable.x || TheDevilScript.enable.y) {
            theDevilScript.startPieces();
        }
    }

    public static void auxSpawn(PieceScript newPiece, Vector2Int posGrid, StaticScript.ListColor color, bool sickeness) {
        Vector3 posWorld = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));

        newPiece.color = color;
        newPiece.posGrid = new Vector2Int(posGrid.x, posGrid.y-1);

        StaticScript.pieceMatrix[posGrid.x][posGrid.y-1] = newPiece;
        if(color == StaticScript.ListColor.black) {
            StaticScript.blackPieces.Add(newPiece);
        } else if(color == StaticScript.ListColor.white) {
            StaticScript.whitePieces.Add(newPiece);
        } else if(color == StaticScript.ListColor.gray) {
            StaticScript.grayPieces.Add(newPiece);
        }

        if (sickeness) {
            newPiece.staminaPiece = 0;
        }
    }

    public void SpawnBoard(Vector2Int posGrid) {
        Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
        Vector3 start  = target + animBoardOffset;

        StartCoroutine(StaticScript.boardMatrix[posGrid.x][posGrid.y].MoveToPosition(target, start, animBoardDur, true));
    }

    public void changeTurn() {
        if(player == ListColor.white) {
            player = ListColor.black;
            foreach(PieceScript p in whitePieces) {
                for(int i = 0; i < p.cd.Length; i++) {
                    if (p.cd[i] > 0) {
                        p.cd[i]--;    
                    }    
                    if (p.staminaPiece <= 0) {
                        p.staminaPiece = 1;
                    }
                }
            }
        } else if(player == ListColor.black) { 
            player = ListColor.white;    
            foreach(PieceScript p in blackPieces) {
                for(int i = 0; i < p.cd.Length; i++) {
                    if (p.cd[i] > 0) {
                        p.cd[i]--;    
                    }   
                    if (p.staminaPiece <= 0) {
                        p.staminaPiece = 1;
                    }
                }
            }
        }
    }

    private void leftClick() {
        if(selectedPiece == null) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Piece"));

            if (hit.collider != null) {
                PieceScript piece = hit.collider.GetComponent<PieceScript>();
                if (piece != null) {
                    if(piece.color == player) { 
                        if(piece.staminaPiece > 0) {
                            selectedPiece = piece;
                            piece.onSelected();     
                        }
                                     
                    }
                }
            }
        }
    }

    private void rightClick() {
        if(selectedPiece != null) {
            selectedPiece.onUnselected();
            selectedPiece = null;
            selectedMove = 0;
            undoAppearMove();

            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {
                    if (b.active) {
                        if (b.selected){
                            b.onUnselected();
                        }
                    }
                }
            }
        }
    }

    public static void undoAppearMove() {
        auxCanMove = 0;
        foreach(List<BoardScript> l in boardMatrix) {
            foreach(BoardScript b in l) {
                if (b.active) {
                    if (b.possibleMove) {
                        b.changePossibleMove();
                    }
                }
            }
        }
    }

    public void appearMove() {
        if(selectedMove == 1 && auxCanMove != 1) { 
            undoAppearMove();
            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {
                    if (b.active) {
                        if (selectedPiece.canMove1(b)) {
                            b.changePossibleMove();
                        }
                    }
                }
            }
            auxCanMove = 1;
        } else if(selectedMove == 2 && auxCanMove != 2) { 
            undoAppearMove();
            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {
                    if (b.active) {
                        if (selectedPiece.canMove2(b)) {
                            b.changePossibleMove();
                        }
                    }
                }
            }
            auxCanMove = 2;
        } else if(selectedMove == 3 && auxCanMove != 3) { 
            undoAppearMove();
            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {
                    if (b.active) {
                        if (selectedPiece.canMove3(b)) {
                            b.changePossibleMove();
                        }
                    }
                }
            }
            auxCanMove = 3;
        } else if(selectedMove == 4 && auxCanMove != 4) { 
            undoAppearMove();
            foreach(List<BoardScript> l in boardMatrix) {
                foreach(BoardScript b in l) {
                    if (b.active) {
                        if (selectedPiece.canMove4(b)) {
                            b.changePossibleMove();
                        }
                    }
                }
            }
            auxCanMove = 4;           
        }
    }

    public static void finishMove(ListTime time) {
        if(time == ListTime.slow) {
            stamina.x--;
        } else if (time == ListTime.fast) {
            if(stamina.y > 0) {
                stamina.y--;
            } else {
                stamina.x--;
            }
        }

        selectedPiece.selected = false;
        selectedMove = 0;
        selectedPiece = null;

        foreach(List<BoardScript> l in StaticScript.boardMatrix) {
            foreach(BoardScript b in l) {
                if(b != null) { 
                    if (b.selected) { 
                        b.onUnselected();
                    }
                }
            }
        }

        moveUpConcequence.Clear();

        undoAppearMove();
    }

    public void OnMove1(InputValue value) {
        foreach(Vector2Int m in moveUpConcequence) {
            Debug.Log(m);
        }
        if(selectedPiece == null) {
            return;
        }
        selectedMove = 1;
        appearMove();
    }

    public void OnMove2(InputValue value) {
        if(selectedPiece == null) {
            return;
        }
        selectedMove = 2;
        appearMove();
    }

    public void OnMove3(InputValue value) {
        if(selectedPiece == null) {
            return;
        }
        selectedMove = 3;
        appearMove();
    }

    public void OnMove4(InputValue value) {
        if(selectedPiece == null) {
            return;
        }
        selectedMove = 4;
        appearMove();
    }
    
    public void OnPass(InputValue value) {
        changeTurn();
    }
}
