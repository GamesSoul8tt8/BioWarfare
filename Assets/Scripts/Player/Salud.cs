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

    public void TakeDamage(int damage)
    {
        if(!invulnerabilidad)
        {
            salud -= damage;
            StartCoroutine(Invulnerabilidad());

            if(salud <= 0)
            {
                Destroy(gameObject);
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