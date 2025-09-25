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
        foreach (var point in FormationPoints)
        {
            if (point != null)
            {
                Destroy(point.gameObject);
            }
        }

        FormationPoints.Clear();

        GameObject firstPoint = new();
        firstPoint.transform.position = originalPoint;
        firstPoint.transform.SetParent(transform);
        FormationPoints.Add(firstPoint.transform);

        for (int i = 0; i < additionalPointsCount; i++)
        {
            float angle = i * Mathf.PI * 2f / additionalPointsCount;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, originalPoint.y, Mathf.Sin(angle) * radius);
            GameObject go = new();
            go.transform.position = originalPoint + newPos;
            go.transform.SetParent(transform);

            FormationPoints.Add(go.transform);
        }
    }
}