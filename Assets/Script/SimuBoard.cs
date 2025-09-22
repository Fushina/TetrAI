using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SimuBoard
{
    public int[,] grid;
    public int Width, Height;

    public SimuBoard(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        grid = new int[Width, Height];
    }

    //Le copie fonctionne 
    public void CopierTile(Tilemap tilemap)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector3Int cellPos = new Vector3Int((-Width / 2) + x, (-Height / 2) + y, 0);
                if (tilemap.HasTile(cellPos))
                {
                    grid[x, y] = 1;
                }
                else
                {
                    grid[x, y] = 0;
                }
            }
        }
    }

    //print fonctionne
    public void printSimu()
    {
        string line = "";
        for (int y = Height - 1; y >= 0; y--)
        {

            for (int x = 0; x < Width; x++)
            {
                line = line + grid[x, y].ToString();
            }
            line = line + "\n";
        }
        Debug.Log(line);
    }

    public SimuBoard Clone()
    {
        SimuBoard clone = new SimuBoard(Width, Height);
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                clone.grid[x, y] = this.grid[x, y];
            }
        }
        return clone;
    }

    public bool placePiece(SimuPiece simuPiece)
    {
        //simuPiece.pos position dans la grid
        bool canPlace = false;
        //parcourt toute la hauteur jusqu'à trouver une place qui correspond et pose la piece
        //TODO refaire l'algo pour tester la piece où elle va car elle peut acceder à des endroit impossible pour le moment
        //Cette partie ne fonctionne pas car la piece va essayer d'acceder à une place qui est surement inacessible

        int hauteur = 0;
        for (int y = Height -1 ; y > 0; y--)
        {
            if (grid[simuPiece.pos, y] == 1)
            {
                hauteur = y;
                break;
            }
        }
        for (int y = hauteur; y < Height - simuPiece.height; y++)
        {
            canPlace = true;

            //simuPiece.Print();
            for (int i = 0; i < simuPiece.height && canPlace; i++)
            {
                //Debug.Log("y : " + (y + i));
                for (int j = 0; j < simuPiece.width && canPlace; j++)
                {
                    //Debug.Log("x : " + (simuPiece.pos + j));
                    if (grid[simuPiece.pos + j, y + i] + simuPiece.grid[i, j] == 2)
                    {
                        canPlace = false;
                    }
                }
            }
            if (canPlace)
            {
                for (int i = 0; i < simuPiece.height; i++)
                {
                    for (int j = 0; j < simuPiece.width; j++)
                    {
                        grid[simuPiece.pos + j, y + i] = grid[simuPiece.pos + j, y + i] + simuPiece.grid[i, j];
                    }
                }
                return canPlace;
            }


        }
        return canPlace;
    }
}