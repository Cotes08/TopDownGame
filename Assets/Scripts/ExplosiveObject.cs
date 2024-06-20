using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : InteractableObject
{
    private bool canStartCorrutine = true;

    [SerializeField] private GameObject item;
    [SerializeField] private Info info;

    public override void Interact()
    {
        //Si el jugador tiene los objetos necesarios y la corrutina no esta activa podra comenzar una
        //En el caso de que no se le informara de que objeto necesita para explotar el obstaculo
        if (gameManager.HasItem(item.name) && canStartCorrutine)
        {
            StartCoroutine(BombExplode()); 
        }
        else
        {
            info.Interact();
        }
    }

    private IEnumerator BombExplode()
    {
        canStartCorrutine = false;
        //Instanciamos una bomba
        GameObject bomb = Instantiate(item, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2);

        //Tras unos segundos hacemos un sonido, destruimos tanto la bomba como el obstaculo y guardamos el id del osbtaculo desrtuido
        AudioManager.Instance.PlaySFX("Explosion");
        Destroy(bomb);
        Destroy(gameObject);
        gameManager.InteractableObject[Id] = false;

        canStartCorrutine = true;
    }
}

    

