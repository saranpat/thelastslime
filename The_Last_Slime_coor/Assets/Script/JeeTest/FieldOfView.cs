﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    public float viewRadius = 1.5f;
    [Range(0, 360)]
    public float viewAngle = 45;

    public LayerMask targetMask;
    public LayerMask Wizard_targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [HideInInspector]
    public float AddDebugViewAngle = 0.135f;

    private float meshResolution = 1;
    private int edgeResolveIterations = 4;
    private float edgeDstThreshold = 0.5f;

    public MeshFilter viewMeshFilter;
    public Material[] _Material;
    Mesh viewMesh;
    MeshRenderer viewMeshRenderer;

    private AI_Move _AI_Move;
    private WardScript _WardScript;
    private bool IsWizardAI;


	// Use this for initialization
	void Start () {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        viewMeshRenderer = viewMeshFilter.gameObject.GetComponent<MeshRenderer>();

        if(this.gameObject.GetComponent<AI_Move>() != null)
        {
            _AI_Move = this.gameObject.GetComponent<AI_Move>();
            IsWizardAI = _AI_Move.Wizard_AI;
        }
        else
        {
            IsWizardAI = false;
        }
        if (this.gameObject.GetComponent<WardScript>() != null)
        {
            _WardScript = this.gameObject.GetComponent<WardScript>();
        }
        StartCoroutine("FindTargetsWithDelay", .02f);
	}

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    private Collider2D[] targetsInViewRadius;
    //ค้นหาเป้าหมายเมื่อเจอจะเก็บเป็น List ของ visibleTargets
    void FindVisibleTargets()
    {
        visibleTargets.Clear();

        if (IsWizardAI == true)
        {
            targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, Wizard_targetMask);
        }
        else
        {
            targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
        }

        
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Debug.Log(targetsInViewRadius[i].gameObject.name + " stay in Radius");
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 1.75f)// /2
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    Debug.Log(target.gameObject.name);
                }
            }
        }



        // เปลี่ยนสีเมื่อเจอผยู้เล่น
        if (_AI_Move != null)
            if (_AI_Move.isDetect)//(visibleTargets.Count > 0)
            {
                viewMeshRenderer.material = _Material[1];
            }
            else if (_AI_Move.alertState)//(visibleTargets.Count > 0)
            {
                viewMeshRenderer.material = _Material[2];
            }
            else
            {
                viewMeshRenderer.material = _Material[0];
            }
        if (_WardScript != null)
            if (_WardScript.Get_isAlarm())//(visibleTargets.Count > 0)
            {
                viewMeshRenderer.material = _Material[1];
            }
            else
            {
                viewMeshRenderer.material = _Material[0];
            }

    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(-Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius + AddDebugViewAngle, obstacleMask);

        if (Physics2D.Raycast(transform.position, dir, viewRadius + AddDebugViewAngle, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * (viewRadius + AddDebugViewAngle), viewRadius + AddDebugViewAngle, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
