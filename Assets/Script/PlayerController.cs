using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public int healthCount;
    public int coinCount;

    int jumpCount = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    public Text healthText;
    public Text coinText;
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float hVelocity = 0;
        float vVelocity = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hVelocity = -moveSpeed;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            hVelocity = moveSpeed;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
        }
        else
        {
            animator.SetFloat("xVelocity", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 0)
        {
            jumpCount = 1;
            animator.SetTrigger("JumpTrigger");
            audioSource.PlayOneShot(audioClips[0]);
            vVelocity = jumpForce;
        }

        hVelocity = Mathf.Clamp(rb.velocity.x + hVelocity, -5, 5);

        rb.velocity = new Vector2(hVelocity, rb.velocity.y + vVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int rand = Random.Range(1, 2);

            healthCount -= 10;
            audioSource.PlayOneShot(audioClips[rand]);
            healthText.GetComponent<Text>().text = "Health: " + healthCount;
        }

        if (collision.gameObject.tag == "Coin")
        {
            coinCount++;
            audioSource.PlayOneShot(audioClips[3]);
            Destroy(collision.gameObject);
            coinText.GetComponent<Text>().text = "Coin: " + coinCount;
        }

        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }
    }
}
