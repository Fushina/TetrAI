using UnityEngine;
using UnityEngine.Tilemaps;


public class Board : MonoBehaviour
{
    private GameManager gameManager;
    public int clearedLine = 0;
    public int blockPlace = 0;
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public IA ia;
    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    public float[] weights = new float[4];
    public bool gameOver = false;
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        this.ia = new IA(this, calculate_weight());
        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    void Update()
    { 
    }

    private float[] calculate_weight()
    {

        weights[0] = Random.value > 0.5f ? gameManager.parents1[0] +  Random.Range(-0.1f, 0.1f): gameManager.parents2[0] +  Random.Range(-0.1f, 0.1f);
        weights[1] = Random.value > 0.5f ? gameManager.parents1[1] +  Random.Range(-0.1f, 0.1f) : gameManager.parents2[1] +  Random.Range(-0.1f, 0.1f);
        weights[2] = Random.value > 0.5f ? gameManager.parents1[2] +  Random.Range(-0.1f, 0.1f) : gameManager.parents2[2] +  Random.Range(-0.1f, 0.1f);
        weights[3] = Random.value > 0.5f ? gameManager.parents1[3] +  Random.Range(-0.1f, 0.1f) : gameManager.parents2[3] +  Random.Range(-0.1f, 0.1f);
        Debug.Log("Nouveaux poids:" + string.Join(",", weights));
        return weights;

    }


    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];
        //TetrominoData data = tetrominoes[0];
        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            ia.check_best_move(activePiece, tilemap);
            Set(activePiece);
        }
        else
        {
            Debug.Log("La partie c'est terminé il devait placer un " + data.tetromino + "\n Nbr de block : = " + blockPlace);
            GameOver();
        }
    }

    public void GameOver()
    {

        tilemap.ClearAllTiles();
        gameManager.endGame(this);
        gameOver = true;
        // Do anything else you want on game over here..
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
        //Destroy(piece.gameObject);
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        clearedLine++;
        RectInt bounds = Bounds;

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    public int envoyerResult()
    {
        //On veut qu'il y a plus de poids si il supprime plus de ligne
        return blockPlace + (5 * clearedLine);  
    }

}