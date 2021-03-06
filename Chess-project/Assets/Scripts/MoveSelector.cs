
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;

    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private Vector2Int posOld;
    private List<Vector2Int> moveLocations;
    
    private List<GameObject> locationHighlights;

   
    void Start ()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);

        
    }

    void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);
            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            if (Input.GetMouseButtonDown(0))
            {
               
                // Reference Point 2: check for valid move location
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
                    if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                    {
                        
                        CancelMove();
                    }
                }
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }




                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameObject pieceObject = movingPiece;
                    Piece piece = pieceObject.GetComponent<Piece>();

                    


                    GameObject[,] pieces = GameManager.instance.getPieces();

                   
                    //  addsd
                    
                        GameManager.instance.Move(movingPiece, gridPoint);

                        if (gridPoint.y == 0)
                        {
                            GameManager.instance.PromoteBlack(movingPiece, gridPoint);
                        }
                        if (gridPoint.y == 7)
                        {
                            GameManager.instance.PromoteWhite(movingPiece, gridPoint);
                        }
                    
                    
                    
                }
                
                else
                {
                    
                    
                    
                        GameManager.instance.CapturePieceAt(gridPoint);

                        GameManager.instance.Move(movingPiece, gridPoint);
                    
                    
                    

                }


                bool isInCheck2 = GameManager.instance.check();
                if (isInCheck2)
                {
                    Debug.Log("CANT MOVE INTO CHECK");
                    GameManager.instance.Move(movingPiece, posOld);

                }
                else
                {
                    // Reference Point 3: capture enemy piece here later
                    ExitState();
                }

            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
        
    }

 
    private void CancelMove()
    {
        this.enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        GameManager.instance.DeselectPiece(movingPiece);
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        posOld = Geometry.GridFromPoint(movingPiece.transform.position);
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }

    }

    private void ExitState()
    {
        this.enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        GameManager.instance.NextPlayer();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
        
    }
}
