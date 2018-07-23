using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipTileLoader : MonoBehaviour
{
    [SerializeField]
    private Tilemap shipMap;

    [SerializeField]
    private List<TileBase> tilePalette;

    [SerializeField]
    private List<GameObject> tileObjects;

    // Use this for initialization
    void Start()
    {
        FileStream wFile = new FileStream("savedShipTiles.txt", FileMode.Open);
        byte[,] arr = ReadTileMapFromFile(wFile);
        LoadFromArray(arr, shipMap);
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected byte[,] ReadTileMapFromFile(FileStream file)
    {
        byte[] singleByte = new byte[1];

        // read x dimension
        file.Read(singleByte, 0, 1);
        int sizeX = (int)singleByte[0];

        // read y dimension
        file.Read(singleByte, 0, 1);
        int sizeY = (int)singleByte[0];

        byte[,] tiles = new byte[sizeY, sizeX];
        byte[] line = new byte[sizeX];

        for (int y = 0; y < sizeY; y++)
        {
            file.Read(line, 0, sizeX);
            for (int x = 0; x < sizeX; x++)
                tiles[y, x] = line[x];
        }
        file.Close();

        return tiles;
    }

    protected void WriteTileMapToFile(FileStream file, Tilemap writtenMap)
    {
        BoundsInt bounds = writtenMap.cellBounds;
        TileBase[] allTiles = writtenMap.GetTilesBlock(bounds);

        file.WriteByte((byte)bounds.size.x);
        file.WriteByte((byte)bounds.size.y);

        int len = allTiles.Length;

        for (int i = 0; i < len; i++)
            file.WriteByte(TileToByte(allTiles[i]));
        file.Close();
    }


    private TileBase ByteToTile(byte b)
    {
        if (b > tilePalette.Capacity || b < 0)
            return null;
        else
            return tilePalette[b];
    }


    private byte TileToByte(TileBase tb)
    {
        return (byte)tilePalette.IndexOf(tb);
    }


    protected void LoadFromArray(byte[,] arr, Tilemap targetMap)
    {
        Vector3Int pos = new Vector3Int();
        int yLen = arr.GetUpperBound(0);
        int xLen = arr.GetUpperBound(1);

        targetMap.ClearAllTiles();
        for (int y = yLen - 1; y >= 0; y--)
        {
            for (int x = 0; x < xLen; x++)
            {
                pos.Set(x - xLen / 2, y - yLen / 2, 0);
                byte b = arr[y, x];
                targetMap.SetTile(pos, ByteToTile(b));

                if (b != 255)
                {
                    GameObject tileLogicObjectPrefab = tileObjects[b];

                    if (tileLogicObjectPrefab != null)
                    {
                        GameObject tileLogicObject = Instantiate(tileLogicObjectPrefab);
                        tileLogicObject.transform.SetParent(targetMap.transform);
                        Vector3 tileMiddlePos = new Vector3(
                            targetMap.transform.position.x + pos.x + 0.5f,
                            targetMap.transform.position.y + pos.y + 0.5f, 0);
                        tileLogicObject.transform.position = tileMiddlePos;

                    }
                }
            }
        }
    }

    protected byte[,] SaveToArray(Tilemap savedMap)
    {
        BoundsInt bounds = savedMap.cellBounds;
        TileBase[] allTiles = savedMap.GetTilesBlock(bounds);

        int yLen = bounds.size.y;
        int xLen = bounds.size.x;

        byte[,] tileBytes = new byte[yLen, xLen];

        for (int y = 0; y < yLen; y++)
            for (int x = 0; x < xLen; x++)
                tileBytes[y, x] = TileToByte(allTiles[x + y * xLen]);

        return tileBytes;
    }
}
