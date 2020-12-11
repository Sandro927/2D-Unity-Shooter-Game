using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
 



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
            Destroy(this.gameObject);
                
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {


            Player _player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (_player != null)
            {
                if (powerupID == 0)
                {
                    _player.tshotactivate();
                }
                else if (powerupID == 1)
                {
                    _player.speedup();
                }
                else if (powerupID == 2)
                {
                    _player.ShieldsActive();
                }
            }


            
            Destroy(this.gameObject);
        }



    }
  
}

