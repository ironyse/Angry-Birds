using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController UIController;
    public Slingshooter Slingshooter;
    public TrailController TrailController;
    public BoxCollider2D TapCollider;

    public List<Bird> Birds;
    public List<Enemy> Enemies;

    private bool _isGameEnded = false;    
    private Bird _shotBird;

    public static bool IsPaused = false;

    void Start() {
        _isGameEnded = false;
        IsPaused = false;        

        for(int i = 0; i < Birds.Count; i++) {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrails;
        }

        for (int i = 0; i < Enemies.Count; i++) {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        Slingshooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsPaused = true;
        }

        if (_isGameEnded || IsPaused)
        {
            // show pause screen
            string text = _isGameEnded ? "Game Over" : "Paused";
            UIController.ChangeTitlePause(text);
            UIController.SetActiveContinueButton(_isGameEnded ? false : true);
            UIController.ShowPauseScreen();
            IsPaused = true;
        }
    }

    public void ChangeBird() {
        if (!TapCollider) return;

        TapCollider.enabled = false;

        if (_isGameEnded) return;

        Birds.RemoveAt(0);

        if (Birds.Count > 0) {
            Slingshooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy) {
        for (int i = 0; i < Enemies.Count; i++) {
            if (Enemies[i].gameObject == destroyedEnemy) {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if (Enemies.Count == 0) {
            _isGameEnded = true;
        }
    }

    public void AssignTrails(Bird bird) {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    public void OnMouseUp() {
        if (_shotBird != null) {
            _shotBird.OnTap();
        }
    }
}
