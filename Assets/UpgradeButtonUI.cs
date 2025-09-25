using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private Button upgradeButton;

    public Button UpgradeButton => upgradeButton;
    
    public void InitButtonUI(Sprite icon, string text)
    {
        upgradeIcon.sprite = icon;
        upgradeText.text = text;
    }
}