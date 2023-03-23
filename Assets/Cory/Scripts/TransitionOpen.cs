using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TransitionOpen : MonoBehaviour
{
    [SerializeField]
    public Image transitionScreen;
    public static bool fade = false;
    // Start is called before the first frame update
    void Start()
    {
        var tempColor = transitionScreen.color;
        transitionScreen.color = tempColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade == true)
        {
            StartCoroutine("fadeIn");
            fade = false;
        }
    }

    public IEnumerator fadeIn ()
    {
        transitionScreen.enabled = true;
        // loop over 1 second backwards
        for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                transitionScreen.color = new Color(0, 0, 0, i);
                yield return null;
            }

            transitionScreen.enabled = true;
        }
    
}
