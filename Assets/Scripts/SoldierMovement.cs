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
    private bool caminarDerecha;

    [SerializeField] private Transform wCheck, pCheck, gCheck;
    [SerializeField] private bool wallDetected, pitDetected, isGround;
    [SerializeField] private float radioDeteccion;
    [SerializeField] private LayerMask haySuelo;

    private void Start()
    {
        velocidad = GetComponent<Enemigo>().velocidad;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        pitDetected = !Physics2D.OverlapCircle(pCheck.position, radioDeteccion, haySuelo);
        wallDetected = Physics2D.OverlapCircle(wCheck.position, radioDeteccion, haySuelo);
        if(wallDetected || pitDetected)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if(isStatic)
        {
            anim.SetBool("Static", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }else
        {
            anim.SetBool("Static", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (!caminarDerecha)
            {
                rb.velocity = new Vector2(velocidad * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-velocidad * Time.deltaTime, rb.velocity.y);
            }
        }
    }

    private void Flip(){
        caminarDerecha = !caminarDerecha;
        transform.localScale = new Vector2(-1*transform.localScale.x, transform.localScale.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wCheck.position, radioDeteccion);
        Gizmos.DrawWireSphere(gCheck.position, radioDeteccion);
        Gizmos.DrawWireSphere(pCheck.position, radioDeteccion);
    }
}