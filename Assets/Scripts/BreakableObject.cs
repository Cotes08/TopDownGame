using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : InteractableObject
{

    [SerializeField] private ItemSO itemSO;
    [SerializeField] private bool hasCoin;
    [SerializeField] private Item item;
    [SerializeField] private Info info;

    public override void Interact()
    {
        //Si el jugador tiene el item para romper el objeto entra
        if (gameManager.HasItem(itemSO.name))
        {
            if (hasCoin)
            {
                //Instanciamos una moneda con un el id de su objeto padre +100 para evitar problemas con los ya existentes
                Item coin = Instantiate(item, transform.position, Quaternion.identity);
                coin.GetComponent<Item>().Id = this.Id + 100;
            }
            AudioManager.Instance.PlaySFX("Break");
            Destroy(gameObject);
            gameManager.InteractableObject[Id] = false;
        }
        else
        {
            //Esto abrira un cuadro de dialogo indicandonos que item debemos usar
            info.Interact();
        }
    }
}
