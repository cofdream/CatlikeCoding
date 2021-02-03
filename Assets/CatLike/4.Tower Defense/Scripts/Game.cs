using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] GameBoard board;
    [SerializeField] GameTileContentFactory gameTileContentFactory;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        board.Initialize(boardSize, gameTileContentFactory);
        board.ShowGrid = true;
    }
    private void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternativeTouch();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            board.ShowPaths = !board.ShowPaths;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            board.ShowGrid = !board.ShowGrid;
        }
    }
    private void HandleTouch()
    {
        GameTile gameTile = board.GetTile(TouchRay);
        if (gameTile != null)
        {
            board.ToggleDestination(gameTile);
        }
    }
    private void HandleAlternativeTouch()
    {
        GameTile gameTile = board.GetTile(TouchRay);
        if (gameTile != null)
        {
            board.ToggleWall(gameTile);
        }
    }
}
