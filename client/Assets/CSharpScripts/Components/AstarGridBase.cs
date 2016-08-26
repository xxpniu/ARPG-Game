using UnityEngine;
using System.Collections;
using Astar;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class AstarGridBase : MonoBehaviour {

    public const string GROUND = "Ground";
    private Collider ncollider;
	// Use this for initialization
	void Start () 
    {
        
	}
	
    public void GetPosByVector3(Vector3 v)
    {
        
    }

	// Update is called once per frame
	void Update () 
    {
        if (CalGrid)
        {
            CalGrid = false;
            CalNodes();
        }
	}

    public bool CalGrid = false;

    public void CalNodes()
    {

        if (ncollider == null)
        {
            Awake();
        }

        var bounds = ncollider.bounds;
 
        grid = new GridBase();
        grid.maxX = Mathf.FloorToInt((bounds.max.x - bounds.min.x) / gridSize.x);
        grid.maxY = 1;
        grid.maxZ = Mathf.FloorToInt((bounds.max.z - bounds.min.z) / gridSize.z);
        grid.offsetX = this.transform.position.x;
        grid.offsetY = this.transform.position.y;
        grid.offsetZ = this.transform.position.z;
        grid.sizeX = gridSize.x;
        grid.sizeY = gridSize.y;
        grid.sizeZ = gridSize.z;

        grid.grid = new Node[grid.maxX, grid.maxY, grid.maxZ];

        int pX = 0;
        int pZ = 0;
        for (var x = bounds.min.x; x < bounds.max.x; x += gridSize.x)
        {
            pZ = 0;
            for (var z = bounds.min.z; z < bounds.max.z; z += gridSize.z)
            {
                if (pZ >= grid.maxZ || pX >= grid.maxX)
                    continue;
                var n = new Node(pX, 0, pZ);
                grid.grid[pX, 0, pZ] = n;
                n.isWalkable = true;
                var hits = Physics.BoxCastAll(new Vector3(x, this.transform.position.y, z),gridSize, Vector3.down);
                if (hits.Length != 1)
                    n.isWalkable = false;
                else
                {
                    if (hits[0].collider.tag != GROUND)
                    {
                        n.isWalkable = false;
                    }
                }
                pZ++;
                //DrawRect(new Vector3(x, 0, z)+(gridSize/2), gridSize);
            }
            pX++;
        }

       
    }

    void Awake()
    {
        ncollider = this.GetComponent<Collider>();
    }
   
    public GridBase grid;

    public Vector3 gridSize = new Vector3(0.5f,0.1f,0.5f); 
  
    public void OnDrawGizmos()
    {
        if (grid == null)
        {
            CalNodes();
        }


        for (var x = 0; x < grid.maxX; x++)
        {
            for (var z = 0; z < grid.maxZ; z++)
            {
                var n = grid.grid[x, 0, z];
                if (!n.isWalkable)
                    continue;
                Gizmos.color = new Color(Color.white.r, Color.white.g, Color.white.b, 0.3f);
                if (n.showActived)
                {
                    Gizmos.color = Color.red;
                }
                var p = new Vector3(x * gridSize.x, 0, z * gridSize.z ) + gridSize;
                DrawRect(p, gridSize);
            }
        }
    }



    private void DrawRect(Vector3 center, Vector3 size)
    {
        
        var p1 = new Vector3(center.x - size.x / 2, center.y, center.z - size.z / 2);
        var p2 = new Vector3(center.x + size.x / 2, center.y, center.z - size.z / 2);
        var p3 = new Vector3(center.x + size.x / 2, center.y, center.z + size.z / 2);
        var p4 = new Vector3(center.x - size.x / 2, center.y, center.z + size.z / 2);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
        Gizmos.DrawLine(p1, p3);
    }
}
