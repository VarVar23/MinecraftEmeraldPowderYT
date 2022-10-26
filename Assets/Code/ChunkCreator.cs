using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkCreator : MonoBehaviour
{
    private const int _chunkHeight = 128;
    private const int _chunkWidth = 10;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private List<Vector3> _verticles = new List<Vector3>();
    private List<int> _triangles = new List<int>();

    private int[,,] Blocks = new int[_chunkWidth, _chunkHeight, _chunkWidth];

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();


        //Заполнение блоками
        for (int x = 0; x < _chunkWidth; x++)
        {
            for (int y = 0; y < _chunkHeight; y++)
            {
                for (int z = 0; z < _chunkWidth; z++)
                {
                    if(y < 15)
                        Blocks[x, y, z] = 1;
                    if(y >= 15 && y < 20 && z % 3 == 0)
                        Blocks[x, y, z] = 1;

                    if (y >= 15 && y < 20 && x % 3 == 0)
                        Blocks[x, y, z] = 1;
                }
            }
        }

        for (int x = 0; x < _chunkWidth; x++)
        {
            for(int y = 0; y < _chunkHeight; y++)
            {
                for(int z = 0; z < _chunkWidth; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }


        _mesh.vertices = _verticles.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _meshFilter.mesh = _mesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        if (Blocks[x, y, z] == 0) return;

        var position = new Vector3Int(x, y, z);

        if (CheckBlock(position + Vector3Int.back) == 0) Front(position);
        if (CheckBlock(position + Vector3Int.forward) == 0) Back(position);
        if (CheckBlock(position + Vector3Int.up) == 0) Up(position);
        if (CheckBlock(position + Vector3Int.down) == 0) Down(position);
        if (CheckBlock(position + Vector3Int.right) == 0) Right(position);
        if (CheckBlock(position + Vector3Int.left) == 0) Left(position);
    }

    private int CheckBlock(Vector3Int pos)
    {
        if(pos.x >= 0 && pos.x < _chunkWidth &&
           pos.y >= 0 && pos.y < _chunkHeight &&
           pos.z >= 0 && pos.z < _chunkWidth)
        {
            return Blocks[pos.x, pos.y, pos.z];
        }

        return 0;
    }
    private void Front(Vector3 position)
    {
        _verticles.Add(new Vector3(1, 0, 0) + position);
        _verticles.Add(new Vector3(0, 0, 0) + position);
        _verticles.Add(new Vector3(0, 1, 0) + position);
        _verticles.Add(new Vector3(1, 1, 0) + position);

        AddTriangles(false);
    }
    private void Back(Vector3 position)
    {
        _verticles.Add(new Vector3(1, 0, 1) + position);
        _verticles.Add(new Vector3(0, 0, 1) + position);
        _verticles.Add(new Vector3(0, 1, 1) + position);
        _verticles.Add(new Vector3(1, 1, 1) + position);

        AddTriangles(true);
    }
    private void Up(Vector3 position)
    {
        _verticles.Add(new Vector3(1, 1, 0) + position);
        _verticles.Add(new Vector3(0, 1, 0) + position);
        _verticles.Add(new Vector3(0, 1, 1) + position);
        _verticles.Add(new Vector3(1, 1, 1) + position);

        AddTriangles(false);
    }
    private void Down(Vector3 position)
    {
        _verticles.Add(new Vector3(1, 0, 0) + position);
        _verticles.Add(new Vector3(0, 0, 0) + position);
        _verticles.Add(new Vector3(0, 0, 1) + position);
        _verticles.Add(new Vector3(1, 0, 1) + position);

        AddTriangles(true);
    }
    private void Right(Vector3 position)
    {
        _verticles.Add(new Vector3(1, 0, 1) + position);
        _verticles.Add(new Vector3(1, 0, 0) + position);
        _verticles.Add(new Vector3(1, 1, 0) + position);
        _verticles.Add(new Vector3(1, 1, 1) + position);

        AddTriangles(false);
    }
    private void Left(Vector3 position)
    {
        _verticles.Add(new Vector3(0, 0, 1) + position);
        _verticles.Add(new Vector3(0, 0, 0) + position);
        _verticles.Add(new Vector3(0, 1, 0) + position);
        _verticles.Add(new Vector3(0, 1, 1) + position);

        AddTriangles(true);
    }

    private void AddTriangles(bool rotate)
    {
        if(rotate)
        {
            _triangles.Add(_verticles.Count - 2);
            _triangles.Add(_verticles.Count - 3);
            _triangles.Add(_verticles.Count - 4);

            _triangles.Add(_verticles.Count - 4);
            _triangles.Add(_verticles.Count - 1);
            _triangles.Add(_verticles.Count - 2);
        }
        else
        {
            _triangles.Add(_verticles.Count - 4);
            _triangles.Add(_verticles.Count - 3);
            _triangles.Add(_verticles.Count - 2);

            _triangles.Add(_verticles.Count - 2);
            _triangles.Add(_verticles.Count - 1);
            _triangles.Add(_verticles.Count - 4);
        }
    }    
}
