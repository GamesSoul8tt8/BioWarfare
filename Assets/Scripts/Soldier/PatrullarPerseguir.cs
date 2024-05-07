using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrullarPerseguir : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float distanciaMinima;
    private SpriteRenderer spriteRenderer;
    private Collider2D visionCollider;
    private Transform visionConeTransform;
    private Transform jugadorTransform;
    private bool persiguiendo = false;
    private int siguientePaso = 0;

    [SerializeField] private Transform[] puntosMovimiento;
    private Vector2 targetPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        visionCollider = transform.Find("VisionCone").GetComponent<Collider2D>();
        visionConeTransform = transform.Find("VisionCone");
        jugadorTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (puntosMovimiento.Length > 0)
        {
            targetPosition = puntosMovimiento[siguientePaso].position;
            Girar();
        }
        else
        {
            Debug.LogError("No hay puntos de movimiento configurados.");
        }
    }

    private void Update()
    {
        if (persiguiendo)
        {
            float xTarget = jugadorTransform.position.x;
            Vector2 targetPosition = new Vector2(xTarget, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocidadMovimiento * Time.deltaTime);

            if (transform.position.x < xTarget)
            {
                spriteRenderer.flipX = false;
                visionConeTransform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                spriteRenderer.flipX = true;
                visionConeTransform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            if (puntosMovimiento.Length > 0)
            {
                targetPosition = puntosMovimiento[siguientePaso].position;

                transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocidadMovimiento * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < distanciaMinima)
                {
                    siguientePaso += 1;
                    if (siguientePaso >= puntosMovimiento.Length)
                    {
                        siguientePaso = 0;
                    }
                    targetPosition = puntosMovimiento[siguientePaso].position;
                    Girar();
                }
            }

            if (visionCollider.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>()))
            {
                Debug.Log("¡Jugador detectado!");
                persiguiendo = true;
            }
        }
    }


    private void Girar()
    {
        if (transform.position.x < puntosMovimiento[siguientePaso].position.x)
        {
            spriteRenderer.flipX = false;
            visionConeTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            spriteRenderer.flipX = true;
            visionConeTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}