using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatTableView : MonoBehaviour
{

    [SerializeField]
    private GameObject cell;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private float cellHeight;
    [SerializeField]
    private float offset;

    private List<string> chat;

    private void Start()
    {
        Debug.Log(cell.transform.localPosition);
    }

    public void SetChat(List<string> list)
    {
        chat = list;
        ReloadData();
    }

    public void ReloadData()
    {
        Transform[] children = content.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; ++i)
            Destroy(children[i].gameObject);

        for (int i = 0; i < chat.Count; ++i)
        {
            Vector3 position = cell.transform.localPosition;
            position.y = (i + 1) * -cellHeight + offset;

            Transform newCell = Instantiate(cell).transform;
            newCell.gameObject.SetActive(true);
            newCell.SetParent(content);
            newCell.localPosition = position;
            newCell.GetComponentInChildren<Text>().text = chat[i];
        }

        content.sizeDelta = new Vector2(0, (float)(chat.Count + 1) * cellHeight + offset);
    }

}
