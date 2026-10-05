using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class Auth : MonoBehaviour
{
  public TMP_InputField emailField;
  public TMP_InputField codeField;
  public TMP_Text buttonText;
  private bool isSendEmail = false;
  private bool isSendCode = false;

    void Start()
    {
      if (codeField) {
        codeField.gameObject.SetActive(false);
      }
    }

    public void StartGame()
    {
      string email = "";

      if (emailField && !isSendEmail) {
        email = emailField.text;
        PlayerPrefs.SetString("Email", email);
        SendEmailToBack(email);
        buttonText.text = "Почати";
      }

      if (codeField && isSendEmail && !isSendCode) {
        bool checkStatusCode = CheckCode(codeField.text);
        if (checkStatusCode) {
          SceneManager.LoadScene("Scene_2");
        }
      }
      
    }

    public bool CheckCode(string code) {
      bool status = false;
      if (code == "1234") {
        status = true;
      }

      return  status;
    }

    public void SendEmailToBack(string email)
    {
      //TO DO
      // Відправляємо запит на бек для того щоб відравив бек клієнту код перевірки на пошту
      bool status = true;

      if (status) {
        isSendEmail = true;
        // Показуємо поле коду
        codeField.gameObject.SetActive(true);
        emailField.gameObject.SetActive(false);
      }

    } 
}
