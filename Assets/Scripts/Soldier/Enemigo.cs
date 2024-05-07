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
    
    // Varaibles que afectan al Jugador
    public float retrocesoFuerzaX; 
    public float retrocesoFuerzaY;

    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TomarDamage(float damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            Muerte();
        }else{
            Debug.Log(damage);
        }
    }

    private void Muerte()
    {
        Destroy(gameObject);
    }
}
