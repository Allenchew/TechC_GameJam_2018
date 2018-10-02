using UnityEngine;
using UnityEditor;

public class ObjectRandomCreate : EditorWindow
{
    private static ObjectRandomCreate instance;
    private GameObject parent;
    private GameObject[] prefabs;
    private int prefabCount;
    private int currentPrefabCount;
    private int createCount;
    private float createY;
    private float rangeXMin;
    private float rangeXMax;
    private float rangeZMin;
    private float rangeZMax;
    private bool randomXRotation;
    private bool randomYRotation;
    private bool randomZRotation;


    [MenuItem("GameObject/Custom Create/Random Creates")]
    static void Init()
    {
        instance = GetWindow<ObjectRandomCreate>(true, "Random Creates");
    }


    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
    static void DrawGizmos(Transform aTarget, GizmoType gizmoType)
    {
        if(instance == null) { return; }
        //Sceneビューに範囲表示
        Vector3 originPosition = Vector3.zero;
        if (instance.parent) { originPosition = instance.parent.transform.position; }
        Vector3 leftDown = originPosition + new Vector3(instance.rangeXMin, 0, instance.rangeZMin);
        Vector3 leftTop = originPosition + new Vector3(instance.rangeXMin, 0, instance.rangeZMax);
        Vector3 rightDown = originPosition + new Vector3(instance.rangeXMax, 0, instance.rangeZMin);
        Vector3 rightTop = originPosition + new Vector3(instance.rangeXMax, 0, instance.rangeZMax);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(leftDown, leftTop);
        Gizmos.DrawLine(leftDown, rightDown);
        Gizmos.DrawLine(rightDown, rightTop);
        Gizmos.DrawLine(leftTop, rightTop);
    }

    private void OnEnable()
    {
        if(Selection.gameObjects.Length == 0) { return; }
        parent = Selection.gameObjects[0];
    }

    private void OnGUI()
    {
        parent = EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true) as GameObject;

        //プレハブの設定
        GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        prefabCount = EditorGUILayout.IntField("size",prefabCount);
        if(prefabCount <= 0) { prefabCount = 1; }
        if(prefabs == null)
        {
            currentPrefabCount = prefabCount;
            prefabs = new GameObject[prefabCount];
        }
        if(currentPrefabCount != prefabCount)
        {
            currentPrefabCount = prefabCount;
            GameObject[] copyPrefab = prefabs;
            int copyPrefabCount = copyPrefab.Length;
            prefabs = new GameObject[prefabCount];
            for(int i = 0; i < prefabCount; ++i)
            {
                if(i >= copyPrefabCount) { return; }
                prefabs[i] = copyPrefab[i];
            }
        }
        for(int i = 0; i < prefabs.Length; ++i)
        {
            prefabs[i] = EditorGUILayout.ObjectField("Prefab" + i, prefabs[i], typeof(GameObject), true) as GameObject;
        }

        //オブジェクト生成の設定
        GUILayout.Label("CreateObjectOption", EditorStyles.boldLabel);
        createCount = EditorGUILayout.IntField("Create Count", createCount);
        if(createCount < 0) { createCount = 0; }
        createY = EditorGUILayout.FloatField("Create Position Y", createY);
        rangeXMin = EditorGUILayout.FloatField("Create Range X Min", rangeXMin);
        rangeXMax = EditorGUILayout.FloatField("Create Range X Max", rangeXMax);
        rangeZMin = EditorGUILayout.FloatField("Create Range Z Min", rangeZMin);
        rangeZMax = EditorGUILayout.FloatField("Create Range Z Max", rangeZMax);
        randomXRotation = EditorGUILayout.Toggle("Random X Rotation", randomXRotation);
        randomYRotation = EditorGUILayout.Toggle("Random Y Rotation", randomYRotation);
        randomZRotation = EditorGUILayout.Toggle("Random Z Rotation", randomZRotation);

        //生成ボタン
        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Create")) { RandomCreate(); }
    }

    //オブジェクト生成
    private void RandomCreate()
    {
        for(int i = 0; i < createCount; ++i)
        {
            float randomX = Random.Range(rangeXMin, rangeXMax);
            float randomZ = Random.Range(rangeZMin, rangeZMax);
            int createPrefabIndex = Random.Range(0, prefabCount);
            float rotationX = 0;
            float rotationY = 0;
            float rotationZ = 0;
            if (randomXRotation == true)
            {
                rotationX = Random.Range(-180.0f,180.0f);
            }
            if(randomYRotation == true)
            {
                rotationY = Random.Range(-180.0f, 180.0f);
            }
            if (randomZRotation == true)
            {
                rotationZ = Random.Range(-180.0f, 180.0f);
            }
            Vector3 createPosition = new Vector3(randomX, createY, randomZ);
            GameObject createObject = Instantiate(prefabs[createPrefabIndex], createPosition, Quaternion.Euler(rotationX,rotationY,rotationZ));
            if(parent != null)
            {
                Vector3 parentPosition = parent.transform.position;
                createObject.transform.position += parentPosition;
                createObject.transform.SetParent(parent.transform);
            }
            Undo.RegisterCreatedObjectUndo(createObject, "Random Creates");
        }
    }
}
