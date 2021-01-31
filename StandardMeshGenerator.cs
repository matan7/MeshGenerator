using UnityEngine;

public class StandardMeshGenerator : MeshGenerator
{
    public Mesh Mesh;
    public readonly MeshType TypeOfMesh = MeshType.Standard;
    public float Width;
    public float Lenght;
    [Range(2, 256)] public int NumOfDivisions;

    private Vector3[] _vertices;
    private int _numberOfVertices;
    private int[] _trises;
    private Vector2[] _uv;

    public override Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        int i = _numberOfVertices = NumOfDivisions * NumOfDivisions;
        int rowQuadsNum = NumOfDivisions - 1;
        float uvStep = 1f / (NumOfDivisions - 1);

        _vertices = new Vector3[_numberOfVertices];
        _uv = new Vector2[_numberOfVertices];
        _trises = new int[rowQuadsNum * rowQuadsNum * 6];

        float xPart = Width / rowQuadsNum;
        float zPart = Lenght / rowQuadsNum;
        for (int x = 0; x < NumOfDivisions; x++)
        {
            for (int z = 0; z < NumOfDivisions; z++)
            {
                _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, zPart * z);
                _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, uvStep * z);
            }
        }

        int columnsave = 0;
        int columnsave2 = 0;
        for (int row = 0; row < rowQuadsNum; row++)
        {
            for (int column = 0; column < rowQuadsNum; column++)
            {
                int n = column + columnsave2;
                int f = column * 6 + columnsave;
                _trises[f] = n;
                _trises[f + 1] = _trises[f + 3] = n + 1;
                _trises[f + 2] = _trises[f + 5] = n + NumOfDivisions;
                _trises[f + 4] = n + NumOfDivisions + 1;
            }
            columnsave += 6 * rowQuadsNum;
            columnsave2 += NumOfDivisions;
        }

        mesh.vertices = _vertices;
        mesh.triangles = _trises;
        mesh.uv = _uv;
        mesh.RecalculateNormals();
        Debug.Log($"Mesh generated. \n Number of vertex: {_numberOfVertices} \n Number of triangles: {rowQuadsNum * rowQuadsNum * 2}");
        return mesh;
    }
}