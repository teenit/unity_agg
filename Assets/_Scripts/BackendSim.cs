using UnityEngine;
using System.Collections.Generic;

public class BackendSim : MonoBehaviour
{
    public class Bird
    {
        public string Name { get; set; }
        public int Time { get; set; }
        public int Coins { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int Fps { get; set; }

        public Bird() {}

        public Bird(string name = "", int time = 0, int coins = 0, string description = "", string title = "", int fps = 10)
        {
            Name = name;
            Time = time;
            Coins = coins;
            Description = description;
            Title = title;
            Fps = fps;
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
            Bird newBird = new Bird($"bird_{i}", 10, i * 2);
            birds.Add(newBird);
        }

        Bird randomBird = birds[Random.Range(0, birds.Count)];

        detector.SetupEgg(randomBird.Name, randomBird.Time, randomBird.Coins);

        Debug.Log($"Обрана пташка: {randomBird.Name}");
    }

    // Колекція вилуплених пташок (поки імітація, потім прийде з бекенду)
    public Bird[] GetBirds()
    {
        return new Bird[]
        {
            new Bird("", 0, 0, "", "", 24), // string name, int time, int coins, string description
            new Bird("bird_3", 10, 6, "Description 3", "title3", 12),
            new Bird("bird_3", 10, 6, "Description 3", "title3", 12),
            new Bird("bird_5", 10, 10, "Description 5", "title5", 24),
            new Bird("bird_7", 10, 14, "Description 7", "title7", 12),
            new Bird("bird_11", 10, 22, "Description 11", "title11", 12),
        };
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
            new Egg("ultra", 0),
            new Egg("ultra", 1),
            new Egg("ultra", 8),
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
            new Egg("common", 8),
            new Egg("limit", 0),
            new Egg("ultra", 8),
        };
    }
}