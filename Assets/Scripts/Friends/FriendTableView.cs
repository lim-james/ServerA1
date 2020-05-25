using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendTableView : MonoBehaviour
{

    [SerializeField]
    private GameObject cell;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private float cellHeight;
    [SerializeField]
    private float offset;

    private List<string> friends;

    private void Start()
    {
        Debug.Log(cell.transform.localPosition);
    }

    public void SetFriends(List<string> list)
    {
        friends = list;
        ReloadData();
    }

    public void ReloadData()
    {
        Transform[] children = content.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; ++i)
            Destroy(children[i].gameObject);

        for (int i = 0; i < friends.Count; ++i)
        {
            Vector3 position = cell.transform.localPosition;
            position.y = (i + 1) * -cellHeight + offset;

            Transform newCell = Instantiate(cell).transform;
            newCell.gameObject.SetActive(true);
            newCell.SetParent(content);
            newCell.localPosition = position;
            newCell.GetComponentInChildren<Text>().text = friends[i];
        }

        content.sizeDelta = new Vector2(0, (float)(friends.Count + 1) * cellHeight + offset);
    }

}
