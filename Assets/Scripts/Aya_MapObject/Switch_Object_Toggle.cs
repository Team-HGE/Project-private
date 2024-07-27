using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Object_Toggle : InteractableObject
{
    [Header("GameObject")]
    [SerializeField] GameObject onSwitch;
    [SerializeField] GameObject offSwitch;
    [SerializeField] MeshRenderer[] lightObjectMesh;
    [SerializeField] GameObject[] lights;
    [SerializeField] bool turnLight;

    [Header("Material")]
    [SerializeField] Material[] materials;

    [Header("AudioSource")]
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void ActivateInteraction()
    {
        if (turnLight)
        {
            GameManager.Instance.player.playerInteraction.SetActive(true);
            GameManager.Instance.player.interactableText.text = "²ô±â";
        }
        else
        {
            GameManager.Instance.player.playerInteraction.SetActive(true);
            GameManager.Instance.player.interactableText.text = "ÄÑ±â";
        }
    }

    public override void Interact()
    {
        if (!turnLight)
        {
            foreach (MeshRenderer mesh in lightObjectMesh)
            {
                if (mesh != null)
                {
                    if (mesh.gameObject.name == "Room_Celling")
                    {
                        mesh.materials[1] = materials[1];
                    }
                    else
                    {
                        mesh.materials[0] = materials[1];
                    }
                }
            }
            foreach (GameObject obj in lights)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            turnLight = true;
            offSwitch.SetActive(false);
            onSwitch.SetActive(true);
            audioSource.Play();
            
        }
        else
        {
            foreach (MeshRenderer mesh in lightObjectMesh)
            {
                if (mesh.gameObject.name == "Room_Celling")
                {
                    mesh.materials[1] = materials[0];
                }
                else
                {
                    mesh.materials[0] = materials[0];
                }
            }
            foreach (GameObject obj in lights)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            turnLight = false;
            offSwitch.SetActive(true);
            onSwitch.SetActive(false);
            audioSource.Play();
        }
    }
}
