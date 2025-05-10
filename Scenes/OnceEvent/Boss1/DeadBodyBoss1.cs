using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyBoss1 : MonoBehaviour
{
    private GameObject _interactObject;
    private InteractableObject _interactable;
    [HideInInspector] public bool BeginAbsorb;

    private void Awake()
    {
        _interactObject = this.transform.GetChild(1).gameObject;
        _interactable = _interactObject.GetComponent<InteractableObject>();
        _interactable._interact += OnInteract;
    }

    public void OpenInteract()
    {
        _interactObject.SetActive(true);
    }

    private void OnInteract()
    {
        BeginAbsorb = true;
        _interactObject.SetActive(false);
    }
}
