using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PinballUI : MonoBehaviour
{
    [SerializeField] private int multiplier;
    [SerializeField] private int carBaseScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multText;
    private int score = 0;
    private int mult = 1;

    public void CarHit()
    {
        score += carBaseScore * mult;
        mult *= multiplier;
        UpdateUI();
    }

    public void ResetMult()
    {
        mult = 1;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        multText.text = "x " + mult.ToString();
    }
}
