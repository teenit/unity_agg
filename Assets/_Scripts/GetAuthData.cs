using UnityEngine;
using TMPro;

public class GetAuthData : MonoBehaviour
{
    public TMP_Text userName;

    void Start()
    {
        userName.text = GetNameText();
    }

    public string GetNameText()
    {
        return PlayerPrefs.GetString("UserName");
    }
}
