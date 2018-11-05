using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {
    public float speed = 15f;

    
    public GameObject enemyPrefab;
    public float width = 11f;
    public float height = 5f;
    float xMin;
    float xMax;
    private bool movingRight = true;
    public float spawnDelay = 0.5f;
    

    // Use this for initialization
    void Start () {

        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        
        

        xMin = leftBoundary.x;
        xMax = rightBoundary.x;

        SpawnEnemies();


    }
    
    void SpawnEnemies()
    {
        
        foreach (Transform child in transform)
        {   // Burada child bu scriptin ait olduğu GameObject nesnesinin altında bulunan GameObjet nesnelerinin transform bilgilerini tek tek alır.
            // Position objelerinin yani.
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child; 
            // Klonlanan Enemy objelerini Position(child) objelerinin alt objesine gönderir.
            // Özetle der ki enemy nin parent ı sen ol Position(child) nesenesi!!
        }
    }

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
        
        
       
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width,height));
    }

    // Update is called once per frame
    void Update () {

        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);
        //Debug.Log(xMax+" "+ xMin);

       /* if (leftEdgeOfFormation <= xMin || rightEdgeOfFormation >= xMax)
        {
            movingRight = !movingRight; //Burada yukarıdaki koşullardan biri sağlandığı anda buradaki True değeri False oluyor. 
                                        //Tekrar koşullardan biri sağlandığında ise tam tersi False True oluyor ve bu şekilde devam ediyor sürekli.
        }                               // Aşşağıdaki kod ile aynı mânaya geliyor yani.
                                           (NOT: Bu kodda sıkıntı var speed değerini arttırınca Düşmanlar takılıyor)*/

        if (leftEdgeOfFormation < xMin)
        {
            movingRight = true;
        }
        else if(rightEdgeOfFormation > xMax)
        {
            movingRight = false;
        }

        if (AllMembersDead())
        {
            //Debug.Log("Empty Formation");
            SpawnUntilFull();// Burada AllMembersDead metodu Enemy ler yeniden üretildiği anda false oluyor. Dolayısıyla 1 kere çalışıyor. 
                           // Devamlı "Empty Formation" yazmıyor.
        }
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;

    }


    bool AllMembersDead()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if(childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }


}
