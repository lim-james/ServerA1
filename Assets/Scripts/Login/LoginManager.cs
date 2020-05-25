using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private InputField usernameField;
    [SerializeField]
    private InputField passwordField;
    [SerializeField]
    private Button enterButton;
    [SerializeField]
    private Text messageLabel;
    [SerializeField]
    private float segueDelay;

    private float et;
    private bool success;

    private void SetError(string message)
    {
        messageLabel.color = new Color(1, 0, 0);
        messageLabel.text = message;
    }

    private void SetSuccess(string message)
    {
        messageLabel.color = new Color(1, 1, 1);
        messageLabel.text = message;
    }

    private bool UserExist(string username)
    {
        return username == "root";
    }

    private void Start()
    {
        et = 0.0f;
        success = false;
    }

    private void Update()
    {
        if (success && (et += Time.deltaTime) > segueDelay)
            SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }

    public void EnterHandler()
    {
        enterButton.enabled = false;

        string username = usernameField.text;
        string password = passwordField.text;

        if (username.Length == 0)
        {
            SetError("Please enter username");
            enterButton.enabled = true;
            return;
        }

        if (username.Length > 20)
        {
            SetError("Choose a shorter username");
            enterButton.enabled = true;
            return;
        }

        if (password.Length < 8)
        {
            SetError("Password too short. (Length between 8 and 20)");
            enterButton.enabled = true;
            return;
        }

        if (password.Length > 20)
        {
            SetError("Password too long. (Length between 8 and 20)");
            enterButton.enabled = true;
            return;
        }

        string error;

        if (UserExist(username))
        {
            success = AccountManager.Instance().Login(username, password, out error);
            if (success) SetSuccess("Welcome back " + username);
        }
        else
        {
            success = AccountManager.Instance().Signup(username, password, out error);
            if (success) SetSuccess("Account created. Welcome " + username);
        }

        if (!success)
        {
            enterButton.enabled = true;
            SetError(error);
        }
    }

}
