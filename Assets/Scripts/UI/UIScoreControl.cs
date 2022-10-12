using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreControl : MonoBehaviour
{
    [SerializeField] List<ScoreData> scores = new List<ScoreData>();

    private void Update()
    {
        for (int i = 0; i < scores.Count; i++)
            scores[i].UpdateScore();

        scores.Sort((ScoreData a, ScoreData b) => a.Score.CompareTo(b.Score));

        for (int i = 0; i < scores.Count; i++)
            scores[i].SetScaleText(i == scores.Count-1 ? Vector3.one * 1.25f : Vector3.one);
    }
}

[System.Serializable]
public class ScoreData
{
    [SerializeField] int score;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI textMesh;

    public int Score => score;

    public void UpdateScore() 
    {
        if(player != null)
        {
            score = Mathf.RoundToInt(player.position.z);
            score = score < 0 ? 0 : score;
            textMesh.text = $"{score:0}";
        }
    }

    public void SetScaleText(Vector3 scale) => textMesh.transform.localScale = scale;
}