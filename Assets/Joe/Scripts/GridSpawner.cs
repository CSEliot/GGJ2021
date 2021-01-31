using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private int[] numbersOfPrefabsToSpawn;
    [SerializeField] private int gridX = 10;
    [SerializeField] private int gridZ = 10;
    [SerializeField] private float gridSpacingOffset = 1f;
    
    private Vector3 gridOrigin = Vector3.zero;
    private bool[,] taken;

    // Start is called before the first frame update
    void Start()
    {
        gridOrigin = gameObject.transform.position;

        taken = new bool[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                taken[x, z] = false;
            }
        }

        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            for (int j = 0; j < numbersOfPrefabsToSpawn[i]; j++)
            {
                int randX = Random.Range(0, gridX);
                int randZ = Random.Range(0, gridZ);

                while (taken[randX, randZ])
                {
                    randX = Random.Range(0, gridX);
                    randZ = Random.Range(0, gridZ);
                }

                Vector3 spawnPosition = new Vector3(randX * gridSpacingOffset, 0, randZ * gridSpacingOffset) + gridOrigin;
                GameObject spawnedObject = Instantiate(prefabsToSpawn[i], spawnPosition, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
