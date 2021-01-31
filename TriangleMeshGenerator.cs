using UnityEngine;

public class TriangleMeshGenerator : MeshGenerator
{
    private const float TRIANGLE_HEIGHT = 0.8660254f;

    public Mesh Mesh;
    public readonly MeshType TypeOfMesh = MeshType.Triangle;
    public bool IsEquilateral;
    public float Width;
    public float Lenght;
    [Range(2, 255)] public int NumOfDivisions;

    private Vector3[] _vertices;
    private int _numberOfVertices;
    private int[] _trises;
    private Vector2[] _uv;

    public override Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        bool isNumberOfDivisionsEven = NumOfDivisions % 2 == 0;
        int i = _numberOfVertices = NumOfDivisions * NumOfDivisions + (isNumberOfDivisionsEven ? NumOfDivisions / 2 : (NumOfDivisions - 1) / 2);
        int rowQuadsNum = NumOfDivisions - 1;
        float uvStep = 1f / (NumOfDivisions - 1);
        float uvHalfStep = uvStep / 2;
        _vertices = new Vector3[_numberOfVertices];
        _uv = new Vector2[_numberOfVertices];

        float xPart = IsEquilateral ? Width / rowQuadsNum * TRIANGLE_HEIGHT : Width / rowQuadsNum;
        float zPart = Lenght / rowQuadsNum;
        float zHalfPart = zPart / 2f;

        for (int x = 0; x < NumOfDivisions; x++)
        {
            if(x % 2 == 0)
                for (int z = 0; z < NumOfDivisions; z++)
                {
                    _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, zPart * z);
                    _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, uvStep * z);
                }
            else
            {
                _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, 0);
                _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, 0);
                for (int z = 0; z < rowQuadsNum; z++)
                {
                    _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, zPart * z + zHalfPart);
                    _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, uvStep * z + uvHalfStep);
                }
                _vertices[_numberOfVertices - i] = new Vector3(xPart * x, 0, _vertices[_numberOfVertices - i - 1].z + zHalfPart);
                _uv[_numberOfVertices - i--] = new Vector2(uvStep * x, _uv[_numberOfVertices - i - 2].y + uvHalfStep);
            }
        }

        _trises = new int[(NumOfDivisions * 2 - 1) * rowQuadsNum * 3];
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
                _trises[f + 2] = _trises[f + 5] = n + NumOfDivisions + 1;
                _trises[f + 4] = n + NumOfDivisions + 2;
            }
            int colTemp = columnsave2;
            columnsave += 6 * rowQuadsNum;
            columnsave2 += NumOfDivisions;
            if (row % 2 == 0)
            {
                _trises[columnsave] = colTemp;
                _trises[columnsave + 1] = colTemp + NumOfDivisions + 1;
                _trises[columnsave + 2] = colTemp + NumOfDivisions;
                columnsave += 3;
            }
            else
            {
                _trises[columnsave] = columnsave2 - 1;
                _trises[columnsave + 1] = columnsave2;
                _trises[columnsave + 2] = columnsave2 + NumOfDivisions;
                columnsave += 3;
                columnsave2 += 1;
            }
        }
        mesh.vertices = _vertices;
        mesh.triangles = _trises;
        mesh.uv = _uv;
        mesh.RecalculateNormals();
        Debug.Log($"Mesh generated. \n Number of vertex: {_numberOfVertices} \n Number of triangles: {(NumOfDivisions * 2 - 1) * rowQuadsNum}");
        return mesh;
    }
}