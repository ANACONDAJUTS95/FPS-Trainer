using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 1;

    public bool damageEnemy, damagePlayer;

    public AudioClip bulletSound; // Add an AudioClip field for the collect sound

    private int maxHealth, currentHealth;

    // For GOAP:
    [SerializeField] 

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {

        //theRB.velocity = transform.forward * moveSpeed; // Set the bullet's velocity
        theRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Instead, apply force in the forward direction.
        theRB.AddForce(transform.forward * moveSpeed, ForceMode.VelocityChange);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // GOAP OnTriggers:
        if (other.gameObject.tag == "Enemy_GOAP_HS")
        {
            other.transform.parent.GetComponent<Bot>().health = 0;
            Debug.Log("HEADSHOT hit");
        }

        if (other.gameObject.tag == "Enemy_GOAP_BS")
        {
            other.transform.parent.GetComponent<Bot>().health -= damage; // damage to the bodyshot, check inspector(bullet prefab)
            Debug.Log("BODYSHOT hit");
        }


        // Generic AI OnTriggers:
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }

        if (other.gameObject.tag == "Headshot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 3);
            Debug.Log("Headshot hit");
        }

        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            //Debug.Log("Hit Player at " + transform.position);
            PlayerHealthController.instance.DamagePlayer(damage);
        }


        //Destroy(gameObject);
        //Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);

        // Play the collect sound when colliding with any object
        PlayBulletSound();

        DestroyBullet();
    }

    private void PlayBulletSound()
    {
        if (bulletSound != null)
        {
            // Create an audio source to play the collect sound
            AudioSource.PlayClipAtPoint(bulletSound, transform.position);
        }
    }

    private void DestroyBullet()
    {
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
        Destroy(gameObject);
    }

    //making sure the bullet will hit
    void FixedUpdate()
    {
        StartCoroutine(Predict());
    }

    IEnumerator Predict()
    {
        Vector3 prediction = transform.position + theRB.velocity * Time.fixedDeltaTime;

        RaycastHit hit2;
        int layerMask = ~LayerMask.GetMask("Bullet");
        Debug.DrawLine(transform.position, prediction);

        if (Physics.Linecast(transform.position, prediction, out hit2, layerMask))
        {
            transform.position = hit2.point;
            theRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            theRB.isKinematic = true;
            yield return 0;
            OnTriggerEnterFixed(hit2.collider);
        }
    }

    void OnTriggerEnterFixed(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
