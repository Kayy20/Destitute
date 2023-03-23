using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuableManager : MonoBehaviour
{
    [SerializeField] GameObject valPanel;
    [SerializeField] Text valText;

    private int pointsPerVal = 25;

    private void Start()
    {
        StartCoroutine(StartDelay());
    }

    private void ShowValuableText() {
        int numValuables = InventoryController.Instance.RemoveValuables();

        StaticClassVariables.score += numValuables * pointsPerVal;
        
        valText.text = $"Your valuables have been stashed.";
        if (numValuables > 0) {
            StartCoroutine(ValPanelDelay());
        }


    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.25f);
        ShowValuableText();
    }

    IEnumerator ValPanelDelay() {

        if (MovementController.Instance) {
            MovementController.Instance.canMove = false;
        }

        valPanel.SetActive(true);

        yield return new WaitForSeconds(5);
        if (MovementController.Instance)
        {
            MovementController.Instance.canMove = true;
        }

        valPanel.SetActive(false);
    }
}
