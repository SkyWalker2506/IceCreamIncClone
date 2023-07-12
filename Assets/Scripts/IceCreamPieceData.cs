using UnityEngine;
using UnityEngine.Splines;

namespace IceCreamInc.IceCreamMechanic
{
    public class IceCreamPieceData
    {
        public BezierKnot BezierKnot;
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public Quaternion StartRotation;
        public Quaternion EndRotation;
        public float Time { get; private set; }

        public IceCreamPieceData(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRotation)
        {
            StartPosition = startPos;
            EndPosition = endPos;
            StartRotation = startRot;
            EndRotation = endRotation;
            Time = 0;
            BezierKnot = new BezierKnot
            {
                Position = StartPosition,
                Rotation = StartRotation
            };
        }

        public void UpdatePieceData(float time)
        {
            Time = Mathf.Clamp01(time);
            BezierKnot.Position = Vector3.Lerp(StartPosition, EndPosition, Time);
            BezierKnot.Rotation = Quaternion.Lerp(StartRotation, EndRotation, Time);
        }
            
    }
}
