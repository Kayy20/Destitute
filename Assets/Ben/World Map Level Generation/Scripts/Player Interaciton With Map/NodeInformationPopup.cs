using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeInformationPopup : MonoBehaviour
{

    [SerializeField] Text foodNum, pharmNum, survNum;

    public void UpdateNumbers(int foodNum, int pharmNum, int survNum)
    {
        this.foodNum.text = "" + foodNum;
        this.pharmNum.text = "" + pharmNum;
        this.survNum.text = "" + survNum;
    }



}
