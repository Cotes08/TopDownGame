using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private int availableItems = 0;
    private List<ItemSO> inventoryCopy;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private GameObject iventoryFrame;
    [SerializeField] private Image[] itemsImages;
    [SerializeField] private GameObject iventoryInfo;
    [SerializeField] private TextMeshProUGUI coinNumber;

    //Nos suscribimos a los eventos
    private void OnEnable()
    {
        gameManager.OnNewItem += AddItem;
        gameManager.OnUpdateCoins += UpdateCoins;
        gameManager.OnRemoveItem += RemoveItem;
    }

    void Start()
    {
        //Inicializamos los items en la copia y los cargamos en la interfaz
        inventoryCopy = new List<ItemSO>(gameManager.PlayerInventory);
        foreach (var item in inventoryCopy)
        {
            AddItem(item);
        }

        //Inicilizamos las monedas
        UpdateCoins(gameManager.Coins);
    }

    void Update()
    {
        //Al pulsar la i mostramos el inventario
        if (Input.GetKeyDown(KeyCode.I))
        {
            iventoryFrame.SetActive(!iventoryFrame.activeSelf);
            iventoryInfo.SetActive(!iventoryInfo.activeSelf);
        }
    }

    public void AddItem(ItemSO itemData)
    {
        //Al recibir un evento activamos la imagen correspondiente al obtener un item y le cargamos los datos necesarios
        if (itemsImages[availableItems])
        {
            itemsImages[availableItems].gameObject.SetActive(true);
            itemsImages[availableItems].GetComponent<Image>().sprite = itemData.icon;
            availableItems++;

            //Guardamos los items del player para que no se pierdan
            gameManager.PlayerInventory.Add(itemData);
        }
    }
    
    public void RemoveItem(ItemSO itemData)
    {
        //Al recibir el evento comprobamos que ese objeto existia
        if (gameManager.PlayerInventory.Contains(itemData))
        {
            //Si el objeto esta disponible para borrarlo se besactivara su imagen, reducimos los objetos disponibles
            if (itemsImages[availableItems])
            {
                itemsImages[availableItems - 1].gameObject.SetActive(false);
                availableItems--;

                //Eliminamos el item de la lista de items del jugador
                gameManager.PlayerInventory.Remove(itemData);
            }
        }
    }

    //Al recibir el evento actualizamos las monedas de la interfaz con las que nos pasen
    public void UpdateCoins(int coinCount)
    {
        coinNumber.text = coinCount.ToString();
    }

    //Pare evitar problemas de duplicidad en los items los eliminamos de la lista
    private void OnDestroy()
    {
        foreach (var item in inventoryCopy)
        {
            gameManager.PlayerInventory.Remove(item);
        }
    }
}
