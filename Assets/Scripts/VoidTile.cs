using UnityEngine;
using System.Collections;

public class VoidTile : MonoBehaviour
{
    [SerializeField]private float spawnX, spawnY;
    public Vector2 spawnPoint;
    void Start()
    {
    }

    void Update(){
        spawnPoint = new Vector2(spawnX, spawnY);
    }
}
