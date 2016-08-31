using UnityEngine;
using System;

namespace Pathfinding
{
    public class BreadCrumb : IComparable<BreadCrumb>
    {
        public Node Node;

        public BreadCrumb prev;
        public BreadCrumb next;
        public int cost = Int32.MaxValue;
        public bool onClosedList = false;
        public bool onOpenList = false;

        public BreadCrumb(Node node)
        {
            this.Node = node;
        }

        public BreadCrumb(Node node, BreadCrumb parent) : this(node)
        {
            this.next = parent;
            parent.prev = this;
        }

        //Overrides default Equals so we check on position instead of object memory location
        public override bool Equals(object obj)
        {
            return (obj is BreadCrumb) && this.Equals((BreadCrumb)obj);
        }

        //Faster Equals for if we know something is a BreadCrumb
        public bool Equals(BreadCrumb breadcrumb)
        {
            return Node.Equals(breadcrumb.Node);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }

        #region IComparable<> interface
        public int CompareTo(BreadCrumb other)
        {
            return cost.CompareTo(other.cost);
        }
        #endregion
    }
}
