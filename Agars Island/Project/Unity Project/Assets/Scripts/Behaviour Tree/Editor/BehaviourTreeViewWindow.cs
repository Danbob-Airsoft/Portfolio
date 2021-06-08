using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviourTree;

/*
 * This script is to visualise a BT_Tree at RUN-TIME
 * Nothing happens at editor-time as the tree is not
 * created until Start()
 *
 * To use this tool, put this script in Assets/Editor folder
 * Go to the top of the window and click Window -> Behaviour Tree
 * This will open the BT_Tree viewing window.
 * At RUN-TIME, select an object which has a BT_Tree attached and
 * it will show the current state of the whole tree and each
 * individual node inside the tree.
 *
 * SUCCESS => Green
 * RUNNING => Yellow
 * FAILED => Red
 * UNDEFINED => Gray
 * 
 */
 
/// <summary>
///
/// See above
/// 
/// Created by: Lewis Arnold
/// Edited by:
/// </summary>
public class BehaviourTreeViewWindow : EditorWindow
{
    GameObject obj;
    GameObject prevFrameObj;
    BT_Tree objTree = new BT_Tree();

    Rect rootRect = new Rect(600, 0, 0, 0);

    Vector2 currMousePos;
    Vector2 prevMousePos;
    Vector2 deltaPos;
    Rect globalOffset = new Rect(0, 0, 0, 0);

    float maxOffset = 1600;

    public Rect testWindow = new Rect(10, 10, 10, 10);

    class WindowNode
    {
        public Rect windowRect;
        public string windowText;
        public Color color;
    }

    struct Line
    {
        public Vector3 from;
        public Vector3 to;
        public Color color;
    }

    List<WindowNode> windowRects = new List<WindowNode>();
    List<WindowNode> windowRectsCopy = new List<WindowNode>();
    List<Line> windowLinks = new List<Line>();


    [MenuItem("Window/Behaviour Tree")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(BehaviourTreeViewWindow));
    }

    private void OnGUI()
    {
        wantsMouseMove = true;

        // scroll the boxes
        Event e = Event.current;
        if(e.type == EventType.MouseDrag)
        {
            globalOffset.x += e.delta.x;
            globalOffset.y += e.delta.y;
        }


        // set default color to be safe
        GUI.color = Color.white;

        if (Selection.gameObjects.Length > 0)
            obj = Selection.gameObjects[0];
        else
            obj = null;

        if(obj)
        {
            if (obj.GetComponent<localTree>())
            {
                objTree = obj.GetComponent<localTree>().tree;
            }
        }

        // generate tree data
        if(objTree.getRoot() != null)
        {
            BT_Node currNode = objTree.getRoot();

            windowRects.Clear();

            // recursive!
            ExploreNode(currNode, 0, currNode.childNodes.Count, 0.5f, ref rootRect);

            windowRectsCopy = windowRects;
        }

        // draw lines and windows

        Handles.BeginGUI();
        foreach (Line line in windowLinks)
        {
            Handles.color = line.color;

            Vector3 fromTemp = line.from;
            Vector3 toTemp = line.to;

            fromTemp.x += globalOffset.x;
            fromTemp.y += globalOffset.y;
            toTemp.x += globalOffset.x;
            toTemp.y += globalOffset.y;

            Handles.DrawLine(fromTemp, toTemp);
        }
        Handles.EndGUI();


        BeginWindows();
  
        for(int i=0; i< windowRectsCopy.Count; i++)
        {
            GUI.color = windowRects[i].color;

            Rect tempRect = windowRects[i].windowRect;
            tempRect.x += globalOffset.x;
            tempRect.y += globalOffset.y;

            windowRects[i].windowRect = GUI.Window(i, tempRect, WindowFunction, windowRects[i].windowText);
        }

        testWindow = GUI.Window(100, testWindow, WindowFunction, "WOW");

        EndWindows();

        Repaint();

    }
    void WindowFunction(int windowID)
    {
        GUI.DragWindow();
    }

    void ExploreNode(BT_Node node, int depth, float siblings, float childPercent, ref Rect parent)
    {

        // create rect in correct place
        float offset = Map(depth, 5.0f, 0.0f, 0.0f, maxOffset);
        offset = ((offset * childPercent) - offset/2);
        offset /= (((float)depth + 1));
        //offset /= siblings;
        Rect rect = new Rect(parent.x + offset, parent.y + 100, 80, 50);

        // add rect to list
        WindowNode windowNode = new WindowNode();
        windowNode.windowRect = rect;

        // set window text
        //windowNode.windowText += node.nodeType.ToString();
        windowNode.windowText += node.GetType().ToString();
        windowRects.Add(windowNode);

        // set window color based on type
        if (node.nodeType == NodeType.NODE_BEHAVIOUR)
            windowNode.color = Color.blue;
        else if (node.nodeType == NodeType.NODE_SELECTOR)
            windowNode.color = Color.magenta;
        else if (node.nodeType == NodeType.NODE_SEQUENCER)
            windowNode.color = Color.cyan;
        else if (node.nodeType == NodeType.NODE_UNDEFINED)
            windowNode.color = Color.black;

        // create line to parent

        // base case
        if (node.childNodes.Count > 0)
        {
            int i = 0;
            foreach (BT_Node child in node.childNodes)
            {
                ExploreNode(child, depth+1, node.childNodes.Count, (float)i/(node.childNodes.Count-1), ref rect);
                i++;
            }
        }

        Line line = new Line();
        line.from = rect.center;
        line.to = parent.center;
        line.color = Color.red;

        // override window color if success
        if (node.nodeState == NodeState.NODE_SUCCESS)
        {
            windowNode.color = Color.green;
            line.color = Color.green;
        }
        else if (node.nodeState == NodeState.NODE_RUNNING)
        {
            windowNode.color = Color.yellow;
            line.color = Color.yellow;
        }
        else if (node.nodeState == NodeState.NODE_FAILURE)
        {
            windowNode.color = Color.red;
            line.color = Color.red;
        }
        else if (node.nodeState == NodeState.NODE_UNDEFINED)
        {
            windowNode.color = Color.grey;
            line.color = Color.grey;
        }


        //parent.x += offset / 3;


        windowLinks.Add(line);
    }

    float Map(float start, float a1, float a2, float b1, float b2)
    {
        return b1 + (((start - a1) * (b2 - b1)) / (a2 - a1));
    }
}