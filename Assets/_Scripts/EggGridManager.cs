using UnityEngine;

public class EggGridManager : MonoBehaviour
{
    [Header("Grid")]
    public GameObject gridPanel;
    public Transform gridContainer;
    public GameObject eggCellPrefab;
    public GameObject eggCellLimitPrefab;
    public GameObject eggCellSimplePrefab;
    public BackendSim backendSim;

    [Header("Hatching Screen")]
    public GameObject hatchPanel;
    public UISwipeDetector detector;

    private const int birdQuantity = 11;
    private bool isSpawned = false;

    public void ShowGrid()
    {
        if (!isSpawned)
        {
            SpawnEggs();
            isSpawned = true;
        }
        gridPanel.SetActive(true);
    }

    public void HideGrid()
    {
        gridPanel.SetActive(false);
    }

    public void SelectEgg(string birdName, int hatchTime, int birdCoins)
    {
        HideGrid();
        hatchPanel.SetActive(true);
        detector.SetupEgg(birdName, hatchTime, birdCoins);
    }

    void SpawnEggs()
    {
        if (eggCellPrefab == null) { Debug.LogError("EggGridManager: eggCellPrefab не прив'язаний!"); return; }
        if (gridContainer == null) { Debug.LogError("EggGridManager: gridContainer не прив'язаний!"); return; }

        var eggsCount = backendSim.GetEggCounts();
        Debug.Log(eggsCount.common);

        for (int i = 1; i <= 5; i++)
        {
            GameObject cell = Instantiate(eggCellPrefab, gridContainer);
            EggCell eggCell = cell.GetComponent<EggCell>();
            if (eggCell == null) { Debug.LogError("EggGridManager: на префабі немає компонента EggCell!"); return; }
            eggCell.Init($"bird_{i}", 1, i * 2, this);
        }

         for (int i = 6; i <= 11; i++)
        {
            GameObject cell = Instantiate(eggCellLimitPrefab, gridContainer);
            EggCell eggCell = cell.GetComponent<EggCell>();
            if (eggCell == null) { Debug.LogError("EggGridManager: на префабі немає компонента EggCell!"); return; }
            eggCell.Init($"bird_{i}", 1, i * 2, this);
        }
    }
}
