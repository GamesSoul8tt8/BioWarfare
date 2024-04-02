using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateMelee : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float damageGolpe;
    [SerializeField] private float tiempoAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;
    private float direccion;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoAtaques;
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Golpe");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if(colisionador.CompareTag("Enemy"))
            {
                if(colisionador.transform.position.x < transform.position.x)
                {
                    direccion = -1f;
                }else{
                    direccion = 1f;
                }
                StartCoroutine(colisionador.transform.GetComponent<Enemigo>().TomarDamage(damageGolpe, direccion));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}