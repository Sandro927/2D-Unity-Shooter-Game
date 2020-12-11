using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    private bool _isEnemyLaser = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        } else
        {
            MoveDown();
        }

    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            _speed = 8;
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);



            Destroy(this.gameObject);


        }
    }

    void MoveDown()
    {
        _speed = 6;
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
       
        if (transform.position.y < -8f)
        {
            _speed = 5;
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if  (player != null)
            {
                player.Damage();
            }
            
            
        }
        
    }

}
