using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using System.Linq;

[System.Serializable]
public class BirdConfig
{
    public string birdName;
    public float fps = 10f;
}

public class UISwipeDetector : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("UI Елементи")]
    public Slider tempSlider;
    public Image fillImage;
    public TextMeshProUGUI debugText;
    public UnityEvent onBirdClick;

    [Header("Механіка вилуплення")]
    public Image eggDisplayImage;
    public Image birdImage;
    public BirdConfig[] birds;
    public Coins coins;

    private string currentBirdName;
    private bool isHatched = false;
    private bool isInitialized = false;
    private Coroutine _animCoroutine;

    [Header("Налаштування Градієнта")]
    public Gradient temperatureGradient;

    [Header("Налаштування температури")]
    public float temperature = 49.0f;
    public float maxTemp = 100f;
    public float minTemp = 0f;

    [Header("Баланс гри")]
    public float coolingSpeed = 1.0f;
    public float moveThreshold = 50.0f;
    public float timer = 1.0f;

    private Vector2 lastPos;

    public void SetupEgg(string birdName, float hatchTime)
    {
        currentBirdName = birdName;
        timer = hatchTime;
        isHatched = false;
        temperature = 49.0f;
        isInitialized = true;

        if (eggDisplayImage != null)
            eggDisplayImage.enabled = true;

        if (birdImage != null)
            birdImage.enabled = false;

        Debug.Log($"Яйце налаштовано: {birdName}, час: {hatchTime}с");
    }

    void Hatch()
    {
        isHatched = true;

        if (eggDisplayImage != null)
            eggDisplayImage.enabled = false;

        BirdConfig config = System.Array.Find(birds, b => b.birdName == currentBirdName);
        if (config == null)
        {
            Debug.LogWarning($"BirdConfig для '{currentBirdName}' не знайдено!");
            return;
        }

        Sprite[] frames = Resources.LoadAll<Sprite>($"birds/{currentBirdName}")
            .OrderBy(s => int.TryParse(s.name, out int n) ? n : 0)
            .ToArray();

        if (frames.Length == 0)
        {
            Debug.LogWarning($"Кадри в Resources/birds/{currentBirdName}/ не знайдено!");
            return;
        }

        birdImage.sprite = frames[0];
        birdImage.enabled = true;
        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        _animCoroutine = StartCoroutine(AnimateBird(frames, config.fps));
        Debug.Log($"{currentBirdName} вилупився! Кадрів: {frames.Length}");
    }

    IEnumerator AnimateBird(Sprite[] frames, float fps)
    {
        int i = 0;
        float interval = 1f / fps;
        while (true)
        {
            birdImage.sprite = frames[i % frames.Length];
            i++;
            yield return new WaitForSeconds(interval);
        }
    }

    void Start()
    {
        if (tempSlider != null)
        {
            tempSlider.minValue = minTemp;
            tempSlider.maxValue = maxTemp;
            tempSlider.value = temperature;
        }

        if (fillImage == null && tempSlider != null)
            fillImage = tempSlider.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        if (!isInitialized || isHatched) return;

        if (temperature > minTemp)
            temperature -= coolingSpeed * Time.deltaTime;

        if (temperature > 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                Hatch();
            }
        }

        temperature = Mathf.Clamp(temperature, minTemp, maxTemp);
        UpdateVisuals();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isHatched) return;

        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
        birdImage.enabled = false;
        isHatched = false;
        isInitialized = false;

        coins?.AddCoins();
        onBirdClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isHatched || !isInitialized) return;

        float distance = Vector2.Distance(eventData.position, lastPos);
        if (distance > moveThreshold)
            temperature += 0.5f;

        lastPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData) { }

    void OnDisable()
    {
        if (_animCoroutine != null) StopCoroutine(_animCoroutine);
    }

    void UpdateVisuals()
    {
        if (tempSlider != null)
        {
            tempSlider.value = temperature;
            float normalizedTemp = (temperature - minTemp) / (maxTemp - minTemp);
            if (fillImage != null)
                fillImage.color = temperatureGradient.Evaluate(normalizedTemp);
        }

        if (debugText != null)
        {
            if (!isInitialized)
                debugText.text = "Очікування даних...";
            else if (isHatched)
                debugText.text = $"Це {currentBirdName}!";
            else
                debugText.text = $"Т: {temperature:F1} | Час: {timer:F1}";
        }
    }
}
