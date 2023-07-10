using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class IceCreamSplineCreator : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private IceCreamCircleData[] _iceCreamCircleDatas;

    //[ContextMenu("Create Spline")]
    public void CreateIceCreamSpline()
    {
        _splineContainer.Spline.Clear();

        foreach (IceCreamCircleData data in _iceCreamCircleDatas)
        {
            AddNodesCircular(data);
        }
    }

    private void AddNodesCircular(IceCreamCircleData data)
    {
        float angleInterval = 360f / data.PieceCount;
        float radiusInterval = data.RadiusInterval / data.PieceCount;
        for (int i = 0; i < data.PieceCount; i++)
        {
            float radius = data.BaseRadius-radiusInterval*i;
            float angle = angleInterval * i * (Mathf.PI / 180);
            float x = Mathf.Cos(angle);
            float y = data.BaseHeight+data.HeightInterval*i/data.PieceCount;
            float z = Mathf.Sin(angle);
            Vector3 position = new Vector3(x * radius, y, -z *radius);
            BezierKnot bezierKnot = new BezierKnot
            {
                Position = position,
                Rotation = Quaternion.LookRotation(new Vector3(x,0,-z).normalized)
            };
            Debug.Log(bezierKnot.Rotation);
            _splineContainer.Spline.Add(bezierKnot);       
        }
    }
    
    [Serializable]
    private struct IceCreamCircleData
    {
        public float BaseRadius;
        public float RadiusInterval;
        public float BaseHeight;
        public float HeightInterval;
        public int PieceCount;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(IceCreamSplineCreator))]
public class IceCreamSplineCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        IceCreamSplineCreator iceCreamSplineCreator = (IceCreamSplineCreator)target;
        if (GUILayout.Button("Update Spline"))
        {
            iceCreamSplineCreator.CreateIceCreamSpline();
        }
    }
}
#endif