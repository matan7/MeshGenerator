using UnityEngine;

public abstract class MeshGeneratorBase : MonoBehaviour
{
    public abstract Mesh GenerateMesh();
}
public enum MeshType { Standard, CrossedQuad, Hexagon, Triangle }