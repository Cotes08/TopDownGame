using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    private float inputH;
    private float inputV;
    private bool moving;
    private bool interacting;
    private Vector3 targetDestination;
    private Vector3 interactionPoint;
    private Vector3 lastInput;
    private Collider2D colliderAhead;
    private Animator anim;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask layerColide;
    [SerializeField] private GameManagerSO gameManager;

    public bool Interacting { get => interacting; set => interacting = value; }

    void Start()
    {
        anim = GetComponent<Animator>();

        transform.position = gameManager.InitPlayerPosition;
        //Como no es un vector 3 la rotacion la tenemos en cuenta con la animacion:
        anim.SetFloat("inputV", gameManager.InitPlayerRotation.x);
        anim.SetFloat("inputH", gameManager.InitPlayerRotation.y);

    }

    void Update()
    {
        InputsHandler();
        AnimAndMoves();
    }

    private void AnimAndMoves()
    {
        //Si el player no esta interactuando, no se esta moviendo o los inputs son distintos a cero, entonces nos podremos mover
        if (!interacting && !moving && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("walking", true);
            anim.SetFloat("inputV", inputV);
            anim.SetFloat("inputH", inputH);

            lastInput = new Vector3(inputH, inputV, 0);
            targetDestination = transform.position + lastInput;
            //Nuestro punto de interaccion siempre ira delante nuestro
            interactionPoint = targetDestination;

            colliderAhead = CheckCollision();
            if (!colliderAhead) StartCoroutine(Move());
        }
        else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("walking", false);
        }
    }

    private void InputsHandler()
    {
        //Con esto evitamos que el personaje vaya en diagonal
        if (inputV == 0) inputH = Input.GetAxisRaw("Horizontal");
        if (inputH == 0) inputV = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction() 
    {
        colliderAhead = CheckCollision();
        //Pare evitar errores nos aseguramos de que el objeto con el que interactuamos implementa la interfaz interactive
        if (colliderAhead && colliderAhead.TryGetComponent(out Interactive interactiveGameObject))
        {
            interactiveGameObject.Interact();
        }
    }

    IEnumerator Move()
    {
        moving = true;
        while (transform.position != targetDestination)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetDestination, playerSpeed * Time.deltaTime);
            yield return null;
        }
        //De esta forma actualizamos el punto de interaccion por delante nuestra, teniendo en cuenta nuestro anterior input y la nueva poscion del personaje
        interactionPoint = transform.position + lastInput;
        moving = false;
    }

    private Collider2D CheckCollision()
    {
        //En el overlap tenemos en cuenta las capas con las que podremos colisionar
        return Physics2D.OverlapCircle(interactionPoint, interactionRadius, layerColide);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(interactionPoint, interactionRadius);
    }
}
