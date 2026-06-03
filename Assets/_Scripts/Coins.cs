using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public TMP_Text coinsElem;

    void Start()
    {
        UpdateCoins();
    }

    public void UpdateCoins(){
        Debug.Log(coinsElem);
      
        if (coinsElem) {
            coinsElem.text = GetCoins().ToString();
        }
        
    }

    public void ClearCoins(){
        PlayerPrefs.SetInt("Coins", 0);
        UpdateCoins();
    }

    public int GetCoins()
    {
        return PlayerPrefs.HasKey("Coins") ? PlayerPrefs.GetInt("Coins") : 0;
    }

    public void AddCoins(int coins = 0)
    {

        int currentCoins = GetCoins();
        currentCoins += coins;

        PlayerPrefs.SetInt("Coins", currentCoins);

        coinsElem.text = currentCoins.ToString();
    }
}
