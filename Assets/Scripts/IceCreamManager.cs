using System.Collections.Generic;
using System.Linq;
using System.Timers;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class IceCreamManager : MonoBehaviour
{
    [SerializeField] private IceCreamUIManager _iceCreamUIManager;
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private GameObject _iceCreamPiecePrefab;
    [SerializeField] private Transform _iceCreamDispenser;
    [SerializeField] private float _dispenserMoveSpeed = .5f;
    [SerializeField] private float _pieceMoveSpeed = 1f;
    [SerializeField] private float _pieceRotateSpeed = 1f;
    [SerializeField] private Color[] _colors;
    private Color _currentColor;
    private int _currentPieceIndex;
    private bool _doCreate; 
    private bool _isDispenserMoving;
    private Dictionary<Transform, Material> _iceCreamPieces = new Dictionary<Transform, Material>();
    private Timer _timer;

    private void Awake()
    {
        _iceCreamUIManager.CreateButtons(_colors);
        CreateIceCreamPieces();
        _timer = new Timer(2);
    }

    private void OnEnable()
    {
        _iceCreamUIManager.OnPourIceCreamPiece += OnPourIceCreamButton;
        _iceCreamUIManager.OnStopPour += StopPour;
        _iceCreamUIManager.OnResetButton += Reset;
    }

    private void OnDisable()
    {
        _iceCreamUIManager.OnPourIceCreamPiece -= OnPourIceCreamButton;
        _iceCreamUIManager.OnStopPour -= StopPour;       
        _iceCreamUIManager.OnResetButton -= Reset;
    }

    private void Update()
    {
        if (_splineContainer.Spline.Knots.Count() > _currentPieceIndex)
        {
            if (_doCreate && !_isDispenserMoving)
            {
                PourIceCreamPiece();
            }
        }
    }

    private void Reset()
    {
        foreach (Transform piece in _iceCreamPieces.Keys)
        {
            piece.position=Vector3.zero;
            piece.gameObject.SetActive(false);
            _currentPieceIndex = 0;
            _iceCreamDispenser.position = new Vector3(0, _iceCreamDispenser.position.y, 0);
        }
    }

    private void CreateIceCreamPieces()
    {
        GameObject piece; 
        for (int i = 0; i < _splineContainer.Spline.Knots.Count(); i++)
        {
            piece = Instantiate(_iceCreamPiecePrefab);
            _iceCreamPieces.Add(piece.transform, piece.GetComponentInChildren<MeshRenderer>().material);
            piece.SetActive(false);
        }
    }
    
    private void OnPourIceCreamButton(Color color)
    {
        _doCreate = true;
        _currentColor = color;
    }
    
    private void StopPour()
    {
        _doCreate = false;
    }

    void PourIceCreamPiece()
    {
       // _isDispenserMoving = true;
        Transform piece = _iceCreamPieces.Keys.ToArray()[_currentPieceIndex];
        _iceCreamPieces[piece].color = _currentColor;
        BezierKnot knot = _splineContainer[0].Knots.ToArray()[_currentPieceIndex];
        Vector3 pieceTargetPos = knot.Position;
        quaternion pieceTargetRot = knot.Rotation;
        Vector3 dispenserTargetPos = new Vector3(pieceTargetPos.x, _iceCreamDispenser.position.y, pieceTargetPos.z);
        _iceCreamDispenser.DOMove(dispenserTargetPos, _dispenserMoveSpeed).SetEase(Ease.Linear);
        MovePiece(piece, dispenserTargetPos, pieceTargetPos, pieceTargetRot);
        _currentPieceIndex++;
    }

    void MovePiece(Transform piece, Vector3 startPos, Vector3 targetPos, Quaternion targetRot)
    {
        piece.position = startPos;
        piece.gameObject.SetActive(true);
        piece.DOMove(targetPos, _pieceMoveSpeed).SetEase(Ease.Linear).SetSpeedBased(false);
        piece.DORotateQuaternion(targetRot, _pieceRotateSpeed).SetEase(Ease.Linear).SetSpeedBased(false);
    }

    void OnDispenserMoved()
    {
       // _isDispenserMoving = false;
        _currentPieceIndex++;
    }
    
}