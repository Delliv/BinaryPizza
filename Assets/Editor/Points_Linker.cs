using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Points_Linker
{
    //Undo
    //https://youtu.be/qfGzwy_wSlw?list=RDCMUC7M-Wz4zK8oikt6ATcoTwBA&t=1342
    //https://docs.unity3d.com/ScriptReference/Undo.html


    /*[MenuItem("Roads/Linker %g")]
    public static void LinkerElements()
    {
        GameObject[] roads_array = Selection.gameObjects;

        if (roads_array.Length == 2)
        {
            if (roads_array[0].CompareTag("road") && roads_array[1].CompareTag("road"))
            {
                List<GameObject> exit_point = new List<GameObject>();
                List<GameObject> enter_point = new List<GameObject>();

                for (int a = 0; a < 2; a++)
                {
                    for (int i = 0; i < roads_array[a].transform.childCount; i++)
                    { //Recorremos la carretera numero 1 en busqueda de sus salidas
                        if (roads_array[a].transform.GetChild(i).name == "Exit")
                        {
                            exit_point.Add(roads_array[a].transform.GetChild(i).gameObject);
                        }
                    }
                    for (int i = 0; i < roads_array[(a+1)%2].transform.childCount; i++)
                    { //Recorremos la carretera numero 2 en busqueda de sus entradas
                        if (roads_array[(a + 1) % 2].transform.GetChild(i).name == "Enter")
                        {
                            enter_point.Add(roads_array[(a + 1) % 2].transform.GetChild(i).gameObject);
                        }
                    }

                    float distance_between_points = 999;
                    GameObject exit_good = null, enter_good = null;
                    for (int i = 0; i < exit_point.Count; i++)
                    {
                        for (int j = 0; j < enter_point.Count; j++)
                        {
                            if (Vector3.Distance(exit_point[i].transform.position, enter_point[j].transform.position) < distance_between_points)
                            {
                                distance_between_points = Vector3.Distance(exit_point[i].transform.position, enter_point[j].transform.position);
                                exit_good = exit_point[i].gameObject;
                                enter_good = enter_point[j].gameObject;
                            }
                        }
                    }

                    if (exit_good != null && enter_good != null)
                    {
                        if (exit_good.GetComponent<path_point>().next_points.Count >= 1)
                        {
                            for (int i = 0; i < exit_good.GetComponent<path_point>().next_points.Count; i++)
                            {
                                if (exit_good.GetComponent<path_point>().next_points[i].GetInstanceID() != enter_good.GetInstanceID())
                                {
                                    exit_good.GetComponent<path_point>().next_points.Add(enter_good);
                                    enter_point.Clear();
                                    exit_point.Clear();
                                }
                                else
                                {
                                    Debug.Log("La connexion ya existe.");
                                }
                            }
                        }
                        else
                        {
                            exit_good.GetComponent<path_point>().next_points.Add(enter_good);
                            enter_point.Clear();
                            exit_point.Clear();
                        }
                    }
                }
                Debug.Log("Linkado de carreteras correcto.");
            }
        }
        else
        {
            Debug.Log("Necesitas seleccionar 2 carreteras.");
        }
    }*/

    /*[MenuItem("Roads/Linker %g")]
    public static void LinkerElements()
    {
        GameObject[] roads_array = Selection.gameObjects;

        if (roads_array.Length == 2)
        {
            if (roads_array[0].CompareTag("road") && roads_array[1].CompareTag("road"))
            {
                List<int> exit_point = new List<int>();
                List<int> enter_point = new List<int>();

                for (int a = 0; a < 2; a++)
                {
                    for (int i = 0; i < roads_array[a].transform.childCount; i++)
                    { //Recorremos la carretera numero 1 en busqueda de sus salidas
                        if (roads_array[a].transform.GetChild(i).name == "Exit")
                        {
                            exit_point.Add(i);
                        }
                    }
                    for (int i = 0; i < roads_array[(a + 1) % 2].transform.childCount; i++)
                    { //Recorremos la carretera numero 2 en busqueda de sus entradas
                        if (roads_array[(a + 1) % 2].transform.GetChild(i).name == "Enter")
                        {
                            enter_point.Add(i);
                        }
                    }


                    float distance_between_points = 999;
                    int exit_good = -1, enter_good = -1;
                    for (int i = 0; i < exit_point.Count; i++)
                    {
                        for (int j = 0; j < enter_point.Count; j++)
                        {
                            if (Vector3.Distance(roads_array[a].transform.GetChild(exit_point[i]).transform.position, roads_array[(a + 1) % 2].transform.GetChild(enter_point[j]).transform.position) < distance_between_points)
                            {
                                distance_between_points = Vector3.Distance(roads_array[a].transform.GetChild(exit_point[i]).transform.position, roads_array[(a + 1) % 2].transform.GetChild(enter_point[j]).transform.position);
                                exit_good = i;
                                enter_good = j;
                            }
                        }
                    }


                    if (exit_good != -1 && enter_good != -1)
                    {
                        if (roads_array[a].transform.GetChild(exit_point[exit_good]).GetComponent<path_point>().next_points.Length > 1)
                        {
                            Debug.Log("La connexion ya existe.");
                        }
                        else
                        {
                            roads_array[a].transform.GetChild(exit_point[exit_good]).GetComponent<path_point>().next_points[0] = roads_array[(a + 1) % 2].transform.GetChild(enter_point[enter_good]).gameObject;
                            Debug.Log("Linkado de carreteras correcto.");

                            var so = new SerializedObject(roads_array[a].transform.GetChild(exit_point[exit_good]).GetComponent<path_point>());
                            so.FindProperty("next_points").GetArrayElementAtIndex(0).objectReferenceValue = roads_array[(a + 1) % 2].transform.GetChild(enter_point[enter_good]).gameObject;
                            so.Update();

                            enter_point.Clear();
                            exit_point.Clear();
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Necesitas seleccionar 2 carreteras.");
        }
    }*/

    [MenuItem("Roads/Linker %g")]
    public static void LinkerElements()
    {
        GameObject[] roads_array = Selection.gameObjects;

        if (roads_array.Length == 2)
        {
            roads_array[0].GetComponent<path_point>().next_points[0] = roads_array[1];

            var so = new SerializedObject(roads_array[0].GetComponent<path_point>());
            so.FindProperty("next_points").GetArrayElementAtIndex(0).objectReferenceValue = roads_array[1];
            so.Update();
        }
    }
}
