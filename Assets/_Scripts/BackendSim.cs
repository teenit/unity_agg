using UnityEngine;
using System.Collections.Generic;

public class BackendSim : MonoBehaviour
{
    //структура пташки
    public class Bird
    {
        public string Name { get; set; }
        public int Time { get; set; }

        public Bird() {}

        public Bird(string name, int time)
        {
            Name = name;
            Time = time;
        }
    }

    public UISwipeDetector detector;
    private const int birdQuantity = 11; //константа для кількості пташок

    private List<Bird> birds = new List<Bird>(); //динамічний масив пташок

    void Start()
    {
        //цикл для заповнення масиву
        for (int i = 1; i <= birdQuantity; i++)
        {
            Bird newBird = new Bird($"bird_{i}", 1); //поки що завжди задаємо 1 секунду
            birds.Add(newBird);
        }

        Bird randomBird = birds[Random.Range(0, birds.Count)];

        detector.SetupEgg(randomBird.Name, randomBird.Time);

        Debug.Log($"Обрана пташка: {randomBird.Name}");
    }
}