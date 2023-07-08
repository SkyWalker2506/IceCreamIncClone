using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class IceCreamSplineCreator : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _startRadius = 5;
    [SerializeField] private float _horizontalInterval =.5f;
    [SerializeField] private float _verticalInterval =.5f;
    [SerializeField] private float _unitAngle = 30;
    
    [ContextMenu("Create Spline")]
    void CreateIceCreamSpline()
    {
        _splineContainer.Spline.Clear();
        float currentRadius = _startRadius;
        float currentHeight = 0;
        
        while (currentRadius>0)
        {
            AddNodesCircular(currentRadius , currentHeight);
            currentRadius -= _horizontalInterval;
            currentRadius =  Mathf.Round(currentRadius * 1000000) * .000001f;
            currentHeight += _verticalInterval;
        }

        _splineContainer.Spline.Add(new BezierKnot(new float3(0,currentHeight,0)));       

    }

    private void AddNodesCircular(float radius, float height)
    {
        
        float radiusAngle = _unitAngle / radius;
        int count =  Mathf.CeilToInt(180 / radiusAngle);
        for (int i = 0; i < count; i++)
        {
            _splineContainer.Spline.Add(new BezierKnot(new float3(Mathf.Cos(radiusAngle*i)*radius,height,Mathf.Sin(radiusAngle*i)*radius)));       
        }
    }
}