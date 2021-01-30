using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject gridCube;
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private int[] numbersOfPrefabsToSpawn;
    [SerializeField] private int gridX;
    [SerializeField] private int gridZ;
    [SerializeField] private float gridSpacingOffset = 1f;
    [SerializeField] private Vector3 gridOrigin = Vector3.zero;
    private 

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllOnGrid();
    }

    void SpawnAllOnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, 0, z * gridSpacingOffset) + gridOrigin;
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
