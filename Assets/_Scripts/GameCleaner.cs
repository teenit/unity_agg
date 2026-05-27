using UnityEngine;

public class GameCleaner : MonoBehaviour
{
    [Header("Об'єкти для видалення")]
    public GameObject birdObject;      // Об'єкт із зображенням птаха (Egg)
    public GameObject temperatureUI;   // Весь об'єкт Slider (шкала)
    public GameObject debugTextUI;     // Текст дебагу (якщо треба приховати)

    // Цю функцію ми викликаємо через UnityEvent (OnBirdClick)
    public void ClearPlayfield()
    {
        if (birdObject != null) birdObject.SetActive(false);
        if (temperatureUI != null) temperatureUI.SetActive(false);
        if (debugTextUI != null) debugTextUI.SetActive(false);

        Debug.Log("<color=white>Поле очищено!</color>");
    }
}