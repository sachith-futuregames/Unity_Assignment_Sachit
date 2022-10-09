using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int TeamCount;
    public TextMeshProUGUI[] TeamLabels;
    public int[] TeamScores;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetScoreBox()
    {
        for(int i = 0; i < TeamCount; ++i)
        {
            TeamLabels[i].gameObject.transform.parent.gameObject.SetActive(true);
        }
        UpdateScores();
    }
    public void UpdateScores()
    {
        for(int i = 0; i < TeamCount; ++i)
        {
            TeamLabels[i].text = TeamScores[i].ToString();
        }
    }
}
