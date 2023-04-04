using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ProBuilder;
using System.Linq;


[System.Serializable]
public class DrawData : MonoBehaviour
{
    [System.Serializable]
    public struct drawDataStruct
    {
        public VariablesData variablesData;
    }
    public drawDataStruct drawVariables;

    [System.Serializable]
    public class VariablesData
    {
        public Vector3 mouseCurrentPosition, startClickPosition, endClickPosition;
        public Vector3[] allPoints;
        [SerializeField]
        public LineRenderer mouseLineRenderer;
        public bool resetIsDone, whiteBoardButtonIsClicked = false;
        public List<Vector2> linePoints = new List<Vector2>();
        public Mesh createdShapeMesh;
        [SerializeField]
        public GameObject carDraw, firstWheel, secondWheel;
        [SerializeField]
        public Camera usedCameraForUI;
        [SerializeField]
        public Button whiteBoardButton;
        //public ProBuilderMesh meshProBuilder;
        public CombineInstance[] combineMesh;
        //public List<BezierPoint> points = new List<BezierPoint>();
    }
}

