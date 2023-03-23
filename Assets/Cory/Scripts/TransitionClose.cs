using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TransitionClose : MonoBehaviour
{
    [SerializeField]
    public Image transitionScreen;
    bool fade = true;
    // Start is called before the first frame update
    void Start()
    {
        var tempColor = transitionScreen.color;
        transitionScreen.color = tempColor;
        StartCoroutine("fadeIn");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator fadeIn()
    {
        if (fade)
        {
            // loop over 1 second backwards
            for (float i = 2; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                transitionScreen.color = new Color(0, 0, 0, i);
                yield return null;
            }

            transitionScreen.enabled = false;
        }
    }

}
