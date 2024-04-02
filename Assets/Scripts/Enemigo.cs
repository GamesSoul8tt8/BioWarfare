using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private string nombre;
    [SerializeField] private float vida;
    public float damageCausado;
    public float velocidad;
    [SerializeField] private float fuerzaRetrocesoX;
    [SerializeField] private float fuerzaRetrocesoY;
    [SerializeField] private float duracionRetroceso;
    // Varaibles que afectan al Jugador
    public float retrocesoFuerzaX; 
    public float retrocesoFuerzaY;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator TomarDamage(float damage, float direccion)
    {
        vida -= damage;
        if (vida <= 0)
        {
            Muerte();
        }else{
            rb.AddForce(new Vector2(direccion*fuerzaRetrocesoX, fuerzaRetrocesoY), ForceMode2D.Force);
            yield return new WaitForSeconds(duracionRetroceso);
            rb.AddForce(new Vector2(-direccion*fuerzaRetrocesoX, -fuerzaRetrocesoY), ForceMode2D.Force);
        }
    }

    private void Muerte()
    {
        Destroy(gameObject);
    }
}
