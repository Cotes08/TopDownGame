using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName="GameManager")]
public class GameManagerSO : ScriptableObject
{
    private Player player;
    public event Action<ItemSO> OnNewItem;
    public event Action<ItemSO> OnRemoveItem;
    public event Action<int> OnUpdateCoins;

    //Inicializamos la posicion donde queramos que empiece
    [NonSerialized] private Vector3 initPlayerPosition = new Vector3(2.5f, -5.5f, 0);
    [NonSerialized] private Vector3 initPlayerRotation = new Vector3(0, 0, 0);
    [NonSerialized] private Dictionary<int, bool> interactableObject = new Dictionary<int, bool>();
    [NonSerialized] private List<ItemSO> playerInventory = new List<ItemSO>();
    [NonSerialized] private int coins = 0;


    public Vector3 InitPlayerPosition { get => initPlayerPosition; }
    public Vector3 InitPlayerRotation { get => initPlayerRotation; }
    public Dictionary<int, bool> InteractableObject { get => interactableObject; }
    public List<ItemSO> PlayerInventory { get => playerInventory; }
    public int Coins { get => coins;}

    private void OnEnable()
    {
        //El game manager se sucribe al evento de la carga de una nueva escena
        SceneManager.sceneLoaded += SceneLoaded;
    }

    //Es en este metodo cuando buscamos al player u objetos que queramos tener traqueados
    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        player = FindAnyObjectByType <Player>();
    }

    public void ChangePlayerState(bool state)
    {
       //Negamos el estado para que sea variable segun si esta interactuando o no
       player.Interacting = !state;
    }

    //Funcion de carga de escena teniendo en cuenta posicion y rotacion
    public void LoadNewScene(Vector3 newPosition, Vector2 newRotation, string newSceneName)
    {
        SceneManager.LoadScene(newSceneName);
        initPlayerPosition = newPosition;
        initPlayerRotation = newRotation;
    }

    //Evento para añadir un item
    public void NewItem(ItemSO itemSO)
    {
        OnNewItem?.Invoke(itemSO);
    }

    //Evento para eliminar un item
    public void RemoveItem(ItemSO itemSO)
    {
        OnRemoveItem?.Invoke(itemSO);
    }

    //Evento para añadir monedas
    public void AddCoins(int coin)
    {
        coins += coin;
        OnUpdateCoins?.Invoke(coins);
    }

    //Evento para eliminar monedas
    public void RemoveCoins(int coin) 
    { 
        coins -= coin;
        OnUpdateCoins?.Invoke(coins);
    }

    //Funcion para saber si el jugador tiene el objeto
    public bool HasItem(string itemName)
    {
        //Hacemos una copia del inventario para no tocar el original
        List<ItemSO> inventoryCopy = new List<ItemSO>(playerInventory);
        foreach (var item in inventoryCopy)
        {
            if (item.name == itemName) return true;
        }
        return false;
    }

}
