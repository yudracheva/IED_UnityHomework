using Services.PlayerData;
using TMPro;
using UnityEngine;

public class HeroWaveDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberCountText;

    private NumberOfWaves _number;

    public void Construct(NumberOfWaves number)
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
