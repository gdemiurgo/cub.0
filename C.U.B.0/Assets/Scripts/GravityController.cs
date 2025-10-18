using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject spawnPosCam;
    [SerializeField]
    private bool activated;
    [SerializeField] private GameObject gravityObj;

    GameObject sceneObj;
    GameObject sceneRotator;
    GameObject gravitySelectorObj;
    Transform target;
    Vector3 rotTarget;
    bool canRotate = true;
    bool rotating;
    bool hitCeiling;
    int rotUpDir;
    bool gravityMode;
    int gravityLayer;
    Camera camera;
    float camLerpTime;
    float iniFov;

    bool changeFov;
    bool fovDown;

    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        sceneObj = GameObject.FindGameObjectWithTag("SceneObj");
        sceneRotator = GameObject.FindGameObjectWithTag("SceneRotator");
        gravityLayer = LayerMask.GetMask("GravityLayer");
        target = GameObject.FindGameObjectWithTag("GameController").transform;
        gravitySelectorObj = GameObject.FindGameObjectWithTag("GravitySelector");
        gravitySelectorObj.SetActive(false);

        camera = spawnPosCam.GetComponent<Camera>();
        iniFov = camera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        // Control de "Cambio de gravedad"
        // Lanzamos un rayo en la direccion que mira el player y obtenemos la normal de la pared que toca. 
        // Seria la direccion que tendria el eje "Y" del player(hacia donde apuntaria la cabeza si se pusiese de pie en esa pared)

        if(activated && GameController.sharedGameController.GetStartCapsuleClosed())
        {
            RaycastHit gravityHit;
            if (Physics.Raycast(spawnPosCam.transform.position, spawnPosCam.transform.forward, out gravityHit, 100) && canRotate)
            {
                if(gravityHit.transform.tag == "GWall")
                {
                    if (Vector3.up != gravityHit.normal)
                    {
                        // Si pulsamos el boton derecho del raton habilitamos el modo "cambio de gravedad"
                        if (Input.GetKey(KeyCode.Mouse1))
                        {
                            rotTarget = gravityHit.normal;

                            float dirAngle = Vector3.Angle(transform.up, rotTarget);
                            if (dirAngle > 100)
                            {
                                float playerForwardAngle = Vector3.Angle(transform.forward, GameController.sharedGameController.transform.forward);

                                if(playerForwardAngle > 91)
                                {
                                    target.eulerAngles = new Vector3(0, 0, 0);
                                }
                                else
                                {
                                    target.eulerAngles = new Vector3(0, 360, 0);
                                }
                            }

                            SetRot();
                            canRotate = false;
                            changeFov = true;
                        }
                    }

                    // Habilitamos el selector de gravedad si apuntamos a una pared que permita cambiar la gravedad
                    gravitySelectorObj.SetActive(true);
                }
                else
                {
                    gravitySelectorObj.SetActive(false);
                }
            }
            else
            {
                gravitySelectorObj.SetActive(false);
            }
        }
        else
        {
            gravitySelectorObj.SetActive(false);
        }

        // Rotamos el objeto padre que hemos asignado al escenario hasta alinear su eje Y con el del player
        if (rotating)
        {
            Rotate();
        }

        if(changeFov)
        {
            ChangeFov();
        }
    }

    void ChangeFov()
    {
        if(!fovDown)
        {
            camLerpTime += 0.08f;

            if (camLerpTime >= 1)
            {
                fovDown = true;
            }
        }
        else
        {
            camLerpTime -= 0.08f;

            if (camLerpTime <= 0)
            {
                changeFov = false;
                fovDown = false;
            }
        }

        camera.fieldOfView = Mathf.Lerp(iniFov, iniFov + 2, camLerpTime);
        Time.timeScale = Mathf.Lerp(1, 0.8f, camLerpTime);
    }

    void Rotate()
    {
        //float step = speed * Time.deltaTime;

        // Rotamos
        //sceneRotator.transform.rotation = Quaternion.RotateTowards(sceneRotator.transform.rotation, target.rotation, step);

        //sceneRotator.transform.rotation = Quaternion.Slerp(sceneRotator.transform.rotation, target.rotation, 0.12f);

        /*float angle = Vector3.Angle(sceneRotator.transform.up, target.transform.up);
        if(angle < 0.1f)
        {
            sceneRotator.transform.up = target.transform.up;
        }

        if (sceneRotator.transform.up == target.transform.up)
        {
            ResetRot();
            rotating = false;
            Debug.Log("MISMA ROTACION");
        }*/

        //NEW WAY
        gravityObj.transform.rotation = Quaternion.Slerp(gravityObj.transform.rotation, target.rotation, 0.12f);

        Physics.gravity = gravityObj.transform.eulerAngles;

        float angle = Vector3.Angle(gravityObj.transform.up, target.transform.up);
        if (angle < 0.1f)
        {
            gravityObj.transform.up = target.transform.up;
        }

        if (gravityObj.transform.up == target.transform.up)
        {
            rotating = false;
        }
    }

    void SetRot()
    {
        // Colocamos un objeto vacio en la posicion del player
        // Lo rotamos alineando su eje "Y" con el de la normal de la pared obtenida antes
        // Hacemos que ese objeto sea padre del escenario para poder rotarlo en la direccion deseada y usando como pivote
        // de rotacion la posicion del player, asi parece que el que gira es en realidad el jugador

        /*sceneRotator.transform.position = transform.position;
        sceneRotator.transform.rotation = Quaternion.FromToRotation(Vector3.up, rotTarget);
        sceneObj.transform.SetParent(sceneRotator.transform);*/

        //NEW WAY
        rotating = true;
    }

    void ResetRot()
    {
        // Hacemos que el objeto que rota ya no sea padre del escenario
        sceneObj.transform.parent = null;
        canRotate = true;
    }

    public void SetActivated(bool activated)
    {
        this.activated = activated;
    }
}
