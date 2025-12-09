using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct BallsShotData : IComponentData
{
    public int ballsNumber;
    public float spawnInterval;
    public float nextSpawnTime;
    public float3 direction;
}