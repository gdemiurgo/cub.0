using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

[InitializeOnLoad]
public static class SelectParentCollapseHierarchy
{
    private static bool selectionModeActive = false;
    private static List<GameObject> pendingParentSelection = new List<GameObject>();

    static SelectParentCollapseHierarchy()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // --- ACTIVAR / DESACTIVAR CON SHIFT + X ---
        if (e.type == EventType.KeyDown && e.shift && e.keyCode == KeyCode.X)
        {
            selectionModeActive = !selectionModeActive;
            SceneView.RepaintAll();

            Debug.Log(selectionModeActive
                ? "🔹 Modo seleccionar padre ACTIVADO (clic o Ctrl+clic para selección múltiple)"
                : "🔸 Modo seleccionar padre DESACTIVADO");

            e.Use();
        }

        // --- SALIR CON ESC ---
        if (selectionModeActive && e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            selectionModeActive = false;
            Debug.Log("🔸 Modo seleccionar padre DESACTIVADO");
            e.Use();
        }

        // --- CLIC IZQUIERDO (con o sin CTRL) ---
        if (selectionModeActive && e.type == EventType.MouseDown && e.button == 0)
        {
            GameObject picked = HandleUtility.PickGameObject(e.mousePosition, true);

            if (picked != null && picked.transform.parent != null)
            {
                GameObject parent = picked.transform.parent.gameObject;

                if (e.control)
                {
                    // Selección múltiple: añadir o quitar de la selección actual
                    var current = new List<GameObject>(Selection.gameObjects);

                    if (current.Contains(parent))
                        current.Remove(parent);
                    else
                        current.Add(parent);

                    pendingParentSelection = current;
                }
                else
                {
                    // Selección única
                    pendingParentSelection = new List<GameObject> { parent };
                }

                e.Use();
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;
            }
        }

        // --- AVISO VISUAL ---
        if (selectionModeActive)
        {
            Handles.BeginGUI();
            GUI.color = new Color(1f, 0.9f, 0.3f, 0.9f);
            GUILayout.BeginArea(new Rect(10, 10, 420, 30), EditorStyles.helpBox);
            GUILayout.Label("Modo seleccionar padre activo  (Shift+X o ESC para salir)  |  Ctrl+Clic = múltiple");
            GUILayout.EndArea();
            Handles.EndGUI();
        }
    }

    private static void OnEditorUpdate()
    {
        if (pendingParentSelection.Count > 0)
        {
            Selection.objects = pendingParentSelection.ToArray();
            CollapseAllInHierarchy();
            Debug.Log($"👆 Seleccionados {pendingParentSelection.Count} objeto(s) padre(s)");
            pendingParentSelection.Clear();
        }
    }

    private static void CollapseAllInHierarchy()
    {
        var hierarchy = GetHierarchyWindow();
        if (hierarchy == null) return;

        MethodInfo setExpandedRecursive = hierarchy.GetType()
            .GetMethod("SetExpandedRecursive", BindingFlags.Instance | BindingFlags.NonPublic);

        if (setExpandedRecursive == null) return;

        foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
        {
            setExpandedRecursive.Invoke(hierarchy, new object[] { go.GetInstanceID(), false });
        }
    }

    private static EditorWindow GetHierarchyWindow()
    {
        return EditorWindow.GetWindow(
            typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow"));
    }
}
