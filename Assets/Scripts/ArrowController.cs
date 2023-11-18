using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour
{
    [SerializeField] GameObject effect;
//    [SerializeField] Text scoreValueText;



    private void OnTriggerEnter2D(Collider2D collision){
        if (!(collision.gameObject.tag == "Player"))
        {
            Destroy(gameObject);
                if ((collision.gameObject.CompareTag("Enemy")))
                {
                    //Instantiate(effect, collision.gameObject.transform.position,Quaternion.identity);
                    Destroy(collision.gameObject);
                    GameObject.Find("Level Manager").GetComponent<LevelManager>().AddScore(100);
                    
                }
        }
    }

    private void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
