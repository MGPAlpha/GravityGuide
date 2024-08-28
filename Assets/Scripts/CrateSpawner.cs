using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private Transform spawnPoint; 

    [SerializeField] private GameObject ownedCrate;
    [SerializeField] private GameObject preparedCrate;

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

    public void NotifyReadyToClose() {
        open = false;
        _anim.SetFloat("opening", open ? 1 : -1);
        _anim.SetFloat("closing", !open ? 1 : -1);
    }

    public void NotifyFullyClosed() {
        PrepareNextCrate();
    }

    public void Spawn() {
        open = true;
        _anim.SetFloat("opening", open ? 1 : -1);
        _anim.SetFloat("closing", !open ? 1 : -1);
        if (!preparedCrate) {
            PrepareNextCrate();
        }
        DestroyOwned();
        preparedCrate.GetComponent<GravityObject>().UpdateGravityDirection(transform.TransformDirection(Vector2.down));
        ownedCrate = preparedCrate;
        preparedCrate = null;
    }

    void PrepareNextCrate() {
        preparedCrate = Instantiate(cratePrefab, spawnPoint.position, spawnPoint.rotation);
        preparedCrate.GetComponent<GravityObject>().personalGravity = transform.TransformDirection(Vector2.down);
    }
}
