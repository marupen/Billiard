using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public bool shoot = false;
    [Header("Set in Inspector")]
    public Image PowerBar;
    public Material material;
    public float m_MovePower = 5; // The force added to the ball to move it.
    public float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.

    private Vector3 move;
    // the world-relative desired move direction, calculated from the camForward and user input.
    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera
    private const float k_GroundRayLength = 0.7f; // The length of the ray to check if the ball is grounded.
    private Rigidbody m_Rigidbody;
    private float TimeAfterKeyDown = 0;
    private float DeltaTime = 0;
    private Ray ray = new Ray();
    private LineRenderer line = null;
    private RaycastHit hit; // Coordinates end of Trajectory line
        

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        // Set the maximum angular velocity.
        GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
        PowerBar.fillAmount = 0;
    }
    public void Move(Vector3 moveDirection)
    {
        // If using torque to rotate the ball...
        if ((Input.GetKeyDown(KeyCode.Space) || shoot)  && Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength))
        {
            TimeAfterKeyDown = Time.time;
        }
        else if ((Input.GetKeyUp(KeyCode.Space) || shoot)  && Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength))
        {
            DeltaTime = Time.time - TimeAfterKeyDown;
            if (DeltaTime > 1f) DeltaTime = 1;
            shoot = false;
            m_Rigidbody.velocity = moveDirection*m_MovePower*DeltaTime;
            DeltaTime = 0f;
            TimeAfterKeyDown = 0f;
            PowerBar.fillAmount = 0f;
        }

    }

    public void Shoot()
    {
        shoot = true;
    }

    private void Awake()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use world-relative controls in this case, which may not be what the user wants, but hey, we warned them!
        }
        // Create LineRenderer object and set their parameters
        line = new GameObject("TrajectoryLine").AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.enabled = false;
    }

    private void DrawRay()
    {
        ray.origin = transform.position;
        ray.direction = move;
        line.SetPosition(0, ray.origin);
        if(Physics.Raycast(ray, out hit))
        {
            line.SetPosition(1, hit.point);
            line.enabled = true;
        }
        else line.enabled = false;
    }

    private void Update()
    {
        // Get the axis and jump input.
        // calculate move direction
        if (cam != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = (1*camForward).normalized;
            DrawRay();
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = (1*Vector3.forward).normalized;
        }
        // Call the Move function of the ball controller
        Move(move);
        // Draw bar of shoot power
        if (TimeAfterKeyDown != 0)
        {
            DeltaTime = Time.time - TimeAfterKeyDown;
            PowerBar.fillAmount = DeltaTime;
        }
    }
 }
