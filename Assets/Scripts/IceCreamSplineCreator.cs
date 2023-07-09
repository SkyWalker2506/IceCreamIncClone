using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class IceCreamSplineCreator : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private IceCreamCircleData[] _iceCreamCircleDatas;

    [ContextMenu("Create Spline")]
    void CreateIceCreamSpline()
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
        for (int i = 0; i < data.PieceCount; i++)
        {
            float angle = angleInterval * i * (Mathf.PI / 180);
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            _splineContainer.Spline.Add(new BezierKnot(new float3(x*data.Radius,data.Height,z*data.Radius)));       
        }
    }
}

[Serializable]
public struct IceCreamCircleData
{
    public float Radius;
    public float Height;
    public int PieceCount;
}