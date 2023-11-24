using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PathPoint
{
    public Vector3 pos;
    public Vector3 rot;
}

public class ObjectFollowPathBehaviour : MonoBehaviour
{
    [SerializeField] private PathPoint[] points;
    [SerializeField] private float duration;
    [SerializeField] private UnityEvent onMoveComplete;
    private Coroutine coroutine;

    [ContextMenu("Move")]
    public void Move()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        for (int i = 0; i < points.Length; i++)
        {
            yield return MoveToPoint(points[i]);
        }

        onMoveComplete?.Invoke();
    }

    private IEnumerator MoveToPoint(PathPoint _point)
    {
        float _timer = 0f;
        Vector3 _startPos = transform.position;
        Vector3 _startRot = transform.rotation.eulerAngles;
        while (_timer < (duration / points.Length))
        {
            _timer += Time.deltaTime;
            float _lerp = _timer / (duration / points.Length);
            transform.position = Vector3.Lerp(_startPos, _point.pos, _lerp);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(_startRot, _point.rot, _lerp));
            yield return null;
        }
    }
}
