﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class Node : MonoBehaviour
{
    public int num;

    private Text sIntersection;
    GameObject GO;

    //	private static int originCarCap;

    //private DisplayManager displayManager;

    public Node(int n)
    {
        this.num = n;
    }

    public int get()
    {
        return num;
    }

    private static int size2lastNode;

    //create two globle number to pass two nodes number to gameContrll.cs
    public static int passNode1;
    public static int passNode2;

    //three index to store truck cap for each path
    public static int originCap;
    public static int laterCap;
    private static int size2last2Node;
    private int size2last3Node;

    //this array store the gameobject represent the capacity of each path.
    private static GameObject[] capPath;

    //basic value that the scroll bar need to add
    private static float scrollBasicProfit;
    private static float scrollBasicProfitOnce;
    private static float scrollBasicTime;
    private static float scrollBasicTimeOnce;
    private static float secondBlue;

    //decide which kind of node should use
    private bool redN;
    private bool blueN;
    private bool greenN;
    private bool rbN;
    private bool rgN;
    private bool gbN;
    private bool rgbN;

    //decide if in this node
    private bool isMouseOver;

    //judge if the backtodepot function is running
    private static bool backToDepot;

    //create lists of array to assign road information
    public static List<List<int>> redAl;
    public static List<List<int>> blueAl;
    public static List<List<int>> greenAl;

    //creat lists of array to assign truck information
    public static List<List<float>> redTruckCap;
    public static List<List<float>> blueTruckCap;
    public static List<List<float>> greenTruckCap;

    //create lists of array to assign profit information
    public static List<float> redProfitAl = new List<float>();
    public static List<float> greenProfitAl = new List<float>();
    public static List<float> blueProfitAl = new List<float>();

    //create lists of array to assign time inforamtion
    public static List<float> redTimeAl = new List<float>();
    public static List<float> greenTimeAl = new List<float>();
    public static List<float> blueTimeAl = new List<float>();

    public static List<int> storePath = new List<int>();
    public static List<float> storeTruckCap = new List<float>();
    public static float storeProfit;
    public static float storeTime;

    //static arrays store rgb line renders
    public static bool[,] redLineArray = new bool[6, 6];
    public static bool[,] blueLineArray = new bool[6, 6];
    public static bool[,] greenLineArray = new bool[6, 6];

    //three points that the checkmark will always appear
    public static Vector2 v1;
    public static Vector2 v2;
    public static Vector2 v3;


    //set six variables to store the last game profit total
    private float redProfitLastTime;
    private float blueProfitLastTime;
    private float greenProfitLastTime;
    private float redTimeLastTime;
    private float blueTimeLastTime;
    private float greenTimeLastTime;

    //static array to make animation in the CheckMark.cs
    public static int nodeCount;
    public static int[] nodeArray;

    //static arrays to store rgb path cycle. The difference with above is it's not bidirection
    public static bool[,] rgbPathArray = new bool[6, 6];
    public static bool[,] redPathArray = new bool[6, 6];
    public static bool[,] greenPathArray = new bool[6, 6];
    public static bool[,] bluePathArray = new bool[6, 6];

    //create int array to store how many path in it
    public static int[,] redPathNum = new int[6, 6];
    public static int[,] greenPathNum = new int[6, 6];
    public static int[,] bluePathNum = new int[6, 6];


    //store the color of every node
    public static Dictionary<string, string> nodeDic = new Dictionary<string, string>();

    void Awake()
    {
        GO = GameObject.Find("GameController");
        sIntersection = GameObject.Find("intersection").GetComponent<Text>();
        if (this.num == 1) {
            storeTime = 0;
            storeProfit = 0;

            if (SceneManager.GetActiveScene().name == "start")
            {
                redLineArray = new bool[6, 6];
                blueLineArray = new bool[6, 6];
                greenLineArray = new bool[6, 6];

                rgbPathArray = new bool[6, 6];
                redPathArray = new bool[6, 6];
                greenPathArray = new bool[6, 6];
                bluePathArray = new bool[6, 6];

                //create int array to store how many path in it
                redPathNum = new int[6, 6];
                greenPathNum = new int[6, 6];
                bluePathNum = new int[6, 6];

            }

            if (SceneManager.GetActiveScene().name == "level2") {
                redLineArray = new bool[21, 21];
                blueLineArray = new bool[21, 21];
                greenLineArray = new bool[21, 21];

                rgbPathArray = new bool[21, 21];
                redPathArray = new bool[21, 21];
                greenPathArray = new bool[21, 21];
                bluePathArray = new bool[21, 21];

                //create int array to store how many path in it
                redPathNum = new int[21, 21];
                greenPathNum = new int[21, 21];
                bluePathNum = new int[21, 21];
            }

            redAl = new List<List<int>>();
            blueAl = new List<List<int>>();
            greenAl = new List<List<int>>();
            redTruckCap = new List<List<float>>();
            blueTruckCap = new List<List<float>>();
            greenTruckCap = new List<List<float>>();


            //Debug.Log ("redAL " + redAl.Count);
            v1 = getVector(GameObject.Find("depot").transform.position, GameObject.Find("node2").transform.position, GameObject.Find("node3").transform.position);
            v2 = getVector(GameObject.Find("depot").transform.position, GameObject.Find("node2").transform.position, GameObject.Find("node5").transform.position);
            v3 = getVector(GameObject.Find("depot").transform.position, GameObject.Find("node3").transform.position, GameObject.Find("node4").transform.position);

            //		gameControll.redTruckNum = 0;
            //		gameControll.blueTruckNum = 0;
            //			for (int i = 1; i < 6; i++) {
            //				for (int j = 1; j < 6; j++) {
            //					Debug.Log(i+ " "+j+ " "+redPathNum[i,j]);
            //				}
            //			}
        }

        //getInitialSolution();
        if (!RefreshButton.refresh) {
            //getInitialSolution();
            if (ClickToLoadAsync.solution == 0)
            {
                int r = Random.Range(1, 4);
                if (r % 3 == 1)
                {
                    Dictionary<List<int>, List<float>> r0 = new Dictionary<List<int>, List<float>>();
                    r0[new List<int> { 1, 3, 4, 1 }] = new List<float> { 50, 50, 0 };
                    r0[new List<int> { 1, 4, 5, 4, 1 }] = new List<float> { 0, 50, 0, 0 };
                    Dictionary<List<int>, List<float>> b0 = new Dictionary<List<int>, List<float>>();
                    b0[new List<int> { 1, 3, 2, 1 }] = new List<float> { 0, 50, 50 };
                    b0[new List<int> { 1, 2, 5, 2, 1 }] = new List<float> { 0, 50, 0, 0 };
                    TruckPath solution0 = new TruckPath(r0, b0);
                    getInitialSolution0(solution0);
                }

                if (r % 3 == 2)
                {
                    Dictionary<List<int>, List<float>> r1 = new Dictionary<List<int>, List<float>>();
                    r1[new List<int> { 1, 2, 5, 4, 1 }] = new List<float> { 0, 50, 50, 0 };
                    r1[new List<int> { 1, 2, 3, 1 }] = new List<float> { 0, 17f, 50 };
                    r1[new List<int> { 1, 2, 1 }] = new List<float> { 0, 50 };
                    Dictionary<List<int>, List<float>> b1 = new Dictionary<List<int>, List<float>>();
                    b1[new List<int> { 1, 3, 2, 3, 4, 5, 4, 1 }] = new List<float> { 0, 0, 33f, 0, 0, 0, 0 };
                    b1[new List<int> { 1, 4, 3, 4, 5, 4, 1 }] = new List<float> { 0, 0, 50, 0, 0, 0 };
                    b1[new List<int> { 1, 4, 1 }] = new List<float> { 50, 0 };
                    TruckPath solution1 = new TruckPath(r1, b1);
                    getInitialSolution0(solution1);
                }

                if (r % 3 == 0)
                {
                    Dictionary<List<int>, List<float>> r2 = new Dictionary<List<int>, List<float>>();
                    r2[new List<int> { 1, 2, 5, 4, 1 }] = new List<float> { 0, 50, 50, 0 };
                    r2[new List<int> { 1, 3, 4, 1 }] = new List<float> { 35, 15, 50 };
                    r2[new List<int> { 1, 3, 2, 1 }] = new List<float> { 15, 50, 0 };
                    Dictionary<List<int>, List<float>> b2 = new Dictionary<List<int>, List<float>>();
                    b2[new List<int> { 1, 2, 3, 4, 1 }] = new List<float> { 50, 0, 35, 0 };

                    TruckPath solution2 = new TruckPath(r2, b2);

                    getInitialSolution0(solution2);
                }
            }else if (ClickToLoadAsync.solution == 1)
            {
                Dictionary<List<int>, List<float>> r0 = new Dictionary<List<int>, List<float>>();
                r0[new List<int> { 1, 3, 4, 1 }] = new List<float> { 50, 50, 0 };
                r0[new List<int> { 1, 2, 5, 2, 1 }] = new List<float> { 0, 50, 0, 0 };
                Dictionary<List<int>, List<float>> b0 = new Dictionary<List<int>, List<float>>();
                b0[new List<int> { 1, 3, 2, 1 }] = new List<float> { 0, 50, 50 };
                b0[new List<int> { 1, 2, 5, 2, 1 }] = new List<float> { 0, 50, 0, 0 };
                TruckPath solution0 = new TruckPath(r0, b0);
                getInitialSolution0(solution0);
            }else if(ClickToLoadAsync.solution == 2)
            {
                Dictionary<List<int>, List<float>> r1 = new Dictionary<List<int>, List<float>>();
                r1[new List<int> { 1, 2, 5, 4, 1 }] = new List<float> { 0, 50, 50, 0 };
                r1[new List<int> { 1, 2, 3, 1 }] = new List<float> { 0, 17.5f, 50 };
                r1[new List<int> { 1, 2, 1 }] = new List<float> { 0, 50 };
                Dictionary<List<int>, List<float>> b1 = new Dictionary<List<int>, List<float>>();
                b1[new List<int> { 1, 3, 2, 3, 4, 5, 4, 1 }] = new List<float> { 0, 0, 32.5f, 0, 0, 0, 0 };
                b1[new List<int> { 1, 4, 3, 4, 5, 4, 1 }] = new List<float> { 0, 0, 50, 0, 0, 0 };
                b1[new List<int> { 1, 4, 1 }] = new List<float> { 50, 0 };

                TruckPath solution1 = new TruckPath(r1, b1);

                getInitialSolution0(solution1);
            }else if (ClickToLoadAsync.solution == 3)
            {
                Dictionary<List<int>, List<float>> r2 = new Dictionary<List<int>, List<float>>();
                r2[new List<int> { 1, 2, 5, 4, 1 }] = new List<float> { 0, 50, 50, 0 };
                r2[new List<int> { 1, 3, 4, 1 }] = new List<float> { 35, 15, 50 };
                r2[new List<int> { 1, 3, 2, 1 }] = new List<float> { 15, 50, 0 };
                Dictionary<List<int>, List<float>> b2 = new Dictionary<List<int>, List<float>>();
                b2[new List<int> { 1, 2, 3, 4, 1 }] = new List<float> { 50, 0, 35, 0 };

                TruckPath solution2 = new TruckPath(r2, b2);

                getInitialSolution0(solution2);
            }
        }
    }




    void Start()
    {
        RefreshButton.refresh = false;
    }


    void Update() {
        //Debug.Log (intersection);

        //set refresh button back to false;
        if (RefreshButton.refresh && CheckMark.refreshed && storeTruck.refreshed) {
            RefreshButton.refresh = false;
        }

        if (CheckMark.nextStep == true) {
            laterCap = gameControll.capArray[passNode1, passNode2];
            int t = originCap - laterCap;
            storeTruckCap.Add(t);
            //Debug.Log ("length of this" + storeTruckCap.Count);
            backToDepot = false;
            if (gameControll.blueTruck) {
                blueAl.Add(storePath);
                blueTruckCap.Add(storeTruckCap);
                string s1 = "";
                string s2 = "";
                foreach (int i1 in storePath) {
                    s1 += i1.ToString() + ",";
                }
                foreach (int i1 in storeTruckCap) {
                    s2 += i1.ToString() + ",";
                }
                JSONClass details = new JSONClass();
                details["BlueTruckPath"] = "Node" + s1 + "StoredCap" + s2;
                TheLogger.instance.TakeAction(9, details);
                gameControll.blueTruck = false;
                storeProfit = gameControll.blueProfitOnce;
                storeTime = gameControll.blueTimeOnce;
                blueProfitAl.Add(storeProfit);
                blueTimeAl.Add(storeTime);
                //Debug.Log ("store Time" + storeTime);
                gameControll.blueProfitOnce = 0;
                gameControll.blueTimeOnce = 0;
            }
            if (gameControll.redTruck) {
                redAl.Add(storePath);
                redTruckCap.Add(storeTruckCap);
                string s1 = "";
                string s2 = "";
                foreach (int i1 in storePath) {
                    s1 += i1.ToString() + ",";
                }
                foreach (int i1 in storeTruckCap) {
                    s2 += i1.ToString() + ",";
                }
                JSONClass details = new JSONClass();
                details["RedTruckPath"] = "Node" + s1 + "StoredCap" + s2;
                TheLogger.instance.TakeAction(9, details);
                gameControll.redTruck = false;
                storeProfit = gameControll.redProfitOnce;
                storeTime = gameControll.redTimeOnce;
                //Debug.Log ("store Time" + storeTime);
                redProfitAl.Add(storeProfit);
                redTimeAl.Add(storeTime);
                gameControll.redProfitOnce = 0;
                gameControll.redTimeOnce = 0;
            }
            if (gameControll.greenTruck) {
                greenAl.Add(storePath);
                greenTruckCap.Add(storeTruckCap);
                string s1 = "";
                string s2 = "";
                foreach (int i1 in storePath) {
                    s1 += i1.ToString() + ",";
                }
                foreach (int i1 in storeTruckCap) {
                    s2 += i1.ToString() + ",";
                }
                JSONClass details = new JSONClass();
                details["GreenTruckPath"] = "Node" + s1 + "StoredCap" + s2;
                TheLogger.instance.TakeAction(9, details);
                gameControll.greenTruck = false;
                storeProfit = gameControll.greenProfitOnce;
                storeTime = gameControll.greenTimeOnce;
                //Debug.Log ("store Time" + storeTime);
                greenProfitAl.Add(storeProfit);
                greenTimeAl.Add(storeTime);
                gameControll.greenTimeOnce = 0;
                gameControll.greenProfitOnce = 0;
            }
            Destroy(GameObject.FindGameObjectWithTag("truckText"));
            //Debug.Log (" You have finish a cycle, please start another one!");

            //gameControll.saveToFile ("a cycle is finished.");

            //GameObject.Find ("ModalControl").GetComponent<testWindow> ().takeAction("You have finish a cycle!");

            //add truck scripts here
            //	GameObject.Find("storeTruck").GetComponent<storeTruck>().addTruck(0);

            //reset most of the things to the beginning here
            gameControll.twoNode.Clear();
            //			GameObject.Find ("GameController").GetComponent<gameControll> ().resetCursor ();
            GO.GetComponent<gameControll>().resetDepot();
            int i = 0;
            panelController.blueTextOnce.text = i.ToString();
            panelController.redTextOnce.text = i.ToString();
            panelController.greenTextOnce.text = i.ToString();
            panelController.blueTimeOnce.text = i.ToString();
            panelController.redTimeOnce.text = i.ToString();
            panelController.greenTimeOnce.text = i.ToString();
            CheckMark.nextStep = false;
        }


        if (Input.GetMouseButtonDown(1) && isMouseOver) {
            rightClicktoCancelPath(gameControll.redTruck, gameControll.blueTruck, gameControll.greenTruck);
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }


    void OnMouseDown()
    {

        //        Debug.Log(this.num + this.redN.ToString() + " redN " + this.blueN.ToString() + " blueN "+ this.rbN.ToString());
        //Debug.Log (num);
        //int temp;
        //Queue<int> t = gameControll.twoNode;
        int size = gameControll.twoNode.Count;

        //Debug.Log ("size " + size);
        if (size == 1)
        {
            int firstOfSize1 = gameControll.twoNode.Peek();
            if (gameControll.validPath(firstOfSize1, num))
            {
                GetComponent<AudioSource>().Play();
                gameControll.twoNode.Enqueue(num);
                //Debug.Log("this is the node" + num);
                size2lastNode = num;
                //string processtoSave = "connect depot to node " + num.ToString ();
                //gameControll.saveToFile (processtoSave);
                //Debug.Log (size2lastNode);

                //here get the capacity of the path
                passNode1 = firstOfSize1;
                passNode2 = num;
                size2last2Node = 1;
                //Debug.Log ("in first size2last2node " + size2last2Node);
                //change the image of node here
                clickChangeColor();
                JSONClass details = new JSONClass();
                details["ClickNode"] = num.ToString();
                TheLogger.instance.TakeAction(2, details);

                foreach (int i in validPathAnimation(passNode1)) {
                    string temp = "node" + i;
                    if (i == 1) {
                        temp = "depot";
                    }
                    Behaviour halo = (Behaviour)GameObject.Find(temp).GetComponent("Halo");
                    halo.enabled = false;
                }

                foreach (int i in validPathAnimation(passNode2)) {
                    string temp = "node" + i;
                    if (i == 1) {
                        temp = "depot";
                    }
                    Behaviour halo = (Behaviour)GameObject.Find(temp).GetComponent("Halo");
                    halo.enabled = true;
                }

                if (passNode1 == 1)
                {
                    GameObject.Find("GameController").GetComponent<gameControll>().resetDepot();
                }

                clickChangeAnimation(passNode1, passNode2);
                originCap = gameControll.capArray[passNode1, passNode2];
                //Debug.Log ("origin"+originCap);

                //add node number to store the path
                storePath.Add(passNode2);

                //this function used to create new game object to realize the line render.
                //                if (!pathAlreadyExist(passNode1, passNode2, gameControll.redTruck, gameControll.greenTruck, gameControll.blueTruck))
                //                {
                //                    if (redLineArray[passNode1, passNode2] || greenLineArray[passNode1, passNode2] || blueLineArray[passNode1, passNode2])
                //                    {
                //                        createAndDestroyObjectForLineRender();
                //						//createObjectForLineRender();
                //                    }
                //                    else
                //                    {
                //                        createObjectForLineRender();
                //                    }
                //                }
                string pathname = pathName(passNode1, passNode2, rgbPathArray);
                setBoolArray(passNode1, passNode2, rgbPathArray);
                GameObject path = new GameObject();
                path.name = pathname;
                path.AddComponent<LineRenderer>();
                path.tag = "linerender";
                path.AddComponent<LineAnimation>();
                string dupObj = "newPathAnim" + passNode1.ToString() + passNode2.ToString();
                if (pathname == dupObj) {
                    string existObj = "pathAnim" + passNode1.ToString() + passNode2.ToString();
                    if (GameObject.Find(existObj) != null) {
                        GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                        //setIndicatorUnseen (num1, num2);
                    }
                }
                path.GetComponent<LineAnimation>().rectAnimation(passNode1, passNode2);

                redTimeLastTime = gameControll.redTimeTotal;
                blueTimeLastTime = gameControll.blueTimeTotal;
                greenTimeLastTime = gameControll.greenTimeTotal;
                redProfitLastTime = gameControll.redProfitTotal;
                blueProfitLastTime = gameControll.blueProfitTotal;
                greenProfitLastTime = gameControll.greenProfitTotal;
                //here is the second version of UI design
                setInitialValue(passNode1, passNode2);
                //Debug.Log ("initial 2 " + gameControll.blueTimeOnce);

                //				Debug.Log ("passnode1 is " + passNode1);
                //				Debug.Log ("passnode2 is " + passNode2);

                //pathCap.initializeSlider ();

                //this is the first version of ui design
                //find inactive here is a grame object contrlled from mygameObject
                //				GameObject findInactive = gameControll.myGameObject;
                //				findInactive.SetActive(true);

                //				modifyCap (inputControl.capVal, firstOfSize1, num);
                //				Debug.Log("you collect "+ inputControl.capVal
                //					+" the capacity of this path "+firstOfSize1+ num+" remains: " +  gameControll.capArray[firstOfSize1,num]);
            }
            else
            {
                string toSave = "this node is not connected with the node " + firstOfSize1 + " please select a valid one! ";
                //Debug.Log(toSave);
                JSONClass details = new JSONClass();
                details["Incorrect Operation"] = toSave;
                TheLogger.instance.TakeAction(10, details);
                //GameObject.Find ("ModalControl").GetComponent<testWindow> ().takeAction (toSave);
            }
        }
        else if (size == 2)
        {
            GetComponent<AudioSource>().Play();
            //temp = gameControll.twoNode.Dequeue ();
            //Debug.Log ("remove" + temp);
            //Debug.Log(size2lastNode);

            if (gameControll.validPath(size2lastNode, num) && !backToDepot)
            {
                laterCap = gameControll.capArray[size2last2Node, size2lastNode];
                //		Debug.Log ("later cap" + laterCap);
                int tempCap = originCap - laterCap;
                if (gameControll.redTruck) {
                    storeTruckCap.Add(tempCap);
                } else if (gameControll.blueTruck) {
                    storeTruckCap.Add(tempCap);
                } else if (gameControll.greenTruck) {
                    storeTruckCap.Add(tempCap);
                }
                //				Debug.Log ("later" + laterCap);
                //				Debug.Log ("truckCap is" + tempCap);
                gameControll.twoNode.Dequeue();
                //Debug.Log ("remove " + temp);
                pathCap.desableSlider();
                gameControll.twoNode.Enqueue(num);
                passNode1 = size2lastNode;
                passNode2 = num;
                Debug.Log("passnode1 " + passNode1);
                Debug.Log("passnode2 " + passNode2);
                JSONClass details = new JSONClass();
                details["ClickNode"] = num.ToString();
                string key = "store capacity between " + passNode1 + " and " + passNode2;
                details[key] = tempCap.ToString();
                TheLogger.instance.TakeAction(2, details);

                size2last3Node = size2last2Node;
                size2last2Node = passNode1;
                originCap = gameControll.capArray[passNode1, passNode2];
                //Debug.Log ("origin cap" + originCap);
                //				GameObject inputTab=GameObject.Find("InputTab");
                //				inputTab.SetActive (true);

                //change node color here
                clickChangeColor();
                clickChangeAnimation(passNode1, passNode2);
                Debug.Log(passNode1 + " 1 " + passNode2 + " 2");


                //this function used to create new game object to realize the line render.
                //                if (!pathAlreadyExist(passNode1, passNode2,gameControll.redTruck,gameControll.greenTruck,gameControll.blueTruck))
                //                {
                //                    if(redLineArray[passNode1,passNode2] || greenLineArray[passNode1, passNode2] || blueLineArray[passNode1, passNode2])
                //                    {
                //                        createAndDestroyObjectForLineRender();
                //						//createObjectForLineRender();
                //                    }else
                //                    {
                //                        createObjectForLineRender();
                //                    }
                //                }

                foreach (int i in validPathAnimation(passNode1)) {
                    string temp = "node" + i;
                    if (i == 1) {
                        temp = "depot";
                    }
                    Behaviour halo = (Behaviour)GameObject.Find(temp).GetComponent("Halo");
                    halo.enabled = false;
                }

                if (passNode2 != 1) {
                    foreach (int i in validPathAnimation(passNode2)) {
                        string temp = "node" + i;
                        if (i == 1) {
                            temp = "depot";
                        }
                        Behaviour halo = (Behaviour)GameObject.Find(temp).GetComponent("Halo");
                        halo.enabled = true;
                    }
                }

                string pathname = pathName(passNode1, passNode2, rgbPathArray);
                setBoolArray(passNode1, passNode2, rgbPathArray);
                GameObject path = new GameObject();
                path.name = pathname;
                path.AddComponent<LineRenderer>();
                path.tag = "linerender";
                path.AddComponent<LineAnimation>();
                string dupObj = "newPathAnim" + passNode1.ToString() + passNode2.ToString();
                if (pathname == dupObj) {
                    string existObj = "pathAnim" + passNode1.ToString() + passNode2.ToString();
                    if (GameObject.Find(existObj) != null) {
                        GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                        //setIndicatorUnseen (num1, num2);
                    }
                }
                path.GetComponent<LineAnimation>().rectAnimation(passNode1, passNode2);

                GameObject.Find(pathname).GetComponent<LineAnimation>().rectAnimation(passNode1, passNode2);

                storePath.Add(passNode2);

                //GameObject.Find ("GameController").GetComponent<LineAnimation> ().rectAnimation (passNode1,passNode2);
                //second ui update here
                redTimeLastTime = gameControll.redTimeTotal;
                blueTimeLastTime = gameControll.blueTimeTotal;
                greenTimeLastTime = gameControll.greenTimeTotal;
                redProfitLastTime = gameControll.redProfitTotal;
                blueProfitLastTime = gameControll.blueProfitTotal;
                greenProfitLastTime = gameControll.greenProfitTotal;


                setInitialValue(passNode1, passNode2);
                //pathCap.initializeSlider ();
                //pathCap.

                //string toSave="connect node " + passNode1 + " and node " + passNode2;
                //gameControll.saveToFile (toSave);

                //modify node back to depot here
                nodeBackToDepot();


                //				GameObject findInactive = gameControll.myGameObject;
                //				findInactive.SetActive(true);

                //				modifyCap (inputControl.capVal, size2lastNode, num);
                //				Debug.Log ("you collect "+ inputControl.capVal
                //					+" the capacity of this path "+size2lastNode+ num+" remains: " +  gameControll.capArray[size2lastNode,num]);
                size2lastNode = num;
                //Debug.Log (size2lastNode);

                //here I need to add a few lines to process if the player come back to the depot
            }
            else
            {
                string toSave = "this node is not connected with the node " + size2lastNode + " please select a valid one! ";
                //Debug.Log(toSave);
                JSONClass details = new JSONClass();
                details["Incorrect Operation"] = toSave;
                TheLogger.instance.TakeAction(10, details);
                //GameObject.Find ("ModalControl").GetComponent<testWindow> ().takeAction (toSave);
            }
        }
    }



    //here is the function that called when node back to depot
    public static void nodeBackToDepot()
    {
        if (Node.passNode2 == 1) {
            originCap = gameControll.capArray[passNode1, passNode2];
            GameObject.Find("GameController").GetComponent<gameControll>().resetDepot();
            backToDepot = true;
            setConfirmPathButton(storePath);
            //if (intersection != 0)
            //{
            //    intersection--;
            //}
        }
    }

    //function to modify capacity of truck and path;
    //this can be the function with the first version
    public static void modifyCap(int num, int node1, int node2)
    {
        gameControll.carCap -= num;
        gameControll.capArray[node1, node2] -= num;
        gameControll.capArray[node2, node1] -= num;
        //string toSave = " collect " + num + " units of debris.";
        //modify the text in path of the UI
        //		foreach (GameObject obj in capPath) {
        //			int num1 = obj.GetComponent<pathCap> ().node [0];
        //			int num2 = obj.GetComponent<pathCap> ().node [1];
        //			if ((num1 == node1 && num2 == node2) || (num1 == node2 && num2 == node1)) {
        //				obj.GetComponentInChildren<Text> ().text = gameControll.capArray [node1, node2].ToString ();
        //				break;
        //			}
        //		}

        if (gameControll.redTruck)
        {
            gameControll.redProfitTotal += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.redProfitOnce += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.redTimeTotal += gameControll.timeArray[node1, node2] + num * 10;
            gameControll.redTimeOnce += gameControll.timeArray[node1, node2] + num * 10;
            string findName = "redTruckText" + (gameControll.redTruckNum - 1).ToString();
            GameObject.Find(findName).GetComponent<Text>().text = gameControll.carCap.ToString();
            //Debug.Log (gameControll.redDebrisTotal);
            //Debug.Log (panelController.redText);
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            panelController.redTextOnce.text = gameControll.redProfitOnce.ToString();
            panelController.redTimeOnce.text = gameControll.redTimeOnce.ToString();
        }

        if (gameControll.blueTruck)
        {
            gameControll.blueProfitTotal += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.blueProfitOnce += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.blueTimeTotal += gameControll.timeArray[node1, node2] + num * 10;
            gameControll.blueTimeOnce += gameControll.timeArray[node1, node2] + num * 10;
            string findName = "blueTruckText" + (gameControll.blueTruckNum - 1).ToString();
            GameObject.Find(findName).GetComponent<Text>().text = gameControll.carCap.ToString();
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();
            panelController.blueTextOnce.text = gameControll.blueProfitOnce.ToString();
            panelController.blueTimeOnce.text = gameControll.blueTimeOnce.ToString();
        }

        if (gameControll.greenTruck)
        {
            gameControll.greenProfitTotal += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.greenProfitOnce += num * 10 - gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.greenTimeTotal += gameControll.timeArray[node1, node2] + num * 10;
            gameControll.greenTimeOnce += gameControll.timeArray[node1, node2] + num * 10;
            string findName = "greenTruckText" + (gameControll.greenTruckNum - 1).ToString();
            GameObject.Find(findName).GetComponent<Text>().text = gameControll.carCap.ToString();
            panelController.greenText.text = gameControll.greenProfitTotal.ToString();
            panelController.greenTime.text = gameControll.greenTimeTotal.ToString();
            panelController.greenTextOnce.text = gameControll.greenProfitOnce.ToString();
            panelController.greenTimeOnce.text = gameControll.greenTimeOnce.ToString();
        }
    }

    //this setInital for the modify in the update time
    public static void setInitialValue(int node1, int node2)
    {
        if (gameControll.redTruck)
        {
            gameControll.redProfitTotal -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.redProfitOnce -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.redTimeTotal += gameControll.timeArray[node1, node2];
            gameControll.redTimeOnce += gameControll.timeArray[node1, node2];
            scrollBasicProfit = gameControll.redProfitTotal;
            scrollBasicProfitOnce = gameControll.redProfitOnce;
            scrollBasicTime = gameControll.redTimeTotal;
            scrollBasicTimeOnce = gameControll.redTimeOnce;
            string findName = "redTruckText" + (gameControll.redTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS + "/100";
            //			originCarCap = gameControll.carCap;
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.redDebrisTotal);
            //Debug.Log (panelController.redText);
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            panelController.redTextOnce.text = gameControll.redProfitOnce.ToString();
            panelController.redTimeOnce.text = gameControll.redTimeOnce.ToString();
        }

        if (gameControll.blueTruck)
        {
            gameControll.blueProfitTotal -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.blueProfitOnce -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.blueTimeTotal += gameControll.timeArray[node1, node2];
            gameControll.blueTimeOnce += gameControll.timeArray[node1, node2];
            scrollBasicProfit = gameControll.blueProfitTotal;
            scrollBasicProfitOnce = gameControll.blueProfitOnce;
            scrollBasicTime = gameControll.blueTimeTotal;
            scrollBasicTimeOnce = gameControll.blueTimeOnce;
            //Debug.Log ("initial" + gameControll.blueTimeOnce);
            //			originCarCap = gameControll.carCap;
            string findName = "blueTruckText" + (gameControll.blueTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.blueDebrisTotal);
            //Debug.Log (panelController.blueText);
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();
            panelController.blueTextOnce.text = gameControll.blueProfitOnce.ToString();
            panelController.blueTimeOnce.text = gameControll.blueTimeOnce.ToString();
        }

        if (gameControll.greenTruck)
        {
            gameControll.greenProfitTotal -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.greenProfitOnce -= gameControll.timeArray[node1, node2] / 2 * 7;
            gameControll.greenTimeTotal += gameControll.timeArray[node1, node2];
            gameControll.greenTimeOnce += gameControll.timeArray[node1, node2];
            scrollBasicProfit = gameControll.greenProfitTotal;
            scrollBasicProfitOnce = gameControll.greenProfitOnce;
            scrollBasicTime = gameControll.greenTimeTotal;
            scrollBasicTimeOnce = gameControll.greenTimeOnce;
            //			originCarCap = gameControll.carCap;
            string findName = "greenTruckText" + (gameControll.greenTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.greenDebrisTotal);
            //Debug.Log (panelController.greenText);
            panelController.greenText.text = gameControll.greenProfitTotal.ToString();
            panelController.greenTime.text = gameControll.greenTimeTotal.ToString();
            panelController.greenTextOnce.text = gameControll.greenProfitOnce.ToString();
            panelController.greenTimeOnce.text = gameControll.greenTimeOnce.ToString();
        }
        pathCap.initializeSlider();
    }

    public static void modifyInUpdate(int num, int node1, int node2)
    {
        gameControll.carCap -= num;
        gameControll.capArray[node1, node2] -= num;
        gameControll.capArray[node2, node1] -= num;
        if (gameControll.redTruck)
        {
            gameControll.redProfitTotal += num * 10;
            gameControll.redProfitOnce += num * 10;
            gameControll.redTimeTotal += num * 10;
            gameControll.redTimeOnce += num * 10;
            string findName = "redTruckText" + (gameControll.redTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.redDebrisTotal);
            //Debug.Log (panelController.redText);
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            panelController.redTextOnce.text = gameControll.redProfitOnce.ToString();
            panelController.redTimeOnce.text = gameControll.redTimeOnce.ToString();
        }

        if (gameControll.blueTruck)
        {
            gameControll.blueProfitTotal += num * 10;
            gameControll.blueProfitOnce += num * 10;
            gameControll.blueTimeTotal += num * 10;
            gameControll.blueTimeOnce += num * 10;
            string findName = "blueTruckText" + (gameControll.blueTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.blueDebrisTotal);
            //Debug.Log (panelController.blueText);
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();
            panelController.blueTextOnce.text = gameControll.blueProfitOnce.ToString();
            panelController.blueTimeOnce.text = gameControll.blueTimeOnce.ToString();
        }

        if (gameControll.greenTruck)
        {
            gameControll.greenProfitTotal += num * 10;
            gameControll.greenProfitOnce += num * 10;
            gameControll.greenTimeTotal += num * 10;
            gameControll.greenTimeOnce += num * 10;
            string findName = "greenTruckText" + (gameControll.greenTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.greenDebrisTotal);
            //Debug.Log (panelController.greenText);
            panelController.greenText.text = gameControll.greenProfitTotal.ToString();
            panelController.greenTime.text = gameControll.greenTimeTotal.ToString();
            panelController.greenTextOnce.text = gameControll.greenProfitOnce.ToString();
            panelController.greenTimeOnce.text = gameControll.greenTimeOnce.ToString();
        }
    }

    //count the profit of modify the number by slider
    public static void modifyBySlider(int origin, int now, int originCar)
    {
        //Debug.Log ("carCap " + originCar);
        gameControll.carCap = originCar - (origin - now);
        if (gameControll.redTruck)
        {
            gameControll.redProfitTotal = scrollBasicProfit + (origin - now) * 10;
            gameControll.redProfitOnce = scrollBasicProfitOnce + (origin - now) * 10;
            gameControll.redTimeTotal = scrollBasicTime + (origin - now) * 10;
            gameControll.redTimeOnce = scrollBasicTimeOnce + (origin - now) * 10;
            string findName = "redTruckText" + (gameControll.redTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.redDebrisTotal);
            //Debug.Log (panelController.redText);
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            panelController.redTextOnce.text = gameControll.redProfitOnce.ToString();
            panelController.redTimeOnce.text = gameControll.redTimeOnce.ToString();
        }

        if (gameControll.blueTruck)
        {
            gameControll.blueProfitTotal = scrollBasicProfit + (origin - now) * 10;
            gameControll.blueProfitOnce = scrollBasicProfitOnce + (origin - now) * 10;
            gameControll.blueTimeTotal = scrollBasicTime + (origin - now) * 10;
            gameControll.blueTimeOnce = scrollBasicTimeOnce + (origin - now) * 10;
            string findName = "blueTruckText" + (gameControll.blueTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.blueDebrisTotal);
            //Debug.Log (panelController.blueText);
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();
            panelController.blueTextOnce.text = gameControll.blueProfitOnce.ToString();
            panelController.blueTimeOnce.text = gameControll.blueTimeOnce.ToString();
        }

        if (gameControll.greenTruck)
        {
            gameControll.greenProfitTotal = scrollBasicProfit + (origin - now) * 10;
            gameControll.greenProfitOnce = scrollBasicProfitOnce + (origin - now) * 10;
            gameControll.greenTimeTotal = scrollBasicTime + (origin - now) * 10;
            gameControll.greenTimeOnce = scrollBasicTimeOnce + (origin - now) * 10;
            string findName = "greenTruckText" + (gameControll.greenTruckNum - 1).ToString();
            int truckS = 100 - gameControll.carCap;
            string truckStore = truckS.ToString() + "/100";
            GameObject.Find(findName).GetComponent<Text>().text = truckStore;
            //Debug.Log (gameControll.greenDebrisTotal);
            //Debug.Log (panelController.greenText);
            panelController.greenText.text = gameControll.greenProfitTotal.ToString();
            panelController.greenTime.text = gameControll.greenTimeTotal.ToString();
            panelController.greenTextOnce.text = gameControll.greenProfitOnce.ToString();
            panelController.greenTimeOnce.text = gameControll.greenTimeOnce.ToString();
        }
    }

    //the funtion is used to change node color.
    private void changeNodeColor(string toWhich)
    {
        string pathToNode = "Node/" + toWhich;
        this.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToNode) as Sprite;
        string dicName = "node" + passNode2.ToString();
        //Debug.Log("this is dicName " + dicName);
        if (nodeDic.ContainsKey(dicName))
        {
            nodeDic.Remove(dicName);
            nodeDic.Add(dicName, pathToNode);
        }
        else
        {
            nodeDic.Add(dicName, pathToNode);
        }
    }


    private void clickChangeColor()
    {
        if (!(redN || greenN || blueN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                changeNodeColor("R");
                redN = true;
            }
            else if (gameControll.blueTruck)
            {
                changeNodeColor("B");
                blueN = true;
            }
            else if (gameControll.greenTruck)
            {
                changeNodeColor("G");
                greenN = true;
            }
        }
        else if (redN && !(greenN || blueN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.blueTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RB");
                blueN = true;
                rbN = true;
            }
            else if (gameControll.greenTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RG");
                greenN = true;
                rgN = true;
            }
        }
        else if (blueN && !(greenN || redN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RB");
                redN = true;
                rbN = true;
            }
            else if (gameControll.greenTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("GB");
                greenN = true;
                gbN = true;
            }
        }
        else if (greenN && !(blueN || redN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RG");
                redN = true;
                rgN = true;
            }
            else if (gameControll.blueTruck)
            {
                if (this.gameObject.name != "depot")
                    gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("GB");
                blueN = true;
                gbN = true;
            }
        }
        else if (rbN && !(greenN || rgN || gbN || rgbN))
        {
            if (gameControll.greenTruck)
            {
                changeNodeColor("RGB");
                greenN = true;
                rgN = true;
                gbN = true;
                rgbN = true;
            }
        }
        else if (rgN && !(blueN || rbN || gbN || rgbN))
        {
            if (gameControll.blueTruck)
            {
                changeNodeColor("RGB");
                blueN = true;
                rbN = true;
                gbN = true;
                rgbN = true;
            }
        }
        else if (gbN && !(redN || rbN || rgN || rgbN))
        {
            if (gameControll.redTruck)
            {
                changeNodeColor("RGB");
                redN = true;
                rbN = true;
                rgN = true;
                rgbN = true;
            }
        }
    }

    private void changeNodeColor(string toWhich, int num)
    {
        string pathToNode = "Node/" + toWhich;
        this.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToNode) as Sprite;
        string dicName = "node" + num.ToString();
        //Debug.Log (dicName); 
        //GameObject.Find (dicName).GetComponent<Image> ().sprite = Resources.Load<Sprite> (pathToNode) as Sprite;
        //Debug.Log("this is dicName " + dicName);
        if (nodeDic.ContainsKey(dicName))
        {
            nodeDic.Remove(dicName);
            nodeDic.Add(dicName, pathToNode);
        }
        else
        {
            nodeDic.Add(dicName, pathToNode);
        }
    }

    public static void changeNodeColor(string toWhich, GameObject n) {
        string pathToNode = "Node/" + toWhich;
        n.GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToNode) as Sprite;
        string dicName = n.name;
        //Debug.Log("this is dicName " + dicName);
        if (nodeDic.ContainsKey(dicName))
        {
            nodeDic.Remove(dicName);
            nodeDic.Add(dicName, pathToNode);
        }
        else
        {
            nodeDic.Add(dicName, pathToNode);
        }
    }

    private void clickChangeColor(int num)
    {
        //string name = "node" + num.ToString ();
        if (!(redN || greenN || blueN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                changeNodeColor("R", num);
                redN = true;
            }
            else if (gameControll.blueTruck)
            {
                changeNodeColor("B", num);
                blueN = true;
            }
            else if (gameControll.greenTruck)
            {
                changeNodeColor("G", num);
                greenN = true;
            }
        }
        else if (redN && !(greenN || blueN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.blueTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RB", num);
                blueN = true;
                rbN = true;
            }
            else if (gameControll.greenTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RG", num);
                greenN = true;
                rgN = true;
            }
        }
        else if (blueN && !(greenN || redN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RB", num);
                redN = true;
                rbN = true;
            }
            else if (gameControll.greenTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("GB", num);
                greenN = true;
                gbN = true;
            }
        }
        else if (greenN && !(blueN || redN || rgN || rbN || gbN || rgbN))
        {
            if (gameControll.redTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("RG", num);
                redN = true;
                rgN = true;
            }
            else if (gameControll.blueTruck)
            {
                gameControll.intersection++;
                sIntersection.text = gameControll.intersection.ToString();
                changeNodeColor("GB", num);
                blueN = true;
                gbN = true;
            }
        }
        else if (rbN && !(greenN || rgN || gbN || rgbN))
        {
            if (gameControll.greenTruck)
            {
                changeNodeColor("RGB", num);
                greenN = true;
                rgN = true;
                gbN = true;
                rgbN = true;
            }
        }
        else if (rgN && !(blueN || rbN || gbN || rgbN))
        {
            if (gameControll.blueTruck)
            {
                changeNodeColor("RGB", num);
                blueN = true;
                rbN = true;
                gbN = true;
                rgbN = true;
            }
        }
        else if (gbN && !(redN || rbN || rgN || rgbN))
        {
            if (gameControll.redTruck)
            {
                changeNodeColor("RGB", num);
                redN = true;
                rbN = true;
                rgN = true;
                rgbN = true;
            }
        }
    }


    private void clickChangeAnimation(int num1, int num2) {
        string animName1 = "node" + passNode1.ToString();
        string animName2 = "node" + passNode2.ToString();
        if (passNode1 == 1) {
            animName1 = "depot";
        }
        GameObject.Find(animName1).GetComponent<Animator>().enabled = false;
        if (nodeDic.ContainsKey(animName1))
        {
            //Debug.Log("in contains "+animName1);
            string toWhich = nodeDic[animName1];
            string pathToNode = toWhich;
            //GameObject.Find(animName1).GetComponent<Animator>().enabled = false;
            GameObject.Find(animName1).GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToNode) as Sprite;
        }
        if (passNode2 == 1) {
            GameObject.Find(animName1).GetComponent<Animator>().enabled = false;
            return;
        }
        if (gameControll.redTruck) {
            GameObject.Find(animName2).GetComponent<NodeAnimation>().redAnimation();
        }
        if (gameControll.greenTruck) {
            GameObject.Find(animName2).GetComponent<NodeAnimation>().greenAnimation();
        }
        if (gameControll.blueTruck) {
            GameObject.Find(animName2).GetComponent<NodeAnimation>().blueAnimation();
        }
    }

    private void setBoolArray(int num1, int num2, bool[,] rgb) {
        rgb[num1, num2] = true;
        //Debug.Log(num1 + " " + num2 + " "+bluePathNum[num1, num2]);
        if (gameControll.redTruck)
        {
            redPathArray[num1, num2] = true;
            redPathNum[num1, num2]++;
        }

        if (gameControll.greenTruck)
        {
            greenPathArray[num1, num2] = true;
            greenPathNum[num1, num2]++;
        }

        if (gameControll.blueTruck)
        {
            bluePathArray[num1, num2] = true;
            bluePathNum[num1, num2]++;
        }
    }

    private string pathName(int num1, int num2, bool[,] rgb) {
        string name = "";
        if (rgb[num1, num2]) {
            name = "newPathAnim" + num1.ToString() + num2.ToString();
        } else {
            name = "pathAnim" + num1.ToString() + num2.ToString();
        }
        return name;
    }

    //use this function to make the function inside wait few seconds.
    IEnumerator waitAnim(GameObject obj1, GameObject obj2)
    {
        //print(Time.time);
        yield return new WaitForSeconds(2);
        Destroy(obj1);
        setLineArray(passNode1, passNode2);
        //print(Time.time);
        LineRenderer lr = obj2.GetComponent<LineRenderer>();
        if (blueLineArray[passNode1, passNode2] && redLineArray[passNode1, passNode2] && !greenLineArray[passNode1, passNode2])
        {
            lr.material = Resources.Load<Material>("Materials/GradientRB") as Material;
            lr.enabled = true;
        }

        if (!blueLineArray[passNode1, passNode2] && redLineArray[passNode1, passNode2] && greenLineArray[passNode1, passNode2])
        {
            lr.material = Resources.Load<Material>("Materials/GradientRG") as Material;
            lr.enabled = true;
        }

        if (blueLineArray[passNode1, passNode2] && !redLineArray[passNode1, passNode2] && greenLineArray[passNode1, passNode2])
        {
            lr.material = Resources.Load<Material>("Materials/GradientBG") as Material;
            lr.enabled = true;
        }

        if (blueLineArray[passNode1, passNode2] && redLineArray[passNode1, passNode2] && greenLineArray[passNode1, passNode2])
        {
            lr.material = Resources.Load<Material>("Materials/GradientRGB") as Material;
            lr.enabled = true;
        }
    }

    private bool pathAlreadyExist(int num1, int num2, bool red, bool green, bool blue)
    {
        if (red)
        {
            if (redLineArray[num1, num2])
            {
                return true;
            }
        }
        if (blue)
        {
            if (blueLineArray[num1, num2])
            {
                return true;
            }
        }
        if (green)
        {
            if (greenLineArray[num1, num2])
            {
                return true;
            }
        }
        return false;
    }

    private Vector2 getVector(Vector2 v1, Vector2 v2, Vector2 v3) {
        Vector2 res = new Vector2((v1.x + v2.x + v3.x) / 3, (v1.y + v2.y + v3.y) / 3);
        return res;
    }

    //this function compare the nearest position of checkmark
    private static Vector2 getSmallest(Vector2 f1, Vector2 f2, Vector2 f3, float x, float y) {
        if (Mathf.Abs(f1.x - x) + Mathf.Abs(f1.y - y) <= Mathf.Abs(f2.x - x) + Mathf.Abs(f2.y - y) && Mathf.Abs(f1.x - x) + Mathf.Abs(f1.y - y) <= Mathf.Abs(f3.x - x) + Mathf.Abs(f3.y - y)) {
            return f1;
        } else if (Mathf.Abs(f2.x - x) + Mathf.Abs(f2.y - y) <= Mathf.Abs(f3.x - x) + Mathf.Abs(f3.y - y)) {
            return f2;
        } else {
            return f3;
        }
    }


    private static void setConfirmPathButton(List<int> al)
    {
        float x = 0.0f;
        float y = 0.0f;
        HashSet<int> set = new HashSet<int>();
        foreach (int num in al) {
            if (set.Add(num)) {
                string numStr = "node" + num.ToString();
                if (num == 1) {
                    numStr = "depot";
                }
                x += GameObject.Find(numStr).transform.position.x;
                y += GameObject.Find(numStr).transform.position.y;
            }
        }
        nodeCount = al.Count;
        nodeArray = new int[nodeCount];
        //IEnumerator e = al.GetEnumerator ();
        //		while (e.MoveNext ()) {
        //			nodeArray [i] = e.Current;
        //			i++;
        //		}
        int i = 0;
        foreach (int value in al) {
            nodeArray[i] = value;
            i++;
        }
        int count = set.Count;
        x = x / count;
        y = y / count;
        float z = 100f;
        Vector2 v4 = getSmallest(v1, v2, v3, x, y);
        x = v4.x;
        y = v4.y;
        GameObject checkMark = new GameObject();
        checkMark.name = "checkMark";
        Transform parentTransform = GameObject.Find("gamePanel").GetComponent<Transform>();
        checkMark.transform.SetParent(parentTransform);
        checkMark.transform.position = new Vector3(x, y, z);
        checkMark.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        BoxCollider2D collider = checkMark.AddComponent<BoxCollider2D>();
        collider.enabled = true;
        collider.size = new Vector2(100, 100);
        Image cm = checkMark.AddComponent<Image>();
        cm.sprite = Resources.Load<Sprite>("Image/checkmark") as Sprite;
        //cm.color = new Color (1, 0, 0);
        checkMark.AddComponent<CheckMark>();
        checkMark.AddComponent<AudioSource>();
        checkMark.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/success1");
        checkMark.GetComponent<AudioSource>().volume = 0.8f;
        checkMark.GetComponent<AudioSource>().playOnAwake = false;
    }

    public void setLineArray(int num1, int num2)
    {
        if (gameControll.redTruck)
        {
            redLineArray[num1, num2] = true;
            redLineArray[num2, num1] = true;
        }
        if (gameControll.blueTruck)
        {
            blueLineArray[num1, num2] = true;
            blueLineArray[num2, num1] = true;
        }
        if (gameControll.greenTruck)
        {
            greenLineArray[num2, num1] = true;
            greenLineArray[num1, num2] = true;
        }
    }


    private IEnumerable<int> validPathAnimation(int num1) {
        for (int i = 0; i < gameControll.nodePath.GetLength(1); i++) {
            if (gameControll.nodePath[num1, i]) {
                yield return i;
            }
        }
    }



    public bool RedN {
        get {
            return this.redN;
        }

        set {
            this.redN = value;
        }
    }

    public bool GreenN {
        get {
            return this.greenN;
        }

        set {
            this.greenN = value;
        }
    }

    public bool BlueN {
        get {
            return this.blueN;
        }

        set {
            this.blueN = value;
        }
    }
    public bool RBN {
        get {
            return this.rbN;
        }

        set {
            this.rbN = value;
        }
    }

    public bool RGN {
        get {
            return this.rgN;
        }

        set {
            this.rgN = value;
        }
    }

    public bool GBN {
        get {
            return this.gbN;
        }

        set {
            this.gbN = value;
        }
    }

    public bool RGBN {
        get {
            return this.rgbN;
        }

        set {
            this.rgbN = value;
        }
    }


    private void rightClicktoCancelPath(bool r, bool b, bool g) {
        if (passNode2 == this.num && passNode2 != 1 && (r||g||b)) {
            Debug.Log(this.num + " node num");
			gameControll.twoNode.Clear ();
			if (size2last2Node == 1) {
				gameControll.twoNode.Enqueue (1);
			} else if(size2last2Node!=0){
				int num = size2last2Node;
				gameControll.twoNode.Enqueue (size2last3Node);
				gameControll.twoNode.Enqueue (num);
			}

			foreach (int i in validPathAnimation(passNode1)) {
				string temp = "node" + i;
				if (i == 1) {
					temp = "depot";
				}
				Behaviour halo = (Behaviour) GameObject.Find (temp).GetComponent ("Halo");
				halo.enabled = true;
			}

			if (passNode2 != 1) {
				foreach (int i in validPathAnimation(passNode2)) {
					string temp = "node" + i;
					if (i == 1) {
						temp = "depot";
					}
					Behaviour halo = (Behaviour) GameObject.Find (temp).GetComponent ("Halo");
					halo.enabled = false;
				}
			}

            if (storePath.Count >= 4)
            {
                size2lastNode = storePath[storePath.Count - 2];
                size2last2Node = storePath[storePath.Count - 3];
                size2last3Node = storePath[storePath.Count - 4];
                storePath.RemoveAt(storePath.Count - 1);
            }else if (storePath.Count == 3)
            {
                size2lastNode = storePath[storePath.Count - 2];
                size2last2Node = storePath[storePath.Count - 3];
                size2last3Node = 0;
                storePath.RemoveAt(storePath.Count - 1);
            }else if (storePath.Count == 2)
            {
                size2lastNode = storePath[storePath.Count - 2];
                size2last2Node = 0;
                size2last3Node = 0;
                storePath.RemoveAt(storePath.Count - 1);
            }

            gameControll.capArray [size2last2Node, size2lastNode] = originCap;
			//gameControll.carCap = originCarCap;
			Debug.Log("originCap " +originCap);
            pathCap.ReturnToRealValue();
			pathCap.desableSlider ();
			gameControll.redProfitTotal = redProfitLastTime;
			gameControll.blueProfitTotal = blueProfitLastTime;
			gameControll.greenProfitTotal = greenProfitLastTime;
			gameControll.redProfitOnce = 0;
			gameControll.blueProfitOnce = 0;
			gameControll.greenProfitOnce = 0;
			gameControll.redTimeTotal = redTimeLastTime;
			gameControll.blueTimeTotal = blueTimeLastTime;
			gameControll.greenTimeTotal = greenTimeLastTime;
			gameControll.redTimeOnce = 0;
			gameControll.blueTimeOnce = 0;
			gameControll.greenTimeOnce = 0;
			updatePanelText ();
			updateLineRender (r,g,b);
			updateNodeColor (r, g, b);
            //Debug.Log(passNode1+ " passNode1");
            //Debug.Log(passNode2 + " passNode2");
			clickRemoveAnimation (passNode1, passNode2);
            if (storePath.Count >= 2)
            {
                passNode1 = storePath[storePath.Count - 2];
                passNode2 = storePath[storePath.Count - 1];
            }else
            {
                passNode1 = 0;
                passNode2 = 1;
            }
            //Debug.Log(passNode1 + " passNode1  1");
            //Debug.Log(passNode2 + " passNode2  2");
        }
	}

    private void clickRemoveAnimation(int num1, int num2)
    {
        string animName1 = "node" + num1.ToString();
        string animName2 = "node" + num2.ToString();
        if (num1 == 1)
        {
            animName1 = "depot";
        }
        if (num2 == 1)
        {
            animName2 = "depot";
        }

        GameObject.Find(animName2).GetComponent<Animator>().enabled = false;
        if (nodeDic.ContainsKey(animName2))
        {
            //Debug.Log("in contains "+animName1);
            string toWhich = nodeDic[animName2];
            string pathToNode = toWhich;
            //GameObject.Find(animName1).GetComponent<Animator>().enabled = false;
            GameObject.Find(animName2).GetComponent<Image>().sprite = Resources.Load<Sprite>(pathToNode) as Sprite;
        }
        if (num1 == 1)
        {
            //Debug.Log(num + "   1111");
            GameObject.Find(animName1).GetComponent<Animator>().enabled = false;
            return;
        }
        if (gameControll.redTruck)
        {
            GameObject.Find(animName1).GetComponent<NodeAnimation>().redAnimation();
        }
        if (gameControll.greenTruck)
        {
            GameObject.Find(animName1).GetComponent<NodeAnimation>().greenAnimation();
        }
        if (gameControll.blueTruck)
        {
            GameObject.Find(animName1).GetComponent<NodeAnimation>().blueAnimation();
        }
    }

    private void updateLineRender(bool r,bool b,bool g){

		if (r) {
			Node.redPathNum [passNode1, passNode2]--;
			//Debug.Log ("redNum"+Node.redPathNum [passNode1, passNode2]);
			int redNum = redPathNum [passNode1, passNode2];
			int greenNum = greenPathNum [passNode1, passNode2];
			int blueNum = bluePathNum [passNode1, passNode2];

            //Debug.Log("blueNum" + Node.bluePathNum[passNode1, passNode2]);
            //            int redNum1 = Node.redPathNum[num2, num1];
            string pathString = "pathAnim" + passNode1.ToString () + passNode2.ToString ();
			GameObject pathObj= GameObject.Find (pathString);


			if (redNum == 0) {
				Node.redLineArray [passNode1, passNode2] = false;
				Node.redLineArray [passNode2, passNode1] = false;
				Node.redPathArray [passNode1, passNode2] = false;
				//				}
				if (greenNum == 0 && blueNum == 0 ) {
					//Destroy (pathObj);
					pathObj.GetComponent<LineRenderer>().enabled=false;
				} else if (greenNum > 0 && blueNum == 0 ) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/greenAnim") as Material;
				} else if (greenNum == 0 && blueNum > 0 ) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/blueAnim") as Material;
				} else if (greenNum > 0 && blueNum > 0 ) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/GradientBG") as Material;
				}
			}
				
		}

		if (g) {
			Node.greenPathNum [passNode1, passNode2]--;
			int redNum = Node.redPathNum [passNode1, passNode2];
			int greenNum = Node.greenPathNum [passNode1, passNode2];
			int blueNum = Node.bluePathNum [passNode1, passNode2];
			//            int greenNum1 = Node.greenPathNum[num2, num1];
			string pathString = "pathAnim" + passNode1.ToString () + passNode2.ToString ();
			GameObject pathObj= GameObject.Find (pathString);

			if (greenNum == 0) {
				Node.greenLineArray [passNode1, passNode2] = false;
				Node.greenLineArray [passNode2, passNode1] = false;
				Node.greenPathArray [passNode1, passNode2] = false;
				if (blueNum == 0 && redNum == 0) {
					pathObj.GetComponent<LineRenderer>().enabled=false;
				} else if (redNum > 0 && blueNum == 0 ) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/redAnim") as Material;
				} else if (redNum == 0 && blueNum > 0 ) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/blueAnim") as Material;
				} else if (redNum > 0 && blueNum > 0) {
					pathObj.GetComponent<LineRenderer> ().material = Resources.Load<Material> ("Materials/GradientRB") as Material;
				}	
			}
		}

		if (b) {
			Node.bluePathNum [passNode1, passNode2]--;

			int redNum = Node.redPathNum [passNode1, passNode2];
			int greenNum = Node.greenPathNum [passNode1, passNode2];
			int blueNum = Node.bluePathNum [passNode1, passNode2];
			string pathString = "pathAnim" + passNode1.ToString () + passNode2.ToString ();
			GameObject pathObj= GameObject.Find (pathString);
			if (blueNum == 0) {
				Node.blueLineArray [passNode1, passNode2] = false;
				Node.blueLineArray [passNode2, passNode1] = false;
				Node.bluePathArray [passNode1, passNode2] = false;
				if (greenNum == 0 && redNum == 0)
				{
					pathObj.GetComponent<LineRenderer>().enabled = false;
				}
				else if (redNum > 0 && greenNum == 0)
				{
					pathObj.GetComponent<LineRenderer>().material = Resources.Load<Material>("Materials/redAnim") as Material;
				}
				else if (redNum == 0 && greenNum > 0)
				{
					pathObj.GetComponent<LineRenderer>().material = Resources.Load<Material>("Materials/greenAnim") as Material;
				}
				else if (redNum > 0 && greenNum > 0)
				{
					pathObj.GetComponent<LineRenderer>().material = Resources.Load<Material>("Materials/GradientRG") as Material;
				}
			}
		}
	}
		

	private void updateNodeColor(bool r, bool b, bool g){
		int sumR = 0;
		int sumG = 0;
		int sumB = 0;
		for (int j = 1; j < 6; j++) {
			sumR += Node.redPathNum [passNode2, j];
			sumR += Node.redPathNum [j, passNode2];
			sumG += Node.greenPathNum [passNode2, j];
			sumG += Node.greenPathNum [j, passNode2];
			sumB += Node.bluePathNum [passNode2, j];
			sumB += Node.bluePathNum [j, passNode2];
		}
		if (r) {
			if (sumR == 0) {
				string strNode = "node" + passNode2;
				GameObject node = GameObject.Find (strNode);
				if (passNode2 != 1) {
					if ((node.GetComponent<Node> ().RGN || node.GetComponent<Node> ().RBN) && !node.GetComponent<Node> ().RGBN) {
						gameControll.intersection--;
						if (gameControll.intersection < 0)
							gameControll.intersection = 0;
						GameObject.Find ("intersection").GetComponent<Text> ().text = gameControll.intersection.ToString ();
					}

					node.GetComponent<Node> ().RedN = false;
					node.GetComponent<Node> ().RGN = false;
					node.GetComponent<Node> ().RBN = false;
					node.GetComponent<Node> ().RGBN = false;
				}
				if (sumG == 0 && sumB == 0 && passNode2 != 1) {
					node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/node") as Sprite;
					string dicName = "node" + passNode2;
					if (Node.nodeDic.ContainsKey (dicName)) {
						Node.nodeDic.Remove (dicName);
						Node.nodeDic.Add (dicName, "Node/node");
					} else {
						Node.nodeDic.Add (dicName, "Node/node");
					}
				} else if (sumG > 0 && sumB == 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/G") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/G");
						} else {
							Node.nodeDic.Add (dicName, "Node/G");
						}
					}
				} else if (sumG == 0 && sumB > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/B") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/B");
						} else {
							Node.nodeDic.Add (dicName, "Node/B");
						}
					}
				} else if (sumG > 0 && sumB > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/GB") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/GB");
						} else {
							Node.nodeDic.Add (dicName, "Node/GB");
						}
					}
				}
			}
		}
		if (b) {
			if (sumB == 0) {
				string strNode = "node" + passNode2;
				GameObject node = GameObject.Find (strNode);
				if (passNode2 != 1) {
					if ((node.GetComponent<Node> ().GBN || node.GetComponent<Node> ().RBN) && !node.GetComponent<Node> ().RGBN) {
						gameControll.intersection--;
						if (gameControll.intersection < 0)
							gameControll.intersection = 0;
						GameObject.Find ("intersection").GetComponent<Text> ().text = gameControll.intersection.ToString ();
					}

					node.GetComponent<Node> ().BlueN = false;
					node.GetComponent<Node> ().RBN = false;
					node.GetComponent<Node> ().GBN = false;
					node.GetComponent<Node> ().RGBN = false;
				}
				if (sumG == 0 && sumR == 0 && passNode2 != 1) {
					node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/node") as Sprite;
					string dicName = "node" + passNode2;
					if (Node.nodeDic.ContainsKey (dicName)) {
						Node.nodeDic.Remove (dicName);
						Node.nodeDic.Add (dicName, "Node/node");
					} else {
						Node.nodeDic.Add (dicName, "Node/node");
					}
				} else if (sumG > 0 && sumR == 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/G") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/G");
						} else {
							Node.nodeDic.Add (dicName, "Node/G");
						}
					}
				} else if (sumG == 0 && sumR > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/R") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/R");
						} else {
							Node.nodeDic.Add (dicName, "Node/R");
						}
					}
				} else if (sumG > 0 && sumR > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/RG") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/RG");
						} else {
							Node.nodeDic.Add (dicName, "Node/RG");
						}
					}
				}
			}
		}


		if (g) {
			if (sumG == 0) {
				string strNode = "node" + passNode2;
				GameObject node = GameObject.Find (strNode);
				if (passNode2 != 1) {
					if ((node.GetComponent<Node> ().GBN || node.GetComponent<Node> ().RGN) && !node.GetComponent<Node> ().RGBN) {
						gameControll.intersection--;
						if (gameControll.intersection < 0)
							gameControll.intersection = 0;
						GameObject.Find ("intersection").GetComponent<Text> ().text = gameControll.intersection.ToString ();
					}

					node.GetComponent<Node> ().GreenN = false;
					node.GetComponent<Node> ().RGN = false;
					node.GetComponent<Node> ().GBN = false;
					node.GetComponent<Node> ().RGBN = false;
				}
				if (sumB == 0 && sumR == 0 && passNode2 != 1) {
					node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/node") as Sprite;
					string dicName = "node" + passNode2;
					if (Node.nodeDic.ContainsKey (dicName)) {
						Node.nodeDic.Remove (dicName);
						Node.nodeDic.Add (dicName, "Node/node");
					} else {
						Node.nodeDic.Add (dicName, "Node/node");
					}
				} else if (sumB > 0 && sumR == 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/B") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/B");
						} else {
							Node.nodeDic.Add (dicName, "Node/B");
						}
					}
				} else if (sumB == 0 && sumR > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/R") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/R");
						} else {
							Node.nodeDic.Add (dicName, "Node/R");
						}
					}
				} else if (sumB > 0 && sumR > 0) {
					if (passNode2 != 1) {
						node.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Node/RB") as Sprite;
						string dicName = "node" + passNode2;
						if (Node.nodeDic.ContainsKey (dicName)) {
							Node.nodeDic.Remove (dicName);
							Node.nodeDic.Add (dicName, "Node/RB");
						} else {
							Node.nodeDic.Add (dicName, "Node/RB");
						}
					}
				}
			}
		}

	}

	private void updatePanelText(){
		panelController.redText.text = gameControll.redProfitTotal.ToString();
		panelController.redTime.text = gameControll.redTimeTotal.ToString();
		panelController.redTextOnce.text = gameControll.redProfitOnce.ToString();
		panelController.redTimeOnce.text = gameControll.redTimeOnce.ToString();
		panelController.blueText.text = gameControll.blueProfitTotal.ToString();
		panelController.blueTime.text = gameControll.blueTimeTotal.ToString();
		panelController.blueTextOnce.text = gameControll.blueProfitOnce.ToString();
		panelController.blueTimeOnce.text = gameControll.blueTimeOnce.ToString();
		panelController.greenText.text = gameControll.greenProfitTotal.ToString();
		panelController.greenTime.text = gameControll.greenTimeTotal.ToString();
		panelController.greenTextOnce.text = gameControll.greenProfitOnce.ToString();
		panelController.greenTimeOnce.text = gameControll.greenTimeOnce.ToString();
	}


    private void getInitialSolution()
    {

        if (this.num == 1)
        {
            //make this as a function will save a lot of time
            //I can just input routes as the variable in game to build this route.
            //first path

            capPath = GameObject.FindGameObjectsWithTag("cap");

            foreach (GameObject obj in capPath)
            {
                //obj.GetComponentInChildren<Text>().text = "50";
                obj.GetComponentInChildren<Text>().text = "0";
            }

            gameControll.blueTruck = true;
            gameControll.redTruck = false;
            string pathname = pathName(1, 3, rgbPathArray);
            setBoolArray(1, 3, rgbPathArray);
            GameObject path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            string dupObj = "newPathAnim13";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim13";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(1, 3);

            pathname = pathName(3, 2, rgbPathArray);
            setBoolArray(3, 2, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim32";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim32";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(3, 2);

            pathname = pathName(2, 1, rgbPathArray);
            setBoolArray(2, 1, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim21";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim21";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(2, 1);

            blueAl.Add(new List<int>() { 1, 3, 2, 1 });
            //clickChangeColor (3);
            //clickChangeColor (2);
            blueTruckCap.Add(new List<float>() { 0, 50, 50 });
            float profit = -(3 + 3 + 1) / 2 * 7 + 10 * 100;
            float time = 100 * 10 + 7;
            blueProfitAl.Add(profit);
            blueTimeAl.Add(time);


            //		GameObject.Find("storeTruck").GetComponent<storeTruck>().addTruck(0);
            //		gameControll.blueTruckNum++;

            //storePath.Clear ();
            gameControll.blueTruck = true;
            gameControll.redTruck = false;
            pathname = pathName(1, 2, rgbPathArray);
            setBoolArray(1, 2, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim12";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim12";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(1, 2);

            pathname = pathName(2, 5, rgbPathArray);
            setBoolArray(2, 5, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim25";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim25";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(2, 5);

            pathname = pathName(5, 2, rgbPathArray);
            setBoolArray(5, 2, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim52";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim52";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(5, 2);


            pathname = pathName(2, 1, rgbPathArray);
            setBoolArray(2, 1, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim21";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim21";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(2, 1);

            //nodeBackToDepot ();
            blueAl.Add(new List<int>() { 1, 2, 5, 2, 1 });
            blueTruckCap.Add(new List<float>() { 0, 50, 0, 0 });
            float profit1 = -(1 + 1.8f + 1.8f + 1) / 2 * 7 + 10 * 50;
            float time1 = 50 * 10 + 1 + 1.8f + 1.8f + 1;
            profit += -(1 + 1.8f + 1.8f + 1) / 2 * 7 + 10 * 50;
            time += 50 * 10 + 1 + 1.8f + 1.8f + 1;
            blueProfitAl.Add(profit1);
            blueTimeAl.Add(time1);
            //clickChangeColor (2);
            //clickChangeColor (5);
            //clickChangeColor (2);
            gameControll.blueProfitTotal = profit;
            gameControll.blueTimeTotal = time;
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();

            //storePath.Clear ();
            //
            //		GameObject.Find("storeTruck").GetComponent<storeTruck>().addTruck(1);
            //		gameControll.blueTruckNum++;

            gameControll.redTruck = true;
            gameControll.blueTruck = false;
            pathname = pathName(1, 3, rgbPathArray);
            setBoolArray(1, 3, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim13";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim13";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(1, 3);

            pathname = pathName(3, 4, rgbPathArray);
            setBoolArray(3, 4, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim34";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim34";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(3, 4);

            pathname = pathName(4, 1, rgbPathArray);
            setBoolArray(4, 1, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim41";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim41";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(4, 1);

            redAl.Add(new List<int>() { 1, 3, 4, 1 });
            redTruckCap.Add(new List<float> { 50, 50, 0 });
            //clickChangeColor (3);
            //clickChangeColor (4);
            profit = -(3 + 5 + 4) / 2 * 7 + 10 * 100;
            time = 100 * 10 + 3 + 5 + 4;
            redProfitAl.Add(profit);
            redTimeAl.Add(time);
            //		GameObject.Find("storeTruck").GetComponent<storeTruck>().addTruck(0);
            //		gameControll.redTruckNum++;


            pathname = pathName(1, 4, rgbPathArray);
            setBoolArray(1, 4, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim14";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim14";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(1, 4);

            pathname = pathName(4, 5, rgbPathArray);
            setBoolArray(4, 5, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim45";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim45";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(4, 5);

            pathname = pathName(5, 4, rgbPathArray);
            setBoolArray(5, 4, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim54";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim54";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(5, 4);

            pathname = pathName(4, 1, rgbPathArray);
            setBoolArray(4, 1, rgbPathArray);
            path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            dupObj = "newPathAnim41";
            if (pathname == dupObj)
            {
                string existObj = "pathAnim41";
                if (GameObject.Find(existObj) != null)
                {
                    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
                    //setIndicatorUnseen (num1, num2);
                }
            }
            path.GetComponent<LineAnimation>().rectAnimation(4, 1);

            redAl.Add(new List<int>() { 1, 4, 5, 4, 1 });
            redTruckCap.Add(new List<float> { 50, 50, 0, 0 });
            //clickChangeColor (4);
            //clickChangeColor (5);
            //clickChangeColor (4);
            profit1 = -(4 + 5 + 5 + 4) / 2 * 7 + 10 * 100;
            profit += -(4 + 5 + 5 + 4) / 2 * 7 + 10 * 100;
            time1 = 100 * 10 + 18;
            time += 100 * 10 + 18;
            redProfitAl.Add(profit1);
            redTimeAl.Add(time1);
            gameControll.redProfitTotal = profit;
            gameControll.redTimeTotal = time;
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            //		GameObject.Find("storeTruck").GetComponent<storeTruck>().addTruck(1);
            //		gameControll.redTruckNum++;
            gameControll.redTruck = false;
            //			intersection = 2;
            //			sIntersection.text = intersection.ToString ();

            //			for (int i = 1; i < 6; i++) {
            //				for (int j = 1; j < 6; j++) {
            //					Debug.Log(i+ " "+j+ " "+redPathNum[i,j]);
            //				}
            //			}
        }

        if (this.num == 2)
        {
            gameControll.blueTruck = true;
            clickChangeColor(2);
            //			Debug.Log (redN.ToString() + this.num);
            //			Debug.Log (blueN.ToString() + this.num);
            //			Debug.Log (RBN.ToString() + this.num);
            //			Debug.Log (intersection+" " + this.num);
            gameControll.blueTruck = false;
        }

        if (this.num == 4)
        {
            gameControll.redTruck = true;
            clickChangeColor(4);
            gameControll.redTruck = false;
            //			Debug.Log (redN.ToString() + this.num);
            //			Debug.Log (blueN.ToString() + this.num);
            //			Debug.Log (RBN.ToString() + this.num);
            //			Debug.Log (intersection+" " + this.num);
        }


        if (this.num == 5)
        {
            gameControll.redTruck = true;
            clickChangeColor(5);
            gameControll.redTruck = false;
            gameControll.blueTruck = true;
            clickChangeColor(5);
            gameControll.blueTruck = false;
            //			Debug.Log (redN.ToString() + this.num);
            //			Debug.Log (blueN.ToString() + this.num);
            //			Debug.Log (RBN.ToString() + this.num);
            //			Debug.Log (intersection+" " + this.num);
        }

        if (this.num == 3)
        {
            gameControll.redTruck = true;
            clickChangeColor(3);
            gameControll.redTruck = false;
            gameControll.blueTruck = true;
            clickChangeColor(3);
            gameControll.blueTruck = false;
            //			Debug.Log (redN.ToString() + this.num);
            //			Debug.Log (blueN.ToString() + this.num);
            //			Debug.Log (RBN.ToString() + this.num);
            //			Debug.Log (intersection+" " + this.num);
        }
    }

    private void getInitialSolution0(TruckPath TrackSolution)
    {
        var redD = TrackSolution.RedDic;
        var blueD = TrackSolution.BlueDic;
        if (this.num == 1)
        {
            capPath = GameObject.FindGameObjectsWithTag("cap");

            foreach (GameObject obj in capPath)
            {
                //obj.GetComponentInChildren<Text>().text = "50";
                obj.GetComponentInChildren<Text>().text = "0";
            }
            float profit = 0f;
            float time = 0f;
            foreach(KeyValuePair<List<int>,List<float>> entry in blueD)
            {
                List<int> truckPathList = entry.Key;
                List<float> truckCapList = entry.Value;
                float singleProfit=0;
                float singleTime = 0;
                GameObject indi = new GameObject();
                indi.AddComponent<CheckMark>();
                indi.AddComponent<storeTruck>();
                for (int i = 0; i < truckPathList.Count - 1; i++)
                {
                    addSinglePath(truckPathList[i], truckPathList[i + 1], true, false);
                    indi.GetComponent<CheckMark>().addIndicator(truckPathList[i], truckPathList[i + 1], false, false, true);
                    profit += -gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] / 2 * 7 + truckCapList[i] * 10;
                    time+= gameControll.timeArray[truckPathList[i], truckPathList[i + 1]]+ truckCapList[i] * 10;
                    singleProfit += -gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] / 2 * 7 + truckCapList[i] * 10;
                    singleTime += gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] + truckCapList[i] * 10;
                }
                blueAl.Add(truckPathList);
                blueTruckCap.Add(truckCapList);
                blueProfitAl.Add(singleProfit);
                blueTimeAl.Add(singleTime);
                indi.GetComponent<storeTruck>().initializeValue(false, true);
            }
            gameControll.blueProfitTotal = profit;
            gameControll.blueTimeTotal = time;
            panelController.blueText.text = gameControll.blueProfitTotal.ToString();
            panelController.blueTime.text = gameControll.blueTimeTotal.ToString();

            profit = 0f;
            time = 0f;
            foreach (KeyValuePair<List<int>, List<float>> entry in redD)
            {
                List<int> truckPathList = entry.Key;
                List<float> truckCapList = entry.Value;
                float singleProfit = 0;
                float singleTime = 0;
                //CheckMark indicator = new CheckMark();
                GameObject indi = new GameObject();
                indi.AddComponent<CheckMark>();
                indi.AddComponent<storeTruck>();
                for (int i = 0; i < truckPathList.Count - 1; i++)
                {
                    addSinglePath(truckPathList[i], truckPathList[i + 1], false, true);
                    indi.GetComponent<CheckMark>().addIndicator(truckPathList[i], truckPathList[i + 1], true, false, false);
                    profit += -gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] / 2 * 7 + truckCapList[i] * 10;
                    time += gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] + truckCapList[i] * 10;
                    singleProfit += -gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] / 2 * 7 + truckCapList[i] * 10;
                    singleTime += gameControll.timeArray[truckPathList[i], truckPathList[i + 1]] + truckCapList[i] * 10;
                }
                redAl.Add(truckPathList);
                redTruckCap.Add(truckCapList);
                redProfitAl.Add(singleProfit);
                redTimeAl.Add(singleTime);
                indi.GetComponent<storeTruck>().initializeValue(true, false);
            }
            gameControll.redProfitTotal = profit;
            gameControll.redTimeTotal = time;
            panelController.redText.text = gameControll.redProfitTotal.ToString();
            panelController.redTime.text = gameControll.redTimeTotal.ToString();
            gameControll.blueTruck = false;
            gameControll.redTruck = false;
        }

        IList<HashSet<int>> colorshouldChange=colorfulNode(TrackSolution);
        foreach(int t in colorshouldChange[0])
        {
            if (this.num == t && t!=1)
            {
                gameControll.blueTruck = true;
                clickChangeColor(t);
                gameControll.blueTruck = false;
            }
        }

        foreach (int t in colorshouldChange[1])
        {
            if (this.num == t && t!=1)
            {
                gameControll.redTruck = true;
                clickChangeColor(t);
                gameControll.redTruck = false;
            }
        }
    }


    void addSinglePath(int f, int s, bool b, bool r)
    {
        gameControll.blueTruck = b;
        gameControll.redTruck = r;
        string pathname = pathName(f, s, rgbPathArray);
        string dupObj = "newPathAnim" + f.ToString() + s.ToString();
        setBoolArray(f, s, rgbPathArray);
        GameObject path=new GameObject();
        //GameObject path = new GameObject();
        //path.name = pathname;
        //path.AddComponent<LineRenderer>();
        //path.tag = "linerender";
        //path.AddComponent<LineAnimation>();
        if (pathname != dupObj)
        {
            //string existObj = "pathAnim" + f.ToString() + s.ToString();
            //if (GameObject.Find(existObj) != null)
            //{
            //    GameObject.Find(existObj).GetComponent<LineRenderer>().enabled = false;
            //}
            //path = new GameObject();
            path.name = pathname;
            path.AddComponent<LineRenderer>();
            path.tag = "linerender";
            path.AddComponent<LineAnimation>();
            path.GetComponent<LineAnimation>().rectAnimation(f, s);

        }
    }

    IList<HashSet<int>> colorfulNode(TruckPath TrackSolution)
    {
        var redD = TrackSolution.RedDic;
        var blueD = TrackSolution.BlueDic;
        HashSet<int> redNode = new HashSet<int>();
        HashSet<int> blueNode = new HashSet<int>();
        IList<HashSet<int>> res = new List<HashSet<int>>();
        foreach(KeyValuePair<List<int>, List<float>> entry in blueD)
        {
            foreach(int t in entry.Key)
            {
                blueNode.Add(t);
            }
        }
        res.Add(blueNode);

        foreach (KeyValuePair<List<int>, List<float>> entry in redD)
        {
            foreach (int t in entry.Key)
            {
                redNode.Add(t);
            }
        }
        res.Add(redNode);

        return res;
    }
}

public class TruckPath
{
    //bool redPath;
    //bool bluePath;
 //   List<int> truckPathList;
 //   List<int> truckCapList;
    Dictionary<List<int>, List<float>> blueDic;
    Dictionary<List<int>, List<float>> redDic;

    public TruckPath(Dictionary<List<int>, List<float>> rDic, Dictionary<List<int>, List<float>> bDic)
    {
        //redPath = red;
        //bluePath = blue;
        //       truckPathList = Path;
        //       truckCapList = Cap;
        blueDic = bDic;
        redDic = rDic;
    }

    //public List<int> TruckPathList{
    //    get { return truckPathList; }
    //}

    //public List<int> TruckCapList
    //{
    //    get { return TruckCapList; }
    //}

    public Dictionary<List<int>, List<float>> RedDic
    {
        get { return redDic; }
    }

    public Dictionary<List<int>, List<float>> BlueDic
    {
        get { return blueDic; }
    }

    //public bool Red
    //{
    //    get { return redPath; }
    //}

    //public bool Blue
    //{
    //    get { return bluePath; }
    //}
}