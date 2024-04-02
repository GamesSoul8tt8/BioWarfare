using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour
{
    private Rigidbody2D rb2D;

    [Header("Movimiento")]
    private Vector2 input;
    private float movimientoHorizontal = 0f;
    [SerializeField]private float velocidadMovimiento;
    [Range(0, 0.3f)][SerializeField]private float suavizadoMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private LayerMask esSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    private bool salto = false;

    [Header("Animación")]
    private Animator animator;

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Vector3 dimensionesCajaPared;
    private bool enPared;
    private bool deslizando;
    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoPared;

    [Header("Agacharse")]
    [SerializeField] private Transform controladorTecho;
    [SerializeField] private float radioTecho;
    [SerializeField] private float multiplicadorVelAgachado;
    private CapsuleCollider2D colisionador;
    private bool agachar = false;
    private float alturaOriginalColisionador;
    private Vector2 centroOriginal;
    [SerializeField] private float reduccionColisionador;

    [Header("Dash")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private float tiempoDash;
    private float gravedadInicial;
    private bool puedeHacerDash = true;
    private bool sePuedeMover = true;

    [Header("Escaleras")]
    [SerializeField] private float velocidadEscalar;
    private bool escalando;

    
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colisionador = GetComponent<CapsuleCollider2D>();
        alturaOriginalColisionador = colisionador.size.y;
        centroOriginal = colisionador.offset;
        gravedadInicial = rb2D.gravityScale;

    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        movimientoHorizontal = input.x * velocidadMovimiento;
        
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animator.SetFloat("SpeedY", rb2D.velocity.y);
        animator.SetBool("Deslizando", deslizando);

        if (Input.GetButtonDown("Jump")){
            salto = true;
        }

        if (!enSuelo && enPared && input.x != 0)
        {
            deslizando = true;
        }else
        {
            deslizando = false;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (!agachar)
            {
                agachar = true;
                Debug.Log("Agachado");
                alturaOriginalColisionador = colisionador.size.y;
                centroOriginal = colisionador.offset;
                //colisionador.size = new Vector2(colisionador.size.x, colisionador.size.y / reduccionColisionador);
                //colisionador.offset = new Vector2(centroOriginal.x, -colisionador.size.y / reduccionColisionador);
                colisionador.size = new Vector2(colisionador.size.x, 0.4f);
                colisionador.offset = new Vector2(centroOriginal.x, -0.1f);
            }else
            {
                if (Physics2D.OverlapCircle(controladorTecho.position, radioTecho, esSuelo))
                {
                    Debug.Log("Hay un obstáculo arriba, no puedes levantarte completamente.");
                    agachar = true;
                }else{
                    agachar = false;
                    Debug.Log("De pie");
                    colisionador.size = new Vector2(colisionador.size.x, alturaOriginalColisionador);
                    colisionador.offset = centroOriginal;
                }
            }

            animator.SetBool("Crouch", agachar);
            
        }

        if(Input.GetButtonDown("Dash") && puedeHacerDash && enSuelo)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate(){
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, esSuelo);
        animator.SetBool("EnSuelo",enSuelo);

        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, esSuelo);

        if(sePuedeMover)
        {
            Mover(movimientoHorizontal * Time.fixedDeltaTime, salto, agachar);
        }

        Escalar();
        salto = false;

        if (deslizando)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Clamp(rb2D.velocity.y, - velocidadDeslizar, float.MaxValue));
        }
    }

    private void Mover(float mover, bool saltar, bool agachar)
    {
        if (agachar)
        {
            mover *= multiplicadorVelAgachado;
        }

        if (!saltandoPared)
        {
            Vector3 velocidadObjetivo = new Vector2(mover,rb2D.velocity.y);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoMovimiento);
        }

        if (enSuelo)
        {
            saltandoPared = false;
        }

        if (mover > 0 && !mirandoDerecha && !saltandoPared)
        {
            Girar();
        }
        else if ( mover < 0 && mirandoDerecha && !saltandoPared)
        {
            Girar();
        }

        if (enSuelo && saltar && !deslizando && !agachar)
        {
            Salto();
        }

        if (enPared && saltar && deslizando)
        {
            Girar();
            SaltoPared();
        }
    }

    private void SaltoPared()
    {
        enPared = false;
        rb2D.velocity = new Vector2(fuerzaSaltoParedX * -input.x, fuerzaSaltoParedY);
        CambioSaltoPared();
    }

    private void CambioSaltoPared()
    {
        saltandoPared = true;
    }

    private void Salto()
    {
        enSuelo = false;
        rb2D.AddForce(new Vector2(0f, fuerzaSalto));
    }

    private IEnumerator Dash()
    {
        sePuedeMover = false;
        puedeHacerDash = false;
        rb2D.velocity = new Vector2(velocidadDash * transform.localScale.x, 0);
        colisionador.size = new Vector2(colisionador.size.x, 0.275f);
        colisionador.offset = new Vector2(centroOriginal.x, -0.175f);
        animator.SetTrigger("Dash");

        yield return new WaitForSeconds(tiempoDash);
        sePuedeMover = true;
        puedeHacerDash = true;

        if (Physics2D.OverlapCircle(controladorTecho.position, radioTecho, esSuelo))
        {
            Debug.Log("Hay un obstáculo arriba, no puedes levantarte completamente.");
            agachar = true;
        }else{
            agachar = false;
            Debug.Log("De pie");
            colisionador.size = new Vector2(colisionador.size.x, alturaOriginalColisionador);
            colisionador.offset = centroOriginal;
        }
        animator.SetBool("Crouch", agachar);
    }

    private void Escalar()
    {
        if(colisionador.IsTouchingLayers(LayerMask.GetMask("Ladder")) && (input.y !=0 || escalando))
        {
            Debug.Log("Escalera");
            Vector2 velocidadSubida = new Vector2(rb2D.velocity.x, input.y * velocidadEscalar);
            rb2D.velocity = velocidadSubida;
            rb2D.gravityScale = 0;
            escalando = true;
        }else{
            rb2D.gravityScale = gravedadInicial;
            escalando = false;
        }

        if(enSuelo)
        {
            escalando = false;
            //Debug.Log("Dejo de escalar");
        }
        animator.SetBool("Escalar", escalando);
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
        Gizmos.DrawWireSphere(controladorTecho.position, radioTecho);
    }
}
