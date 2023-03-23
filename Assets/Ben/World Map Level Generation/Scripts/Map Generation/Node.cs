using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerExitHandler
{

    public string name;

    [SerializeField] List<Node> connectedNodes = new List<Node>();

    public NodeInformation Information;

    public bool moved;
    public Vector2 Location
    {
        get { return location; }
        set
        {
            location = value;
            Information.x = (int)location.x;
            Information.y = (int)location.y;
        }
    }

    Vector2 location;

    public Node cameFromNode;

    public AudioSoundSO sound;

    public PlayerMap Player { get; set; }

    public List<Line> ConnectedLines = new List<Line>();

    [SerializeField] GameObject houseImage;
    [SerializeField] GameObject XOImage;
    [SerializeField] Sprite xImage, oImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.mapScroll.moveable && Player.clickable)
        {

            Debug.Log($"NODE IS {gameObject.name}");
            SoundController.PlaySound(sound);
            Player.CheckClickedNode(this);
        }
    }

    private void OnDestroy()
    {
        foreach (Line fucker in ConnectedLines)
        {
            Destroy(fucker.gameObject);
        }
    }

    public List<Node> GetConnectedNodes()
    {
        return connectedNodes;
    }

    public void addNode(Node node)
    {
        connectedNodes.Add(node);
    }

    public void removeNode(Node node)
    {
        connectedNodes.Remove(node);
    }

    public void ClearPath()
    {
        cameFromNode = null;
    }

    public void CalculateFCost()
    {
        Information.fCost = Information.gCost + Information.hCost;
    }

    public void ResetNodeList()
    {
        connectedNodes.Clear();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (!MapScrolling.pressed)
           if ( InventoryController.Instance.HasMap() || Information.nextInLine)
            {
                Player.InformationDisplay(eventData.position, Information.Food, Information.Medical, Information.Survival);
            }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!MapScrolling.pressed)
        {
            Player.DestroyInformation();
        }
    }

    public void Interacted(bool leaving)
    {
        // Sets the image, if leaving = true : turn to X, else make it an O
        if (!XOImage.GetComponent<Image>().enabled)
        {
            XOImage.GetComponent<Image>().enabled = true;
        }

        XOImage.GetComponent<Image>().sprite = leaving ? oImage : xImage;
        Information.playerHere = leaving;
    }
    
    //private void Update()
    //{
    //    // Animating if this is where player resides
    //    if (Information.nextInLine && !Information.playerHere)
    //    {
    //        // Make it larger/smaller animation
    //        if (gettingBigger)
    //        {
    //            if (transform.localScale.x < 1.05)
    //            {
    //                transform.localScale += new Vector3(0.25f, 0.25f) * Time.deltaTime;
    //            }
    //            else
    //            {
    //                gettingBigger = false;
    //            }
    //        }
    //        else
    //        {
    //            if (transform.localScale.x > .95)
    //            {
    //                transform.localScale -= new Vector3(0.25f, 0.25f) * Time.deltaTime;
    //            }
    //            else
    //            {
    //                gettingBigger = true;
    //            }
    //        }
            
    //    }
    //}

}

[System.Serializable]
public class NodeInformation
{
    public int x;
    public int y;

    public int gCost = 0;
    public int hCost = 0;
    public int fCost = 0;

    public bool accessed;
    public bool visited;
    public bool visible;
    public bool resourcesGiven;
    public bool nextInLine;

    public bool playerHere;

    public int nodeNum;

    // Folling names to be changed with confirmation on what they acutally are
    //-----------------------------------
    public int Food { get { return food; } set { food = value; } }
    int food;
    public int Medical { get { return medical; } set { medical = value; } }
    int medical;
    public int Survival { get { return survival; } set { survival = value; } }
    int survival;
    //------------------------------------
}
