using UnityEngine;

public class BackendSim : MonoBehaviour
{
    public UISwipeDetector detector;

    // Список імен птахів — мають збігатися з BirdConfig.birdName в Inspector
    private string[] birdNames = { "Bi" };

    void Start()
    {
        string randomBird = "Bird_2";//birdNames[Random.Range(0, birdNames.Length)];
        detector.SetupEgg(randomBird, 15.0f);
    }
}
