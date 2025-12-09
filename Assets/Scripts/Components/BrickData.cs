using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct BrickData : IComponentData
{
    public int lives;
}

public struct BrickTag : IComponentData
{

}
