using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMap : MonoBehaviour
{

    public static PlayerMap Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMap>();
            }
            return instance;
        }
        private set { instance = value; }
    }

    private static PlayerMap instance;

    public bool clickable;

    /*
     * Generating a MapGeneration.Instance if there isn't any generated
     * This is the player for moving
     * Managing encounter Procs
     * Node Management / Selecting which node to travel to
     */
    [SerializeField] GameObject mapObject;
    //[SerializeField] Sprite image; // Image of the player to be moving around
    public MapScrolling mapScroll;
    public Node currentVisitingNode;

    public int pathIterations;

    public PlayerMapInformation mapInformation;


    public GameObject informationDisplay;
    [SerializeField] GameObject infoObj;
    [SerializeField] GameObject encounterUI;
    [SerializeField] GameObject eventSystem;
    [SerializeField] GameObject saveScreen;
    [SerializeField] GameObject loadScreen;

    private EncounterList encounterList;
    [SerializeField] private EncounterSO encounter;

    [SerializeField] private GameObject campDisplay;
    [SerializeField] private GameObject inventoryManager;
    [SerializeField] private GameObject dragObject;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject camp3D;


    [SerializeField]
    Camera cam;

    public string levelToLoadName;
    public string fileName;
    string playerFilePath;
    string mapFilePath;

    public StopStation currentStation;

    private Node nextNode;
    private ProgressionLine progressionLine;

    bool loading;
    public enum StopStation
    {
        Node,
        EncounterBefore,
        Camp,
        EncounterAfter,
        EncounterRewards
    }


    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Title Screen", LoadSceneMode.Single);
    }


    void OnEnable()
    {
        try
        {
            inventoryManager.SetActive(true);
            InventoryController.Instance.SetInstanceAgain(inventoryManager.GetComponent<InventoryController>());
        }
        catch { }

        //encounterUI.SetActive(false);
        //campDisplay.SetActive(false);

        if (currentVisitingNode == null)
        {
            // coming from bunker

            float difX = (Screen.width / 2) - transform.position.x;
            float difY = (Screen.height * 0.6f) - transform.position.y;
            Vector3 dif = new Vector2(difX, difY);
            Vector3 newTransform = mapObject.transform.position + dif;

            //mapScroll.MapZoom(new Vector2(1f, 1f), new Vector2(2f, 2f), newTransform, 3, 1);
        }

        cam.gameObject.SetActive(true);
        background.SetActive(true);
        eventSystem.SetActive(true);
        camp3D.SetActive(true);

        try
        {
            if (mapObject.GetComponentsInChildren<Transform>().Length <= 10)
            {
                // regenerate the map bc it wasn't generated apparently
                GenerateNewMap();
            }
        }
        catch { }

        mapObject.transform.Translate(Vector3.zero);
        try
        {
            ShowHideNodes();
        }
        catch { };

    }

    private void Awake()
    {
        // For when the sprite is inserted
        // if (GetComponent<Image>().sprite == null) GetComponent<Image>().sprite = image;

        mapScroll = mapObject.GetComponent<MapScrolling>();

        CD.Log(CD.Programmers.BEN, "LOADING NEW PLAYER OBJ");

        playerFilePath = Application.persistentDataPath + "/PlayerMap.data";
        mapFilePath = Application.persistentDataPath + "/" + fileName + ".data";

        if (File.Exists(playerFilePath) && File.Exists(mapFilePath))
        {
            // Ask Player if they want to load from file
            // To be changed to title screen for easier data? (I guess)
            AskLoadScreen();
        }
        else
        { // No Save Files, so make new map
            ClearMapObj();
            GenerateNewMap();
        }

        // Load encounters
        encounterList = new EncounterList();
    }

    private void ClearMapObj()
    {
        foreach (Transform t in mapObject.GetComponentsInChildren<Transform>())
        {
            if (t != null && t != mapObject)
            {
                if (!t.gameObject.Equals(gameObject) && !t.gameObject.name.Equals("Background") && !t.gameObject.name.Equals("DragObject") && !t.gameObject.name.Equals("Map") && !t.gameObject.name.Equals("Legend"))
                {
                    if (!t.parent.gameObject.name.Equals("Legend"))
                    {
                        Destroy(t.gameObject);
                    }
                }
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        instance = this;
    }

    private void AskLoadScreen()
    {
        loadScreen.SetActive(true);
    }

    public void LoadNewMap()
    {
        loadScreen.SetActive(false);
        GenerateNewMap();

    }

    public void LoadFromFile()
    {
        loadScreen.SetActive(false);

        // Player File
        FileStream dataStream = new FileStream(playerFilePath, FileMode.Open);

        BinaryFormatter converter = new BinaryFormatter();
        mapInformation = converter.Deserialize(dataStream) as PlayerMapInformation;

        dataStream.Close();

        // Map File
        dataStream = new FileStream(mapFilePath, FileMode.Open);
        MapGeneration.Instance = new MapGeneration(
            converter.Deserialize(dataStream) as MapData,
            Resources.Load("Ben/Start Node"),
            Resources.Load("Ben/End Node"),
            Resources.Load("Ben/Node"),
            Resources.Load("Ben/Path"),
            Resources.Load("Ben/ProgressionPath"),
            mapObject,
            this,
            mapFilePath);

        dataStream.Close();
        mapObject.GetComponentInParent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        StartCoroutine(CheckMapLoaded());
    }

    //public void SpawnNode(Vector3 locaiton)

    private IEnumerator CheckMapLoaded()
    {
        yield return new WaitForSeconds(0.5f);

        while (!MapGeneration.Instance.MapGenerated)
        {
            yield return new WaitForSeconds(0.5f);

        }
        // Map has loaded, place the player on the node that they left at
        foreach (GameObject g in MapGeneration.Instance.NodeList)
        {
            if (g.GetComponent<Node>().Information.nodeNum.Equals(mapInformation.currentNodeNum))
            {
                transform.position = g.transform.position;
                currentVisitingNode = g.GetComponent<Node>();
            }
        }

        ShowHideNodes();


        StartCoroutine(SpreadPoints());

    }

    private IEnumerator SpreadPoints()
    {
        mapObject.GetComponentInParent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        mapObject.GetComponentInParent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);

        // wait a second
        yield return null;

        // Spread them out now...
        MapGeneration.Instance.SpreadPoints(1f);


        dragObject.transform.SetAsFirstSibling();
        background.transform.SetAsFirstSibling();
        transform.SetAsLastSibling();

        //try
        //{
        //    if (mapObject.GetComponentsInChildren<Transform>().Length <= 7)
        //    {
        //        // regenerate the map bc it wasn't generated apparently
        //        GenerateNewMap();
        //        yield break;
        //    }
        //}
        //catch { }



        if (!loading)
        {
            loading = true;
            MoveToBunker();
        }
        

    }

    bool debugNodes = false;
    [SerializeField] GameObject debugText;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F7))
        //{
        //    // This enables Debug such that Daniel can access any node without getting any encounters/camp
        //    debugNodes = !debugNodes;
        //    debugText.SetActive(!debugText.activeInHierarchy);
        //    ShowHideNodes();
        //}
    }

    public void SaveMap()
    {
        MapGeneration.Instance.SaveMap();
        SavePlayerMap();
    }

    void SavePlayerMap()
    {
        FileStream dataStream = new FileStream(playerFilePath, FileMode.Create);

        PlayerMapInformation mapInformation = new PlayerMapInformation();

        mapInformation.currentNodeNum = currentVisitingNode.Information.nodeNum;

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, mapInformation);

        dataStream.Close();

        CD.Log(CD.Programmers.BEN, "Save Location: " + playerFilePath);
    }

    public void Close()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
// TODO change to back to title screen
        Application.Quit();
#endif
    }

    private void ShowSaveScreen()
    {
        //saveScreen.SetActive(true);
    }

    private void HideSaveScreen()
    {
        saveScreen.SetActive(false);
    }

    // Clicked a Node, need to check if it's a node they can travel to
    public void CheckClickedNode(Node nodeClicked)
    {

        if (debugNodes)
        {
            currentStation = StopStation.Node;
            // Go to that node no questions asked
            if (nodeClicked == MapGeneration.Instance.EndNode || nodeClicked.CompareTag("EndNode"))
            {
                NextLoop();
            }
            else
            {
                nextNode = nodeClicked;
                MoveTo3D();
            }

            return;
        }

        // Can't revisit currently visited node
        if (nodeClicked.Equals(currentVisitingNode)) return;

        // Check the clicked node's from node line
        foreach (Line line in nodeClicked.ConnectedLines)
        {
            if (line.fromNode.Equals(currentVisitingNode))
            {

                //nextNode = nodeClicked;
                //progressionLine = FindConnectedLine(nextNode).connectedProgressionLine;
                //if (nextNode != MapGeneration.Instance.EndNode)
                //    progressionLine.functionToCall += MoveTo3D;
                //else
                //{
                //    progressionLine.functionToCall += NextLoop;
                //}
                //progressionLine.ProgressTo(StopStation.Node);

                //break;

                clickable = false;

                if (encounterList == null) encounterList = new EncounterList();
                // Generate encounter from the before list
                encounter = encounterList.CheckEncounterProc(AnswerType.WhereItHappens.BeforeCamp);

                nextNode = nodeClicked;
                progressionLine = FindConnectedLine(nextNode).connectedProgressionLine;

                if (encounter == null)
                {
                    // No encounter
                    currentStation = StopStation.Camp;
                    progressionLine.functionToCall += MoveToCamp;
                    progressionLine.ProgressTo(StopStation.Camp);

                }
                else
                {
                    // Encounter happened
                    currentStation = StopStation.EncounterBefore;
                    progressionLine.functionToCall += MoveToEncounter;
                    progressionLine.ProgressTo(StopStation.EncounterBefore);
                }

                break;
            }
        }
    }

    public void ContinueAfterCheck(StopStation whereTheCheckHappened)
    {
        //Vector3 dif = currentVisitingNode.transform.position - transform.position;
        Line l;

        CD.Log(CD.Programmers.BEN, $"{currentStation}, where?: {whereTheCheckHappened}");

        try
        {
            progressionLine.functionToCall -= MoveTo3D;
        }
        catch { }
        

        switch (whereTheCheckHappened)
        {

            case StopStation.EncounterBefore:
                // Move to camp
                currentStation = StopStation.Camp;
                progressionLine.functionToCall += MoveToCamp;
                progressionLine.ProgressTo(StopStation.Camp);
                break;
            case StopStation.EncounterAfter:
                // Move to node
                currentStation = StopStation.Node;
                progressionLine.functionToCall += MoveTo3D;
                progressionLine.ProgressTo(StopStation.Node);
                break;
            case StopStation.Camp:
                // Check if encounter already happened
                if (encounter == null)
                {
                    encounter = //null;
                    // Uncomment this to enable encounters
                        encounterList.CheckEncounterProc(AnswerType.WhereItHappens.AfterCamp);
                    if (encounter == null)
                    {
                        // No encounter - Move to Node
                        currentStation = StopStation.Node;
                        progressionLine.functionToCall += MoveTo3D;
                        progressionLine.ProgressTo(StopStation.Node);

                    }
                    else
                    {
                        // Encounter happened - Move to (50%)
                        currentStation = StopStation.EncounterAfter;
                        progressionLine.functionToCall += MoveToEncounter;
                        progressionLine.ProgressTo(StopStation.EncounterAfter);
                    }
                }
                else
                {
                    // Encounter already happened - Move to Node
                    currentStation = StopStation.Node;
                    if (nextNode != MapGeneration.Instance.EndNode || !nextNode.CompareTag("EndNode"))
                    {
                        // ARE WE COMPLETELY SURE THAT THIS IS NOT THE END NODE!!!
                        if (hasNextLine(nextNode))
                            progressionLine.functionToCall += MoveTo3D;
                        else
                            progressionLine.functionToCall += NextLoop;
                    }
                    else
                    {
                        // Get the player to the bunker to relax for a bit
                        progressionLine.functionToCall += NextLoop;
                    }

                    progressionLine.ProgressTo(StopStation.Node);
                }
                break;
        }
    }

    private bool hasNextLine(Node node)
    {

        foreach (Line line in node.ConnectedLines)
        {
            if (line.fromNode.Equals(node))
            {
                return true;
            }
        }

        return false;
    }

    private void MoveToCamp()
    {
        campDisplay.SetActive(true);
        mapObject.SetActive(false);

        progressionLine.functionToCall -= MoveToCamp;

    }

    private void MoveToEncounter()
    {
        EncounterManager.Instance.EncounterGO = encounterUI.GetComponent<EncounterUI>();
        EncounterManager.Instance.ShowEncounter(encounter, currentStation);
        mapObject.SetActive(false);

        progressionLine.functionToCall -= MoveToEncounter;

    }

    private void MoveTo3D()
    {

        currentVisitingNode.Interacted(false);

        currentVisitingNode = nextNode;
        currentVisitingNode.Interacted(true);
        nextNode.Information.visited = true;

        inventoryManager.SetActive(false);
        background.SetActive(false);
        camp3D.SetActive(false);
        SpawnManager.SetSpawnSettings(currentVisitingNode);

        //progressionLine.functionToCall -= MoveTo3D;

        clickable = true;

        // Load Justin Scene
        StartCoroutine(SceneLoading(levelToLoadName));

    }

    public void MoveToBunker()
    {
        
        inventoryManager.SetActive(false);
        background.SetActive(false);
        CD.Log(CD.Programmers.BEN, "BUNKER MOVE!");
        clickable = true;
        // Load Bunker Scene

        try
        {
            progressionLine.functionToCall -= NextLoop;
        }
        catch { }

        loading = true;
        StartCoroutine(SceneLoading("Bunker"));
    }

    public void InformationDisplay(Vector3 location, int food, int pharm, int surv)
    {

        location -= Vector3.forward;

        if (informationDisplay != null)
        {
            informationDisplay.transform.position = location;
        }
        else
        {
            informationDisplay = Instantiate(infoObj, location, Quaternion.identity, mapObject.transform);
            //informationDisplay.transform.SetAsFirstSibling();
            informationDisplay.transform.localScale = new Vector2(informationDisplay.transform.localScale.x / mapObject.transform.localScale.x, informationDisplay.transform.localScale.y / mapObject.transform.localScale.y);
            informationDisplay.GetComponent<NodeInformationPopup>().UpdateNumbers(food, pharm, surv);
        }

    }
    public void DestroyInformation()
    {
        if (informationDisplay != null) Destroy(informationDisplay);
    }

    public void CentrePlayer()
    { // When player is done interacting with anything it moves the player into the centre of the screen, zoomed in
        mapObject.transform.localScale = new Vector2(2f, 2f);
        float difX = (Screen.width / 2) - currentVisitingNode.transform.position.x;
        float difY = (Screen.height / 2) - currentVisitingNode.transform.position.y;
        Vector3 dif = new Vector2(difX, difY);
        mapObject.transform.position += dif;

        ShowHideNodes();
    }


    public void NextLoop()
    {
        // Cory Do Shit
        endNodeInSight = false;
        Leave3D.currLoop += 1;
        GenerateNewMap();

    }

    public void GenerateNewMap()
    {
        StopAllCoroutines();
        mapObject.GetComponentInParent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        mapObject.GetComponentInParent<CanvasScaler>().referencePixelsPerUnit = 100;
        mapObject.GetComponentInParent<CanvasScaler>().scaleFactor = 1;
        StartCoroutine(ClearAndStartNewMap());

    }

    public static void ResetInstance()
    {
        Instance = null;
    }

    public bool ResetMap()
    {
        if (this == null)
        {
            MapGeneration.Instance = null;
            PlayerMap.Instance = null;
            return false;
        }
        if (mapObject == null)
        {
            mapObject = GetComponentInParent<Transform>().gameObject;
        }
        mapObject.transform.position = Vector3.zero;
        mapObject.transform.localScale = Vector3.one;
        mapObject.GetComponent<RectTransform>().rect.Set(0, 0, 100, 100);

        return true;
    }

    private IEnumerator ClearAndStartNewMap()
    {
        //try
        //{
        if (MapGeneration.Instance != null)
            MapGeneration.Instance.ResetAll();
        //}
        //catch { CD.Log(CD.Programmers.BEN, "Reset Map Error"); }


        yield return new WaitForSeconds(0.5f);
        MapGeneration.Instance = new MapGeneration(
            Resources.Load("Ben/Start Node"),
            Resources.Load("Ben/End Node"),
            Resources.Load("Ben/Node"),
            Resources.Load("Ben/Path"),
            Resources.Load("Ben/ProgressionPath"),
            mapObject,
            this,
            mapFilePath);

        MapGeneration.Instance.GenerateNewMap(pathIterations);

        // Setting the currentVisitingNode to the start node
        currentVisitingNode = MapGeneration.Instance.StartNode;
        currentVisitingNode.Information.visible = true;
        currentVisitingNode.Information.visited = true;
        mapScroll.moveable = true;
        transform.position = MapGeneration.Instance.StartNode.gameObject.transform.position;


        if (mapObject.GetComponentsInChildren<Transform>().Length <= 10)
        {
            // regenerate the map bc it wasn't generated apparently
            MapGeneration.Instance.ResetAll();
            CD.Log(CD.Programmers.BEN, "Resetting map because of not loading...");
            ClearMapObj();
            yield return new WaitForSeconds(0.5f);
            GenerateNewMap();
            yield break;
        }

        Node[] node = FindObjectsOfType<Node>();

        int nodeCount = 0; // This is for when map makes a black hole...

        foreach (Node n in node)
        {
            if (n.name.Equals("Start Node"))
            {
                nodeCount++;
                if (nodeCount > 1)
                {
                    MapGeneration.Instance.ResetAll();
                    CD.Log(CD.Programmers.BEN, "Resetting map because of black hole...");
                    ClearMapObj();
                    yield return new WaitForSeconds(0.5f);
                    GenerateNewMap();
                    yield break;
                }
            }
        }

        StartCoroutine(SpreadPoints());

    }

    bool endNodeInSight = false;

    // Show the next 2 layers of the available paths, hide the rest
    public void ShowHideNodes()
    { // Search through the available paths and find where the currently visited node is in each path. Make sure that the next two in the list are enabled, the rest hidden

        foreach (List<Node> nodeList in MapGeneration.Instance.Path)
        {
            foreach (Node n in nodeList)
            {
                if (n.Information.visible)
                {
                    if (!n.Equals(MapGeneration.Instance.EndNode && !endNodeInSight))
                        HideToLines(n);
                }
            }
        }

        MapGeneration.Instance.StartNode.Information.visible = true;
        MapGeneration.Instance.StartNode.Information.visited = true;

        if (currentVisitingNode == null) { currentVisitingNode = MapGeneration.Instance.StartNode; }

        transform.position = currentVisitingNode.transform.position;
        #region comments
        //for (int i = 0; i < MapGeneration.Instance.Path.Count; i++)
        //{
        //    int count = 0;
        //    bool found = false;
        //    for (int j = 1; j < MapGeneration.Instance.Path[i].Count; j++)
        //    {

        //        if (!MapGeneration.Instance.Path[i][j].Information.visited && !MapGeneration.Instance.Path[i][j].Equals(MapGeneration.Instance.StartNode)
        //            && !MapGeneration.Instance.Path[i][j].Information.visible && !MapGeneration.Instance.Path[i][j].Equals(MapGeneration.Instance.EndNode))
        //        { // If it's already set visible don't hide it (already iterated through prior path)
        //            //MapGeneration.Instance.Path[i][j].gameObject.SetActive(false);
        //            MapGeneration.Instance.PathLines[i][j - 1].SetActive(false);
        //        }

        //        if (count < 2)
        //        { // Making sure the count of visible next nodes are less than 2
        //            if (currentVisitingNode.Equals(MapGeneration.Instance.StartNode))
        //            { // Starting node, make sure we signal found instantly
        //                found = true;
        //            }

        //            if (found)
        //            { // Making the node visible, and corresponding path
        //              //if (!MapGeneration.Instance.Path[i][j].gameObject.activeInHierarchy)
        //              //{
        //              //CD.Log(CD.Programmers.BEN, "NODE SET VISIBEL");
        //                MapGeneration.Instance.Path[i][j].gameObject.SetActive(true);
        //                MapGeneration.Instance.Path[i][j].Information.visible = true;
        //                MapGeneration.Instance.PathLines[i][j - 1].SetActive(true);
        //                //}
        //                // Increment count for visible nodes
        //                count++;
        //                if (MapGeneration.Instance.Path[i][j].Equals(MapGeneration.Instance.EndNode))
        //                { // End node is in the next 2 which is visible
        //                    MapGeneration.Instance.EndNode.Information.visible = true;
        //                    MapGeneration.Instance.PathLines[i][j - 1].SetActive(true);

        //                }

        //                if (count == 1)
        //                {
        //                    MapGeneration.Instance.Path[i][j].Information.nextInLine = true;
        //                }
        //                else MapGeneration.Instance.Path[i][j].Information.nextInLine = false;

        //            }
        //            else if (MapGeneration.Instance.Path[i][j].Equals(currentVisitingNode))
        //            { // Found current node in path, now to make sure the next two are showing in the path
        //                found = true;
        //            }
        //            // Need to check if the past node is visible. If not hide the path between them
        //            if (!MapGeneration.Instance.Path[i][j - 1].Information.visible && !MapGeneration.Instance.Path[i][j - 1].Information.visited)
        //            {
        //                MapGeneration.Instance.PathLines[i][j - 1].SetActive(false);
        //            }
        //        }
        //        else
        //        {
        //            if (!MapGeneration.Instance.Path[i][j].Information.visible)
        //                if (!MapGeneration.Instance.Path[i][j].Equals(MapGeneration.Instance.EndNode))
        //                {
        //                    //MapGeneration.Instance.Path[i][j].gameObject.SetActive(false);
        //                    MapGeneration.Instance.PathLines[i][j - 1].SetActive(false);
        //                }
        //                else if (!MapGeneration.Instance.EndNode.Information.visible)
        //                {
        //                    MapGeneration.Instance.PathLines[i][j - 1].SetActive(false);
        //                }
        //        }
        //    }
        //}
        #endregion

        

        // Reformatting for the fuckers
        for (int i = 0; i < MapGeneration.Instance.Path.Count; i++)
        {
            List<Node> path = MapGeneration.Instance.Path[i];

            if (!debugNodes)
            {

                if (path.Contains(currentVisitingNode))
                {
                    int count = 0; // make sure that we only go 2 in front
                    bool found = false; // found it?

                    // iterate to find where in the path this node is
                    foreach (Node n in path)
                    {
                        if (count < 1)
                        {
                            if (found)
                            {
                                if (count < 1)
                                {
                                    if (n.Equals(MapGeneration.Instance.EndNode))
                                    {
                                        endNodeInSight = true;
                                        n.Information.visible = true;
                                    }
                                    ActivateFromLines(n);
                                    count++;
                                }
                            }
                            else
                            {
                                if (n.Equals(currentVisitingNode))
                                {
                                    found = true;
                                    ActivateFromLines(n);
                                    ShowPastLines(n);
                                }
                            }
                        }
                        else
                        {
                            // hide the rest
                            if (count < 2)
                            {
                                count++;
                                n.Information.visible = true;
                            }
                            else
                            {
                                HideToLines(n);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Node n in path)
                    {
                        if (!n.Information.visible)
                        {
                            if (!n.Equals(MapGeneration.Instance.EndNode || !MapGeneration.Instance.EndNode.Information.visible))
                            {
                                HideToLines(n);
                            }
                        }
                    }
                }



                if (InventoryController.Instance.HasMap())
                {
                    foreach (Node n in path)
                    {
                        if (!n.Information.visible)
                        {
                            if (!n.Equals(MapGeneration.Instance.EndNode || !MapGeneration.Instance.EndNode.Information.visible))
                            {
                                foreach (Line line in n.ConnectedLines)
                                {
                                    line.gameObject.SetActive(true);
                                    n.Information.visible = true;
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                foreach (Node n in path)
                {
                    if (!n.Information.visible)
                    {
                        foreach (Line line in n.ConnectedLines)
                        {
                            line.gameObject.SetActive(true);
                        }
                        n.Information.visible = true;
                    }
                }
            }



        }


    }

    private void ShowPastLines(Node node)
    {
        node.Information.nextInLine = false;
        foreach (Line line in node.ConnectedLines)
        {
            if (line.toNode.Equals(node))
            {
                if (line.fromNode.Information.visited)
                {
                    // Show this line
                    line.gameObject.SetActive(true);
                    ShowPastLines(line.fromNode);
                }
                else
                {
                    line.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ActivateFromLines(Node node)
    {
        foreach (Line line in node.ConnectedLines)
        {
            if (line.fromNode.Equals(node))
            {
                line.gameObject.SetActive(true);
                node.Information.nextInLine = true;
                node.Information.visible = true;

                if (node.Equals(currentVisitingNode))
                {
                    ActivateFromLines(line.toNode);
                }
                else
                {
                    line.toNode.Information.visible = true;
                }
            }
        }
    }

    private void HideToLines(Node node)
    {
        foreach (Line line in node.ConnectedLines)
        {
            if (line.toNode.Equals(node))
            {
                line.gameObject.SetActive(false);
                node.Information.visible = false;
                node.Information.nextInLine = false;
            }
        }
    }

    private IEnumerator SceneLoading(string levelName)
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        cam.gameObject.SetActive(false);
        eventSystem.SetActive(false);
        mapObject.transform.parent.gameObject.SetActive(false);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
        loading = false;

    }

    //private void MoveTo(StopStation stop)
    //{
    //    mapScroll.moveable = false;
    //    yield return new WaitForSeconds(1f);

    //    for (float t = 0f; t <= 1; t += Time.deltaTime / inTime)
    //    {
    //        transform.position = Vector3.Lerp(currentLocation, moveToLocation, t);
    //        yield return null;
    //    }
    //    mapScroll.moveable = true;

    //    yield return new WaitForSeconds(1f);

    //    switch (stop)
    //    {
    //        case StopStation.EncounterBefore:
    //            currentStation = StopStation.EncounterBefore;
    //            MoveToEncounter(StopStation.EncounterBefore);
    //            break;
    //        case StopStation.EncounterAfter:
    //            currentStation = StopStation.EncounterBefore;
    //            MoveToEncounter(StopStation.EncounterAfter);
    //            break;
    //        case StopStation.Camp:
    //            currentStation = StopStation.Camp;
    //            MoveToCamp();
    //            break;
    //        case StopStation.Node:
    //            currentStation = StopStation.Node;
    //            CentrePlayer();
    //            encounter = null;
    //            // Uncomment below to enable Moving to 3D scene
    //            MoveTo3D();
    //            // Uncomment above to enable Moving to 3D scene
    //            break;
    //    }

    //}


    private Line FindConnectedLine(Node nextNode)
    {
        // returns the line which connects the current node and nextNode
        foreach (Line line in currentVisitingNode.ConnectedLines)
        {
            if (line.fromNode.Equals(currentVisitingNode) && line.toNode.Equals(nextNode)) return line;
        }

        return null;
    }

    

}


[System.Serializable]
public class PlayerMapInformation
{
    public int currentNodeNum;
}