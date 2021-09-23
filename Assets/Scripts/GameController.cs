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
    private bool _isWin = false;
    private Bird _shotBird;

    public static bool IsPaused = false;
    

    void Start() {
        // pengaturan awal saat game baru dimulai
        _isGameEnded = false;
        IsPaused = false;        

        // menambahkan delegate event pada birds ketika bird ditembakkan dan dihancurkan
        for(int i = 0; i < Birds.Count; i++) {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrails;
        }
        // menambahkan delegate event pada enemies ketika enemy dihancurkan
        for (int i = 0; i < Enemies.Count; i++) {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        TapCollider.enabled = false;
        Slingshooter.InitiateBird(Birds[0]);
        _shotBird = Birds[0];        
    }

    private void Update()
    {
        // mengecek input setiap frame
        if (Input.GetKeyDown(KeyCode.P))
        {
            IsPaused = true;
        }

        // menampilkan pause/game over screen
        if (_isGameEnded || IsPaused)
        {
            // show pause screen
            string text = _isGameEnded ? (_isWin ? "You Win!": "You Lose") : "Paused";
            UIController.ChangeTitlePause(text);
            UIController.SetActiveContinueButton(_isGameEnded ? false : true);
            UIController.ShowPauseScreen();
            IsPaused = true;
        }
    }

    public void ChangeBird() {
        if (!TapCollider) return;
        //mematikan tap collider ketika sedang proses penggantian bird
        TapCollider.enabled = false; 

        if (_isGameEnded) return;

        Birds.RemoveAt(0);

        if (Birds.Count > 0) {
            Slingshooter.InitiateBird(Birds[0]);
            _shotBird = Birds[0];            
        } else {            
            if (Enemies.Count == 0) {
                _isWin = true;
            } else {
                _isWin = false;
            }

            StartCoroutine(GameOver(3));

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
            _isWin = true;
            StartCoroutine(GameOver(3));
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

    private IEnumerator GameOver(float second) {
        yield return new WaitForSeconds(second);
        _isGameEnded = true;
    }
}
