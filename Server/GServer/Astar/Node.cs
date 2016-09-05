namespace Astar
{
    public class Node
    {
        public Node(int x, int y, int z, bool iswalkable)
        {
            this.x = x;this.z = z;this.y =y;
            this.isWalkable = iswalkable;
        }

        public Node(int x, int y, int z) : this(x, y, z, true)
        {
            
        }

        public Node() : this(0, 0, 0) { }

        private int lockCount = 0;

        //Node's position in the grid
        public int x;
        public int y;
        public int z;

        //Node's costs for pathfinding purposes
        public float hCost;
        public float gCost;
        
        public float fCost
        {
            get //the fCost is the gCost+hCost so we can get it directly this way
            {
                return gCost + hCost;
            }
        }

        public Node parentNode;
        public bool isWalkable = true;
        public bool showActived = false;

        public bool IsWalkable { get { if (lockCount > 0) return false; return isWalkable; }}

        //Types of nodes we can have, we will use this later on a case by case examples
        public NodeType nodeType;
        public enum NodeType
        {
            ground,
            air
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Node)) return false;

            var n = obj as Node;
            return n.x == x && n.y == y && n.z == z;
        }

        public override int GetHashCode()
        {
            return string.Format("({0},{1},{2})", x, y, z).GetHashCode();
        }


        public void Lock()
        {
            lockCount++;
        }

        public void Unlock()
        {
            lockCount--;
        }
    }
}
