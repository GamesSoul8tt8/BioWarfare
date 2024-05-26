using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Salud : MonoBehaviour
{
    [SerializeField]private float salud;
    [SerializeField]private float saludMaxima;
    [SerializeField] private float tiempoInvulnerabilidad;
    [SerializeField] private AudioClip hurt, death;

    Rigidbody2D rb;
    private bool invulnerabilidad;
    private Animator anim;
    [SerializeField] private HealthBar barraVida;

    public bool dead;
    private void Start()
    {
        salud = saludMaxima;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        barraVida.iniciarBarraVida(salud);
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
        if(!invulnerabilidad && salud > 0)
        {
            salud -= damage;
            ControladorSonidos.Instance.EjecutarSonido(hurt);
            barraVida.CambiarVidaActual(salud);
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerabilidad());

            if(salud <= 0)
            {
                dead = true;
                ControladorSonidos.Instance.EjecutarSonido(death);
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