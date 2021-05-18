

using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        var locations = new List<Vector2Int>();

        int forwardDirection = GameManager.instance.currentPlayer.forward;

        Vector2Int forward = new Vector2Int(gridPoint.x + 1, gridPoint.y - 2);
        if (GameManager.instance.PieceAtGrid(forward) == false)
        {
            locations.Add(forward);
        }
        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + 2);
        if (GameManager.instance.PieceAtGrid(forwardRight) == false)
        {
            locations.Add(forwardRight);
        }

        Vector2Int forwardLeft = new Vector2Int(gridPoint.x + 2, gridPoint.y - 1);
        if (GameManager.instance.PieceAtGrid(forwardLeft) == false)
        {
            locations.Add(forwardLeft);
        }
        Vector2Int left = new Vector2Int(gridPoint.x + 2, gridPoint.y + 1);
        if (GameManager.instance.PieceAtGrid(left) == false)
        {
            locations.Add(left);
        }
        Vector2Int right = new Vector2Int(gridPoint.x - 1, gridPoint.y - 2);
        if (GameManager.instance.PieceAtGrid(right) == false)
        {
            locations.Add(right);
        }
        Vector2Int backLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + 2);
        if (GameManager.instance.PieceAtGrid(backLeft) == false)
        {
            locations.Add(backLeft);
        }
        Vector2Int backRight = new Vector2Int(gridPoint.x - 2, gridPoint.y - 1);
        if (GameManager.instance.PieceAtGrid(backRight) == false)
        {
            locations.Add(backRight);
        }
        Vector2Int back = new Vector2Int(gridPoint.x - 2, gridPoint.y + 1);
        if (GameManager.instance.PieceAtGrid(back) == false)
        {
            locations.Add(back);
        }
        Vector2Int forwardAttc = new Vector2Int(gridPoint.x + 1, gridPoint.y - 2);
        if (GameManager.instance.PieceAtGrid(forwardAttc))
        {
            locations.Add(forwardAttc);
        }

        Vector2Int forwardRightAttc = new Vector2Int(gridPoint.x + 1, gridPoint.y + 2);
        if (GameManager.instance.PieceAtGrid(forwardRightAttc))
        {
            locations.Add(forwardRightAttc);
        }

        Vector2Int forwardLeftAttc = new Vector2Int(gridPoint.x + 2, gridPoint.y - 1);
        if (GameManager.instance.PieceAtGrid(forwardLeftAttc))
        {
            locations.Add(forwardLeftAttc);
        }
        Vector2Int leftAttc = new Vector2Int(gridPoint.x + 2, gridPoint.y + 1);
        if (GameManager.instance.PieceAtGrid(leftAttc))
        {
            locations.Add(leftAttc);
        }
        Vector2Int rightAttc = new Vector2Int(gridPoint.x - 1, gridPoint.y - 2);
        if (GameManager.instance.PieceAtGrid(rightAttc))
        {
            locations.Add(rightAttc);
        }
        Vector2Int backAttc = new Vector2Int(gridPoint.x - 2, gridPoint.y + 1);
        if (GameManager.instance.PieceAtGrid(backAttc))
        {
            locations.Add(backAttc);
        }
        Vector2Int backLeftAttc = new Vector2Int(gridPoint.x - 1, gridPoint.y + 2);
        if (GameManager.instance.PieceAtGrid(backLeftAttc))
        {
            locations.Add(backLeftAttc);
        }
        Vector2Int backRightAttc = new Vector2Int(gridPoint.x - 2, gridPoint.y - 1);
        if (GameManager.instance.PieceAtGrid(backRightAttc))
        {
            locations.Add(backRightAttc);
        }
        return locations;
    }
}