using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Camp : MonoBehaviour
{

    public static Camp Instance;

    [SerializeField] GameObject map;
    public bool water, food, woundCured;
    public Text waterT, foodT, continueButtonText, woundT;
    public GameObject woundImageObj;
    public Sprite woundImage;
    public List<GameObject> woundImages;
    public GameObject woundParent;
    public List<GameObject> tutorialTexts; 
    /*
     * 0 = Select Food / Water
     * 1 = Select Continue Button
     * 2 = Wound Descriptor
     * 3 = Select Medkit
     */

    public bool tutorialShownAlready;
    public bool woundTutorialShown;

    private void OnEnable()
    { // Meaning it's time to activate the check;
        InventoryController.Instance.ToggleCampUI(true);
        CampController.Instance.SelectedFood += FoodSelected;
        CampController.Instance.SelectedWater += WaterSelected;
        waterT.color = Color.gray;
        foodT.color = Color.gray;
        Leave3D.currLevelNumber += 1;
        FollowerObjScript.Speed += 1;


        if (!tutorialShownAlready)
        {
            tutorialTexts[0].SetActive(true);
        }

        if (StaticClassVariables.Wounds >= 3)
        {
            // Lose
            /*
            SceneManager.LoadSceneAsync("Title Screen");
            SceneManager.UnloadSceneAsync("MapScene");
            */
            ResetA();
            InventoryController.Instance.ToggleCampUI(false);
            gameObject.SetActive(false);

            map.SetActive(true);

            EndScreen.Instance.ShowEndScreen(CauseofDeath.Wounds);
        }

        CD.Log(CD.Programmers.BEN, $"Wounds: {StaticClassVariables.Wounds}");

        if (StaticClassVariables.Wounds > 0)
        {
            woundT.gameObject.SetActive(true);
            UpdateWoundImages();

            if (!woundTutorialShown)
            {
                tutorialTexts[2].SetActive(true);
                tutorialTexts[3].SetActive(true);
            }

        }
        

    }

    public void OnClickContinue()
    {
        if (food && water)
        {
            StartCoroutine(DelayTest());
        }
        else
        {
            // Lose, go to main screen..
            /*
            SceneManager.LoadSceneAsync("Title Screen");
            SceneManager.UnloadSceneAsync("MapScene");
            */
            ResetA();
            InventoryController.Instance.ToggleCampUI(false);
            gameObject.SetActive(false);

            map.SetActive(true);

            MapGeneration.Instance.ResetAll();

            if (water)
                EndScreen.Instance.ShowEndScreen(CauseofDeath.Hunger);
            EndScreen.Instance.ShowEndScreen(CauseofDeath.Thirst);

        }
    }

    private IEnumerator DelayTest()
    {

        if (!tutorialShownAlready && tutorialTexts[1].activeInHierarchy)
        {
            tutorialTexts[1].SetActive(false);
            tutorialShownAlready = true;
        }
        if (!woundTutorialShown && tutorialTexts[2].activeInHierarchy)
        {
            tutorialTexts[2].SetActive(false);
            tutorialTexts[3].SetActive(false);
            woundTutorialShown = true;
        }

        // Move to Inventory Management
        ResetA();
        InventoryController.Instance.DeleteCampItems();
        //InventoryController.Instance.ToggleCampUI(false);
        yield return new WaitForSeconds(0.25f);
        //InventoryController.Instance.ToggleCampUI(true);
        //GetComponent<WoundCamp>().UpdateWoundImages();

        InventoryController.Instance.ToggleCampUI(false);
        gameObject.SetActive(false);

        map.SetActive(true);
        PlayerMap player = map.GetComponentInChildren<PlayerMap>();
        player.ContinueAfterCheck(PlayerMap.StopStation.Camp);

    }

    void FoodSelected()
    {
        food = true;
        foodT.color = Color.white;

        if (tutorialTexts[0].activeInHierarchy)
        {
            tutorialTexts[0].SetActive(false);
            tutorialTexts[1].SetActive(true);
        }
    }

    void WaterSelected()
    {
        water = true;
        waterT.color = Color.white;

        if (tutorialTexts[0].activeInHierarchy)
        {
            tutorialTexts[0].SetActive(false);
            tutorialTexts[1].SetActive(true);
        }
    }

    public void UpdateWoundImages(bool updateStatic = false)
    {
        if (updateStatic)
        {
            for (int i = 0; i < woundImages.Count; i++)
            {
                Destroy(woundImages[i]);
            }

            woundImages.Clear();

            woundCured = true;


            if (StaticClassVariables.Wounds == 0)
            {
                woundT.gameObject.SetActive(false);
            }

        }


        for (int i = 0; i < StaticClassVariables.Wounds; i++)
        {
            // draw stuff
            GameObject g = Instantiate(woundImageObj, woundParent.transform);
            g.GetComponent<Image>().sprite = woundImage;
            woundImages.Add(g);
        }
    }


    private void ResetA()
    {
        food = false;
        water = false;
        woundCured = false;

        foreach (GameObject g in woundImages)
            Destroy(g);
        woundImages.Clear();

        woundT.gameObject.SetActive(false);

        CampController.Instance.SelectedFood -= FoodSelected;
        CampController.Instance.SelectedWater -= WaterSelected;
    }

    void Awake()
    {
        Instance = this;
    }

}
