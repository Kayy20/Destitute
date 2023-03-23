using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionLine : MonoBehaviour
{

    public Action functionToCall;

    public float totalLength;
    
    public void SetTotalLength(float length)
    {
        totalLength = length;
    }

    public void ProgressTo(PlayerMap.StopStation place)
    {
        // this will either be beforeEncounter, camp, afterEncounter, or Node (This will mean next node)
        GetComponent<Image>().enabled = true;
        //CD.Log(CD.Programmers.BEN, "Coroutine moving?");
        StartCoroutine(progressLineMove(place));
        
    }

    private IEnumerator progressLineMove(PlayerMap.StopStation place, float inTime = 2.5f)
    {
        //CD.Log(CD.Programmers.BEN, "Coroutine Activated!");
        RectTransform r = gameObject.GetComponent<RectTransform>();

        float currentLength = r.sizeDelta.x;
        Vector3 currentPosition = transform.localPosition;


        //CD.Log(CD.Programmers.BEN, $"Place: {place}");

        yield return new WaitForSeconds(1f);

        //CD.Log(CD.Programmers.BEN, $"Place: {place} #2");

        float dividerLoc = 1;
        float dividerLen = 1;
        switch (place)
        {
            case PlayerMap.StopStation.Node:
                dividerLoc = 0;
                dividerLen = 1;
                break;
            case PlayerMap.StopStation.EncounterBefore:
                dividerLoc = 0.75f;
                dividerLen = 0.25f;
                break;
            case PlayerMap.StopStation.Camp:
                dividerLoc = 0.5f;
                dividerLen = 0.5f;
                break;
            case PlayerMap.StopStation.EncounterAfter:
                dividerLoc = 0.25f;
                dividerLen = 0.75f;
                break;
        }

        //float targetLength = totalLength * divider;
        float targetLength = totalLength * dividerLen;
        Vector3 targetPosition = new Vector3(totalLength * dividerLoc / 2, transform.localPosition.y);
        

        float updatedLength = currentLength;

        //CD.Log(CD.Programmers.BEN, $"Place: {place}, targetLength: {targetLength}, targetPosition: {targetPosition}");


        for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            //CD.Log(CD.Programmers.BEN, $"t: {t}");
            updatedLength = Mathf.Lerp(currentLength, targetLength, t);
            r.sizeDelta = new Vector2(updatedLength, r.sizeDelta.y);
            transform.localPosition = Vector2.Lerp(currentPosition, targetPosition, t);
            // Need to move the line half the targetPos
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //CD.Log(CD.Programmers.BEN, "Invoke!!");
        functionToCall.Invoke();

    }

}
