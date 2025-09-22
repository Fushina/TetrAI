using UnityEngine;

public class SimuPiece
{
    public Vector2Int[] cells;   // offsets de la forme
    public int pos;       // position sur le board
    public int rotation;        // index de rotation (0-3)
    public int[,] grid;
    public int height, width;

    public SimuPiece(Piece p, int pos)
    {
        // Copie des offsets de la vraie pièce
        cells = new Vector2Int[p.cells.Length];
        for (int i = 0; i < p.cells.Length; i++)
        {
            cells[i] = (Vector2Int)p.cells[i];
        }


        // Position initiale = spawn de la vraie pièce
        this.pos = pos;
        rotation = p.rotationIndex; // ou p.rotationIndex si tu as ça
        toGrid();
    }

    public void toGrid()
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var c in cells)
        {
            if (c.x < minX) minX = c.x;
            if (c.x > maxX) maxX = c.x;
            if (c.y < minY) minY = c.y;
            if (c.y > maxY) maxY = c.y;
        }

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;
        this.height = height;
        this.width = width;
        grid = new int[height, width];

        // Initialisation vide
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[y, x] = 0;

        // Placement des cellules
        foreach (var c in cells)
        {
            int gx = c.x - minX;
            int gy = c.y - minY;
            grid[gy, gx] = 1; // inverser Y pour affichage
        }
    }

    public void Print()
    {
        string output = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                output += grid[y, x];
            output += "\n";
        }

        Debug.Log(output);
    }
    
}
