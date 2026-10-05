using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalBehaviour : MonoBehaviour
{
    [Header("<color=orange>Renderer</color>")]
    [SerializeField] private string _activationName = "_ActivationStatus";
    [SerializeField] private Texture2D[] _destinationTextures;
    [SerializeField] private string _destinationName = "_Destination";
    [SerializeField] private Color[] _mainColors;
    [SerializeField] private string _mainColorName = "_MainColor";
    [SerializeField] private Color[] _secondaryColors;
    [SerializeField] private string _secondaryColorName = "_SecondaryColor";
    [SerializeField] private string _crFloatName = "_CoroutineStatus";
    [SerializeField] private float _deactivationTimeRate = 2.0f;
    [SerializeField] private float _activationTimeRate = 2.0f;

    private bool _isChangeActive = false;
    private int _destinationIndex = 0;

    private InputSystem_Actions _inputs;
    private Material _material;
    private Renderer _renderer;

    private void Awake()
    {
        _inputs = new InputSystem_Actions();
    }

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;

        _material.SetTexture(_destinationName, _destinationTextures[_destinationIndex]);
        _material.SetColor(_mainColorName, _mainColors[_destinationIndex]);
        _material.SetColor(_secondaryColorName, _secondaryColors[_destinationIndex]);
    }

    #region Inputs
    private void OnEnable()
    {
        _inputs.Enable();

        _inputs.Player.Interact.performed += Interaction;
    }

    private void OnDisable()
    {
        _inputs.Disable();

        _inputs.Player.Interact.performed -= Interaction;
    }
    #endregion

    private void Interaction(InputAction.CallbackContext value)
    {
        if((_destinationTextures.Length == _mainColors.Length && _mainColors.Length == _secondaryColors.Length) && !_isChangeActive)
        {
            Debug.Log($"<color=green>A.</color>");

            StartCoroutine(ChangeDestination());
        }
        else
        {
            Debug.Log($"<color=red>B.</color>");
        }

        Debug.Log($"<color=orange>C.</color>");
    }

    private IEnumerator ChangeDestination()
    {
        _isChangeActive = true;

        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime;

            _material.SetFloat(_activationName, Mathf.Lerp(1.0f, 0.0f, t));
            _material.SetFloat(_crFloatName, Mathf.Lerp(0.0f, 1.0f, t));

            yield return null;
        }

        if(_destinationIndex >= _destinationTextures.Length - 1)
        {
            _destinationIndex = 0;
        }
        else
        {
            _destinationIndex++;
        }

        _material.SetTexture(_destinationName, _destinationTextures[_destinationIndex]);
        _material.SetColor(_mainColorName, _mainColors[_destinationIndex]);
        _material.SetColor(_secondaryColorName, _secondaryColors[_destinationIndex]);

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime;

            _material.SetFloat(_activationName, Mathf.Lerp(0.0f, 1.0f, t));
            _material.SetFloat(_crFloatName, Mathf.Lerp(1.0f, 0.0f, t));

            yield return null;
        }

        _isChangeActive = false;
    }
}
