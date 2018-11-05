using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);//Direk gameObject deseydik Shredder yok olurdu ancak col.gameObject diyince ona çarpanlar yok oluyor!!! ÖNEMLİİİ
    }
}
