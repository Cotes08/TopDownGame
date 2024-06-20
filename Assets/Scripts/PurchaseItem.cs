using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseItem : InteractableObject
{

    [SerializeField] private ItemSO itemData;

    public override void Interact()
    {
        //Comprobamos que tengamos el dinero suficiente para comprarlo
        if (gameManager.Coins >= itemData.value)
        {
            //Si es el caso nos quitamos las monedas y lo añadimos a nuestro inventario
            gameManager.RemoveCoins(itemData.value);
            gameManager.NewItem(itemData);

            AudioManager.Instance.PlaySFX("Pickup");

            Destroy(gameObject);
            //Ponemos esto para que no vuelva a aparecer el objeto comprado
            gameManager.InteractableObject[id] = false;
        }
    }
}
