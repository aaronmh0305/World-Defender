using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {

    void OnDrawGizmos()
    {
       
        Gizmos.DrawWireCube(this.transform.position, new Vector3(.75f, .75f, 0f));
    }
}
