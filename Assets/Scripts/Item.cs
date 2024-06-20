using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : InteractableObject
{
    [SerializeField] private ItemSO itemData;//Datos obtenidos por el ScriptableObject

    public override void Interact()
    {
        //Diferenciamos entre item y coin ya que se han planteado con funcionalidades distintas
        if (itemData.name == "Coin")
        {
            //Sumamos el valor de la moneda
            gameManager.AddCoins(itemData.value);
        }
        else
        {
            //Añadimos el objeto al inventario
            gameManager.NewItem(itemData);
        }
        AudioManager.Instance.PlaySFX("Pickup");
        Destroy(gameObject);
        //Ponemos esto para que no vuelva a aparecer el objeto
        gameManager.InteractableObject[id] = false;
    }
}
