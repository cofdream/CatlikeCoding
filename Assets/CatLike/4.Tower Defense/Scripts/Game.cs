using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Vector2Int boardSize = new Vector2Int(11, 11);
    [SerializeField] GameBoard board;
    [SerializeField] GameTileContentFactory gameTileContentFactory;
    [SerializeField] EnemyFactory enemyFactory;
    [SerializeField, Range(0.1f, 10f)] float spawnSpeed = 1f;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    float spawnProgress;


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

        spawnProgress += spawnSpeed * Time.deltaTime;
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;

            SpawnEnemy();
        }
    }
    private void HandleTouch()
    {
        GameTile gameTile = board.GetTile(TouchRay);
        if (gameTile != null)
        {
            board.ToggleWall(gameTile);
        }
    }
    private void HandleAlternativeTouch()
    {
        GameTile gameTile = board.GetTile(TouchRay);
        if (gameTile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(gameTile);
            }
            else
            {
                board.ToggleSpawnPoint(gameTile);
            }
        }
    }

    private void SpawnEnemy() { }
}
