using UnityEngine;

public class BirdGridManager : MonoBehaviour
{
    [Header("Grid")]
    public GameObject gridPanel;
    public Transform gridContainer;
    public GameObject birdCellTemplate; // неактивний об'єкт у сцені, не префаб
    public BackendSim backendSim;

    [Header("Hatching Screen")]
    public GameObject hatchPanel;

    private bool isSpawned = false;

    public void ShowGrid()
    {
        if (!isSpawned)
        {
            SpawnBirds();
            isSpawned = true;
        }
        gridPanel.SetActive(true);
    }

    public void HideGrid()
    {
        gridPanel.SetActive(false);
    }

    public void SelectBird(BackendSim.Bird bird)
    {
        Debug.Log($"Обрано птаха з колекції: {bird.Name}, монети: {bird.Coins}, час: {bird.Time}");
    }

    void SpawnBirds()
    {
        if (birdCellTemplate == null) { Debug.LogError("BirdGridManager: birdCellTemplate не прив'язаний!"); return; }
        if (gridContainer == null) { Debug.LogError("BirdGridManager: gridContainer не прив'язаний!"); return; }
        if (backendSim == null) { Debug.LogError("BirdGridManager: backendSim не прив'язаний!"); return; }

        // Шаблон лише зразок — сам він у сітці не показується
        birdCellTemplate.SetActive(false);

        var birds = backendSim.GetBirds();

        foreach (var bird in birds)
        {
            GameObject cell = Instantiate(birdCellTemplate, gridContainer);
            cell.name = $"BirdCell_{bird.Name}";
            cell.SetActive(true);

            BirdCell birdCell = cell.GetComponent<BirdCell>();
            if (birdCell == null) { Debug.LogError("BirdGridManager: на шаблоні немає компонента BirdCell!"); return; }
            birdCell.Init(bird, this);
        }
    }
}
