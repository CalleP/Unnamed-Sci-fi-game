﻿using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class test : MonoBehaviour {
    //Did not make this class
    public bool render_mesh_normaly = true;
    public bool render_lines_1st = false;
    public bool render_lines_2nd = false;
    public bool render_lines_3rd = false;
    public Color lineColor = new Color(0.0f, 1.0f, 1.0f);
    public Color backgroundColor = new Color(0.0f, 0.5f, 0.5f);
    public bool ZWrite = true;
    public bool AWrite = true;
    public bool blend = true;
    public float lineWidth = 3;
    public int size = 0;

    private Vector3[] lines;
    private ArrayList lines_List;
    public Material lineMaterial;

    public static bool DrawUI = true;

    private Color originalLineColor;
    //private MeshRenderer meshRenderer; 

    /*
    ████████       ▄▀▀■  ▀▀█▀▀  ▄▀▀▄  █▀▀▄  ▀▀█▀▀ 
    ████████       ▀■■▄    █    █■■█  █▀▀▄    █   
    ████████       ■▄▄▀    █    █  █  █  █    █   
    */


    void Start()
    {
        originalLineColor = lineColor;
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (lineMaterial == null)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                        "SubShader { Pass {" +
                                        "   BindChannels { Bind \"Color\",color }" +
                                        "   Blend SrcAlpha OneMinusSrcAlpha" +
                                        "   ZWrite on Cull Off Fog { Mode Off }" +
                                        "} } }");

            /*lineMaterial = new Material ("Shader \"Lines/Colored Blended\" {" +
                                        "SubShader { Pass {" +
                                        "    Blend SrcAlpha OneMinusSrcAlpha" +
                                        "    ZWrite Off Cull Front Fog { Mode Off }" +
                                        "} } }");*/
        }
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        lines_List = new ArrayList();

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i + 2 < triangles.Length; i += 3)
        {
            lines_List.Add(vertices[triangles[i]]);
            lines_List.Add(vertices[triangles[i + 1]]);
            lines_List.Add(vertices[triangles[i + 2]]);
        }

        //lines_List.CopyTo(lines);//arrays are faster than array lists
        lines = (Vector3[])lines_List.ToArray(typeof(Vector3));
        lines_List.Clear();//free memory from the arraylist
        size = lines.Length;
    }

    // to simulate thickness, draw line as a quad scaled along the camera's vertical axis.
    void DrawQuad(Vector3 p1, Vector3 p2)
    {
        float thisWidth = lineWidth;
        //float thisWidth = 1.0f / Screen.width * lineWidth * 0.5f;
        Vector3 edge1 = Camera.main.transform.position - (p2 + p1) / 2.0f;    //vector from line center to camera
        Vector3 edge2 = p2 - p1;    //vector from point to point
        Vector3 perpendicular = Vector3.Cross(edge1, edge2).normalized * thisWidth;

        GL.Vertex(p1 - perpendicular);
        GL.Vertex(p1 + perpendicular);
        GL.Vertex(p2 + perpendicular);
        GL.Vertex(p2 - perpendicular);
    }

    Vector3 to_world(Vector3 vec)
    {
        return gameObject.transform.TransformPoint(vec);
    }

    /*
    ████████       █  █  █▀▀▄  █▀▀▄  ▄▀▀▄  ▀▀█▀▀  █▀▀▀ 
    ████████       █  █  █▀▀   █  █  █■■█    █    █■■  
    ████████       ▀▄▄▀  █     █▄▄▀  █  █    █    █▄▄▄ 
    */
    void Update()
    {
    }

    void OnRenderObject()
    {
        if (!DrawUI)
        {
            return;
        }

        gameObject.GetComponent<Renderer>().enabled = render_mesh_normaly;
        if (lines == null || lines.Length < lineWidth)
        {
            print("No lines");
        }
        else
        {
            lineMaterial.SetPass(0);
            

            if (lineWidth == 1)
            {
                GL.Begin(GL.LINES);
                GL.Color(lineColor);
                for (int i = 0; i + 2 < lines.Length; i += 3)
                {
                    Vector3 vec1 = to_world(lines[i]);
                    Vector3 vec2 = to_world(lines[i + 1]);
                    Vector3 vec3 = to_world(lines[i + 2]);
                    if (render_lines_1st)
                    {
                        GL.Vertex(vec1);
                        GL.Vertex(vec2);
                    }
                    if (render_lines_2nd)
                    {
                        GL.Vertex(vec2);
                        GL.Vertex(vec3);
                    }
                    if (render_lines_3rd)
                    {
                        GL.Vertex(vec3);
                        GL.Vertex(vec1);
                    }
                }
            }
            else
            {
                GL.Begin(GL.QUADS);
                GL.Color(lineColor);
                for (int i = 0; i + 2 < 18; i += 3)
                {
                    Vector3 vec1 = to_world(lines[i]);
                    Vector3 vec2 = to_world(lines[i + 1]);
                    Vector3 vec3 = to_world(lines[i + 2]);
                    if (render_lines_1st) DrawQuad(vec1, vec2);
                    if (render_lines_2nd) DrawQuad(vec2, vec3);
                    if (render_lines_3rd) DrawQuad(vec3, vec1);
                }
            }
            GL.End();
        }
    }

    private Coroutine CurrCoRoutine;

    private void clearCoRoutine()
    {
        if (CurrCoRoutine != null)
        {
            StopCoroutine(CurrCoRoutine);
        }
    }

    public void Fail()
    {
        clearCoRoutine();
        CurrCoRoutine = StartCoroutine(fail());
    }


    IEnumerator fail()
    {
        lineColor = Color.Lerp(lineColor, Color.red, 1.2f);
        yield return new WaitForSeconds(.2f);
        
        while (lineColor != originalLineColor)
        {
            lineColor = Color.Lerp(lineColor, originalLineColor, 0.4f);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Revert()
    {
        clearCoRoutine();
        CurrCoRoutine = StartCoroutine(RevertCoroutine());
    }

    private IEnumerator RevertCoroutine()
    {
        while (lineColor != originalLineColor)
        {
            lineColor = Color.Lerp(lineColor, originalLineColor, 0.09f);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void HighLight()
    {
        clearCoRoutine();
        CurrCoRoutine = StartCoroutine(HighLightCoroutine());
    }

    public IEnumerator HighLightCoroutine()
    {
        while (lineColor != Color.green)
        {
            lineColor = Color.Lerp(lineColor, Color.green, 0.4f);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Powered()
    {
        clearCoRoutine();
        CurrCoRoutine = StartCoroutine(PoweredCoroutine());
    }

    public IEnumerator PoweredCoroutine()
    {
        while (lineColor != Color.green)
        {
            lineColor = Color.Lerp(lineColor, Color.yellow, 0.4f);
            yield return new WaitForSeconds(.1f);
        }
    }


}
