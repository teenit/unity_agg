using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class BirdConfig
{
    public string birdName;
    public Sprite[] frames; // перетягни PNG-кадри сюди в Inspector
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
    public Image birdImage;          // BirdObject — UI Image без Animator
    public BirdConfig[] birds;       // список птахів, заповни в Inspector

    private string currentBirdName;
    private bool isHatched = false;
    private bool isInitialized = false;

    [Header("Налаштування Градієнта")]
    public Gradient temperatureGradient;

    [Header("Налаштування температури")]
    public float temperature = 49.0f;
    public float maxTemp = 100f;
    public float minTemp = 0f;

    [Header("Баланс гри")]
    public float coolingSpeed = 1.0f;
    public float moveThreshold = 50.0f;
    public float timer = 10.0f;

    private Vector2 lastPos;

    public void SetupEgg(string birdName, float hatchTime)
    {
        currentBirdName = birdName;
        timer = hatchTime;
        isHatched = false;
        temperature = 49.0f;
        isInitialized = true;

        if (eggDisplayImage != null)
            eggDisplayImage.gameObject.SetActive(true);

        if (birdImage != null)
            birdImage.gameObject.SetActive(false);

        Debug.Log($"Яйце налаштовано: {birdName}, час: {hatchTime}с");
    }

    void Hatch()
    {
        isHatched = true;

        if (eggDisplayImage != null)
            eggDisplayImage.gameObject.SetActive(false);

        BirdConfig config = System.Array.Find(birds, b => b.birdName == currentBirdName);

        if (config == null || config.frames.Length == 0)
        {
            Debug.LogWarning($"Кадри для '{currentBirdName}' не знайдено!");
            return;
        }

        birdImage.gameObject.SetActive(true);
        StartCoroutine(AnimateBird(config));
        Debug.Log($"{currentBirdName} вилупився!");
    }

    IEnumerator AnimateBird(BirdConfig config)
    {
        int i = 0;
        while (true)
        {
            birdImage.sprite = config.frames[i % config.frames.Length];
            i++;
            yield return new WaitForSeconds(1f / config.fps);
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
        if (isHatched)
        {
            Debug.Log($"Клік на {currentBirdName}!");
            onBirdClick?.Invoke();
        }
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
