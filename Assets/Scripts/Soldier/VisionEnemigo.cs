using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool aturdido, moridos;
    [SerializeField] private AudioClip shoot;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        ataque_var = GetComponent<SoldierMovement>();
    }
    private void Update()
    {
        atacando = GetComponent<SoldierMovement>().isHunting;
        aturdido = GetComponent<Enemigo>().stun;
        moridos = GetComponent<Enemigo>().morido;
        if (jugador != null)
        {
            MovimientoPersonaje movimientoJugador = jugador.GetComponent<MovimientoPersonaje>();
            jugadorRangoDisparo = Physics2D.OverlapBox(controladorAtaque.position, dimensionesAtaque, 0f, capaJugador);
            jugadorEspalda = Physics2D.OverlapBox(controladorVision.position, dimensionesVision, 0f, capaJugador);

            
            if((!movimientoJugador.agachar && !atacando && jugadorEspalda) || jugadorRangoDisparo && !atacando)
            {
                Deteccion();
            }
            if(atacando)
            {
                jugadorRangoMelee = Physics2D.OverlapBox(controladorMelee.position, dimensionesMelee, 0f, capaJugador);
                Debug.Log(movimientoJugador.muerto);
                if(!moridos)
                {
                    if(!movimientoJugador.muerto)
                    {
                        if(!aturdido)
                        {
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
                        }
                    }
                    if(jugadorEspalda)
                    {
                        ataque_var.FlipHunt();
                    }
                }
            }
        }
    }

    private void Disparar()
    {
        ControladorSonidos.Instance.EjecutarSonido(shoot);
        Instantiate(bala, controladordisparo.position, controladordisparo.rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorVision.position, dimensionesVision);
        Gizmos.DrawWireCube(controladorAtaque.position, dimensionesAtaque);
        Gizmos.DrawWireCube(controladorMelee.position, dimensionesMelee);
    }

    private void Deteccion()
    {
        ataque_var.isStatic = false;
        ataque_var.isPatrol = false;
        ataque_var.isHunting = true;
    }

}