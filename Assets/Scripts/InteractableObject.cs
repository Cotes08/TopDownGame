using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, Interactive
{
    //Esta clase abstracta se utiliza principalmenet como clase padre para todos los objetos interactuables como los items
    //Al tenerlo todo aqui nos aseguramos de que todo item que se pueda obtener o destruir en el juego quede registrado

    [SerializeField] protected int id;
    [SerializeField] protected GameManagerSO gameManager;

    public int Id { get => id; set => id = value; }

    private void Start()
    {
        if (gameManager.InteractableObject.ContainsKey(id))
        {
            if (gameManager.InteractableObject[id] == false)
            {
                gameObject.SetActive(false); 
            }
        }
        else
        {
            gameManager.InteractableObject.Add(id, true);
        }
    }

    public abstract void Interact();
}
