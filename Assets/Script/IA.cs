using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class IA
{
    public float w1; //poids lignes complètes sur le mouvement
    public float w2; //poids nbr de trous sur le mouvement
    public float w3; //poids hauteur sur le mouvement
    public float w4; //poids de la rugosté sur le mouvement
    private Board board; //le plateau pour qu'il accede à la piece et information du plateau
    private SimuBoard simu = new SimuBoard(10, 20);//Plateau virtuel
    private SimuPiece simuPiece;

    public IA(Board board, float[] weights)
    {
        this.w1 = weights[0];
        this.w2 = weights[1];
        this.w3 = weights[2];
        this.w4 = weights[3];
        this.board = board;

        simu.CopierTile(board.tilemap);
    }

    public float[] get_weights()
    {
        List<float> weights = new List<float>();
        weights.Add(w1);
        weights.Add(w2);
        weights.Add(w3);
        weights.Add(w4);
        return weights.ToArray();
    }



    public void check_best_move(Piece activePiece, Tilemap tilemap)
    {
        float best_score = float.MinValue;
        int best_pos = 0;
        int best_rot = 0;
        int pos = 0;
        int best_ligne = 0;
        int best_trou = 0;
        int best_hauteur = 0;
        int best_rug = 0;
        simu.CopierTile(tilemap);
        if (activePiece.data.tetromino == board.tetrominoes[0].tetromino)
        {
            activePiece.Move(Vector2Int.down);
        }
        for (int r = 0; r < 4; r++)   // 4 rotations max
        {
            pos = 0;
            bool canMove = true;
            while (activePiece.Move(Vector2Int.left)) ;

            while (canMove)
            {

                // Crée un clone du board
                SimuBoard clone = simu.Clone();
                // Crée une pièce simulée à partir de la vraie pièce
                SimuPiece simuPiece = new SimuPiece(activePiece, pos);

                // Place la pièce simulée dans le clone et retourne false si n'a pas la place d'être placé
                if (clone.placePiece(simuPiece))
                {
                    //clone.printSimu();
                    // Évalue le résultat
                    int ligne = calculLigne(clone);
                    int trou = calculTrou(clone);
                    int hauteurmax = hauteurMax(clone);
                    int rug = calculRugosite(clone);
                    float temp_score = 20f * w1 * ligne
                                    - 10f * w2 * trou
                                    - w3 * hauteurmax
                                    - w4 * rug;

                    if (temp_score > best_score)
                    {
                        best_ligne = ligne;
                        best_trou = trou;
                        best_hauteur = hauteurmax;
                        best_rug = rug;
                        best_score = temp_score;
                        best_pos = pos;
                        best_rot = r;
                    }
                    //Debug.Log("Best pos : " + pos + "\nBest rot : " + r + "\nLigne :" + ligne + "\nTrou : " + trou + "\nHauteur : " + hauteurmax + "\nRug : " + rug + "\nScore : " + temp_score);
                }
                pos++;
                canMove = activePiece.Move(Vector2Int.right);

            }
            activePiece.Rotate(1);
        }
        //Debug.Log("Best pos : " + best_pos + "\nBest rot : " + best_rot + "\nLigne :" + best_ligne + "\nTrou : " + best_trou + "\nHauteur : " + best_hauteur + "\nRug : " + best_rug + "\nScore : " + best_score);

        // Applique le meilleur move sur la vraie pièce Unity
        for (int i = 0; i < best_rot; i++)
            activePiece.Rotate(1);
        while (activePiece.Move(Vector2Int.left)) ;
        for (int i = 0; i < best_pos; i++)
            activePiece.Move(Vector2Int.right);

        while (activePiece.Move(Vector2Int.down)) ;
    }

    private int calculLigne(SimuBoard simu)
    {
        int ligne = 0;
        bool test = true;
        for (int y = 0; y < simu.Height; y++)
        {
            for (int x = 0; x < simu.Width; x++)
            {
                if (simu.grid[x, y] == 0)
                {
                    test = false;
                }
            }
            if (test)
            {
                ligne++;
            }
        }
        return ligne;
    }

    private int calculRugosite(SimuBoard simu)
    {
        int[] heights = new int[simu.Width];
        for (int x = 0; x < simu.Width; x++)
        {
            for (int y = simu.Height - 1; y >= 0; y--)
            {
                if (simu.grid[x, y] != 0)
                {
                    heights[x] = y + 1; // +1 car hauteur en nombre de cases
                    break;
                }
            }
        }
        int rug = 0;
        for (int i = 0; i < simu.Width - 1; i++)
        {
            rug += Mathf.Abs(heights[i] - heights[i + 1]);
        }
        return rug;
    }

    private int hauteurMax(SimuBoard simu)
    {
        int[] heights = new int[simu.Width];
        for (int x = 0; x < simu.Width; x++)
        {
            for (int y = simu.Height - 1; y >= 0; y--)
            {
                if (simu.grid[x, y] != 0)
                {
                    heights[x] = y + 1; // +1 car hauteur en nombre de cases
                    break;
                }
            }
        }

        return Mathf.Max(heights);
    }

    private int calculTrou(SimuBoard simu)
    {
        int[] heights = new int[simu.Width];
        for (int x = 0; x < simu.Width; x++)
        {
            for (int y = simu.Height - 1; y >= 0; y--)
            {
                if (simu.grid[x, y] != 0)
                {
                    heights[x] = y + 1; // +1 car hauteur en nombre de cases
                    break;
                }
            }
        }
        int trou = 0;
        for (int x = 0; x < simu.Width; x++)
        {
            for (int y = 0; y < heights[x]; y++)
            {
                if (simu.grid[x, y] == 0)
                {
                    trou++;
                }
            }
        }
        return trou;
    }
}
