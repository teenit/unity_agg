using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class BirdPage : MonoBehaviour
{

    [Header("BirdPage")]
    public GameObject BirdPagePanel;
    public Image Bird;
    public TMP_Text BirdTitle;
    public TMP_Text Description;
    public TMP_Text Price; //Coins

    public void SetBirdPageInfo(string title = "", string description = "", int price = 0, string birdName = "")
    {
        if (Bird != null)
        {
            // Перший кадр анімації з Resources/Birds/<ім'я птаха>/
            Sprite icon = Resources.LoadAll<Sprite>($"Birds/{birdName}")
                .OrderBy(s => int.TryParse(s.name, out int n) ? n : 0)
                .FirstOrDefault();

            if (icon != null) Bird.sprite = icon;
            else Debug.LogWarning($"BirdCell: кадри в Resources/Birds/{birdName}/ не знайдено!");
        }

        BirdTitle.text = title;
        Description.text = description;
        Price.text = price.ToString();
    }

    public void ShowPanel()
    {
        BirdPagePanel.SetActive(true);
    }

    public void HidePanel()
    {
        BirdPagePanel.SetActive(false);
    }
}
