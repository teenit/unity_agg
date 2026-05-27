using UnityEngine;

public class BackendSim : MonoBehaviour
{
    public UISwipeDetector detector;

    // Список імен птахів — мають збігатися з BirdConfig.birdName в Inspector
    private string[] birdNames = { "bird_4" };

    void Start()
    {
        string randomBird = birdNames[Random.Range(0, birdNames.Length)];
        detector.SetupEgg(randomBird, 5.0f);
    }
}
