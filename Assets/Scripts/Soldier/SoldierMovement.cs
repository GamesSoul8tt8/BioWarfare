using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    private float velocidad;
    Rigidbody2D rb;
    Animator anim;
    public bool isStatic, isPatrol, isHunting;

    public Transform PointA, PointB;
    private bool goA, goB;
    [SerializeField] private bool shouldWait;
    [SerializeField] private bool isWaiting;
    [SerializeField] private float timeWait;
    [SerializeField] private bool A;
    [SerializeField] private Transform jugador;
    

    private void Start()
    {
        goA = true;
        A = true;
        isPatrol = true;
        velocidad = GetComponent<Enemigo>().velocidad;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(isStatic)
        {
            anim.SetBool("Static", true);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }else
        {
            if(isPatrol)
            {
                anim.SetBool("Static", false);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

                if(goA)
                {
                    if(!isWaiting)
                    {
                        anim.SetBool("Static", false);
                        rb.velocity = new Vector2(velocidad * Time.deltaTime, rb.velocity.y);
                    }


                    if (Vector2.Distance(transform.position, PointA.position) < 0.2f)
                    {
                        shouldWait = true;

                        A = false;

                        if(shouldWait)
                        {
                            StartCoroutine(Waiting());
                        }

                        goA = false;
                        goB = true;
                    }
                }
                if(goB)
            {
                if(!isWaiting)
                {
                    anim.SetBool("Static", false);
                    rb.velocity = new Vector2(-velocidad * Time.deltaTime, rb.velocity.y);
                }

                if (Vector2.Distance(transform.position, PointB.position) < 0.2f)
                {
                    shouldWait = true;

                    A = true;

                    if(shouldWait)
                    {
                        StartCoroutine(Waiting());
                    }

                    goA = true;
                    goB = false;
                }
            }
            }if(isHunting)
            {
                anim.SetBool("Static", true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                Debug.Log("Alerta");
            }
        }
    }

    IEnumerator Waiting()
    {
        anim.SetBool("Static", true);
        isStatic = true;
        isPatrol = false;
        isWaiting = true;

        yield return new WaitForSeconds(timeWait);

        Flip();
        anim.SetBool("Static", false);
        isStatic = false;
        isPatrol = true;
        isWaiting = false;
    }

    private void Flip(){
        float angulo = A ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0, angulo, 0);
    }

    public void FlipHunt()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y+180, 0);
    }
}