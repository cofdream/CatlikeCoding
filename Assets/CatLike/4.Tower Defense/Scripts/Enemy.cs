using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform model;

    EnemyFactory originFactory;

    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress;

    float progressFactor;

    float speed;
    float pathOffest;

    Direction direction;
    DirectionChange directionChange;
    float directionAngelFrom, directionAngelTo;
    public EnemyFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory");
            originFactory = value;
        }
    }

    public void Initialize(float scale, float speed, float pathOffest)
    {
        model.localScale = new Vector3(scale, scale, scale);
        this.speed = speed;
        this.pathOffest = pathOffest;
    }

    public bool GameUpdate()
    {
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                OriginFactory.Reclaim(this);
                return false;
            }

            progress = (progress - 1) / progressFactor;

            PrepareNextState();

            progress *= progressFactor;
        }

        if (directionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngelFrom, directionAngelTo, progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }

        return true;
    }

    public void SpawnOn(GameTile tile)
    {
        Debug.Assert(tile.NextTileOnePath != null, "Nowhere to go!", this);
        tileFrom = tile;
        tileTo = tile.NextTileOnePath;

        progress = 0f;

        PrepareIntro();
    }

    private void PrepareIntro()
    {
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;

        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngelFrom = directionAngelTo = direction.GetAngle();

        model.localPosition = new Vector3(pathOffest, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }
    private void PrepareNextState()
    {
        tileFrom = tileTo;
        tileTo = tileTo.NextTileOnePath;

        positionFrom = positionTo;

        if (tileTo == null)
        {
            PrepareOutro();
            return;
        }

        positionTo = tileFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngelFrom = directionAngelTo;

        switch (directionChange)
        {
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;

            default: PrepareTurnAround(); break;
        }
    }

    private void PrepareForward()
    {
        transform.localRotation = direction.GetRotation();
        directionAngelTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffest, 0f);
        progressFactor = speed;
    }
    private void PrepareTurnRight()
    {
        directionAngelTo = directionAngelFrom + 90f;
        model.localPosition = new Vector3(pathOffest - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.25f * (0.5f - pathOffest));
    }
    private void PrepareTurnLeft()
    {
        directionAngelTo = directionAngelFrom - 90f;
        model.localPosition = new Vector3(pathOffest + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * 0.25f * (0.5f + pathOffest));
    }
    private void PrepareTurnAround()
    {
        directionAngelTo = directionAngelFrom + (pathOffest < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffest, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVector();
        progressFactor = speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffest), 0.2f));
    }

    private void PrepareOutro()
    {
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngelTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffest, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }
}