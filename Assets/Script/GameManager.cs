using UnityEngine;
using System.Collections.Generic;
using System;


[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    private List<GameObject> boards = new List<GameObject>();
    private List<Board> result = new List<Board>();
    private int gameRunning = 0;
    public float[] parents1;
    public float[] parents2;
    public int generation = 0;
    public GameObject boardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //on genere ici 2 parents qui vont nous servir de base pour la premiere generation
        this.parents1 = new float[] { UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f)};
        this.parents2 = new float[] { UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f), UnityEngine.Random.Range(0f, 5.0f)};
        CreationNouvelleGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Boards actifs: " + FindObjectsOfType<Board>().Length);
    }

    public void endGame(Board board)
    {
        board.tilemap.ClearAllTiles();
        Destroy(board.activePiece.gameObject);
        result.Add(board);
        Destroy(board.gameObject);
        gameRunning--;
        Debug.Log(gameRunning);
        if (gameRunning == 0)
        {
            foreach (var b in boards)
            {
                Destroy(b);
            }
            AnalyseResult();
            CreationNouvelleGeneration();
        }
    }

    //Analyse des résultats de chaque board pour choisir meilleur parents
    private void AnalyseResult()
    {
        result.Sort((a, b) => b.envoyerResult().CompareTo(a.envoyerResult()));
        foreach (Board b in result)
        {
            Debug.Log("result: " + b.envoyerResult());
        }
        Board[]top2 = result.GetRange(0, Math.Min(2, result.Count)).ToArray();
        parents1 = top2[0].ia.get_weights();
        parents2 = top2[1].ia.get_weights();
        Debug.Log(string.Join(",", parents1));
        Debug.Log(string.Join(",", parents2));
    }

    private void CreationNouvelleGeneration()
    {
        generation++;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int x = -11 + (11 * i);
                int y = -21 + (21 * j);
                boards.Add(Instantiate(boardPrefab, new Vector3(x, y, 0), Quaternion.identity));
                gameRunning++;
            }
        }
    }
}
