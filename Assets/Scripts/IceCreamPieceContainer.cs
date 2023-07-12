using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace IceCreamInc.IceCreamMechanic
{
    public class IceCreamPieceContainer
    {
        private SplineContainer _splineContainer;
        private List<IceCreamPieceData> _iceCreamPieces = new List<IceCreamPieceData>();
        private float _pieceMoveSpeed;
        
        public IceCreamPieceContainer(Color color, float pieceMoveSpeed)
        {
            _splineContainer = new GameObject().AddComponent<SplineContainer>();
            _splineContainer.gameObject.AddComponent<SplineExtrude>();
            _splineContainer.GetComponent<MeshRenderer>().material.color = color;
            _pieceMoveSpeed = pieceMoveSpeed;
        }

        public void AddNode(IceCreamPieceData pieceData)
        {
            _iceCreamPieces.Add(pieceData);
        }

        public void MovePieces()
        {
            if (IsAllPiecesMoved())
            {
                return;
            }

            foreach (var piece in _iceCreamPieces)
            {
                piece.UpdatePieceData(piece.Time + _pieceMoveSpeed * Time.deltaTime);
            }
        }
        
        private bool IsAllPiecesMoved()
        {
            return !_iceCreamPieces.Any(piece => piece.Time<1);
        }
    }

    

}