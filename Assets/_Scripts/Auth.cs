using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class Auth : MonoBehaviour
{
  public TMP_InputField emailField;
  public TMP_InputField codeField;
  public TMP_Text buttonText;
  public TMP_Text errorText; // Перетягніть сюди ваш новий ErrorText в Інспекторі
  private bool isSendEmail = false;
  private bool isSendCode = false;
  private bool isRequesting = false;
  private const string LoginUrl = "http://localhost:8014/login.php";

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
      if (isRequesting) return;
      StartCoroutine(SendLoginRequest(email));
    }

    [System.Serializable]
    private class LoginData
    {
      public string login;
      public string password;
    }

    [System.Serializable]
    private class LoginResponse
    {
      public bool success;
      public string message;
    }

    // Тестовий запит авторизації: email є логіном, пароль поки захардкожений
    private IEnumerator SendLoginRequest(string email)
    {
      isRequesting = true;

      string json = JsonUtility.ToJson(new LoginData { login = email.Trim(), password = "12345" });
      byte[] body = Encoding.UTF8.GetBytes(json);

      using (UnityWebRequest request = new UnityWebRequest(LoginUrl, UnityWebRequest.kHttpVerbPOST))
      {
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.LogWarning(request);
        if (request.result == UnityWebRequest.Result.Success) {
          isSendEmail = true;
          // Показуємо поле коду
          codeField.gameObject.SetActive(true);
          emailField.gameObject.SetActive(false);
          if (buttonText != null) buttonText.text = "Отримати код";
        } else {
          ShowError(GetErrorMessage(request));
        }
      }

      isRequesting = false;
    }

    // Бере message з JSON-відповіді бека; якщо відповіді немає (мережа) — текст помилки запиту
    private string GetErrorMessage(UnityWebRequest request)
    {
      string text = request.downloadHandler != null ? request.downloadHandler.text : null;
      if (!string.IsNullOrEmpty(text)) {
        try {
          LoginResponse response = JsonUtility.FromJson<LoginResponse>(text);
          if (response != null && !string.IsNullOrEmpty(response.message)) {
            return response.message;
          }
        } catch (System.ArgumentException) {
          // відповідь не є JSON — показуємо стандартну помилку нижче
        }
      }
      return request.error;
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