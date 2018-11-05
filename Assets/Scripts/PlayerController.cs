using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
    public GameObject projectile;
    public float speed = 15f;
    public float projectileSpeed = 10f;
    public float firingRate = 0.2f;
    public float health = 500f;
    public float padding = 0.3f;
    float xMin;
    float xMax;
    float xTop;
    public AudioClip playerFire;

    void Start()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x + padding;
        xMax = rightMost.x + padding;
        

    }
    

    void Fire()
    {
        Vector3 newPosition = transform.position + new Vector3(0, 1f, 0);
        GameObject projectileClone = Instantiate(projectile, newPosition, Quaternion.identity) as GameObject;
        projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(playerFire, transform.position);
    }

    void Update () {

        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.0000001f, firingRate);
            
                                                             // InvokeRepeating Space tuşuna basılı kaldığında arka arkaya ateş etmesini sağlıyor.
        }                                                    // Aradaki değer space e basılı turulduktan ne kadar sonra kurşunun atılmaya başayacağını ifade ediyor.
        if (Input.GetKeyUp(KeyCode.Space))                   // En sondaki fringeRate değeri ise Space e basılı tutlduğunda ne sıklıkla ateş edeceğinin süresini belirtiyor.
        {                                                    // Eğer hatırlayamadıysan değiştir ve gör.
            CancelInvoke("Fire");
        }



        if (Input.GetKey(KeyCode.LeftArrow)){
            //transform.position += new Vector3(-speed * Time.deltaTime,0,0);
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            //transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX,transform.position.y,transform.position.y);
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
        SceneManager.LoadScene("Win Screen");
        
    }
}
