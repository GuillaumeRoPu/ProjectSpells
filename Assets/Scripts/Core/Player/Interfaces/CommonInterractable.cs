using UnityEngine;

public class CommonInterractable : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _material1;
    [SerializeField] private Material _material2;
    private bool _activated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _activated = false;
        _renderer.material = _material1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        if (_activated) 
        { 
            _renderer.material = _material1;
            _activated = false;
        }
        else
        {
            _renderer.material = _material2;
            _activated = true;
        }
    }
}
