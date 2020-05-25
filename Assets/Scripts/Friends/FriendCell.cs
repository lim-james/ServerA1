using UnityEngine;
using UnityEngine.UI;

public class FriendCell : MonoBehaviour
{
    [SerializeField]
    private ChatManager chatManager;

    private Button button;
    private Text text;

    private void Awake()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();

        button.onClick.AddListener(this.OnClick);
    }

    private void OnClick()
    {
        chatManager.Open(text.text);
    }

}
