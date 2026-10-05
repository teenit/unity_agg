using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BirdCell : MonoBehaviour
{
    [Header("Елементи клітинки (прив'язати на шаблоні)")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI coinsText;

    private BackendSim.Bird bird;
    private BirdGridManager manager;

    public void Init(BackendSim.Bird data, BirdGridManager gridManager)
    {
        bird = data;
        manager = gridManager;

        if (nameText != null) nameText.text = bird.Name;
        if (coinsText != null) coinsText.text = bird.Coins.ToString();

        if (iconImage != null)
        {
            // Перший кадр анімації з Resources/Birds/<ім'я птаха>/
            Sprite icon = Resources.LoadAll<Sprite>($"Birds/{bird.Name}")
                .OrderBy(s => int.TryParse(s.name, out int n) ? n : 0)
                .FirstOrDefault();

            if (icon != null) iconImage.sprite = icon;
            else Debug.LogWarning($"BirdCell: кадри в Resources/Birds/{bird.Name}/ не знайдено!");
        }

        Button btn = GetComponent<Button>();
        if (btn == null) { Debug.LogError($"BirdCell '{bird.Name}': немає компонента Button на шаблоні!"); return; }
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        manager.SelectBird(bird);
    }
}
