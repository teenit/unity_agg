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

    public class Egg
    {
        public string Rarity { get; set; }
        public int Status { get; set; }

        public Egg(string rarity, int status)
        {
            Rarity = rarity;
            Status = status;
        }
    }

    public Egg[] GetEggs()
    {
        return new Egg[]
        {
            new Egg("common", 0),
            new Egg("common", 1),
            new Egg("common", 8),
            new Egg("common", 0),
            new Egg("common", 0),
            new Egg("common", 0),
            new Egg("common", 0),
            new Egg("common", 0),
            new Egg("simple", 1),
            new Egg("simple", 0),
            new Egg("simple", 8),
            new Egg("simple", 1),
            new Egg("limit", 0),
            new Egg("limit", 1),
            new Egg("limit", 8),
            new Egg("limit", 0),
        };
    }
}