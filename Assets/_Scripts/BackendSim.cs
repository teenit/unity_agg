using UnityEngine;
using System.Collections.Generic;

public class BackendSim : MonoBehaviour
{
    public class Bird
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public int Coins { get; set; }

        public Bird() {}

        public Bird(string name, int time, int coins)
        {
            Name = name;
            Time = time;
            Coins = coins;
        }
    }

    public UISwipeDetector detector;

   
    private const int birdQuantity = 11;

    private List<Bird> birds = new List<Bird>();

    void Start()
    {
        //цикл для заповнення масиву
        for (int i = 1; i <= birdQuantity; i++)
        {
            Bird newBird = new Bird($"bird_{i}", 1, i * 2);
            birds.Add(newBird);
        }

        Bird randomBird = birds[Random.Range(0, birds.Count)];

        detector.SetupEgg(randomBird.Name, randomBird.Time, randomBird.Coins);

        Debug.Log($"Обрана пташка: {randomBird.Name}");
    }

    public (int common, int limit, int simple, int total) GetEggCounts()
    {
        return (
            common: 3, 
            limit: 4, 
            simple: 4,
            total: 11
            );
    }
}