using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinding
{
    public class Node
    {
        public Tile Tile;

        public int X
        {
            get { return Tile.X; }
        }

        public int Y
        {
            get { return Tile.Y; }
        }

        public Node[] ConnectedNodes;

        // Overrides default Equals so we check on position instead of object memory location
        public override bool Equals(object obj)
        {
            return (obj is Node) && this.Equals((Node)obj);
        }

        public bool Equals(Node node)
        {
            return node.Tile.X == this.Tile.Y && node.Tile.Y == this.Tile.Y;
        }
    }

    public class NodeConnection
    {
        public Node Node;
        public float Cost;
    }
}
