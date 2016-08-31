using System;
using UnityEngine;
using Pathfinding;

public static class Pathfinder
{
    public delegate float PathfindingHeuristic(int x1, int y1, int x2, int y2);

    public static PathfindingHeuristic Heuristic;

    public float ManhattenDistance(int x1, int y1, int x2, int y2)

    private static MinHeap<BreadCrumb> openList;

    /// <summary>
    /// Method that switfly finds the best path from start to end.
    /// </summary>
    /// <returns>The starting breadcrumb traversable via .next to the end or null if there is no path</returns>    
    public static BreadCrumb FindPath(TileGraph graph, Node start, Node end)
    {
        //note we just flip start and end here so you don't have to.            
        return FindPathReversed(graph, end, start);
    }

    /// <summary>
    /// Method that switfly finds the best path from start to end. Doesn't reverse outcome
    /// </summary>
    /// <returns>The end breadcrump where each .next is a step back)</returns>
    private static BreadCrumb FindPathReversed(TileGraph graph, Node start, Node end)
    {
        if (openList != null)
            openList.Clear();
        else
            openList = new MinHeap<BreadCrumb>();

        BreadCrumb[,] brWorld = new BreadCrumb[(int)graph.Size.x, (int)graph.Size.y];
        BreadCrumb crumb;
        Node node;
        float cost;
        int diff;

        BreadCrumb current = new BreadCrumb(start);
        current.cost = 0;

        brWorld[start.X, start.Y] = current;
        openList.Insert(current);

        BreadCrumb finish = new BreadCrumb(end);

        while (openList.Count > 0)
        {
            // Find best item and switch it to the 'closedList'
            current = openList.RemoveRoot();
            current.onClosedList = true;

            for (int i = 0; i < current.Node.ConnectedNodes.Length; i++)
            {
                node = current.Node.ConnectedNodes[i];
                // Change this to use tile graph
                //Check if we've already examined a neighbour, if not create a new node for it.
                if (brWorld[node.X, node.Y] == null)
                {
                    crumb = new BreadCrumb(node);
                    brWorld[node.X, node.Y] = crumb;
                }
                else
                {
                    crumb = brWorld[node.X, node.Y];
                }

                //If the node is not on the 'closedList' check it's new score, keep the best
                if (!crumb.onClosedList)
                {
                    diff = 0;
                    if (current.Node.X != crumb.Node.X)
                    {
                        diff += 1;
                    }
                    if (current.Node.Y != crumb.Node.X)
                    {
                        diff += 1;
                    }

                    // FIXME: Use Mathf
                    float heuristic = DistanceHeuristic(crumb.Node.X, crumb.Node.Y, end.X, end.Y);
                    
                    cost = current.cost + diff + heuristic;

                    if (cost < crumb.cost)
                    {
                        crumb.cost = cost;
                        crumb.next = current;
                    }

                    //If the node wasn't on the openList yet, Insert it 
                    if (!crumb.onOpenList)
                    {
                        //Check to see if we're done
                        if (crumb.Equals(finish))
                        {
                            crumb.next = current;
                            return crumb;
                        }
                        crumb.onOpenList = true;
                        openList.Insert(crumb);
                    }
                }

            }
        }
        return null; //no path found
    }
}
