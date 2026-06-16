using UnityEngine;

public class GameCleaner : MonoBehaviour
{
    [Header("Об'єкти для видалення")]
    public GameObject birdObject;      // Об'єкт із зображенням птаха (Egg)
    public GameObject temperatureUI;   // Весь об'єкт Slider (шкала)
    public GameObject debugTextUI;     // Текст дебагу (якщо треба приховати)
    public GameObject barLine1;
    public GameObject barLine2;

    // Цю функцію ми викликаємо через UnityEvent (OnBirdClick)
    public void ClearPlayfield()
    {
        if (birdObject != null) birdObject.SetActive(false);
        if (temperatureUI != null) temperatureUI.SetActive(false);
        if (debugTextUI != null) debugTextUI.SetActive(false);
        if (barLine1 != null) barLine1.SetActive(false);
        if (barLine2 != null) barLine2.SetActive(false);

        Debug.Log("<color=white>Поле очищено!</color>");
    }
}