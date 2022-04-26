using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    private Transform[] childObjects;
    public List<Transform> tiles = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        //Fill up the tiles list
        FillTiles();

        for(int i = 0; i < tiles.Count; i++)
        {
            Vector3 currentPos = tiles[i].position;

            if(i > 0 )
            {
                Vector3 prevPos = tiles[i - 1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }
    }

    void FillTiles()
    {
        tiles.Clear();
        childObjects = GetComponentsInChildren<Transform>();
        foreach(Transform child in childObjects)
        {
            if(child != transform)
            {
                tiles.Add(child);
            }
        }
    }
}
