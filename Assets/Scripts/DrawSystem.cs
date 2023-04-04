using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ProBuilder;
using System.Linq;


public class DrawSystem : MonoBehaviour
{
    private DrawData drawData;
    [SerializeField]
    private GameObject Road;
    private int i = 0;
    public Transform allCylinders;

    private void Start()
    {
        drawData = GetComponent<DrawData>();
        drawData.drawVariables.variablesData.createdShapeMesh = new Mesh();
    }

    private bool drawHasStarted()
    {
        var click = Input.GetMouseButton(0);
        if (click == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Road.gameObject.GetComponent<Transform>().position.z;
            Vector3 pos = drawData.drawVariables.variablesData.usedCameraForUI.ScreenToWorldPoint(mousePos);
            Vector3 differenceBetweenDrawAndCar = new Vector3();
            differenceBetweenDrawAndCar = drawData.drawVariables.variablesData.carDraw.transform.position - pos;
            drawData.drawVariables.variablesData.startClickPosition = pos;
            return true;
        }
        else return false;
    }

    private bool drawHasFinished()
    {
        var click = Input.GetMouseButtonUp(0);
        if (click == true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Road.gameObject.GetComponent<Transform>().position.z;
            Vector3 pos = drawData.drawVariables.variablesData.usedCameraForUI.ScreenToWorldPoint(mousePos);
            Vector3 differenceBetweenDrawAndCar = new Vector3();
            differenceBetweenDrawAndCar = drawData.drawVariables.variablesData.carDraw.transform.position - pos;
            // Difference always in positive
            pos.y += Mathf.Abs(differenceBetweenDrawAndCar.y);
            drawData.drawVariables.variablesData.endClickPosition = pos;
            return true;
        }
        else return false;
    }

    public void createTheShapeAndCombine()
    {
        drawData.drawVariables.variablesData.combineMesh = new CombineInstance[drawData.drawVariables.variablesData.allPoints.Length];
        for (int i = 0; i < drawData.drawVariables.variablesData.allPoints.Length; i++)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.position = drawData.drawVariables.variablesData.allPoints[i];
            cylinder.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);

            if (i == drawData.drawVariables.variablesData.allPoints.Length - 1)
            {
                cylinder.transform.localScale = new Vector3(1, drawData.drawVariables.variablesData.allPoints[i].x - drawData.drawVariables.variablesData.allPoints[i - 1].x, 1);
                if (drawData.drawVariables.variablesData.allPoints[i - 1].y > drawData.drawVariables.variablesData.allPoints[i].y)
                {
                    //cylinder.transform.rotation = Quaternion.AngleAxis(92.5f, Vector3.forward);
                }
                else
                {
                    //cylinder.transform.rotation = Quaternion.AngleAxis(87.5f, Vector3.forward);
                }
            }
            else
            {
                if (drawData.drawVariables.variablesData.allPoints[i + 1].y > drawData.drawVariables.variablesData.allPoints[i].y)
                {
                   // cylinder.transform.rotation = Quaternion.AngleAxis(92.5f, Vector3.forward);
                }
                else
                {
                  //  cylinder.transform.rotation = Quaternion.AngleAxis(87.5f, Vector3.forward);
                }
                cylinder.transform.localScale = new Vector3(1, drawData.drawVariables.variablesData.allPoints[i + 1].x - drawData.drawVariables.variablesData.allPoints[i].x, 1);

        
            }

            //cylinder.gameObject.GetComponent<MeshFilter>().transform.SetParent(allCylinders.transform);
            // COMB
            drawData.drawVariables.variablesData.combineMesh[i].mesh = cylinder.gameObject.GetComponent<MeshFilter>().sharedMesh;

            drawData.drawVariables.variablesData.combineMesh[i].transform = transform.worldToLocalMatrix * cylinder.gameObject.GetComponent<MeshFilter>().transform.localToWorldMatrix;

            //DELETE THE INSTANTIATED CYLINDER
            Destroy(cylinder);
        }
        assignNewMeshToCar();
        addWheelsToCar();
    }

    private void assignNewMeshToCar()
    {
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh.Clear();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh = new Mesh();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh.CombineMeshes(drawData.drawVariables.variablesData.combineMesh, true, true, true);
        Instantiate(drawData.drawVariables.variablesData.carDraw, allCylinders, true) ;
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh.Optimize();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh.MarkDynamic();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<Rigidbody>().ResetCenterOfMass();
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshCollider>().sharedMesh = drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh;
        drawData.drawVariables.variablesData.carDraw.gameObject.transform.position = new Vector3(drawData.drawVariables.variablesData.carDraw.gameObject.transform.position.x, 5, 0);
    }

    private void addWheelsToCar()
    {
        Mesh mesh = new Mesh();
        mesh = drawData.drawVariables.variablesData.carDraw.gameObject.GetComponent<MeshFilter>().mesh;
        //GameObject drawData.drawVariables.variablesData.firstWheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        drawData.drawVariables.variablesData.firstWheel.gameObject.transform.position = new Vector3(mesh.vertices[0].x, mesh.vertices[0].y+2, mesh.vertices[0].z);
        //drawData.drawVariables.variablesData.firstWheel.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
        //drawData.drawVariables.variablesData.firstWheel.transform.localScale = new Vector3(1f, 0.2f, 1f);
        //drawData.drawVariables.variablesData.firstWheel.transform.SetParent(drawData.drawVariables.variablesData.carDraw.gameObject.transform);

        //GameObject drawData.drawVariables.variablesData.secondWheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        drawData.drawVariables.variablesData.secondWheel.gameObject.transform.position = new Vector3(mesh.vertices[mesh.vertexCount - 1].x, mesh.vertices[mesh.vertexCount - 1].y+2, mesh.vertices[mesh.vertexCount - 1].z);
        //drawData.drawVariables.variablesData.secondWheel.transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
        //drawData.drawVariables.variablesData.secondWheel.transform.localScale = new Vector3(1f, 0.2f, 1f);
        //drawData.drawVariables.variablesData.secondWheel.transform.SetParent(drawData.drawVariables.variablesData.carDraw.transform);
    }

    public void DrawUpdate()
    {
            if (drawHasStarted() && !drawHasFinished())
            {
                // For every frame update the position of the new point
                if (!drawData.drawVariables.variablesData.resetIsDone)
                {
                    resetLinePoints();
                    drawData.drawVariables.variablesData.resetIsDone = true;
                    drawData.drawVariables.variablesData.allPoints = null;
                }
                updateMousePosition();
            }
            else if (!drawHasStarted() && drawHasFinished())
            {
                // Delete the old line
                //updateColliderPoints();
                storeShapeIntoMesh();
                //instantiateStoredShape();
                resetLinePoints();
                drawData.drawVariables.variablesData.resetIsDone = false;
                createTheShapeAndCombine();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                instantiateStoredShape();
            }
        //drawData.drawVariables.variablesData.drawData.drawVariables.variablesData.whiteBoardButtonIsClicked = false;
    }

    private void instantiateStoredShape()
    {
        //Instantiate(drawData.drawVariables.variablesData.createdShapeMesh);

        /*
        int verticesNumber = drawData.drawVariables.variablesData.createdShapeMesh.vertexCount;
        int trianglesNumber = drawData.drawVariables.variablesData.createdShapeMesh.triangles.Length;
        Vector3[] vertices = new Vector3[verticesNumber];
        int[] triangles = new int[trianglesNumber];
        vertices=drawData.drawVariables.variablesData.createdShapeMesh.vertices;
        triangles = drawData.drawVariables.variablesData.createdShapeMesh.triangles;


        Mesh newMesh = new Mesh();
        newMesh = drawData.drawVariables.variablesData.carDraw.GetComponent<MeshFilter>().mesh;
        drawData.drawVariables.variablesData.carDraw.GetComponent<MeshFilter>().mesh.Clear();
        for (int i = 0; i < vertices.Length; i++)
        {
            drawData.drawVariables.variablesData.carDraw.GetComponent<MeshFilter>().mesh.vertices[i] = drawData.drawVariables.variablesData.createdShapeMesh.vertices[i];
        }
        for (int i = 0; i < trianglesNumber; i++)
        {
            newMesh.triangles[i] = triangles[i];
        }
        */
        giveDepthToMesh();
        drawData.drawVariables.variablesData.carDraw.GetComponent<MeshFilter>().sharedMesh = drawData.drawVariables.variablesData.createdShapeMesh;
        drawData.drawVariables.variablesData.carDraw.GetComponent<MeshCollider>().sharedMesh = drawData.drawVariables.variablesData.createdShapeMesh;
        drawData.drawVariables.variablesData.carDraw.GetComponent<MeshCollider>().convex = true;
    }

    private void storeShapeIntoMesh()
    {
        drawData.drawVariables.variablesData.allPoints = new Vector3[drawData.drawVariables.variablesData.mouseLineRenderer.positionCount];
        drawData.drawVariables.variablesData.mouseLineRenderer.GetPositions(drawData.drawVariables.variablesData.allPoints);
        Vector3[] lastPositions = new Vector3[drawData.drawVariables.variablesData.mouseLineRenderer.positionCount];
        Vector3 differenceBetweenDrawAndCar = new Vector3();
        differenceBetweenDrawAndCar = drawData.drawVariables.variablesData.carDraw.transform.position - drawData.drawVariables.variablesData.mouseLineRenderer.GetPosition(0);
        drawData.drawVariables.variablesData.mouseLineRenderer.GetPositions(lastPositions);
        for(int i=0;i< drawData.drawVariables.variablesData.mouseLineRenderer.positionCount;i++)
        {
            lastPositions[i].y += differenceBetweenDrawAndCar.y;
        }
        drawData.drawVariables.variablesData.mouseLineRenderer.SetPositions(lastPositions);
        drawData.drawVariables.variablesData.mouseLineRenderer.Simplify(0.1f);
        drawData.drawVariables.variablesData.mouseLineRenderer.BakeMesh(drawData.drawVariables.variablesData.createdShapeMesh,Camera.main,false);
        drawData.drawVariables.variablesData.createdShapeMesh.Optimize();
        drawData.drawVariables.variablesData.createdShapeMesh.RecalculateBounds();
        //this.gameObject.GetComponent<MeshCollider>().sharedMesh = drawData.drawVariables.variablesData.createdShapeMesh;
    }

    private void giveDepthToMesh()
    {
        Vector3[] oldVertices = drawData.drawVariables.variablesData.createdShapeMesh.vertices;
        Vector3[] newVertices = drawData.drawVariables.variablesData.createdShapeMesh.vertices;
        int verticesNumber = drawData.drawVariables.variablesData.createdShapeMesh.vertexCount;
        for (int i=0;i< newVertices.Length;i++)
        {
            newVertices[i].z += 5;
        }
        drawData.drawVariables.variablesData.createdShapeMesh.vertices = oldVertices.Concat(newVertices).ToArray();
        //drawData.drawVariables.variablesData.createdShapeMesh.vertices
    }

    public void addPoints(Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            //Vector3 tan = new Vector3(0f, 0f, 2f);
            //points.Add(new BezierPoint(new Vector3(vertices., vertices.y, 0f), -tan, tan, Quaternion.identity));
        }
    }

    private void resetLinePoints()
    {
        drawData.drawVariables.variablesData.mouseLineRenderer.positionCount = 0;
        i = 0;
        //lineEdgeCollider.points
    }

    private void updateMousePosition()
    {  
        //drawData.drawVariables.variablesData.mouseCurrentPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        drawData.drawVariables.variablesData.mouseLineRenderer.positionCount = i+1;
        drawData.drawVariables.variablesData.mouseLineRenderer.SetPosition(i, drawData.drawVariables.variablesData.startClickPosition);
        //drawData.drawVariables.variablesData.linePoints.Add(drawData.drawVariables.variablesData.mouseCurrentPosition);
        i++;
        // Must check if the position is the same to not create same position
    }

    private void updateColliderPoints()
    {
        //lineEdgeCollider.points = drawData.drawVariables.variablesData.linePoints.ToArray();
        /*
        int i = 2;
        foreach (var points in drawData.drawVariables.variablesData.linePoints)
        {          
            lineEdgeCollider.points.SetValue(points,i);
            i++;
        }
      */
    }




}
