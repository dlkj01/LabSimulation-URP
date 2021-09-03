using DLKJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshCombine : MonoBehaviour
{

 

    // Start is called before the first frame update
    //void Start()
    //{
    //    Combine(transform.gameObject);
    //}
    //private void Combine(GameObject shape)
    //{
    //    // 1. destroy existing meshcollider

    //    // 2. mesh and child meshes into array
    //    MeshFilter[] meshfilters = this.GetComponentsInChildren<MeshFilter>();
    //    CombineInstance[] combine = new CombineInstance[meshfilters.Length];
    //    int i = 0;
    //    while (i < meshfilters.Length)
    //    {
    //        combine[i].mesh = meshfilters[i].sharedMesh;
    //        combine[i].transform = meshfilters[i].transform.localToWorldMatrix;
    //        meshfilters[i].gameObject.SetActive(false);
    //        i++;
    //    }

    //    // 3. combine meshes in array, into one mesh
    //    MeshFilter meshfilter = this.gameObject.GetComponent<MeshFilter>();
    //    meshfilter.mesh = new Mesh();
    //    meshfilter.mesh.CombineMeshes(combine, true);
    //    meshfilter.mesh.RecalculateBounds();
    //    meshfilter.mesh.RecalculateNormals();
    //    meshfilter.mesh.Optimize();
    //    // 4. remake the meshcollider
    //    this.gameObject.GetComponent<MeshCollider>();
    //    this.transform.gameObject.SetActive(true);

    //}

}
