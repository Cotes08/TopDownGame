using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchObjects : InteractableObject
{

    private bool talking = false;

    [SerializeField] private ItemSO itemData;//Datos obtenidos por el ScriptableObject
    [SerializeField, TextArea(1, 5)] private string dialog;
    [SerializeField] private GameObject dialogFrame;
    [SerializeField] private TextMeshProUGUI dialogText;

    public override void Interact()
    {
        //Si interactuamos con un objeto que contenga un item lo añadimos a nuestro inventario
        if (itemData && talking)
        {
            gameManager.NewItem(itemData);
            //Ponemos esto para que no velva a aparecer el objeto
            gameManager.InteractableObject[id] = false;
        }

        //Desactivamos el movimiento del jugador para mostrar la interfaz y darle informacion del objeto que ha recogido
        gameManager.ChangePlayerState(false);
        dialogFrame.SetActive(true);

        if (!talking)
        {
            AudioManager.Instance.PlaySFX("Pickup");
            talking = true;
            dialogText.text = dialog;
        }
        else
        {
            talking = false;
            dialogText.text = "";
            dialogFrame.SetActive(false);
            gameManager.ChangePlayerState(true);
            Destroy(gameObject);
        }
    }
}
