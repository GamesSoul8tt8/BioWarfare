using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private string nombre;
    [SerializeField] private float vida;
    public float damageCausado;
    public float velocidad;
    [SerializeField] private float duracionRetroceso;

    public bool stun, morido;
    private Coroutine damageCoroutine;
    
    // Varaibles que afectan al Jugador
    public float retrocesoFuerzaX; 
    public float retrocesoFuerzaY;
    private bool muerto = false;
    [SerializeField] private AudioClip hurt, death;

    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("Dead", muerto);
    }

    public void TomarDamage(float damage)
    {
        vida -= damage;
        if(vida <= 0)
        {
            muerto = true;
            ControladorSonidos.Instance.EjecutarSonido(death);
            anim.SetBool("Dead", muerto);
            StartCoroutine(Muerte());
        }else
        {
            ControladorSonidos.Instance.EjecutarSonido(hurt);
            anim.SetTrigger("Hurt");
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            // Iniciar una nueva instancia de la coroutine
            damageCoroutine = StartCoroutine(Damage());
        }
    }

    IEnumerator Muerte()
    {
        morido = true;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    IEnumerator Damage()
    {
        stun = true;
        yield return new WaitForSeconds(3f);
        stun = false;
        damageCoroutine = null;
    }
}