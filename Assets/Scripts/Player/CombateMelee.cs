using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateMelee : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    private float damageGolpe;
    [SerializeField] private float tiempoAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;
    [SerializeField] private bool sword;
    [SerializeField] private bool katana;
    private Animator animator;
    [SerializeField] private bool locked;
    [SerializeField] AudioClip punch, Enemydamage;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        locked =  GetComponent<MovimientoPersonaje>().notAttack;
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        if (!locked)
        {
            if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
            {
                Golpe();
                tiempoSiguienteAtaque = tiempoAtaques;
            }
        }
    }

    private void Golpe()
    {
        if(katana)
        {

        }else
        {
            if(sword)
            {

            }else
            {
                ControladorSonidos.Instance.EjecutarSonido(punch);
            }
        }
        animator.SetTrigger("Golpe");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if(colisionador.CompareTag("Enemy"))
            {
                if(katana)
                {
                    damageGolpe = 30;
                }else{
                    if(sword)
                    {
                        damageGolpe = 20;
                    }else
                    {
                        damageGolpe = 10;
                    }
                }
                colisionador.transform.GetComponent<Enemigo>().TomarDamage(damageGolpe);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}