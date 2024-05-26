using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
  private Slider slider;

  public void CambiarVidaMaxima(float vidaMaxima)
  {
    slider.maxValue = vidaMaxima;
  }

  public void CambiarVidaActual(float vida)
  {
    slider.value = vida;
  }

  public void iniciarBarraVida(float vida)
  {
    slider = GetComponent<Slider>();
    CambiarVidaActual(vida);
    CambiarVidaMaxima(vida);
  }
}
