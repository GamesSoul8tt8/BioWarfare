using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Salud : MonoBehaviour
{
    [SerializeField]private float salud;
    [SerializeField]private float saludMaxima;
    [SerializeField] private float tiempoInvulnerabilidad;

    Rigidbody2D rb;
    private bool invulnerabilidad;
    private void Start()
    {
        salud = saludMaxima;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(salud > saludMaxima)
        {
            salud = saludMaxima;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !invulnerabilidad)
        {
            salud -= collision.GetComponent<Enemigo>().damageCausado;
            Debug.Log("Recibiste Daño");
            StartCoroutine(Invulnerabilidad());

            if(collision.transform.position.x > transform.position.x)
            {
                rb.AddForce(new Vector2(-collision.GetComponent<Enemigo>().retrocesoFuerzaX, collision.GetComponent<Enemigo>().retrocesoFuerzaY), ForceMode2D.Force);
            }
            else
            {
                rb.AddForce(new Vector2(collision.GetComponent<Enemigo>().retrocesoFuerzaX, collision.GetComponent<Enemigo>().retrocesoFuerzaY), ForceMode2D.Force);
            }

            if(salud <= 0)
            {
                Debug.Log("Has muerto, seras ...");
            }
        }
    }
    private IEnumerator Invulnerabilidad()
    {
        invulnerabilidad = true;
        yield return new WaitForSeconds(tiempoInvulnerabilidad);
        invulnerabilidad = false;
    }
}