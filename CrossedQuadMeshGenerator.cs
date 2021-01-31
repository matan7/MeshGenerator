using UnityEngine;

public class CrossedQuadMeshGenerator : MeshGenerator
{
    public Mesh Mesh;
    public readonly MeshType TypeOfMesh = MeshType.CrossedQuad;
    public float Width;
    public float Lenght;
    [Range(2, 181)] public int NumOfDivisions;

    private Vector3[] _vertices;
    private int _numberOfVertices;
    private int[] _trises;
    private Vector2[] _uv;

    public override Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        int i = _numberOfVertices = NumOfDivisions * NumOfDivisions + ((NumOfDivisions - 1) * (NumOfDivisions - 1));
        int rowQuadsNum = NumOfDivisions - 1;
        float uvStep = 1f / (NumOfDivisions - 1);
        float uvHalfStep = uvStep / 2;

        _vertices = new Vector3[_numberOfVertices];
        _uv = new Vector2[_numberOfVertices];
        _trises = new int[rowQuadsNum * rowQuadsNum * 12];

        float xPart = Width / rowQuadsNum;
        float zPart = Lenght / rowQuadsNum;
        float xHalfPart = Width / rowQuadsNum / 2;
        float zHalfPart = Lenght / rowQuadsNum / 2;
        for (int x = 0; x < rowQuadsNum; x++)
        {
            for (int z = 0; z < NumOfDivisions; z++)
            {
                _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, zPart * z);
                _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, uvStep * z);
            }
            for (int z = 0; z < rowQuadsNum; z++)
            {
                _vertices[_numberOfVertices - i] = new Vector3(xPart * x + xHalfPart, 1, zPart * z + zHalfPart);
                _uv[_numberOfVertices - i--] = new Vector2(uvStep * x + uvHalfStep, uvStep * z + uvHalfStep);
            }
        }
        for (int z = 0; z < NumOfDivisions; z++)
        {
            _vertices[_numberOfVertices - i] = new Vector3(xPart * rowQuadsNum, 0, zPart * z);
            _uv[_numberOfVertices - i--] = new Vector2(uvStep * rowQuadsNum, uvStep * z);
        }

        int columnsave = 0;
        int columnsave2 = 0;
        for (int row = 0; row < rowQuadsNum; row++)
        {
            for (int column = 0; column < rowQuadsNum; column++)
            {
                int n = column + columnsave2;
                int f = column * 12 + columnsave;
                _trises[f] = _trises[f + 10] = n;
                _trises[f + 1] = _trises[f + 3] = n + 1;
                _trises[f + 2] = _trises[f + 5] = _trises[f + 8] = _trises[f + 11] = n + NumOfDivisions;
                _trises[f + 4] = _trises[f + 6] = 2 * NumOfDivisions + n;
                _trises[f + 7] = _trises[f + 9] = 2 * NumOfDivisions + n - 1;
            }
            columnsave += 12 * rowQuadsNum;
            columnsave2 += NumOfDivisions * 2 - 1;
        }

        mesh.vertices = _vertices;
        mesh.triangles = _trises;
        mesh.uv = _uv;
        mesh.RecalculateNormals();
        Debug.Log($"Mesh generated. \n Number of vertex: {_numberOfVertices} \n Number of triangles: {rowQuadsNum * rowQuadsNum * 4}");
        return mesh;
    }
}
