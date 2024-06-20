using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    private Animator anim, orbAnim;
    private bool hasInteracted = false;

    [SerializeField] private ItemSO itemData;
    [SerializeField] private GameObject orb;
    [SerializeField] private Info info;


    public override void Interact()
    {
        //Solo se ejecuta una vez la corrutina
        if (hasInteracted) return;
        
        //Obtenemos el animator y abrimos el cofre
        anim = GetComponent<Animator>();
        anim.SetBool("Open", true);

        //Creamos el objeto orbe y obtenemos su animator
        orb = Instantiate(orb, transform.position, transform.rotation);
        orbAnim = orb.GetComponent<Animator>();

        StartCoroutine(GetOrb());
        
    }

    private IEnumerator GetOrb()
    {
        hasInteracted = true;

        //Hacemos que el jugador no se mueva
        gameManager.ChangePlayerState(false);

        //Activamos el movimiento de la orbe y hacemos sonido
        AudioManager.Instance.PlaySFX("Complete");
        orbAnim.SetBool("OrbMove", true);
        yield return new WaitForSeconds(2);

        //Obtenemos la orbe
        gameManager.NewItem(itemData);
        Destroy(orb);

        //Le ponemos un mensaje al jugador
        info.Interact();
        yield return new WaitForSeconds(2);
        info.Interact();
        gameManager.ChangePlayerState(true);

        //Destruimos el cofre y guardamos su id
        Destroy(gameObject);
        gameManager.InteractableObject[id] = false;
    }
}
