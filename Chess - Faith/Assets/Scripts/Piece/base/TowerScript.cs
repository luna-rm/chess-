using UnityEngine;

public class TowerScript : PieceScript {

    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
    }

    public override bool canMove1(BoardScript square) {
        if (square.posGrid.x != posGrid.x && square.posGrid.y != posGrid.y) {
            return false;
        }

        var canTarget = StaticScript.ListColor.white;
        if(color == StaticScript.ListColor.white) {
            canTarget = StaticScript.ListColor.black;
        }

        return pieceIsInLine(square.posGrid, canTarget);
    }

    public override bool canMove2(BoardScript square) {
        return false;
    }

    public override bool canMove3(BoardScript square) {
        return false;
    }

    public override bool canMove4(BoardScript square) {
        return false;
    }

    public override void move1(Vector2Int select) {
        animMove(select, false);
        addCd(true, 1, 1);
        GameManagerScript.finishMove(StaticScript.ListTime.slow);
    }

    public override void move2(Vector2Int select) {
        base.move2(select);
    }

    public override void move3(Vector2Int select) {
        base.move3(select);
    }

    public override void move4(Vector2Int select) {
        base.move4(select);
    }

    public override void animMove(Vector2Int newPos, bool jump) {
        base.animMove(newPos, jump);
    }

    public override void addCd(bool pieceSt, int cdMove, int move) {
        base.addCd(pieceSt, cdMove, move); 
    }
}
