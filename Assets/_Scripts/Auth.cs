using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Auth : MonoBehaviour
{
  public TMP_InputField emailField;
  public TMP_InputField codeField;
  public TMP_Text buttonText;
  public TMP_Text errorText; // Перетягніть сюди ваш новий ErrorText в Інспекторі
  private bool isSendEmail = false;
  private bool isSendCode = false;

    void Start()
    {
      if (codeField) {
        codeField.gameObject.SetActive(false);
      }
      if (errorText != null) {
        errorText.text = "";
      }
    }

    public void StartGame()
    {
      if (errorText != null) errorText.text = "";

      string email = "";

      if (emailField && !isSendEmail) {
        email = emailField.text;

        // Перевірка валідності email
        if (!IsValidEmail(email)) {
          ShowError("Некоректний email! Введіть пошту у форматі name@gmail.com");
          return;
        }

        PlayerPrefs.SetString("Email", email);
        SendEmailToBack(email);
        if (buttonText != null) buttonText.text = "Отримати код";
      }

      if (codeField && isSendEmail && !isSendCode) {
        if (string.IsNullOrEmpty(codeField.text)) {
          ShowError("Введіть код підтвердження!");
          return;
        }

        bool checkStatusCode = CheckCode(codeField.text);
        if (checkStatusCode) {
          SceneManager.LoadScene("Scene_2");
        } else {
          ShowError("Неправильний код!");
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

    // Додані функції перевірки та виводу помилки:
    public bool IsValidEmail(string email)
    {
      if (string.IsNullOrWhiteSpace(email))
        return false;

      string trimmedEmail = email.Trim();
      
      if (trimmedEmail.Length < 6 || trimmedEmail.Length > 50)
        return false;

      string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
      return Regex.IsMatch(trimmedEmail, pattern);
    }

    public void ShowError(string message)
    {
      if (errorText != null) {
        errorText.text = message;
      } else {
        Debug.LogWarning(message);
      }
    }
}