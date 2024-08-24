using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugController : MonoBehaviour
{
    
    private Player _player;
    private Collider2D _collider;
    private Rigidbody2D _rb;

    [SerializeField] private float freeMovementSpeed = 15;
    [SerializeField] private float hiSpeedMultiplier = 3;

    [SerializeField] private GameObject cratePrefab;
    [SerializeField] private GameObject gravityPrefab;
    [SerializeField] private Color gravityColor;

    private bool debugMode = false;
    private bool paused = false;
    private bool freeMovement = false;
    private Aura spawnedGravity;
    private int currTeleportIndex = 0;

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
        #if DEBUG
        if (Input.GetKeyDown(KeyCode.Backslash)) {
            ToggleDebugMode();
        }
        #else
        if (debugMode && Input.GetKeyDown(KeyCode.Backslash)) {
            ToggleDebugMode();
        }
        #endif
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
            if (Input.GetKeyDown(KeyCode.I)) {
                ForceInteract();
            }
            if (Input.GetKeyDown(KeyCode.G)) {
                SpawnGravity();
            }
            if (spawnedGravity && Input.GetKeyUp(KeyCode.G)) {
                Debug.Log("Activating new grav");
                ActivateSpawnedGravity();
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                TeleportPrevious();
            }
            if (Input.GetKeyDown(KeyCode.RightBracket)) {
                TeleportNext();
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
        }
        if (spawnedGravity) {
            AimGravity();
        }
    }

    public void ToggleDebugMode() {
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

    void ForceInteract() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D col in colliders) {
            if (col.TryGetComponent<Interactible>(out Interactible d)) {
                d.Interact();
            }
        }
    }

    void SpawnGravity() {
        Vector2 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject spawn = Instantiate(gravityPrefab, spawnPos, Quaternion.identity);
        spawnedGravity = spawn.GetComponent<Aura>();
        spawnedGravity.SetColor(gravityColor);
        AimGravity();
        spawnedGravity.ActivateAura(true);
    }

    Vector2 GetGravityAim() {
        Vector2 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - spawnedGravity.transform.position;
        return aim;
    }

    void AimGravity() {
        Vector2 aim = GetGravityAim();
        float rad = aim.magnitude;
        aim = aim.normalized;
        float arrowDir = Vector2.Angle(aim, Vector2.down);
        if (aim.x > 0) arrowDir = 360 - arrowDir;
        spawnedGravity.GetComponent<SpriteRenderer>().material.SetFloat("_ArrowAngle", arrowDir * 3.14f / 180);
        spawnedGravity.SetRadius(rad);
    }

    void ActivateSpawnedGravity() {
        AimGravity();
        Vector2 newGravDirection = GetGravityAim().normalized;
        spawnedGravity.AlterGravity(newGravDirection);
        Destroy(spawnedGravity.gameObject);
        spawnedGravity = null;
    }

    void TeleportPrevious() {
        bool canUseIndex = DebugTeleportPoint.CloseToTeleportPoint(transform.position, currTeleportIndex);
        int prevIndex = canUseIndex ? DebugTeleportPoint.GetPreviousTeleportPoint(currTeleportIndex) : DebugTeleportPoint.GetPreviousTeleportPoint(transform.position);
        currTeleportIndex = prevIndex;
        Teleport(DebugTeleportPoint.GetTeleportPoint(currTeleportIndex));
    }

    void TeleportNext() {
        bool canUseIndex = DebugTeleportPoint.CloseToTeleportPoint(transform.position, currTeleportIndex);
        int nextIndex = canUseIndex ? DebugTeleportPoint.GetNextTeleportPoint(currTeleportIndex) : DebugTeleportPoint.GetNextTeleportPoint(transform.position);
        currTeleportIndex = nextIndex;
        Teleport(DebugTeleportPoint.GetTeleportPoint(currTeleportIndex));
    }

    void Teleport(Vector2 pos) {
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    private void OnGUI() {
        if (debugMode) {
            GUILayout.Label("Debug mode active");
            GUILayout.Label("P: Pause; Currently " + (paused ? "Paused" : "Unpaused"));
            GUILayout.Label("C: Spawn Crate");
            GUILayout.Label("O: Open Door/Barrier");
            GUILayout.Label("I: Force Interact");
            GUILayout.Label("G (" + (spawnedGravity ? "Drag Release" : "Hold") + "): Adjust Gravity");
            GUILayout.Label("M: Toggle Free Movement");
            if (freeMovement) {
                GUILayout.Label("WASD: Move (Free Movement)");
                GUILayout.Label("Shift: Move Faster (Free Movement)");
            }
            GUILayout.Label("[: Previous Teleport Point");
            GUILayout.Label("]: Next Teleport Point");
        }
    }
}
