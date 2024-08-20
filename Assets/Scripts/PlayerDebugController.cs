using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerDebugController : MonoBehaviour
{
    
    private Player _player;
    private Collider2D _collider;
    private Rigidbody2D _rb;

    [SerializeField] private float freeMovementSpeed = 15;
    [SerializeField] private float hiSpeedMultiplier = 3;

    [SerializeField] private GameObject cratePrefab;

    private bool debugMode = false;
    private bool paused = false;
    private bool freeMovement = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash)) {
            ToggleDebugMode();
        }
        if (debugMode) {
            if (Input.GetKeyDown(KeyCode.P)) {
                TogglePause();
            }
            if (Input.GetKeyDown(KeyCode.M)) {
                ToggleFreeMovement();
            }
            if (Input.GetKeyDown(KeyCode.C)) {
                SpawnCrate();
            }
            if (Input.GetKeyDown(KeyCode.O)) {
                OpenBarrier();
            }
        }
        if (freeMovement) {
            Vector3 movementDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                movementDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.A)) {
                movementDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S)) {
                movementDirection += Vector3.down;
            }
            if (Input.GetKey(KeyCode.D)) {
                movementDirection += Vector3.right;
            }
            bool hiSpeed = Input.GetKey(KeyCode.LeftShift);
            float speedMul = hiSpeed ? hiSpeedMultiplier : 1;
            transform.position += movementDirection * freeMovementSpeed * speedMul * Time.unscaledDeltaTime;
            Debug.Log("name " + transform.gameObject.name);
            Debug.Log("dir " + movementDirection);
        }
    }

    void ToggleDebugMode() {
        debugMode = !debugMode;
        if (!debugMode) {
            if (paused) TogglePause();
            if (freeMovement) ToggleFreeMovement();
        }
    }
    void TogglePause() {
        paused = !paused;
        if (paused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
    void ToggleFreeMovement() {
        freeMovement = !freeMovement;
        if (freeMovement) {
            _rb.velocity = Vector2.zero;
            _collider.enabled = false;
            _player.enabled = false;
        } else {
            _collider.enabled = true;
            _player.enabled = true;
        }
    }
    void SpawnCrate() {
        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(cratePrefab, spawnPos, Quaternion.identity);
    }

    void OpenBarrier() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D col in colliders) {
            if (col.TryGetComponent<Door>(out Door d)) {
                d.Open();
            }
            if (col.TryGetComponent<LightBarrier>(out LightBarrier b)) {
                b.TurnOff();
            }
        }
    }
}
