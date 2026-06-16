using UnityEngine;
using UnityEngine.UI;

public class EggCell : MonoBehaviour
{
    private string birdName;
    private int hatchTime;
    private int birdCoins;
    private EggGridManager manager;

    public void Init(string name, int time, int coins, EggGridManager gridManager)
    {
        birdName = name;
        hatchTime = time;
        birdCoins = coins;
        manager = gridManager;

        Button btn = GetComponent<Button>();
        if (btn == null) { Debug.LogError($"EggCell '{name}': немає компонента Button на префабі!"); return; }
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        manager.SelectEgg(birdName, hatchTime, birdCoins);
    }
}
