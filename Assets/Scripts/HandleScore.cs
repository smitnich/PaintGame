using UnityEngine;
using UnityEngine.UI;

public class HandleScore : MonoBehaviour {

    public Text text;
    private int score = 0;

	public void IncreaseScore(int amount)
    {
        score += amount;
    }
    public int GetScore()
    {
        return score;
    }
    public void UpdateText()
    {
        text.text = score.ToString();
    }
    public void Update()
    {
        UpdateText();
    }

}
