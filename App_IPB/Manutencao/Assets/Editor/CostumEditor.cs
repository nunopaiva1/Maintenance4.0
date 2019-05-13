using UnityEditor;
using UnityEngine;
using System.Collections;

public class CostumEditor : MonoBehaviour
{

    public GameObject content;
    [MenuItem("GameObject/MyCategory/Custom Game Object", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        GameObject instance = Instantiate(Resources.Load("Button", typeof(GameObject))) as GameObject;

        // Create a custom game object
        //GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(instance, "Create " + instance.name);
        Selection.activeObject = instance;
    }


}
