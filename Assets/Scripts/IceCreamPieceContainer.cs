using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

namespace IceCreamInc.IceCreamMechanic
{
    public class IceCreamPieceContainer
    {
        private SplineContainer _splineContainer;
        private List<IceCreamPieceData> _iceCreamPieces = new List<IceCreamPieceData>();
        private float _pieceMoveSpeed;
        
        public IceCreamPieceContainer(Mesh mesh, Color color, float pieceMoveSpeed)
        {
            _splineContainer = new GameObject().AddComponent<SplineContainer>();
            SplineExtrude splineExtrude = _splineContainer.gameObject.AddComponent<SplineExtrude>();
            splineExtrude.Container = _splineContainer;
            splineExtrude.Sides = 100;
            splineExtrude.SegmentsPerUnit = 25;
            splineExtrude.RebuildFrequency = 100;
            splineExtrude.RebuildOnSplineChange = true;
            _splineContainer.GetComponent<MeshFilter>().sharedMesh = mesh;
            MeshRenderer renderer= _splineContainer.GetComponent<MeshRenderer>();
            renderer.material.color = color;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            _pieceMoveSpeed = pieceMoveSpeed;
        }

        public void AddNode(IceCreamPieceData pieceData)
        {
            _iceCreamPieces.Add(pieceData);
            _splineContainer.Spline.Add(new BezierKnot());
            pieceData.SetKnot(_splineContainer, _splineContainer.Spline.Knots.Count() - 1);
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

        public void Destroy()
        {
           Object.Destroy(_splineContainer.gameObject);
        }
    }

    

}