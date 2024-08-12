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
        }
        if (freeMovement) {
            Vector2 movementDirection = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) {
                movementDirection += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A)) {
                movementDirection += Vector2.left;
            }
            if (Input.GetKey(KeyCode.S)) {
                movementDirection += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D)) {
                movementDirection += Vector2.right;
            }
            bool hiSpeed = Input.GetKey(KeyCode.LeftShift);
            float speedMul = hiSpeed ? hiSpeedMultiplier : 1;
            _rb.MovePosition(_rb.position + movementDirection * freeMovementSpeed * speedMul * Time.unscaledDeltaTime);
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
}
