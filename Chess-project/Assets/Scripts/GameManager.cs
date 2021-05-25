
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;

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

    private GameObject[,] pieces;
    public string[,] gameBoard;
    private List<GameObject> movedPawns;

    public GameObject[] freds;

    public Player white;
    public Player black;
    public Player currentPlayer;
    public Player otherPlayer;
    public static bool CamPlace = false;
    public Camera firstPersonCamera;
    public Camera overheadCamera;


    public GameObject[,] getPieces()
    {
        return pieces;
    }

    

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        pieces = new GameObject[8, 8];
        movedPawns = new List<GameObject>();
        ;

        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white;
        otherPlayer = black;

        InitialSetup();
        fixCamera();
    }

    private void InitialSetup()
    {
        
        AddPiece(whiteRook, white, 0, 0);
        AddPiece(whiteKnight, white, 1, 0);
        AddPiece(whiteBishop, white, 2, 0);
        AddPiece(whiteQueen, white, 3, 0);
        AddPiece(whiteKing, white, 4, 0);
        AddPiece(whiteBishop, white, 5, 0);
        AddPiece(whiteKnight, white, 6, 0);
        AddPiece(whiteRook, white, 7, 0);
        
        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
        }
        
        AddPiece(blackRook, black, 0, 7);
        AddPiece(blackKnight, black, 1, 7);
        AddPiece(blackBishop, black, 2, 7);
        AddPiece(blackQueen, black, 3, 7);
        AddPiece(blackKing, black, 4, 7);
        AddPiece(blackBishop, black, 5, 7);
        AddPiece(blackKnight, black, 6, 7);
        AddPiece(blackRook, black, 7, 7);
        
        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
        }
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;

        bool notFred = true;
        int count = 0;
        while (notFred)
        {
            if (freds[count] ==  null)
            {
                freds[count] = pieceObject;
                notFred = false;
               
            }
            count++;
        }
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));


        
        
        return locations;
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }
        
        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
    }

    public void PawnMoved(GameObject pawn)
    {
        movedPawns.Add(pawn);
    }

    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }
    
      

    
    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Debug.Log(currentPlayer.name + " wins!");
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
        }
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;

        for (int i = 0; i < 32; i++)
        {
            //Vector2Int gridPoint2 = Geometry.GridFromPoint(freds[i]);
            //if (gridPoint2.x == gridPoint.x && gridPoint2.y == gridPoint.y) ;
            
        }

        Destroy(pieceToCapture);
        List<GameObject> ps2 = otherPlayer.getPieces();
        ps2.Remove(pieceToCapture);

    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
            
        }

        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null) {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }
 
    public void PromoteBlack(GameObject piece, Vector2Int gridPoint)
    {
        GameObject place = PieceAtGrid(gridPoint);
        if (piece.GetComponent<Piece>().type == PieceType.Pawn)
        {
            if (blackPawn)
            {
                

                Destroy(place);
                AddPiece(blackQueen, black, gridPoint.x, gridPoint.y);

            }
        }
            }
    public void PromoteWhite(GameObject piece, Vector2Int gridPoint)
    {
        GameObject place = PieceAtGrid(gridPoint);
        if (piece.GetComponent<Piece>().type == PieceType.Pawn)
        {
            if (whitePawn)
            {
                

                Destroy(place);
                AddPiece(whiteQueen, white, gridPoint.x, gridPoint.y);

            }
        }
    }
    public void EasterEgg(Vector2Int gridPoint)
    {
        Destroy(PieceAtGrid(gridPoint));
        AddPiece(whiteQueen, white, gridPoint.x, gridPoint.y);
    }
    public void DeletePeice(Vector2Int gridPoint)
    {
        Destroy(PieceAtGrid(gridPoint));
    }

    int cameraCount = 0;
    public void fixCamera()
    {
        ShowFirstPersonView();
    }
    public void NextPlayer()
    {
        cameraCount++;
        
        if ((cameraCount % 2) == 1)
        {
            
            ShowOverheadView();
        }
        else
        {
            
            ShowFirstPersonView();
        }
        Player tempPlayer = currentPlayer;
        bool isInCheck = GameManager.instance.OtherKingInCheck();

        if (isInCheck)
        {
            Debug.Log("check");

        }

        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
        



    }

    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView()
    {
        
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
        CamPlace = true;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView()
    {
        
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
        CamPlace = false;
    }



    public bool check()
    {
        //Vector2Int piecePos = Geometry.GridFromPoint(piece.transform.position);
        gameBoard = new string[8, 8];

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {

                gameBoard[x, y] = "none";
            }
        }


        List<GameObject> ps;
        List<GameObject> ps2;
        ps = currentPlayer.getPieces();
        ps2 = otherPlayer.getPieces();

        List<GameObject> ps3 = new List<GameObject>();

        for (int i = 0; i < ps2.ToArray().Length; i++)
        {
            ps3.Add(ps2[i]);
        }
        for (int i = 0; i < ps.ToArray().Length; i++)
        {
            ps3.Add(ps[i]);
        }

        Vector2Int kingPos = new Vector2Int();

        for (int i = 0; i < ps3.ToArray().Length; i++)
        {
            GameObject a = ps3[i];
            Vector2Int aPos = Geometry.GridFromPoint(a.transform.position);
            if (ps3[i].name.Contains("King"))
            {
                if (ps3[i].name.Contains("Black") && currentPlayer.name == "black") {
                    kingPos = Geometry.GridFromPoint(ps3[i].transform.position);

                }
                if (ps3[i].name.Contains("White") && currentPlayer.name == "white")
                {
                    kingPos = Geometry.GridFromPoint(ps3[i].transform.position);

                }

            }

            gameBoard[aPos.x, aPos.y] = ps3[i].name;

        }

        // fake move piece
        
        // gameBoard[move.x, move.y] = gameBoard[piecePos.x, piecePos.y];
       
        
        //gameBoard[piecePos.x, piecePos.y] = "none";
   



        Vector2Int[] directs =
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,1),
            new Vector2Int(-1,-1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1)
        };

        foreach(Vector2Int dir in directs)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int newPos = new Vector2Int();
                newPos = kingPos + dir * i;
                if (newPos.x > 7 || newPos.x < 0 || newPos.y > 7 || newPos.y < 0)
                {
                    continue;
                }
                if (gameBoard[newPos.x, newPos.y] != "none")
                {
                    string name = gameBoard[newPos.x, newPos.y].ToLower();
                    if (name.Contains(currentPlayer.name))
                    {
                        
                        break;

                    }
                    else // other team
                    {
                        if (name.Contains("rook") && System.Math.Abs(dir.x) != System.Math.Abs(dir.y))
                        {
                            return true;
                        }
                        if (name.Contains("bishop") && System.Math.Abs(dir.x) == System.Math.Abs(dir.y))
                        {
                            return true;
                        }
                        if (name.Contains("queen"))
                        {
                            return true;
                        }
                        if (name.Contains("king") && i == 1)
                        {
                            return true;
                        }
                        if (name.Contains("pawn") && i == 1) // MAKE FORWARD DIR only
                        {
                            return true;
                        }
                    }
                }
            }
           
        }

        Vector2Int[] directs2 =
        {
            new Vector2Int(1,2),
            new Vector2Int(1,-2),
            new Vector2Int(-1,2),
            new Vector2Int(-1,-2),
            new Vector2Int(2,1),
            new Vector2Int(2,-1),
            new Vector2Int(-2,1),
            new Vector2Int(-2,-1)
        };

        foreach (Vector2Int dir in directs2)
        {
            Vector2Int newPos = new Vector2Int();
            newPos = kingPos + dir;
            if (newPos.x > 7 || newPos.x < 0 || newPos.y > 7 || newPos.y < 0)
            {
                continue;
            }
            string name = gameBoard[newPos.x, newPos.y].ToLower();
            if (name.Contains(otherPlayer.name))
            {
                if (name.Contains("knight"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool OtherKingInCheck()
    {


        List<GameObject> ps = currentPlayer.getPieces();
        List<GameObject> ps2 = otherPlayer.getPieces();
        GameObject king = new GameObject();
        Vector2Int kingPos = new Vector2Int();
        for (int i = 0; i < ps2.ToArray().Length; i++)
        {
            if (i < ps2.ToArray().Length)
            {

                if (ps2[i].name.Contains("King"))
                {
                   
                    king = ps2[i];
                    
                    kingPos = Geometry.GridFromPoint(king.transform.position);
                    
                }


            }

        }

        List<Vector2Int> moveLocations4 = new List<Vector2Int>();
        List<Vector2Int> moveLocationsThatWorks = new List<Vector2Int>();

        for (int i = 0; i < ps.ToArray().Length; i++)
        {
            moveLocations4 = MovesForPiece(ps[i]);

            int counter = 0;
            foreach (Vector2Int v in moveLocations4)
            {
                if (counter < moveLocations4.ToArray().Length)
                {
                    moveLocationsThatWorks.Add(v);
                }
                counter = counter + 1;
            }


        }





        bool incheck = false;
        if (moveLocationsThatWorks.Contains(kingPos))
        {

          
            incheck = true;
        }
        return incheck;
    }
}
    