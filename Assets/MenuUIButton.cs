using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI levelText;

    public Button Button => button;
    public TextMeshProUGUI PriceText => priceText;
    public TextMeshProUGUI LevelText => levelText;
}