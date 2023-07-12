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
        

        public float Time { get; private set; }

        public IceCreamPieceData(Vector3 startPos, Vector3 endPos)
        {
            _startPosition = startPos;
            _endPosition = endPos;
            Time = 0;
            _bezierKnot = new BezierKnot(_startPosition);
        }

        public void UpdatePieceData(float time)
        {
            Time = Mathf.Clamp01(time);
            _bezierKnot.Position = Vector3.Lerp(_startPosition, _endPosition, Time);
            _splineContainer.Spline.SetKnot(_knotIndex,_bezierKnot);
        }

        public void SetKnot(SplineContainer splineContainer, int index)
        {
            _splineContainer = splineContainer;
            _knotIndex = index;
        }

    }
}
