using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class IceCreamManager : MonoBehaviour
{
    [SerializeField] private IceCreamUIManager _iceCreamUIManager;
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private Color[] _colors;
    private Color _currentColor;
    private int _currentSplineIndex;
    private bool _doCreate; 
    private bool _isCreating; 
    private void Awake()
    {
        _iceCreamUIManager.CreateButtons(_colors);
    }
    
    private void OnEnable()
    {
        IceCreamUIManager.OnCreatePiece += CreatePiece;
        IceCreamUIManager.OnStopCreate += StopCreate;
    }

    private void OnDisable()
    {
        IceCreamUIManager.OnCreatePiece -= CreatePiece;
        IceCreamUIManager.OnStopCreate -= StopCreate;       
    }

    private void Update()
    {
        if (_doCreate && !_isCreating)
        {
            CreatePiece();
        }
    }

    private void CreatePiece(Color color)
    {
        _doCreate = _splineContainer.Spline.Knots.Count() > _currentSplineIndex;
        _currentColor = color;
    }
    
    private void StopCreate()
    {
        _doCreate = false;
    }

    void CreatePiece()
    {
        Debug.Log(_currentColor);
 
    }
}