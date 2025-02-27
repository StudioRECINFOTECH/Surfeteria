using UnityEngine;
using UnityEngine.UI;

public class LoginUIControllerBridge : MonoBehaviour
{
    public Button FaceBookButton;
    public Button GoogleButton;
    public Button CloseButton;
    public Button LoginButton;
    public GameObject LoginPanel;

    [SerializeField]private GoogleLoginController GoogleLogin;
    [SerializeField] private FacebookController FacebookLogin;

    void Start()
    {
        FaceBookButton.onClick.AddListener(() => FacebookLogin.Login());
        GoogleButton.onClick.AddListener(() => GoogleLogin.OnSignIn());
        CloseButton.onClick.AddListener(() => LoginPanel.SetActive(false));
        LoginButton.onClick.AddListener(() => LoginPanel.SetActive(true));
    }

    private void OnDestroy()
    {
        FaceBookButton.onClick.RemoveAllListeners();
        GoogleButton.onClick.RemoveAllListeners();
        CloseButton.onClick.RemoveAllListeners();
        LoginButton.onClick.RemoveAllListeners();
    }
}
