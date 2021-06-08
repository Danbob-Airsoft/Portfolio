using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColldierCombiner : MonoBehaviour
{
    public MeshCollider thiscollider;
    public MeshFilter CombinedMesh;

    private void Update()
    {
        while (CombinedMesh == null)
        {
            thiscollider = this.GetComponent<MeshCollider>();
            CombinedMesh = this.GetComponent<MeshFilter>();
            thiscollider.sharedMesh = CombinedMesh.sharedMesh;
        }
    }
}
