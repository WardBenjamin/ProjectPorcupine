#region License
// ====================================================
// Project Porcupine Copyright(C) 2016 Team Porcupine
// This program comes with ABSOLUTELY NO WARRANTY; This is free software, 
// and you are welcome to redistribute it under certain conditions; See 
// file LICENSE, which is part of this source code package, for details.
// ====================================================
#endregion
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class TileGraph
    {
        // This class constructs a simple path-finding compatible graph
        // of our world.  Each tile is a node. Each WALKABLE neighbour
        // from a tile is linked via an edge connection.
        public Dictionary<Tile, Node> nodes;

        /// <summary>
        /// Size of the graph, same as the world size measured in tiles.
        /// </summary>
        public Vector2 Size;

        public TileGraph(World world)
        {
            Debug.ULogChannel("TileGraph", "Entered TileGraph");

            /*
             * Loop through all tiles of the world
             * For each tile, create a node
             *  Do we create nodes for non-floor tiles?  NO!
             *  Do we create nodes for tiles that are completely unwalkable (i.e. walls)?  NO!
             */
            nodes = new Dictionary<Tile, Node>();
            Size = new Vector2(world.Width, world.Height);

            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    Tile t = world.GetTileAt(x, y);

                    ////if(t.movementCost > 0) {    // Tiles with a move cost of 0 are unwalkable
                    Node n = new Node();
                    n.Tile = t;
                    nodes.Add(t, n);
                    ////}
                }
            }

            Debug.ULogChannel("Path_TileGraph", "Created " + nodes.Count + " nodes.");

            // Now loop through all nodes again
            // And register tile connections
            foreach (Tile t in nodes.Keys)
            {
                GenerateTileConnections(t);
            }
        }

        public void RegenerateGraphAtTile(Tile changedTile)
        {
            GenerateTileConnections(changedTile);
            foreach (Tile tile in changedTile.GetNeighbors(true))
            {
                GenerateTileConnections(tile);
            }
        }

        private bool IsClippingCorner(Tile curr, Tile neigh)
        {
            // If the movement from curr to neigh is diagonal (e.g. N-E)
            // Then check to make sure we aren't clipping (e.g. N and E are both walkable)
            int dX = curr.X - neigh.X;
            int dY = curr.Y - neigh.Y;

            if (Mathf.Abs(dX) + Mathf.Abs(dY) == 2)
            {
                // We are diagonal
                if (World.Current.GetTileAt(curr.X - dX, curr.Y).MovementCost == 0)
                {
                    // East or West is unwalkable, therefore this would be a clipped movement.
                    return true;
                }

                if (World.Current.GetTileAt(curr.X, curr.Y - dY).MovementCost == 0)
                {
                    // North or South is unwalkable, therefore this would be a clipped movement.
                    return true;
                }

                // If we reach here, we are diagonal, but not clipping
            }

            // If we are here, we are either not clipping, or not diagonal
            return false;
        }

        private void GenerateTileConnections(Tile t)
        {
            Node n = nodes[t];
            List<Node> connectedNodes = new List<Node>();

            // Get a list of neighbours for the tile
            Tile[] neighbors = t.GetNeighbors(true);

            // NOTE: Some of the array spots could be null.
            // If neighbour is walkable, create an connection to the relevant node.
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null && neighbors[i].MovementCost > 0 && IsClippingCorner(t, neighbors[i]) == false)
                {
                    // This neighbor exists, is walkable, and doesn't requiring clipping a corner,
                    // so create a connection and add it to the list.
                    connectedNodes.Add(nodes[neighbors[i]]);
                }
            }

            n.ConnectedNodes = connectedNodes.ToArray();
        }
    }
}
