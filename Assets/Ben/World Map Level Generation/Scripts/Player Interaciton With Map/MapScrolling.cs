using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapScrolling : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private Vector2 lastMousePos;

    public GameObject player;

    public static bool pressed;
    public bool moveable = false;

    Vector3 newScale;

    [Tooltip("How far the player can scroll the map")]
    public Vector2 maxOffset;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && moveable)
        {
            lastMousePos = eventData.position;
            pressed = true;
        }
        //D.Log(CD.Programmers.BEN, "Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && moveable)
        {
            Vector2 currMousePos = eventData.position;
            Vector2 diff = currMousePos - lastMousePos;

            Vector3 newPos = transform.position + (new Vector3(diff.x, diff.y, 0) * (0.5f * transform.localScale.x));

            transform.position = newPos;

            lastMousePos = currMousePos;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (pressed) pressed = !pressed;

        Vector2 currentLocationNormalized;

        bool xNeg = transform.localPosition.x < 0 ? true : false;
        bool yNeg = transform.localPosition.y < 0 ? true : false;

        currentLocationNormalized.x = xNeg ? transform.localPosition.x * -1 : transform.localPosition.x;
        currentLocationNormalized.y = yNeg ? transform.localPosition.y * -1 : transform.localPosition.y;

        if (currentLocationNormalized.x > maxOffset.x * transform.localScale.x * 0.75)
        {
            transform.localPosition = new Vector2(xNeg ? -maxOffset.x * transform.localScale.x : maxOffset.x * transform.localScale.x, transform.localPosition.y);
        }
        
        if (currentLocationNormalized.y > maxOffset.y * transform.localScale.y)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yNeg ? -maxOffset.y * transform.localScale.y : maxOffset.y * transform.localScale.y);
        }

    }


    private void OnEnable()
    {
        moveable = true;
        player.SetActive(true);

        transform.localPosition = Vector2.zero;

    }

    private void Update()
    {
        if (moveable)
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            newScale = new Vector3(wheel, wheel);
            transform.localScale += newScale;

            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + newScale.x, 1f, 3.5f), Mathf.Clamp(transform.localScale.y + newScale.y, 1f, 3.5f), 1);

        }

    }

    public void MapZoom(Vector2 startScale, Vector2 endScale, Vector2 targetLocation, float inTime, float initialWaitTime)
    {
        StartCoroutine(SlowZoom(startScale, endScale, targetLocation, inTime, initialWaitTime));
    }

    private IEnumerator SlowZoom(Vector2 startScale, Vector2 endScale, Vector2 targetLocation, float inTime, float initialWaitTime)
    {

        Vector2 currentLocation = transform.position;
        moveable = false;
        yield return new WaitForSecondsRealtime(initialWaitTime);

        for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            transform.position = Vector3.Lerp(currentLocation, targetLocation, t);
            yield return null;
        }

    }


}
