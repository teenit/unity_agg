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

        var eggs = backendSim.GetEggs();

        for (int i = 0; i < eggs.Length; i++)
        {
            var egg = eggs[i];

            GameObject prefab = egg.Rarity switch
            {
                "limit"  => eggCellLimitPrefab,
                "common" => eggCellPrefab,
                "simple" => eggCellSimplePrefab,
                _        => null
            };

            if (prefab == null) { Debug.LogError($"EggGridManager: невідома рідкість '{egg.Rarity}'!"); return; }

            GameObject cell = Instantiate(prefab, gridContainer);
            EggCell eggCell = cell.GetComponent<EggCell>();

            if (eggCell == null) { Debug.LogError("EggGridManager: на префабі немає компонента EggCell!"); return; }
            eggCell.Init($"bird_{i + 1}", 5, (i + 1) * 2, this);
        }

    }
}
