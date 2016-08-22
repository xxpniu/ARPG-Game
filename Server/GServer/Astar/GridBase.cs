using System;
using System.Collections.Generic;

namespace Astar
{
    public struct Point3
    {
        public float x;
        public float y;
        public float z;
    }

    public class GridBase
    {
        public int maxX = 10;
        public int maxY = 3;
        public int maxZ = 10;

        public Node[,,] grid; // our grid

        public Node GetNode(int x, int y, int z)
        {
            //Used to get a node from a grid,
            //If it's greater than all the maximum values we have
            //then it's going to return null

            Node retVal = null;

            if (x < maxX && x >= 0 &&
                y >= 0 && y < maxY &&
                z >= 0 && z < maxZ)
            {
                retVal = grid[x, y, z];
            }

            return retVal;
        }

        public float sizeX;
        public float sizeY;
        public float sizeZ;

        public float offsetX;
        public float offsetY;
        public float offsetZ;

        public Node GetNodeFromVector3(int x, int y, int z)
        {
            Node retVal = GetNode(x, y, z);
            return retVal;
        }


    }
}
