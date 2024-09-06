using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI text; // 텍스트를 받아올 부분

    public int allscore = 0; // 점수를 받아올 부분

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text=allscore.ToString();
    }
}