using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FirstSpawnPoint : MonoBehaviour
{
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Handles.Label(transform.position + Vector3.up, "Spawn point");
        
    }
}
