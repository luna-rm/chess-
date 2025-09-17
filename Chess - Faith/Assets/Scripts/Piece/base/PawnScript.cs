using UnityEngine;

public class PawnScript : PieceScript {

    bool alreadyMoved = false;

    public override void Start() {
        base.Start();
        tags.Add(StaticScript.ListTags.tier1);
    }

    public override void Update() {
        base.Update();
    }

    public override bool canMove1(BoardScript square) {
        if(isTargetingPiece(square.posGrid) != null) {
            return false;
        }

        if (square.posGrid.y + front() == posGrid.y && square.posGrid.x == posGrid.x) {
            return true;
        }

        if (!alreadyMoved) {
            if (square.posGrid.y + (front()*2) == posGrid.y && square.posGrid.x == posGrid.x) {
                return true;
            }
        }

        return false;
    }

    public override bool canMove2(BoardScript square) {
        if(isTargetingPiece(square.posGrid) != null) {
            return false;
        }

        if (square.posGrid.y + front() == posGrid.y && square.posGrid.x == posGrid.x) {
            return true;
        }

        if (!alreadyMoved) {
            if (square.posGrid.y + (front()*2) == posGrid.y && square.posGrid.x == posGrid.x) {
                return true;
            }
        }

        return false;
    }

    public override bool canMove3(BoardScript square) {
        Debug.Log("4");
        return false;
    }

    public override bool canMove4(BoardScript square) {
        Debug.Log("4");
        return false;
    }

    public override void move1(Vector2Int select) {
        animMove(select, false);
        StaticScript.ListColor target = StaticScript.ListColor.white;
        if(color == StaticScript.ListColor.white) {
            target = StaticScript.ListColor.black;
        }
        countKill(select, target);
        addCd(true, 1, 1);
        GameManagerScript.finishMove(StaticScript.ListTime.slow);
    }

    public override void move2(Vector2Int select) {
        animMove(select, true);
        GameManagerScript.finishMove(StaticScript.ListTime.slow);
    }

    public override void animMove(Vector2Int newPos, bool jump) {
        base.animMove(newPos, jump);
    }

    public override void addCd(bool pieceSt, int cdMove, int move) {
        base.addCd(pieceSt, cdMove, move); 
        alreadyMoved = true;
    }
}
