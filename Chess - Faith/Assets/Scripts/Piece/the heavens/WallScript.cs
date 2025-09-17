using UnityEngine;

public class WallScript : PieceScript {
    public override void Start() {
        base.Start();
        tags.Add(StaticScript.ListTags.tier1);
    }

    public override void Update() {
        base.Update();
    }

    public override bool canMove1(BoardScript square) {
        return false;
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
        base.move1(select);
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
