using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public bool moved;

    public Node fromNode;

    public Node toNode;

    public ProgressionLine connectedProgressionLine;


    public void UpdateLine()
    {
        connectedProgressionLine.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, gameObject.GetComponent<RectTransform>().sizeDelta.y / 2);
        connectedProgressionLine.SetTotalLength(gameObject.GetComponent<RectTransform>().sizeDelta.x);
        connectedProgressionLine.transform.localPosition = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x / 2, 5);
    }

    private void OnDestroy()
    {
        try
        {
            fromNode.ConnectedLines.Remove(this);
        }
        catch { }
    }

}
