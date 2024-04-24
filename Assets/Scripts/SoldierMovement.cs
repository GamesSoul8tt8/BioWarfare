using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SoldierMovement : MonoBehaviour
{
    private float velocidad;
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] private bool isStatic;

    public Transform PointA, PointB;
    private bool goA, goB;
    [SerializeField]private bool shouldWait;
    [SerializeField]private bool isWaiting;
    [SerializeField] private float timeWait;
    

    private void Start()
    {
        goA = true;
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

                    if(shouldWait)
                    {
                        StartCoroutine(Waiting());
                    }

                    goA = true;
                    goB = false;
                }
            }
        }
    }

    IEnumerator Waiting()
    {
        anim.SetBool("Static", true);
        isStatic = true;
        isWaiting = true;

        yield return new WaitForSeconds(timeWait);

        Flip();
        anim.SetBool("Static", false);
        isStatic = false;
        isWaiting = false;
    }

    private void Flip(){
        transform.localScale = new Vector2(-1*transform.localScale.x, transform.localScale.y);
    }
}