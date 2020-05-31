using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class DeleteButton : MonoBehaviour
{

    private Text text;
    private Button button;

    private void Awake()
    {
        text = transform.parent.GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(this.OnClick);
    }

    private void OnClick()
    {
        AccountManager.Instance().RemoveFriend(text.text);
    }

}
