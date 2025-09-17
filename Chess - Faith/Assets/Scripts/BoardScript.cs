using System.Collections;
using UnityEngine;
using static StaticScript;

public class BoardScript : MonoBehaviour {
    [SerializeField] Sprite black_spr;
    [SerializeField] Sprite white_spr;

    public StaticScript.ListColor color;

    [SerializeField] SpriteRenderer SpriteRenderer;

    public Vector2Int posGrid;

    public bool active = false;

    public bool selected = false;
    public bool possibleMove = false;


    public IEnumerator MoveToPosition(Vector3 target, Vector3 start, float duration, bool appear) {
        transform.position = start;
        float elapsed = 0f;

        if (target.y > start.y) {
            moveUpConcequence.Add(posGrid);
        } else {
            moveUpConcequence.Remove(posGrid);
        }

        float waitToStart = 0;
        if(appear) {
            if(posGrid.y % 2 == 1) {
                waitToStart = posGrid.y * duration * 8 + posGrid.x * duration;
            } else {
                waitToStart = posGrid.y * duration * 8 + (9 - posGrid.x) * duration;
            }

            if(posGrid.y > 4 || (posGrid.y == 4 && posGrid.x < 5)) {
                if(posGrid.y % 2 == 1) {
                    waitToStart = (8 - posGrid.y) * duration * 8 + (9 - posGrid.x) * duration;
                } else {
                    waitToStart = (8 - posGrid.y) * duration * 8 + posGrid.x * duration;
                }
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

        while (elapsed < duration) {
            float t = elapsed / duration;
            t = (t < 0.5f) ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2; 

            transform.position = Vector3.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }

    public void onSelected() {
        if (!selected) {  
            Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 target = start + animSelectedOffset;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime, false));
        
            selected = true;
        }
    }

    public void onUnselected() {
        if (selected) {  
            Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 start = target + animSelectedOffset;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime, false));
        
            selected = false;
        }
    }

    public void changePossibleMove() {
        possibleMove = !possibleMove;
        if(possibleMove) {
            Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 target = start + animSelectedOffset/2;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime/2, false));
        } else {
            Vector3 target= StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0));
            Vector3 start = transform.position;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime/2, false));
        }

        if(pieceMatrix[posGrid.x][posGrid.y] != null) {
            pieceMatrix[posGrid.x][posGrid.y].changePossibleMove();
        }
    }

    private void OnMouseEnter() {
        if (possibleMove) {
            Vector3 start = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0)) + animSelectedOffset/2;
            Vector3 target = start + animSelectedOffset/2;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime / 2, false));
        }
    }

    private void OnMouseExit() {
        if (possibleMove) {
            Vector3 target = StaticScript.grid.CellToWorld(new Vector3Int(posGrid.x, posGrid.y, 0)) + animSelectedOffset/2;
            Vector3 start = target + animSelectedOffset/2;
            StartCoroutine(MoveToPosition(target, start, animSelectedTime / 2, false));
        }
    }

    private void OnMouseDown() {
        if (possibleMove) {
            Debug.Log(posGrid);
            if (GameManagerScript.selectedMove == 1) {
                GameManagerScript.selectedPiece.move1(posGrid);
            } else if (GameManagerScript.selectedMove == 2) {
                GameManagerScript.selectedPiece.move2(posGrid);
            } else if (GameManagerScript.selectedMove == 3) {
                GameManagerScript.selectedPiece.move3(posGrid);
            } else if (GameManagerScript.selectedMove == 4) {
                GameManagerScript.selectedPiece.move4(posGrid);
            }  
        }
    }

    private void Update() {
        if (active) {
            SpriteRenderer.enabled = true;
        } else {
            SpriteRenderer.enabled = false;
        }

        if(color == StaticScript.ListColor.black) {
            SpriteRenderer.sprite = black_spr;
        } else if(color == StaticScript.ListColor.white) {
            SpriteRenderer.sprite = white_spr;
        }

        bool upConcequence = false;
        foreach(Vector2Int u in moveUpConcequence) {
            if(u.y > posGrid.y) {
                upConcequence = true;
                break;
            }
        }

        SpriteRenderer.sortingOrder = 20-posGrid.y*2 - 3;

        if (selected || possibleMove || upConcequence) {
            SpriteRenderer.sortingOrder += 2;
        }
    }
}
