using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private Transform spawnPoint; 

    [SerializeField] private GameObject ownedCrate;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyOwned() {
        if (ownedCrate && ownedCrate != null) {
            ownedCrate.GetComponent<DissolveDestroy>().Dissolve();
            ownedCrate = null;
        }
    }

    public void Spawn() {
        DestroyOwned();
        ownedCrate = Instantiate(cratePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
