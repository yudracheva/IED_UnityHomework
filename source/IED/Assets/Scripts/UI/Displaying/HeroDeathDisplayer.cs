using Services.PlayerData;
using TMPro;
using UnityEngine;

public class HeroDeathDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberCountText;

    private NumberOfEnemiesKilled _number;

    public void Construct(NumberOfEnemiesKilled number)
    {
        _number = number;
        _number.Changed += DisplayMoneyCount;
        DisplayMoneyCount();
    }

    public void Cleanup()
    {
        _number.Changed -= DisplayMoneyCount;
    }

    private void DisplayMoneyCount()
    {
        numberCountText.text = _number.Count.ToString();
    }
}
