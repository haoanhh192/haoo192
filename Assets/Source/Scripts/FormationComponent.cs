using System.Collections.Generic;
using UnityEngine;

public class FormationComponent : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> FormationPoints = new();

    /// <summary>
    /// Creates new formation in the Formation Component root with one obligatory point 
    /// in the center and additional points around it on certain radius from it
    /// </summary>
    /// <param name="originalPoint"> Position for the first point </param>
    /// <param name="radius"> Radius for additional points </param>
    /// <param name="additionalPointsCount"> Amount of additional points </param>
    public void RecreateFormation(Vector3 originalPoint, float radius = 1f, int additionalPointsCount = 0)
    {
        // Removing old points
        foreach (var point in FormationPoints)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
        }

        FormationPoints.Clear();

        originalPoint.y = 0;

        // Creating obligatory center point
        GameObject firstPoint = new();
        firstPoint.transform.position = originalPoint;
        firstPoint.transform.SetParent(transform);
        FormationPoints.Add(firstPoint.transform);

        // Creating formation
        for (int i = 0; i < additionalPointsCount; i++)
        {
            float angle = i * Mathf.PI * 2f / additionalPointsCount;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            GameObject go = new();
            go.transform.SetParent(transform);
            go.transform.localPosition = newPos;

            FormationPoints.Add(go.transform);
        }
    }
}