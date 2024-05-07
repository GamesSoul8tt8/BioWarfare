using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class VisionEnemigo : MonoBehaviour
{
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private Vector3 dimensionesAtaque;
    [SerializeField] private Transform controladorMelee;
    [SerializeField] private Vector3 dimensionesMelee;
    [SerializeField] private Transform controladorVision;
    [SerializeField] private Vector3 dimensionesVision;
    [SerializeField] private Transform controladordisparo;
    [SerializeField] LayerMask capaJugador;
    [SerializeField] private bool jugadorEspalda, jugadorRangoDisparo, jugadorRangoMelee;
    [SerializeField] private GameObject bala;
    [SerializeField] private float tiempoEntreDisparo;
    [SerializeField] private float tiempoUltimoDisparo;
    [SerializeField] private float tiempoEsperaDisparo;
    private bool atacando;
    private SoldierMovement ataque_var;
    private GameObject jugador;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        ataque_var = GetComponent<SoldierMovement>();
    }
    private void Update()
    {
        atacando = GetComponent<SoldierMovement>().isHunting;
        if (jugador != null)
        {
            MovimientoPersonaje movimientoJugador = jugador.GetComponent<MovimientoPersonaje>();
            jugadorRangoDisparo = Physics2D.OverlapBox(controladorAtaque.position, dimensionesAtaque, 0f, capaJugador);
            jugadorEspalda = Physics2D.OverlapBox(controladorVision.position, dimensionesVision, 0f, capaJugador);

            if(movimientoJugador != null && !movimientoJugador.agachar && !atacando)
            {
                if(jugadorEspalda || jugadorRangoDisparo)
                {
                    Debug.Log("Visualizado");
                    ataque_var.isStatic = false;
                    ataque_var.isPatrol = false;
                    ataque_var.isHunting = true;
                }
            }
            if(atacando)
            {
                jugadorRangoMelee = Physics2D.OverlapBox(controladorMelee.position, dimensionesMelee, 0f, capaJugador);
                if(jugadorRangoMelee)
                {
                    Debug.Log("Melee");
                }else
                {
                    if(jugadorRangoDisparo)
                    {
                        if (Time.time > tiempoEntreDisparo + tiempoUltimoDisparo)
                        {
                            tiempoUltimoDisparo = Time.time;
                            Invoke(nameof(Disparar), tiempoEsperaDisparo);
                        }
                    }
                }

                if(jugadorEspalda)
                {
                    ataque_var.FlipHunt();
                }
            }
        }
    }

    private void Disparar()
    {
        Instantiate(bala, controladordisparo.position, controladordisparo.rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorVision.position, dimensionesVision);
        Gizmos.DrawWireCube(controladorAtaque.position, dimensionesAtaque);
        Gizmos.DrawWireCube(controladorMelee.position, dimensionesMelee);
    }
}