using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Auth : MonoBehaviour
{
  public TMP_InputField userNameField;
  public TMP_InputField emailField;
    
    public void StartGame()
    {
      string userName = userNameField.text;
      string email = emailField.text;

      Debug.Log("UserName " + userName);
      Debug.Log("Email " + email);

      PlayerPrefs.SetString("UserName", userName);
      PlayerPrefs.SetString("Email", email);

      SceneManager.LoadScene("Scene_2");

    }
}
