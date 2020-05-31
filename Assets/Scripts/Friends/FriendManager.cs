using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun
{
    public class FriendManager : MonoBehaviour
    {
        [SerializeField]
        private InputField addField;
        [SerializeField]
        private Text addErrrorLabel;

        [SerializeField]
        private FriendTableView friendsList;

        private void Awake()
        {
            AccountManager.Instance().addFriendHandler += AddFriendHandler;
            AccountManager.Instance().removeFriendHandler += friendsList.ReloadData;
            AccountManager.Instance().getFriendsHandler += friendsList.ReloadData;
        }

        private void OnDestroy()
        {
            AccountManager.Instance().addFriendHandler -= AddFriendHandler;
            AccountManager.Instance().removeFriendHandler -= friendsList.ReloadData;
            AccountManager.Instance().getFriendsHandler -= friendsList.ReloadData;
        }

        private void Start()
        {
            addField.gameObject.SetActive(false);
            AccountManager.Instance().GetFriends();
            friendsList.SetFriends(AccountManager.Instance().friends);
        }

        // handlers

        public void AddHandler()
        {
            AccountManager.Instance().AddFriend(addField.text);
        }

        public void RemoveHandler()
        {
            AccountManager.Instance().RemoveFriend(addField.text);
        }

        private void AddFriendHandler(bool success, string error)
        {
            if (success)
            {
                addField.gameObject.SetActive(false);
                addField.text = "";
                friendsList.ReloadData();
            }
            else
            {
                addErrrorLabel.text = error;
            }
        }

        // button handlers     

        public void RefreshButtonHanlder()
        {
            AccountManager.Instance().GetFriends();
        }

        public void AddButtonHandler()
        {
            addField.gameObject.SetActive(true);
            addField.text = "";
        }
    }
}
