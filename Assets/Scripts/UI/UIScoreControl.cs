using CryingOnionTools.ScriptableVariables;
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
    [SerializeField] UIntVariable score;
    [SerializeField] Transform player;
    [SerializeField] TextMeshProUGUI textMesh;

    public uint Score => score.Value;

    public void UpdateScore() 
    {
        if(player != null)
        {
            int temp = Mathf.RoundToInt(player.position.z);
            score.Value = (uint)(temp > 0 ? temp : 0);
            textMesh.text = $"{score.Value:0}";
        }
    }

    public void SetScaleText(Vector3 scale) => textMesh.transform.localScale = scale;
}