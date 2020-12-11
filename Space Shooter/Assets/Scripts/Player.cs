using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedmulti = 3;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tshotprefab;
    [SerializeField]
    private float _fireRate = 0.05f;
    private float _canFire;
    public float horizontalInput;
    public float VerticalInput;
    [SerializeField]
    private int _life = 3;
    private SpawnManager _spawnManager;
    private bool _tshot = false;
    private bool _speedactive = false;
    private bool _shieldactive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    private int _score;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is nulll");
        }

        if  (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(isPlayerOne == true)
        {
            CalculateMovement();

            if ((Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) && isPlayerOne == true)
            {
                FireLaser();
            }
        }

       if (isPlayerTwo == true)
        {
            CalculateMovementP2();
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Debug.Log("Pressed");
                FireLaser();
            }
        }

       
       
       
    }

    void CalculateMovement()
    {
        //Horizontal Input
        horizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, VerticalInput, 0);
       
        if (_speedactive == false) {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else if (_speedactive == true)
        {
            transform.Translate(direction * (_speed * _speedmulti) * Time.deltaTime);
        }

 
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.y >= 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y < -3.8f)
            transform.position = new Vector3(transform.position.x, -3.8f, 0);

        if (transform.position.x >= 11)
            transform.position = new Vector3(-11f, transform.position.y, 0);
        else if (transform.position.x <= -11)
            transform.position = new Vector3(11f, transform.position.y, 0);
    }

    void CalculateMovementP2()
    {
        //Horizontal Input
       
 
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad2))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }



        if (_speedactive == true)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * _speedmulti * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad2))
            {
                transform.Translate(Vector3.down * _speed * _speedmulti * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * _speedmulti * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * _speedmulti * Time.deltaTime);
            }
        } else if (_speedactive == false)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(Vector3.up * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad2))
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        }


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.y >= 0)
            transform.position = new Vector3(transform.position.x, 0, 0);
        else if (transform.position.y < -3.8f)
            transform.position = new Vector3(transform.position.x, -3.8f, 0);

        if (transform.position.x >= 11)
            transform.position = new Vector3(-11f, transform.position.y, 0);
        else if (transform.position.x <= -11)
            transform.position = new Vector3(11f, transform.position.y, 0);
    }



    public void Damage()
    {

        if (_shieldactive == true)
        {
            _shieldVisualizer.SetActive(false);
            _shieldactive = false; 
            return;
        }
        _life--;

        if (_life == 2)
        {
            _leftEngine.SetActive(true);
        } else if (_life == 1)
        {
            _rightEngine.SetActive(true);
        }


        _uiManager.UpdateLives(_life);
        if (_life < 1)
        {
            _uiManager.CheckForBestScore(_score);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ShieldsActive()
    {
        _shieldactive = true;
        _shieldVisualizer.SetActive(true);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_tshot == true)
        {
            Instantiate(_tshotprefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();

    }

  



    public void tshotactivate()
    {
        _tshot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tshot = false;
    }

    public void speedup()
    {
        _speedactive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedactive = false;
    }

    public void addScore(int scoreUp)
    {
        _score += scoreUp;
        _uiManager.UpdateScore(_score);
    }

   

}


