using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDestroy : MonoBehaviour
{

    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private float maxSpeedDuringDissolve = 2;

    [SerializeField] private AudioClip dissolveEffect;

    [SerializeField] private DissolveSettings dissolveSettings;

    private bool dissolving = false;
    private float dissolveProgress = 0;

    private Rigidbody2D _rb;
    private Renderer _re;
    private AudioSource _as;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _re = GetComponent<Renderer>();
        TryGetComponent<AudioSource>(out _as);

        if (dissolveSettings.dissolveOnStart) Dissolve();
    }

    // Update is called once per frame
    void Update()
    {
        if (dissolving) {
            dissolveProgress += Time.deltaTime;
            if (_rb && _rb.velocity.magnitude > maxSpeedDuringDissolve) {
                _rb.velocity = _rb.velocity.normalized * maxSpeedDuringDissolve;
            }
            if (_re) _re.material.SetFloat("_Dissolve", dissolveProgress / dissolveTime);
            if (dissolveSettings.destroyOnEnd && dissolveProgress >= dissolveTime) {
                Destroy(gameObject);
            }
        }
    }

    public void Dissolve() {
        dissolving = true;
        if (dissolveSettings.changeLayer) gameObject.layer = LayerMask.NameToLayer("Dissolving");
        if (_as && dissolveEffect) {
            _as.PlayOneShot(dissolveEffect);
        }
        if (_re) _re.material.SetVector("_NoiseOffset", new Vector4(Random.Range(0,100),Random.Range(0,100),0,0));
    }
}
