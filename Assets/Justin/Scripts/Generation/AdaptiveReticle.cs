using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveReticle : MonoBehaviour
{

    bool isIncreasing=true;

    [SerializeField]
    RectTransform reticleTransform;

    [SerializeField]
    float minScale, maxScale, normalSize;

    GameObject reticleGO;

    float scale;
    [SerializeField]
    float warpSpeed=0.2f;

    [SerializeField]
    PlayerItemInteraction pii;

    private void Awake()
    {
        scale = normalSize;
        reticleGO = reticleTransform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (InventoryController.Instance.IsShowing)
            reticleGO.SetActive(false);
        else
            reticleGO.SetActive(true);


        if (pii.CanInteract)
        {

            if (isIncreasing)
            {

                scale += Time.deltaTime*warpSpeed;

                if(scale >= maxScale)
                {
                    scale = maxScale;
                    isIncreasing = false;
                }

            }
            else
            {
                scale -= Time.deltaTime * warpSpeed;

                if (scale <= minScale)
                {
                    scale = minScale;
                    isIncreasing = true;
                }

            }

        }
        else
        {
            scale = normalSize;
        }

        reticleTransform.localScale = new Vector3(scale, scale, scale);

    }
}
