using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct BrickData : IComponentData
{
    public int maxLives;
    public int currentLives;
}

public struct BrickTag : IComponentData
{

}
