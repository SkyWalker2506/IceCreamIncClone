using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using IceCreamInc.UI;
using UnityEngine;
using UnityEngine.Splines;

namespace IceCreamInc.IceCreamMechanic
{
    public class IceCreamManager : MonoBehaviour
    {
        [SerializeField] private IceCreamUIManager _iceCreamUIManager;
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private Transform _iceCreamDispenser;
        [SerializeField] private float _dispenserMoveSpeed = .5f;
        [SerializeField] private float _pieceMoveSpeed = 1f;
        [SerializeField] private float _pieceCreateInterval = .01f;
        [SerializeField] private Color[] _colors;
        private Color _currentColor;
        private int _currentPieceIndex;
        private bool _doCreate; 
        private float lastTimePiecePoured;
        private List<IceCreamPieceContainer> _iceCreamPieceContainers = new List<IceCreamPieceContainer>();
        private IceCreamPieceContainer _currentIceCreamPieceContainer;
       
        private void Awake()
        {
            _iceCreamUIManager.CreateButtons(_colors);
        }

        private void OnEnable()
        {
            _iceCreamUIManager.OnPourIceCreamPiece += OnPourIceCreamButton;
            _iceCreamUIManager.OnStopPour += StopCreate;
            _iceCreamUIManager.OnResetButton += Reset;
        }

        private void OnDisable()
        {
            _iceCreamUIManager.OnPourIceCreamPiece -= OnPourIceCreamButton;
            _iceCreamUIManager.OnStopPour -= StopCreate;       
            _iceCreamUIManager.OnResetButton -= Reset;
        }

        private void Update()
        {
            TryCreateIceCreamPiece();
            MoveIceCreamPieces();
        }

        private void TryCreateIceCreamPiece()
        {
            if (_splineContainer.Spline.Knots.Count() > _currentPieceIndex)
            {
                if (_doCreate && lastTimePiecePoured + _pieceCreateInterval < Time.time)
                {
                    CreateIceCreamPiece();
                }
            }
        }

        private void MoveIceCreamPieces()
        {
            foreach (IceCreamPieceContainer container in _iceCreamPieceContainers)
            {
                container.MovePieces();
            }
        }

        private void Reset()
        {
            foreach (IceCreamPieceContainer container in _iceCreamPieceContainers)
            {
                container.Destroy();
            }
            _iceCreamPieceContainers.Clear();
            _currentIceCreamPieceContainer = null;
            _currentPieceIndex = 0;
            _iceCreamDispenser.position = new Vector3(0, _iceCreamDispenser.position.y, 0);
            _currentIceCreamPieceContainer = null;
        }

        private void OnPourIceCreamButton(Color color)
        {
            _doCreate = true;
            _currentColor = color;
        }
        
        private void StopCreate()
        {
            _doCreate = false;
            _currentIceCreamPieceContainer = null;
        }

        void CreateIceCreamPiece()
        {
            if (_currentIceCreamPieceContainer == null)
            {
                _currentIceCreamPieceContainer = new IceCreamPieceContainer(Instantiate(Resources.Load<Mesh>("SplineMesh")), _currentColor, _pieceMoveSpeed);
                _iceCreamPieceContainers.Add(_currentIceCreamPieceContainer);
                if (_currentPieceIndex != 0)
                {
                    _currentPieceIndex --;
                }   
            }
            
            BezierKnot knot = _splineContainer[0].Knots.ToArray()[_currentPieceIndex];
            Vector3 pieceTargetPos = knot.Position;
            Vector3 dispenserPosition = _iceCreamDispenser.position;
            Vector3 dispenserTargetPos = new Vector3(pieceTargetPos.x, dispenserPosition.y, pieceTargetPos.z);
            _iceCreamDispenser.DOMove(dispenserTargetPos, _dispenserMoveSpeed).SetSpeedBased(true).SetEase(Ease.InSine);
            _currentIceCreamPieceContainer.AddNode(new IceCreamPieceData(dispenserPosition, pieceTargetPos));
            _currentPieceIndex++;
            lastTimePiecePoured = Time.time;
        }

    }
}
