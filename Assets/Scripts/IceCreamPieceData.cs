using UnityEngine;
using UnityEngine.Splines;

namespace IceCreamInc.IceCreamMechanic
{
    public class IceCreamPieceData
    {
        private SplineContainer _splineContainer;
        private int _knotIndex = 0;
        private BezierKnot _bezierKnot;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Quaternion _startRotation;
        private Quaternion _endRotation;
        

        public float Time { get; private set; }

        public IceCreamPieceData(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRotation)
        {
            _startPosition = startPos;
            _endPosition = endPos;
            _startRotation = startRot;
            _endRotation = endRotation;
            Time = 0;
            _bezierKnot = new BezierKnot
            {
                Position = _startPosition,
                Rotation = _startRotation
            };
        }

        public void UpdatePieceData(float time)
        {
            Time = Mathf.Clamp01(time);
            _bezierKnot.Position = Vector3.Lerp(_startPosition, _endPosition, Time);
            _bezierKnot.Rotation = Quaternion.Lerp(_startRotation,_endRotation, Time);
            _splineContainer.Spline.SetKnot(_knotIndex,_bezierKnot);
        }

        public void SetKnot(SplineContainer splineContainer, int index)
        {
            _splineContainer = splineContainer;
            _knotIndex = index;
        }

    }
}
