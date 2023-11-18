using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UnityEngine.WaitForSecondsRealtime1
public class PlayerController : MonoBehaviour
{
    private float mySpeedX;
    private Rigidbody2D myBody;
    private Vector3 defaultLocalScale;
    [SerializeField] float speed;//private 
    [SerializeField] float jumpPower;
    [SerializeField] GameObject arrow;
    public bool onGround; // Zemin üzerinde mi değil mi
    private bool canDoubleJump;
    [SerializeField] bool attacked;
    [SerializeField] float currentAttactTimer;
    [SerializeField] float defaultAttactTimer;
    private Animator myAnimator;
    [SerializeField] int arrowNumber;
    [SerializeField] Text arrowNumberText;
    [SerializeField] AudioClip dieMusic;
    [SerializeField] GameObject winPanel, losePanel;
    // Start is called before the first frame update
    void Start()
    {
        attacked = false;
        myAnimator = gameObject.GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        defaultLocalScale = transform.localScale;
        arrowNumberText.text = arrowNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.GetAxis("Horizontal"));
        mySpeedX = Input.GetAxis("Horizontal"); // -1 ile 1 arasında sağ ve sol ok tuşuna basılma süresine bağlı olarak değerler gelecek.
        myAnimator.SetFloat("Speed", Mathf.Abs(mySpeedX));
        myBody.velocity = new Vector2((mySpeedX * speed), myBody.velocity.y);

        #region  playerın sağ ve sol hareket yüzünün dünmesi
        if (mySpeedX > 0)
        {

            transform.localScale = new Vector3(defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);

        }
        else if (mySpeedX < 0)
        {

            transform.localScale = new Vector3(-defaultLocalScale.x, defaultLocalScale.y, defaultLocalScale.z);

        }
        #endregion

        #region  playerın zıplama özelleiği
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (onGround == true)
            {

                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                canDoubleJump = true;
                myAnimator.SetTrigger("Jump");
            }
            else
            {

                if (canDoubleJump == true)
                {

                    myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);
                    canDoubleJump = false;
                }

            }
        }
        #endregion

        #region playerın ok atmasınının kontrolu

        if (Input.GetMouseButtonDown(0) && arrowNumber > 0)
        {

            if (attacked == false)
            {

                attacked = true;

                myAnimator.SetTrigger("Attack");

                Fire();

                // Invoke("Fire",0.5f); // metod çalışmıyor başka yol bak ; 

            }
        }



        #endregion

        if (attacked == true)
        {

            currentAttactTimer -= Time.deltaTime;

        }
        else
        {
            currentAttactTimer = defaultAttactTimer;
        }

        if (currentAttactTimer <= 0)
        {

            attacked = false;
        }

        void Fire()
        {
            GameObject okumuz = Instantiate(arrow, transform.position, Quaternion.identity);
            okumuz.transform.parent = GameObject.Find("Arrows").transform;

            if (transform.localScale.x > 0)
            {
                okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
            }
            else
            {

                Vector3 okumuzScale = okumuz.transform.localScale;
                okumuz.transform.localScale = new Vector3(-okumuzScale.y, okumuzScale.x);
                okumuz.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);

            }
            arrowNumber--;
            arrowNumberText.text = arrowNumber.ToString();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<TimeController>().enabled = false;
            Die();
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(Wait(true));
            //winPanel.active = true;
            //Time.timeScale = 0;
        }
    }

    public void Die()
    {

        GameObject.Find("Sound Controler").GetComponent<AudioSource>().clip = null;
        GameObject.Find("Sound Controler").GetComponent<AudioSource>().PlayOneShot(dieMusic);

        myAnimator.SetTrigger("Die");
        myAnimator.SetFloat("Speed", 0);

        //myBody.constraints = RigidbodyConstraints2D.FreezePosition;
        myBody.constraints = RigidbodyConstraints2D.FreezeAll;
        enabled = false;
        StartCoroutine(Wait(false));
        //losePanel.SetActive(true); //losePanel.active = true;
        //Time.timeScale = 0;
    }

    IEnumerator Wait(bool win)
    {
        if(win == true){

        
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(1f);
        
        winPanel.SetActive(true);
       }

       else if(win == false){

           //Time.timeScale = 0;
           yield return new WaitForSecondsRealtime(0.75f);
           losePanel.SetActive(true);
       }

    }

  
}
