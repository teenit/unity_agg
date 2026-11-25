using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

// Авторизація без пароля:
// 1) якщо є збережений токен — перевіряємо його (me.php) і одразу заходимо в гру
// 2) інакше: email -> request_code.php -> поле коду -> verify_code.php -> отримуємо токен
public class Auth : MonoBehaviour
{
  public TMP_InputField emailField;
  public TMP_InputField codeField;
  public TMP_Text buttonText;
  public TMP_Text errorText; // Перетягніть сюди ваш новий ErrorText в Інспекторі

  private const string BaseUrl = "http://localhost:8014";
  private const string TokenKey = "Token";

  private bool isSendEmail = false; // код уже надіслано, чекаємо його введення
  private bool isRequesting = false; // зараз іде запит — не дублюємо
  private string sentEmail = "";

  // ---------- Моделі JSON ----------

  [System.Serializable]
  private class EmailRequest
  {
    public string email;
  }

  [System.Serializable]
  private class VerifyRequest
  {
    public string email;
    public string code;
  }

  [System.Serializable]
  private class UserData
  {
    public int id;
    public string username;
    public string email;
    public int coins;
  }

  [System.Serializable]
  private class ApiResponse
  {
    public bool success;
    public string message;
    public string token;
    public UserData user;
  }

  // ---------- Життєвий цикл ----------

  void Start()
  {
    ShowEmailStep();

    // Якщо вже входили раніше — пробуємо увійти за токеном
    string token = PlayerPrefs.GetString(TokenKey, "");
    if (!string.IsNullOrEmpty(token)) {
      StartCoroutine(LoginByToken(token));
    }
  }

  // Викликається кнопкою
  public void StartGame()
  {
    if (isRequesting) return;
    ShowError("");

    if (!isSendEmail) {
      string email = emailField.text.Trim();

      if (!IsValidEmail(email)) {
        ShowError("Некоректний email! Введіть пошту у форматі name@gmail.com");
        return;
      }

      StartCoroutine(RequestCode(email));
    } else {
      string code = codeField.text.Trim();

      if (!Regex.IsMatch(code, @"^\d{6}$")) {
        ShowError("Введіть 6-значний код з пошти!");
        return;
      }

      StartCoroutine(VerifyCode(sentEmail, code));
    }
  }

  // ---------- Запити ----------

  // Крок 1: надсилаємо email, бек створює код
  private IEnumerator RequestCode(string email)
  {
    string json = JsonUtility.ToJson(new EmailRequest { email = email });

    yield return Send("/request_code.php", "POST", json, null, (ok, response, error) => {
      if (ok) {
        sentEmail = email;
        ShowCodeStep();
      } else {
        ShowError(error);
      }
    });
  }

  // Крок 2: надсилаємо email + код, отримуємо токен
  private IEnumerator VerifyCode(string email, string code)
  {
    string json = JsonUtility.ToJson(new VerifyRequest { email = email, code = code });

    yield return Send("/verify_code.php", "POST", json, null, (ok, response, error) => {
      if (ok) {
        PlayerPrefs.SetString(TokenKey, response.token);
        EnterGame(response.user);
      } else {
        ShowError(error);
      }
    });
  }

  // Вхід за збереженим токеном
  private IEnumerator LoginByToken(string token)
  {
    yield return Send("/me.php", "GET", null, token, (ok, response, error) => {
      if (ok) {
        EnterGame(response.user);
      } else {
        // Токен прострочений або недійсний — забуваємо його, гравець входить через email
        PlayerPrefs.DeleteKey(TokenKey);
      }
    });
  }

  // Спільний метод для всіх запитів до бека
  private IEnumerator Send(string path, string method, string json, string token,
                           System.Action<bool, ApiResponse, string> onDone)
  {
    isRequesting = true;

    using (UnityWebRequest request = new UnityWebRequest(BaseUrl + path, method))
    {
      request.downloadHandler = new DownloadHandlerBuffer();

      if (json != null) {
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.SetRequestHeader("Content-Type", "application/json");
      }
      if (token != null) {
        request.SetRequestHeader("Authorization", "Bearer " + token);
      }

      yield return request.SendWebRequest();

      ApiResponse response = ParseResponse(request.downloadHandler.text);
      bool ok = request.result == UnityWebRequest.Result.Success && response != null && response.success;

      string error = null;
      if (!ok) {
        error = (response != null && !string.IsNullOrEmpty(response.message))
          ? response.message
          : "Немає зв'язку з сервером (" + request.error + ")";
      }

      isRequesting = false;
      onDone(ok, response, error);
    }
  }

  private ApiResponse ParseResponse(string text)
  {
    if (string.IsNullOrEmpty(text)) return null;
    try {
      return JsonUtility.FromJson<ApiResponse>(text);
    } catch (System.ArgumentException) {
      return null; // відповідь не є JSON
    }
  }

  // ---------- Вхід у гру ----------

  private void EnterGame(UserData user)
  {
    PlayerPrefs.SetInt("UserId", user.id);
    PlayerPrefs.SetString("Email", user.email);
    // username поки може бути порожнім — тоді показуємо email
    PlayerPrefs.SetString("UserName", string.IsNullOrEmpty(user.username) ? user.email : user.username);
    PlayerPrefs.Save();

    SceneManager.LoadScene("Scene_2");
  }

  // ---------- UI ----------

  private void ShowEmailStep()
  {
    isSendEmail = false;
    if (emailField) emailField.gameObject.SetActive(true);
    if (codeField) codeField.gameObject.SetActive(false);
    if (buttonText) buttonText.text = "Отримати код";
    ShowError("");
  }

  private void ShowCodeStep()
  {
    isSendEmail = true;
    if (codeField) {
      codeField.text = "";
      codeField.gameObject.SetActive(true);
    }
    if (emailField) emailField.gameObject.SetActive(false);
    if (buttonText) buttonText.text = "Увійти";
  }

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
    } else if (!string.IsNullOrEmpty(message)) {
      Debug.LogWarning(message);
    }
  }
}
