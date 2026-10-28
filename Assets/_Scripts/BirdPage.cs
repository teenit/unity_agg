using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class BirdPage : MonoBehaviour
{

    [Header("BirdPage")]
    public GameObject BirdPagePanel;
    public Image BirdImage;
    public TMP_Text BirdTitle;
    public TMP_Text Description;
    public TMP_Text Price; //Coins
    private Coroutine _animCoroutine;
    public BackendSim backendSim;

    public void SetBirdPageInfo(BackendSim.Bird bird)
    {
        if (BirdImage != null)
        {
            // Перший кадр анімації з Resources/Birds/<ім'я птаха>/
            Sprite icon = Resources.LoadAll<Sprite>($"Birds/{bird.Name}")
                .OrderBy(s => int.TryParse(s.name, out int n) ? n : 0)
                .FirstOrDefault();

            if (icon != null) BirdImage.sprite = icon;
            else Debug.LogWarning($"BirdCell: кадри в Resources/Birds/{bird.Name}/ не знайдено!");
        }

        BirdTitle.text = bird.Title;
        Description.text = bird.Description;
        Price.text = bird.Coins.ToString();
        BirdAnimation(bird.Name, bird.Fps);
    }

    public void BirdAnimation(string currentBirdName = "", int fps = 10)
    {
        Sprite[] frames = Resources.LoadAll<Sprite>($"birds/{currentBirdName}")
            .OrderBy(s => int.TryParse(s.name, out int n) ? n : 0)
            .ToArray();

        if (frames.Length == 0)
        {
            Debug.LogWarning($"Кадри в Resources/birds/{currentBirdName}/ не знайдено!");
            return;
        }

        BirdImage.sprite = frames[0];
        BirdImage.enabled = true;
        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(AnimateBird(frames, fps));
    }

    IEnumerator AnimateBird(Sprite[] frames, float fps)
    {
        int i = 0;
        float interval = 1f / fps;
        while (true)
        {
            BirdImage.sprite = frames[i % frames.Length];
            i++;
            yield return new WaitForSeconds(interval);
        }
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
