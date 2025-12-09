using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct WallData : IComponentData
{
    public bool isDestroying; 
}

public struct WallTag : IComponentData
{

}