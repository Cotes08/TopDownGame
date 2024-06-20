using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Button : InteractableObject
{
    private Animator anim;
    [SerializeField] private GameObject wall;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; 
    [SerializeField] private NoiseSettings shakeProfile;
    [SerializeField] private Info info;
    private bool wallMoved = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Interact()   
    {
        //Si no se ha interactuado ya comenzamos la corrutina
        if (!wallMoved)
        {
            wallMoved = true;
            StartCoroutine(WallMove());
        }
    }

    IEnumerator WallMove()
    {
        //Se realiza la animacion y el sonido
        anim.SetBool("Pressed", true);
        AudioManager.Instance.PlaySFX("Button");

        //Bloqueamos al jugador
        gameManager.ChangePlayerState(false);

        AudioManager.Instance.PlaySound("Walls");

        //Cambiamos el perfil de ruido de cinemachine para que la camara tiemble
        CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        noise.m_NoiseProfile = shakeProfile;

        yield return new WaitForSeconds(2);
        AudioManager.Instance.StopAllAudio();

        //Una vez pase el tiempo quitamos el ruido de la camara
        noise.m_NoiseProfile = null;
        //Quitamos la pared
        wall.SetActive(false);
        //Le volvemos a dar control al jugador
        gameManager.ChangePlayerState(true);

        //Le mostramos un texto informativo
        info.Interact();
        yield return new WaitForSeconds(2);
        info.Interact();
    }
}
