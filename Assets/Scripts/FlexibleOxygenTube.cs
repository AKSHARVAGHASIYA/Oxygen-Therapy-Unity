
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FlexibleOxygenTube : MonoBehaviour
{
    [Header("Tube Connection Points")]
    public Transform regulatorEnd;
    public Transform cannulaEnd;

    [Header("Tube Appearance")]
    [Min(0.001f)] public float tubeWidth = 0.012f;
    [Min(2)] public int segments = 24;
    [Min(0f)] public float sag = 0.12f;

    private LineRenderer line;

    private void Awake()
    {
        SetupLine();
    }

    private void OnEnable()
    {
        SetupLine();
        UpdateTube();
    }

    private void OnValidate()
    {
        SetupLine();
        UpdateTube();
    }

    private void LateUpdate()
    {
        UpdateTube();
    }

    private void SetupLine()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        if (line == null)
            return;

        line.useWorldSpace = true;
        line.positionCount = Mathf.Max(2, segments);
        line.startWidth = tubeWidth;
        line.endWidth = tubeWidth;
    }

    private void UpdateTube()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        if (line == null || regulatorEnd == null || cannulaEnd == null)
            return;

        int count = Mathf.Max(2, segments);
        line.positionCount = count;

        Vector3 start = regulatorEnd.position;
        Vector3 end = cannulaEnd.position;

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            Vector3 point = Vector3.Lerp(start, end, t);

            // Keep the middle of the tube hanging slightly downward.
            point.y -= sag * 4f * t * (1f - t);

            line.SetPosition(i, point);
        }
    }
}
