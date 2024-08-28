using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private Transform spawnPoint; 

    [SerializeField] private GameObject ownedCrate;

    private Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool open = false;

    public void DestroyOwned() {
        if (ownedCrate && ownedCrate != null) {
            ownedCrate.GetComponent<DissolveDestroy>().Dissolve();
            ownedCrate = null;
        }
    }

    public void Spawn() {
        open = !open;
        _anim.SetFloat("opening", open ? 1 : -1);
        _anim.SetFloat("closing", !open ? 1 : -1);
        DestroyOwned();
        ownedCrate = Instantiate(cratePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
