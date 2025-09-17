using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManagerScript;
using static StaticScript;
using static TheDevilScript;
using static TheHeavensScript;
using static Unity.Burst.Intrinsics.X86;

public class PieceScript : MonoBehaviour {
    [SerializeField] Sprite blackSpr;
    [SerializeField] Sprite whiteSpr;

    public StaticScript.ListColor color;

    [SerializeField] SpriteRenderer SpriteRenderer;

    public Vector2Int posGrid = new Vector2Int();

    public bool active = false;
    public bool selected = false;
    bool possibleMove = false;

    public int[] cd = {0, 0, 0, 0}; 
    public int staminaPiece = 1;

    public List<ListTags> tags;

    public IEnumerator MoveToPosition(Vector3 target, Vector3 start, bool jump, float duration, bool appear) {
        transform.position = start;
        float elapsed = 0f;

        float waitToStart = 0;
        if(appear) {
            waitToStart = (8 - posGrid.y * 2) * duration;
            if(posGrid.y > 4) {
                waitToStart = (18 - posGrid.y * 2) * duration;
            }
        } else {
            waitToStart = 0;
        }

        while (elapsed < waitToStart) {
            elapsed += Time.deltaTime;
            yield return null;
        }

        active = true;
        elapsed = 0f;

        if(!jump)
        {
            while (elapsed < duration) {
                float t = elapsed / duration;
                t = (t < 0.5f) ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2; 

                transform.position = Vector3.Lerp(start, target, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        } else {
            while (elapsed < duration * 1.5f)
            {
                float t = elapsed / (duration * 1.5f);
                t = t * t * (3f - 2f * t);

                Vector3 pos = Vector3.Lerp(start, target, t);

                pos.y += Mathf.Sin(t * Mathf.PI) * 1;

                transform.position = pos;

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
        }
    }

    public void onSelected() {
        if (!selected) {  
            boardMatrix[posGrid.x][posGrid.y].onSelected();

            Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y + 1, 0));
            Vector3 target = start + animSelectedOffset;
            StartCoroutine(MoveToPosition(target, start, false, animSelectedTime, false));
        
            selected = true;
        }
    }

    public void onUnselected() {
        if (selected) {  
            boardMatrix[posGrid.x][posGrid.y].onUnselected();

            Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y + 1, 0));
            Vector3 start = target + animSelectedOffset;
            StartCoroutine(MoveToPosition(target, start, false, animSelectedTime, false));
        
            selected = false;
        }
    }

    public void changePossibleMove() {
        possibleMove = !possibleMove;
        if(possibleMove) {
            Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y + 1, 0));
            Vector3 target = start + animSelectedOffset/2;
            StartCoroutine(MoveToPosition(target, start, false, animSelectedTime/2, false));
        } else {
            Vector3 target= StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y + 1, 0));
            Vector3 start = transform.position;
            StartCoroutine(MoveToPosition(target, start, false, animSelectedTime/2, false));
        }
    }

    public virtual void Start() {

    }

    public virtual void Update() {
        if (active) {
            SpriteRenderer.enabled = true;
        } else {
            SpriteRenderer.enabled = false;
        }

        if(color == StaticScript.ListColor.black) {
            SpriteRenderer.sprite = blackSpr;
        } else if(color == StaticScript.ListColor.white) {
            SpriteRenderer.sprite = whiteSpr;
        }

        SpriteRenderer.sortingOrder = 20-(posGrid.y*2);

        bool upConcequence = false;
        foreach(Vector2Int u in moveUpConcequence) {
            if(u.y > posGrid.y+1) {
                upConcequence = true;
                break;
            }
        }

        if (selected || possibleMove || upConcequence) {
            SpriteRenderer.sortingOrder += 2;
        }
    }

    public int front() {
        if(color == ListColor.white){
            return -1;
        } else {
            return 1;
        }
    }

    public virtual bool canMove1(BoardScript square) {
        return false;
    }

    public virtual bool canMove2(BoardScript square) {
        return false;
    }

    public virtual bool canMove3(BoardScript square) {
        return false;
    }

    public virtual bool canMove4(BoardScript square) {
        return false;
    }

    public virtual void move1(Vector2Int select) { 

    }

    public virtual void move2(Vector2Int select) { 
            
    }

    public virtual void move3(Vector2Int select) { 
            
    }

    public virtual void move4(Vector2Int select) { 
            
    }

    public virtual void animMove(Vector2Int newPos, bool jump) {
        Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y + 1, 0));
        Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(newPos.x, newPos.y + 1, 0));
        
        StaticScript.pieceMatrix[newPos.x][newPos.y] = this;
        StaticScript.pieceMatrix[posGrid.x][posGrid.y] = null;

        float dist = Mathf.Abs(newPos.x - posGrid.x) + Mathf.Abs(newPos.y - posGrid.y);

        posGrid = newPos;
        
        StartCoroutine(MoveToPosition(target, start, jump, 0.15f * dist, false));
    }

    public static PieceScript isTargetingPiece(Vector2Int posTarget) {
        if (StaticScript.pieceMatrix[posTarget.x][posTarget.y] != null) {
            return StaticScript.pieceMatrix[posTarget.x][posTarget.y];
        } else {
            return null;
        }
    }

    public bool pieceIsInLine(Vector2Int posTarget, StaticScript.ListColor canTarget) {
        Vector2Int step = new Vector2Int(0, 0);

        if (posTarget == posGrid) {
            return false;
        }

        if(posTarget.x > posGrid.x) {
            step.x = 1;
        } else if(posTarget.x < posGrid.x) {
            step.x = -1;
        } 

        if(posTarget.y > posGrid.y) {
            step.y = 1;
        } else if(posTarget.y < posGrid.y) {
            step.y = -1;
        } 

        Vector2Int posAux = posGrid + step;
        
        while(posAux != posTarget) {
            if (StaticScript.pieceMatrix[posAux.x][posAux.y] != null) {
                return false;
            }

            posAux += step;
        }

        PieceScript pieceTargeting = isTargetingPiece(posTarget);
        if(pieceTargeting == null || pieceTargeting.color == canTarget){
            return true;
        }
        return false;
    }

    public virtual void addCd(bool pieceSt, int cdMove, int move) {
        if(pieceSt) {
            if(staminaPiece > 0) { 
                staminaPiece--;
            }
        }

        cd[move] = cdMove;
    }

    public void countKill(Vector2Int posTarget, ListColor colorTarget) {
        PieceScript target = isTargetingPiece(posTarget);
        if(!target) {
            if(target.color == colorTarget || colorTarget == ListColor.none) {
                pieceMatrix[target.posGrid.x][target.posGrid.y] = null;
                if(target.color == ListColor.white) {
                    whitePieces.Remove(target);
                } else { 
                    blackPieces.Remove(target);
                }

                TheDevilScript theDevilScript = FindFirstObjectByType<TheDevilScript>();
                TheHeavensScript theHeavensScript = FindFirstObjectByType<TheHeavensScript>();

                if(TheDevilScript.enable.x || TheDevilScript.enable.y) {
                     theDevilScript.killTrigger(this, target);
                }
               
                Destroy(target);
            }
        }
    }
}
