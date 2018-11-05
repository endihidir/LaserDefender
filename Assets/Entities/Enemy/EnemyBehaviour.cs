using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public GameObject projectile;
    public float health = 300f;
    public float projectileSpeed = 5f;
    public float shotsPerSeconds = 0.8f;
    public int scoreValue = 150;
    private ScoreKeeper scoreKeeper;
    public AudioClip enemyFire;
    public AudioClip enemyDie;

    void Start()
    {
        scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
        
    }

    void Fire()
    {
        // Not: Aşşağıda direk transform.position vermememizin sebebi kurşunların oyun başladığında Enemy lere çarpması ve 
        // Aşşağıdaki Trigger metodunu devreye sokarak Enemy leri Destroy etmesi dir.

        //Vector3 newPosition = transform.position + new Vector3(0, -1f, 0);
        GameObject projectileClone = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(enemyFire,transform.position);
        
    }

    void Update()
    {
        float probability = Time.deltaTime * shotsPerSeconds;
        float rand = Random.value;
        if (rand < probability)
        {
            Fire();
            //Debug.Log(rand + "<" + probability);
        }
       

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Projectile missile = col.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            if (health <= 0)
            {
                Die();

            }
            Destroy(col.gameObject);
        }
        
    }

    void Die()
    {
        Destroy(gameObject);
        scoreKeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(enemyDie, transform.position);
    }
}
