using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MapGeneration
{
    private static MapGeneration instance;
    public static MapGeneration Instance { get { return instance; } set { instance = value; } }

    public float radius = 30;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 10;

    //public int pathIterations = 10;

    bool resetting;

    public string filePath;

    public List<List<Node>> Path { get { return path; } }
    public List<List<GameObject>> PathLines { get { return pathLines; } }

    List<List<GameObject>> pathLines = new List<List<GameObject>>();
    List<List<Node>> path = new List<List<Node>>();

    List<Vector2> points;

    private AStarPath pathfinding;

    Object pathGO, progPathGO;

    Object startNode, endNode, mapNode;
    GameObject startGO, endGO;
    // For accessing startGO and endGO

    public Node StartNode { 
        get 
        { 
            if (startGO == null)
            {
                startGO = (GameObject)GameObject.Instantiate(startNode, mapGO.transform);
                startGO.GetComponent<Node>().Player = Player;
            }
            return startGO.GetComponent<Node>(); 
        } 
    }
    public Node EndNode { 
        get 
        {
            if (endGO == null)
            {
                endGO = (GameObject)GameObject.Instantiate(endNode, mapGO.transform);
                endGO.GetComponent<Node>().Player = Player;
            }
            return endGO.GetComponent<Node>();
        } 
    }

    public PlayerMap Player { get { return player; } set { player = value; } }
    private PlayerMap player;
    public List<GameObject> NodeList { get { return nodeList; } }
    List<GameObject> nodeList = new List<GameObject>();

    GameObject mapGO;

    public bool MapGenerated { get; set; }

    public void DrawStartNode()
    {
        startGO = (GameObject)GameObject.Instantiate(startNode, mapGO.transform);
        startGO.GetComponent<Node>().Player = Player;
        endGO = (GameObject)GameObject.Instantiate(endNode, mapGO.transform);
        endGO.GetComponent<Node>().Player = Player;

        GameObject.Instantiate(mapNode, mapGO.transform);
    }

    public void ReturnToMap(string sceneName)
    {
        // Here is where the map comes back into play
        SceneManager.UnloadSceneAsync(sceneName); // Unloads the scene

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mapGO.transform.parent.gameObject.SetActive(true);
    }

    // Setting values, made for making a new map
    public MapGeneration (Object startNode, Object endNode, Object mapNode, Object pathGO, Object progPathGO, GameObject mapGO, PlayerMap player, string fileName)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.mapNode = mapNode;
        this.pathGO = pathGO;
        this.mapGO = mapGO;
        this.player = player;
        this.progPathGO = progPathGO;
        this.filePath = fileName;

    }
    
    // Made for loading from save file
    public MapGeneration (MapData data, Object startNode, Object endNode, Object mapNode, Object pathGO, Object progPathGO, GameObject mapGO, PlayerMap player, string fileName)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.mapNode = mapNode;
        this.pathGO = pathGO;
        this.mapGO = mapGO;
        this.player = player;
        this.progPathGO = progPathGO;
        this.filePath = fileName;
        LoadMap(data);
    }

    #region RandomPointGeneration
    void DrawMap()
    {
        int count = 0;
        if (points != null)
        {
            //CD.Log(CD.Programmers.BEN, "Drawing Map");

            Vector2 startV = new Vector2(startGO.transform.position.x, startGO.transform.position.y);
            Vector2 endV = new Vector2(regionSize.x / 2, regionSize.y);

            //CD.Log(CD.Programmers.BEN, startNode.transform.position.x + ", " + startNode.transform.position.y);
            startGO.GetComponent<Node>().Location = new Vector2(startGO.transform.position.x, startGO.transform.position.y);
            endGO.GetComponent<Node>().Location = new Vector2(endGO.transform.position.x, endGO.transform.position.y);

            startGO.transform.SetAsFirstSibling();
            endGO.transform.SetAsFirstSibling();


            nodeList.Add(startGO);
            nodeList.Add(endGO);

            foreach (Vector2 point in points)
            {
                //CD.Log(CD.Programmers.BEN, $"point {point}");
                GameObject node = (GameObject)GameObject.Instantiate(mapNode, mapGO.transform);
                node.name = "Node_#" + count;
                node.GetComponent<Node>().Player = Player;
                count++;

                Vector3 loc = new Vector3(point.x - regionSize.x / 2, point.y - regionSize.y / 2, 0);

                node.transform.Translate(loc);

                node.transform.SetAsFirstSibling();

                //CD.Log(CD.Programmers.BEN, startV + " " + endV + ". " + point);

                // Not inside radius of spawn/end
                if ((point.x > startV.x - radius && point.x < startV.x + radius && point.y > startV.y - radius && point.y < startV.y + radius)
                    && (point.x > endV.x - radius && point.x < endV.x + radius && point.y > endV.y - radius && point.y < endV.y + radius))
                {
                    //GameObject.Destroy(node);
                }
                else
                {
                    nodeList.Add(node);
                    node.GetComponent<Node>().Location = new Vector2(node.transform.position.x, node.transform.position.y);
                }

            }
        }
    }

    public void SpreadPoints(float scalar)
    {

        foreach (GameObject n in nodeList)
        {
            if (n != null)
            {
                if (!n.GetComponent<Node>().moved)
                {
                    n.transform.Translate(n.transform.localPosition * scalar);
                    n.GetComponent<Node>().Location = n.transform.localPosition;
                    n.GetComponent<Node>().moved = true;
                }
                
            }
        }

        foreach (List<GameObject> nList in pathLines)
        {
            foreach (GameObject n in nList)
            {
                if (n != null)
                {
                    if (!n.GetComponent<Line>().moved)
                    {
                        Line line = n.GetComponent<Line>();
                        n.GetComponent<RectTransform>().sizeDelta = new Vector2(FindHypotenuse(line.fromNode.Location, line.toNode.Location) * 1.75f, 20);
                        n.transform.localPosition = new Vector3(
                            line.fromNode.Location.x - ((line.fromNode.Location.x - line.toNode.Location.x) / 2),
                            line.fromNode.Location.y - ((line.fromNode.Location.y - line.toNode.Location.y) / 2),
                            0
                            );
                        n.GetComponent<Line>().moved = true;
                        n.GetComponent<Line>().UpdateLine();
                    }
                }
            }
        }

        Player.ShowHideNodes();

        //int count1 = PathLines.Count;
        //for (int i = 0; i < count1; i++)
        //{
        //    int count2 = PathLines[i].Count;
        //    for (int j = 0; j < count2; j++)
        //    {
        //        GameObject.Destroy(PathLines[i][0]);
        //        pathLines[i].RemoveAt(0);
        //    }
        //}


        //DrawRoutes();

    }

    public void GenerateNewMap(int pathIterations)
    {
        // Instantiate start and end nodes if they aren't existing
        if (endGO == null)
        {
            endGO = (GameObject)GameObject.Instantiate(endNode, mapGO.transform);
            endGO.GetComponent<Node>().Player = Player;
        }
        if (startGO == null)
        {
            startGO = (GameObject)GameObject.Instantiate(startNode, mapGO.transform);
            startGO.GetComponent<Node>().Player = Player;
        }

        if (nodeList.Count > 0)
        {
            if (nodeList.Contains(startGO)) nodeList.Remove(startGO);
            if (nodeList.Contains(endGO)) nodeList.Remove(endGO);


            startGO.GetComponent<Node>().ResetNodeList();
            endGO.GetComponent<Node>().ResetNodeList();

            foreach (GameObject g in nodeList)
            {
                GameObject.Destroy(g);
            }
            nodeList.Clear();
        }

        regionSize = new Vector2(-startGO.GetComponent<Transform>().position.y + endGO.GetComponent<Transform>().position.y - radius * 2, 
            -startGO.GetComponent<Transform>().position.y + endGO.GetComponent<Transform>().position.y - radius * 2);

        Vector2 startVector = new Vector2(regionSize.x / 2, 0);
        Vector2 endVector = new Vector2(regionSize.x / 2, regionSize.y);

        points = PoissonDisc.GeneratePoints(radius, regionSize, startVector, endVector, rejectionSamples);
        DrawMap();
        CD.Log(CD.Programmers.BEN, "Map Generated!");


        // Connecting to the next region, so we don't have to click more buttons
        TriangulatePoints(pathIterations);
    }
    #endregion

    #region Triangulating The Points
    public void TriangulatePoints(int pathIterations)
    {

        List<Vector2> nodeLocations = new List<Vector2>();

        //CD.Log(CD.Programmers.BEN, "Start Location: " + startNode.GetComponent<Node>().Location);

        //nodeLocations.Add(startNode.GetComponent<Node>().Location);
        //nodeLocations.Add(endNode.GetComponent<Node>().Location);

        foreach (GameObject g in nodeList)
        {
            nodeLocations.Add(g.GetComponent<Node>().Location);
        }

        // Triangulation stuff
        DelaunayTriangulation triangulation = new DelaunayTriangulation();
        List<Triangle2D> outputTriangles = new List<Triangle2D>();

        outputTriangles.Clear();

        triangulation.Triangulate(nodeLocations);
        triangulation.GetAllTriangles(outputTriangles);

        foreach (GameObject go in nodeList)
        {
            foreach (Triangle2D triangle in outputTriangles)
            {
                Node node = go.GetComponent<Node>();

                if (node.Location == triangle.p0)
                {
                    // add p1 and p2 to the connections
                    Node n1 = FindNodeInTriangleList(triangle.p1);
                    if (n1 != null && !node.GetConnectedNodes().Contains(n1))
                    {
                        node.addNode(n1);
                    }
                    Node n2 = FindNodeInTriangleList(triangle.p2);
                    if (n2 != null && !node.GetConnectedNodes().Contains(n2))
                    {
                        node.addNode(n2);
                    }
                }
                else if (node.Location == triangle.p1)
                {
                    // add p0 and p2 to the connections
                    Node n1 = FindNodeInTriangleList(triangle.p0);
                    if (n1 != null && !node.GetConnectedNodes().Contains(n1))
                    {
                        node.addNode(n1);
                    }
                    Node n2 = FindNodeInTriangleList(triangle.p2);
                    if (n2 != null && !node.GetConnectedNodes().Contains(n2))
                    {
                        node.addNode(n2);
                    }
                }
                else if (node.Location == triangle.p2)
                {
                    // add p0 and p1 to the connections
                    Node n1 = FindNodeInTriangleList(triangle.p0);
                    if (n1 != null && !node.GetConnectedNodes().Contains(n1))
                    {
                        node.addNode(n1);
                    }
                    Node n2 = FindNodeInTriangleList(triangle.p1);
                    if (n2 != null && !node.GetConnectedNodes().Contains(n2))
                    {
                        node.addNode(n2);
                    }
                }
            }
        }
        CD.Log(CD.Programmers.BEN, "Triangulation Complete!");

        // Connecting to the next region, so we don't have to click more buttons
        
        Pathing(pathIterations);

#if UNITY_EDITOR
        DrawLines(triangulation);
#endif
    }

    // Finds the node that is in the found triangle
    private Node FindNodeInTriangleList(Vector2 foundNodeLocation)
    {

        Node nodeToReturn = null;

        foreach (GameObject go in nodeList)
        {
            if (go.GetComponent<Node>().Location == foundNodeLocation)
            {
                nodeToReturn = go.GetComponent<Node>();
                break;
            }
        }

        return nodeToReturn;

    }


    private void DrawLines(DelaunayTriangulation triangulation)
    {
        for (int i = 0; i < triangulation.TriangleSet.TriangleCount; i++)
            triangulation.TriangleSet.DrawTriangle(i, Color.white);
    }
    #endregion
    
    #region A*Pathing
    public void Pathing(int pathIterations)
    {
        List<Node> nodeList = new List<Node>();
        foreach (GameObject g in this.nodeList)
        {
            nodeList.Add(g.GetComponent<Node>());
        }

        nodeList.Remove(startGO.GetComponent<Node>());
        nodeList.Remove(endGO.GetComponent<Node>());

        // Repeating this process 4-5 times, making sure to remove one node from the previous path from the possible node list
        for (int i = 0; i < pathIterations; i++)
        {
            pathfinding = new AStarPath(startGO.GetComponent<Node>(), endGO.GetComponent<Node>(), nodeList);
            // Adding the first available path

            path.Add(pathfinding.FindPath());

            nodeList.Remove(endGO.GetComponent<Node>());

            //foreach (Node node in pathfinding.FindPath())
            //   Debug.Log(node.gameObject.name);
            //path.Add(pathfinding.FindPath());
            endGO.GetComponent<Node>().ClearPath();

            foreach (Node n in nodeList)
            {
                n.ClearPath();
            }


            try
            {
                nodeList.Remove(path[i][Random.Range(0, path[i].Count - 1)]);
            }
            catch 
            {
                MapGeneration.Instance = new MapGeneration(startNode, endNode, mapNode, pathGO, progPathGO, mapGO, Player, filePath);
            }
        }
        RemoveExcessNodes();
        //PrintNodes();
        DrawRoutes();

        MapGenerated = true;

        NodeResourceDistribution();


        // Add Extra Branches where you can. (They must be moving towards the end node though)
        Branch();

    }
    
    private void Branch()
    {
        // Take current path, and find a node in each path, and try to see if the f-cost is lower than this one.

        List<Node> newTempPathList = new List<Node>();

        
        // Iterate through each current path, and try to see if you can branch to other nodes.
        for (int j = 0; j < path.Count; j++)
        {
            if (path[j]!= null)
            {
                List<Node> pathList = path[j];
                for (int i = 0; i < pathList.Count; i++)
                { // Path #?
                    if (pathList[i] == null)
                        break;
                    foreach (Node connectedNode in pathList[i].GetConnectedNodes())
                    {
                        // Add node to temp path
                        newTempPathList.Add(pathList[i]);

                        if (connectedNode != null)
                        { // Meaning this node is actually in the scene
                          // Make sure this node is actually closer to the end node than this one.
                          // Shouldn't have to calculate f-cost again, since it was just used
                            if (connectedNode.Information.y > pathList[i].Information.y)
                            {
                                // connected node is closer than the current node, check if it has a line inbetween them.
                                if (!CheckLine(pathList[i], connectedNode))
                                {
                                    // Nothing here, Connect them and add them to the list, then continue to add the rest of the new path.
                                    newTempPathList.Add(connectedNode);

                                    // Draw lines between them
                                    DrawNewRoutes(newTempPathList);
                                }
                            }
                        }

                        // Clear the temp path
                        newTempPathList.Clear();

                    }
                }
            }
            
        }

        newLineCount = 0;

    }
    static int newLineCount = 0;
    private void DrawNewRoutes(List<Node> nodeList)
    {

        List<GameObject> pathList = new List<GameObject>();

        for (int i = 1; i < nodeList.Count; i++)
        {
            GameObject g = (GameObject)GameObject.Instantiate(pathGO, mapGO.transform);
            g.name = "Custom Line #" + newLineCount++;
            g.transform.position = nodeList[i - 1].transform.position;

            // Calculate angle from node(i - 1) to node(i)
            float angleToRotate = FindAngle(nodeList[i - 1].Location, nodeList[i].Location);

            g.transform.Rotate(new Vector3(0, 0, angleToRotate));


            g.GetComponent<RectTransform>().sizeDelta = new Vector2(FindHypotenuse(nodeList[i - 1].Location, nodeList[i].Location), 20);
            g.transform.position = new Vector3(
                g.transform.position.x - (nodeList[i - 1].Location.x - nodeList[i].Location.x) / 2,
                g.transform.position.y - (nodeList[i - 1].Location.y - nodeList[i].Location.y) / 2,
                0
                );

            GameObject prevLine = CheckIfOtherLineHere(g.transform);

            if (prevLine != null)
            {
                GameObject.Destroy(g);
            }
            else prevLine = g;

            pathList.Add(prevLine);

            // Progress Line
            DrawProgressionLine(prevLine.GetComponent<Line>());


            prevLine.GetComponent<Line>().fromNode = nodeList[i - 1];
            prevLine.GetComponent<Line>().toNode = nodeList[i];

            if (!nodeList[i - 1].ConnectedLines.Contains(prevLine.GetComponent<Line>()))
                nodeList[i - 1].ConnectedLines.Add(prevLine.GetComponent<Line>());
            if (!nodeList[i].ConnectedLines.Contains(prevLine.GetComponent<Line>()))
                nodeList[i].ConnectedLines.Add(prevLine.GetComponent<Line>());
        }
        pathLines.Add(pathList);

    }

   

    public void CleanNewLinesUp()
    {
        // Called from player to check if this has been cleaned up

        List<int> removeLoc = new List<int>();

        // Iterate through new lines
        for (int i = 0; i < pathLines.Count; i++)
        {
            List<GameObject> lineList = pathLines[i];
            if (lineList.Count == 1) // This means it's the additions we have
            {
                Line line = lineList[0].GetComponent<Line>();
                CD.Log(CD.Programmers.BEN, $"{line.fromNode}, {line.toNode}");
                if (line.fromNode == null || line.toNode == null)
                {
                    // delete this shit
                    GameObject.Destroy(line.gameObject);
                    removeLoc.Add(i);
                }
            }
        }

        for (int i = 0; i < removeLoc.Count; i++)
        {
            pathLines.RemoveAt(removeLoc[i] - i);
        }
    }

    private bool CheckLine(Node node1, Node node2)
    {
        // checks if there is a line already between these two nodes

        foreach (Line l in node1.ConnectedLines)
        {
            if (node2.ConnectedLines.Contains(l))
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveExcessNodes()
    {
        foreach(Node node in mapGO.GetComponentsInChildren<Node>())
        {
            foreach(List<Node> pathList in path)
            {
                if (pathList != null)
                    foreach(Node n in pathList)
                    {
                        if (n != null)
                            if (n.Equals(node))
                            {
                                node.Information.accessed = true;
                                break;
                            }
                    }
            }
        }

        foreach(Node node in mapGO.GetComponentsInChildren<Node>())
        {
            if (!node.Information.accessed)
            { // Not found in a list, to destroying it
                //CD.Log(CD.Programmers.BEN, "Destroying Node: " + node.gameObject.name);
                GameObject.Destroy(node.gameObject);
            }
        }

    }

    private void DrawRoutes()
    {
        int lineCount = 0;
        for (int j = 0; j < path.Count; j++)
        {
            List<GameObject> pathList = new List<GameObject>();
            try
            {
                for (int i = 1; i < path[j].Count; i++)
                { // Makes a UI object to show lines connecting the nodes
                    GameObject g = (GameObject)GameObject.Instantiate(pathGO, mapGO.transform);
                    g.name = "Line #" + lineCount++;
                    g.transform.position = path[j][i - 1].transform.position;

                    // Calculate angle from node(i - 1) to node(i)
                    float angleToRotate = FindAngle(path[j][i - 1].Location, path[j][i].Location);

                    g.transform.Rotate(new Vector3(0, 0, angleToRotate));


                    g.GetComponent<RectTransform>().sizeDelta = new Vector2(FindHypotenuse(path[j][i - 1].Location, path[j][i].Location), 20);
                    g.transform.position = new Vector3(
                        g.transform.position.x - (path[j][i - 1].Location.x - path[j][i].Location.x) / 2,
                        g.transform.position.y - (path[j][i - 1].Location.y - path[j][i].Location.y) / 2,
                        0
                        );

                    GameObject prevLine = CheckIfOtherLineHere(g.transform);

                    if (prevLine != null)
                    {
                        GameObject.Destroy(g);
                    }
                    else prevLine = g;

                    pathList.Add(prevLine);

                    // Progress Line
                    DrawProgressionLine(prevLine.GetComponent<Line>());

                    prevLine.GetComponent<Line>().fromNode = path[j][i - 1];
                    prevLine.GetComponent<Line>().toNode = path[j][i];

                    if (!path[j][i - 1].ConnectedLines.Contains(prevLine.GetComponent<Line>()))
                        path[j][i - 1].ConnectedLines.Add(prevLine.GetComponent<Line>());
                    if (!path[j][i].ConnectedLines.Contains(prevLine.GetComponent<Line>()))
                        path[j][i].ConnectedLines.Add(prevLine.GetComponent<Line>());


                }
            }
            catch { ReCreateMap(); }
            

            pathLines.Add(pathList);
            

        }
    }

    private GameObject CheckIfOtherLineHere(Transform t)
    {

        foreach (Transform tr in mapGO.GetComponentsInChildren<Transform>())
        {
            if (tr.position.Equals(t.position) && !tr.gameObject.name.Equals(t.gameObject.name))
            {
                return tr.gameObject;
            }
        }

        return null;
    }

    private void DrawProgressionLine(Line lineAttached)
    {
        if (lineAttached.transform.childCount == 0)
        {
            GameObject p = (GameObject)GameObject.Instantiate(progPathGO, lineAttached.transform);

            lineAttached.GetComponent<Line>().connectedProgressionLine = p.GetComponent<ProgressionLine>();

        }
    }

    private float FindHypotenuse(Vector2 loc1, Vector2 loc2)
    {
        float x = (loc1.x - loc2.x);
        float y = (loc1.y - loc2.y);

        return Mathf.Sqrt((x * x) + (y * y));
    }

    private float FindAngle(Vector2 loc1, Vector2 loc2)
    {
        float angle;

        float x = (loc1.x - loc2.x);
        float y = (loc1.y - loc2.y);

        float rad = Mathf.Atan2(y, x);

        angle = (180 / Mathf.PI) * rad;

        return angle;
    }

    private void PrintNodes()
    {
        foreach (List<Node> pathList in path)
        {
            CD.Log(CD.Programmers.BEN, "---------NEW PATH---------");
            foreach (Node node in pathList)
            {
                CD.Log(CD.Programmers.BEN, node.gameObject.name);
            }
        }
        
    }

    #endregion

    #region Node Resource Distribution
    private void NodeResourceDistribution(int maxNumberResources = 10)
    {
        int maxNum = maxNumberResources;

        StartNode.Information.resourcesGiven = true;

        for (int j = 0; j < path.Count; j++)
        {
            if (path[j] != null)
            {
                List<Node> nodeList = path[j];
                for (int i = 0; i < nodeList.Count; i++)
                { // Path #?
                    if (nodeList[i] == null)
                        break;
                    foreach (Node n in nodeList)
                    {
                        maxNum = maxNumberResources;
                        if (!n.Information.resourcesGiven)
                        {
                            n.Information.Food += 1;
                            n.Information.Medical += 1;
                            n.Information.Survival += 1;

                            maxNum -= 3;

                            while (maxNum > 0)
                            {
                                int rand = (int)Random.Range(0, maxNum > 4 ? 5 : maxNum + 1);
                                //CD.Log(CD.Programmers.BEN, "" + rand);
                                switch ((int)Random.Range(0, 3))
                                {
                                    case 2:
                                        // Food
                                        //CD.Log(CD.Programmers.BEN, "FOOD " + n.gameObject.name + " + " + rand);
                                        if (n.Information.Food + rand > 5)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            n.Information.Food += rand;
                                            maxNum -= rand;
                                        }
                                        break;
                                    case 1:
                                        // Pharm
                                        //CD.Log(CD.Programmers.BEN, "Pharm " + n.gameObject.name + " + " + rand);
                                        if (n.Information.Medical + rand > 5)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            n.Information.Medical += rand;
                                            maxNum -= rand;
                                        }
                                        break;
                                    case 0:
                                        // Surv
                                        //CD.Log(CD.Programmers.BEN, "Surv " + n.gameObject.name + " + " + rand);
                                        if (n.Information.Survival + rand > 5)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            n.Information.Survival += rand;
                                            maxNum -= rand;
                                        }
                                        break;
                                }
                            }
                            n.Information.resourcesGiven = true;
                        }
                    }
                }
            }
        }
        if (player != null)
        {
            player.ShowHideNodes();
        }
        else
        {
            player = GameObject.FindObjectOfType<PlayerMap>();
        }
        NumberNodes();
    }

    void NumberNodes()
    {
        int c = 0;
        foreach(GameObject g in nodeList)
        {
            if (g != null)
            {
                NodeInformation n = g.GetComponent<Node>().Information;
                n.nodeNum = c;
                c++;
            }
        }

        CleanNewLinesUp();

    }

    #endregion

    #region Reset
    public void ResetAll()
    {
        if (!player.ResetMap()) return;

        foreach (List<GameObject> list in pathLines)
            foreach (GameObject g in list)
                if (g != null)
                    g.SetActive(true);

        foreach (Transform n in mapGO.GetComponentsInChildren<Transform>())
        {
            if (n != null)
                if (!n.gameObject.Equals(mapGO) && !n.gameObject.name.Equals("DragObject") && !n.gameObject.name.Equals("PlayerImg") && !n.gameObject.name.Equals("Background"))
                {
                    GameObject.DestroyImmediate(n.gameObject);
                }

        }

        startGO = null;
        endGO = null;

        path.Clear();
        pathLines.Clear();
        nodeList.Clear();

    }

    private void ReCreateMap()
    {
        if (!resetting)
        {
            Player.GenerateNewMap();
            resetting = true;
        }
            
    }

    #endregion


    #region Save and Load
    public void SaveMap()
    {

        MapData data = new MapData();

        data.nodeInformation = new List<NodeInformation>();
        foreach (GameObject g in nodeList)
        {// Parse through the nodeList to get their information
            if (g != null)
            {
                NodeInformation n = g.GetComponent<Node>().Information;
                if (!data.nodeInformation.Contains(n))
                {
                    data.nodeInformation.Add(n);
                }
                    
            }
        }

        data.path = new List<List<NodeInformation>>();
        foreach (List<Node> nList in Path)
        {// Parse through each path in the list to get the information
            List<NodeInformation> infoList = new List<NodeInformation>();
            foreach (Node n in nList)
            { 
                infoList.Add(n.Information);
            }
            data.path.Add(infoList);
        }


        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, data);

        dataStream.Close();

        CD.Log(CD.Programmers.BEN, "Save Location: " + filePath);

    }

    public void LoadMap(MapData data)
    {
        // NodeList Instansiation
        int count = 0;

        

        foreach (NodeInformation node in data.nodeInformation)
        {

            // Check if the node information matches the start/endnode
            if (node.nodeNum == 0) // start node
            {
                startGO = (GameObject)GameObject.Instantiate(startNode, mapGO.transform);
                startGO.GetComponent<Node>().Player = Player;

                startGO.GetComponent<Node>().Information = node;
                startGO.GetComponent<Node>().Location = new Vector2(node.x, node.y);

                nodeList.Add(startGO);
            }
            else if (node.nodeNum == 1) // end node
            {
                endGO = (GameObject)GameObject.Instantiate(endNode, mapGO.transform);
                endGO.GetComponent<Node>().Player = Player;

                endGO.GetComponent<Node>().Information = node;
                endGO.GetComponent<Node>().Location = new Vector2(node.x, node.y);

                nodeList.Add(endGO);
            }
            else
            {
                GameObject n = (GameObject)GameObject.Instantiate(mapNode);
                n.name = "Node_#" + count;
                n.GetComponent<Node>().Player = Player;
                count++;

                Vector3 loc = new Vector3(node.x - regionSize.x / 2, node.y - regionSize.y / 2, 0);

                n.GetComponent<Transform>().Translate(loc);

                n.transform.SetParent(mapGO.transform);

                n.GetComponent<Node>().Location = new Vector2(node.x, node.y);
                n.GetComponent<Node>().Information = node;

                nodeList.Add(n);
            }


                
        }

        // Path Stuff
        foreach (List<NodeInformation> nList in data.path)
        {
            List<Node> newList = new List<Node>();
            foreach (NodeInformation n in nList)
            {
                // Search through the nodeList and compare the nodeNum
                foreach (GameObject g in nodeList)
                {
                    if (g.GetComponent<Node>().Information.nodeNum.Equals(n.nodeNum))
                    {
                        newList.Add(g.GetComponent<Node>());
                        break;
                    }
                }
            }
            path.Add(newList);
        }

        DrawRoutes();
        MapGenerated = true;
    }

    #endregion
}

[System.Serializable]
public class MapData
{
    public List<NodeInformation> nodeInformation;
    public List<List<NodeInformation>> path; 
}
