using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterFlow.System
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public bool reloadLevel;
        public bool keepPlayerPrefs;
        public GameObject[] enemyPrefabs;
        public GameObject indicatorPrefab;
        public TextMeshProUGUI scoreTextObject;
        public TextMeshProUGUI highScoreTextObject;
        public TextMeshProUGUI highScoreTextObjectTime;
        public GameObject newHighScoreUIPrefab;
        public GameObject speedIncreaseUIPrefab;
        public GameObject newStartUIPrefab;
        public TextMeshProUGUI scoreTextObjectMinutes;
        public TextMeshProUGUI scoreTextObjectSeparation;
        public TextMeshProUGUI scoreTextObjectSeconds;
        public float waveTime;
        public float startWait;
        public float initialWaveWaitTime;
        public float spawnWait;
        public float indicatorBeforeSpawnTime;
        public float flowSpeedIncrease;
        private bool _continueSpawning;
        private int _currentHighestScore;
        private float _currentHighestScoreTime;
        private float _currentWaveWait;
        private Vector2 _halfScreenSizeInUnits;
        private int _lastSpawnPosIndex;
        private int _nextSpawnPosIndex;
        private float _playTime;
        private int _score;
        private int _secondLastSpawnPosIndex;
        private Sounds _sounds;
        private Vector2[] _spawnPositions;
        private bool _waitWave = true;

        private GameController()
        {
            GenerateSpawnPositions();
        }

        public float CurrentSpeedMultiplier { get; } = 1;

        private void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this;
        }

        private void Start()
        {
            if (!keepPlayerPrefs) PlayerPrefs.DeleteAll();

            // Set screen collider to correct size
            _halfScreenSizeInUnits.y = Camera.main.orthographicSize;
            _halfScreenSizeInUnits.x = Camera.main.aspect * _halfScreenSizeInUnits.y;
            Camera.main.GetComponent<BoxCollider2D>().size = _halfScreenSizeInUnits * 2f;

            // Reset score at start
            UpdateScore();

            // Load highScore from Playerprefs
            SetCurrentHighScoreText();

            // Start spawning Waves
            StartCoroutine(SpawnWaves());

            _currentWaveWait = initialWaveWaitTime;
            _sounds = FindObjectOfType<Sounds>();
            if (_sounds == null) Debug.Log("Cannot find Sounds script.");
        }

        private void Update()
        {
            UpdatePlayTime();
        }

        private void OnGUI()
        {
            var minutes = Mathf.FloorToInt(_playTime / 60F);
            var seconds = Mathf.FloorToInt(_playTime - minutes * 60);

            scoreTextObjectSeconds.text = $"{seconds:00}";

            if (minutes < 1)
            {
                scoreTextObjectMinutes.text = "";
                scoreTextObjectSeparation.text = "";
            }
            else
            {
                scoreTextObjectMinutes.text = $"{minutes:0}";
                scoreTextObjectSeparation.text = ":";
            }
        }

        private void UpdatePlayTime()
        {
            _playTime += Time.deltaTime;
        }

        private void GenerateSpawnPositions()
        {
            // 24 spawn positions in circular distance around origin
            _spawnPositions = new[]
            {
                new Vector2(0.6f, 4.8f), new Vector2(1.9f, 4.4f), new Vector2(2.9f, 3.8f),
                new Vector2(3.85f, 2.9f), new Vector2(4.5f, 1.8f), new Vector2(4.8f, 0.6f),
                new Vector2(4.8f, -0.6f), new Vector2(4.5f, -1.8f), new Vector2(3.85f, -2.9f),
                new Vector2(2.9f, -3.8f), new Vector2(1.9f, -4.4f), new Vector2(0.6f, -4.8f),
                new Vector2(-0.6f, -4.8f), new Vector2(-1.9f, -4.4f), new Vector2(-2.9f, -3.8f),
                new Vector2(-3.85f, -2.9f), new Vector2(-4.5f, -1.8f), new Vector2(-4.8f, -0.6f),
                new Vector2(-4.8f, 0.6f), new Vector2(-4.5f, 1.8f), new Vector2(-3.85f, 2.9f),
                new Vector2(-2.9f, 3.8f), new Vector2(-1.9f, 4.4f), new Vector2(-0.6f, 4.8f)
            };
        }

        private IEnumerator SpawnWaves()
        {
            _continueSpawning = true;
            yield return new WaitForSeconds(startWait);

            while (_continueSpawning)
            {
                StartCoroutine(WaitForWave(waveTime));

                while (_waitWave)
                {
                    var tomato = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    ChooseRandomSpawnPosition();
                    SpawnIndicator();
                    yield return new WaitForSeconds(indicatorBeforeSpawnTime);
                    SpawnEnemy(tomato);
                    yield return new WaitForSeconds(spawnWait);
                }

                yield return new WaitForSeconds(_currentWaveWait / 2);
                _sounds.PlaySound(2);
                SpawnUIPrefab(speedIncreaseUIPrefab);
                yield return new WaitForSeconds(_currentWaveWait / 2);
                Time.timeScale += flowSpeedIncrease;
                _waitWave = true;
            }
        }

        private void SpawnUIPrefab(GameObject prefab)
        {
            if (prefab != null)
                Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            else
                print("Could not find Prefab");
        }

        private void ChooseRandomSpawnPosition()
        {
            while (_nextSpawnPosIndex == _lastSpawnPosIndex
                   || _nextSpawnPosIndex == _secondLastSpawnPosIndex
                   || _nextSpawnPosIndex - 1 == _lastSpawnPosIndex
                   || _nextSpawnPosIndex - 1 == _secondLastSpawnPosIndex
                   || _nextSpawnPosIndex + 1 == _lastSpawnPosIndex
                   || _nextSpawnPosIndex + 1 == _secondLastSpawnPosIndex
                   || _nextSpawnPosIndex - 2 == _lastSpawnPosIndex
                   || _nextSpawnPosIndex - 2 == _secondLastSpawnPosIndex
                   || _nextSpawnPosIndex + 2 == _lastSpawnPosIndex
                   || _nextSpawnPosIndex + 2 == _secondLastSpawnPosIndex
                   || _nextSpawnPosIndex == 0 && _lastSpawnPosIndex == 23
                   || _nextSpawnPosIndex == 0 && _secondLastSpawnPosIndex == 23
                   || _nextSpawnPosIndex == 0 && _lastSpawnPosIndex == 22
                   || _nextSpawnPosIndex == 0 && _secondLastSpawnPosIndex == 22
                   || _nextSpawnPosIndex == 1 && _lastSpawnPosIndex == 23
                   || _nextSpawnPosIndex == 1 && _secondLastSpawnPosIndex == 23
                   || _lastSpawnPosIndex == 0 && _nextSpawnPosIndex == 23
                   || _lastSpawnPosIndex == 0 && _secondLastSpawnPosIndex == 23
                   || _lastSpawnPosIndex == 0 && _nextSpawnPosIndex == 22
                   || _lastSpawnPosIndex == 0 && _secondLastSpawnPosIndex == 22
                   || _lastSpawnPosIndex == 1 && _nextSpawnPosIndex == 23
                   || _lastSpawnPosIndex == 1 && _secondLastSpawnPosIndex == 23
                   || _secondLastSpawnPosIndex == 0 && _nextSpawnPosIndex == 23
                   || _secondLastSpawnPosIndex == 0 && _lastSpawnPosIndex == 23
                   || _secondLastSpawnPosIndex == 0 && _nextSpawnPosIndex == 22
                   || _secondLastSpawnPosIndex == 0 && _lastSpawnPosIndex == 22
                   || _secondLastSpawnPosIndex == 1 && _nextSpawnPosIndex == 23
                   || _secondLastSpawnPosIndex == 1 && _lastSpawnPosIndex == 23)
                _nextSpawnPosIndex = Random.Range(0, 24);
        }

        private void SpawnIndicator()
        {
            var newIndicator = Instantiate(indicatorPrefab,
                _spawnPositions[_nextSpawnPosIndex], Quaternion.identity);
            newIndicator.transform.up = (newIndicator.transform.position - Vector3.zero).normalized;
            if (Physics2D.Raycast(newIndicator.transform.position, -newIndicator.transform.up, 10)
                is RaycastHit2D hit) newIndicator.transform.position = hit.point;
        }

        private void SpawnEnemy(GameObject tomato)
        {
            Instantiate(tomato, _spawnPositions[_nextSpawnPosIndex], Quaternion.identity)
                .transform.GetChild(0).Rotate(0, 0, Random.Range(0.0f, 1.0f));
            _secondLastSpawnPosIndex = _lastSpawnPosIndex;
            _lastSpawnPosIndex = _nextSpawnPosIndex;
        }

        private IEnumerator WaitForWave(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _waitWave = false;
        }

        public void UpdateScore(int x = 0)
        {
            _score += x == 0 ? -_score : x;
            scoreTextObject.text = _score.ToString();
        }

        public void GameOver()
        {
            _continueSpawning = false;

            Time.timeScale = 1.0f;

            if (_score > _currentHighestScore)
                PlayerPrefs.SetInt("highScore", _score);

            if (_playTime > 10f && _playTime > _currentHighestScoreTime)
            {
                PlayerPrefs.SetFloat("highScoreTime", _playTime);
                _sounds.PlaySound(1);
                TriggerNewHighScoreCircle();
            }
            else
            {
                _sounds.PlaySound(0);
                SpawnUIPrefab(newStartUIPrefab);
            }

            SetCurrentHighScoreText();
            _playTime = 0f;
            if (reloadLevel) ReloadLevel();
        }

        private void SetCurrentHighScoreText()
        {
            _currentHighestScore = PlayerPrefs.GetInt("highScore", 0);
            highScoreTextObject.text = _currentHighestScore.ToString();
            _currentHighestScoreTime = PlayerPrefs.GetFloat("highScoreTime", 0);
            var highScoreMinutes = Mathf.FloorToInt(_currentHighestScoreTime / 60F);
            var highScoreSeconds = Mathf.FloorToInt(_currentHighestScoreTime - highScoreMinutes * 60);

            if (highScoreMinutes < 1)
                highScoreTextObjectTime.text = $"{highScoreSeconds:00}";
            else
                highScoreTextObjectTime.text = $"{highScoreMinutes:0}:{highScoreSeconds:00}";
        }

        private void TriggerNewHighScoreCircle()
        {
            var highScoreMinutes = Mathf.FloorToInt(_playTime / 60F);
            var highScoreSeconds = Mathf.FloorToInt(_playTime - highScoreMinutes * 60);

            if (highScoreMinutes < 1)
            {
                if (newHighScoreUIPrefab != null)
                {
                    newHighScoreUIPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                        $"{highScoreSeconds:00}";
                    newHighScoreUIPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                        $"{highScoreSeconds:00}";
                }
            }
            else
            {
                if (newHighScoreUIPrefab != null)
                {
                    newHighScoreUIPrefab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                        $"{highScoreMinutes:0}:{highScoreSeconds:00}";
                    newHighScoreUIPrefab.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
                        $"{highScoreMinutes:0}:{highScoreSeconds:00}";
                }
            }

            SpawnUIPrefab(newHighScoreUIPrefab);
        }

        private static void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}