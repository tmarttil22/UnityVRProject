using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{

    private Animator _animator;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;
    private SkinnedMeshRenderer _mesh;
    private const string animatorGripParam = "Grip";
    private const string animatorTriggerParam = "Trigger";
    public float speed;
    private static readonly int Grip = Animator.StringToHash(animatorGripParam);
    private static readonly int Trigger = Animator.StringToHash(animatorTriggerParam);

    [SerializeField] private GameObject followObject;
    [SerializeField] private float followSpeed = 200f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    private Transform _followTarget;
    private Rigidbody _body;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        
        _followTarget = followObject.transform;
        _body = GetComponent<Rigidbody>();
        _body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _body.interpolation = RigidbodyInterpolation.Interpolate;
        _body.mass = 20f;

        _body.position = _followTarget.position;
        _body.rotation = _followTarget.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    void FixedUpdate()
    {
        PhysicsMove();
    }

    private void PhysicsMove() {
        if (_followTarget == null) return;

        // Position correction
        Vector3 targetPosition = _followTarget.position + _followTarget.rotation * positionOffset;
        Vector3 positionDelta = targetPosition - _body.position;
        Vector3 velocityTarget = positionDelta * followSpeed;

        _body.linearVelocity = Vector3.Lerp(_body.linearVelocity, velocityTarget, Time.fixedDeltaTime * followSpeed);

        // Rotation correction
        Quaternion targetRotation = _followTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion deltaRotation = targetRotation * Quaternion.Inverse(_body.rotation);
    
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
    
        if (Mathf.Abs(angle) > 0.01f) {
            Vector3 angularVelocityTarget = axis * (angle * Mathf.Deg2Rad * rotateSpeed);
            _body.angularVelocity = Vector3.Lerp(_body.angularVelocity, angularVelocityTarget, Time.fixedDeltaTime * rotateSpeed);
        }
    }

    public void SetGrip(float v) {
        gripTarget = v;
    }

    public void SetTrigger(float v) {
        triggerTarget = v;
    }

    void AnimateHand() {
        if (gripCurrent != gripTarget) {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            _animator.SetFloat(animatorGripParam, gripCurrent);
        }
        if (triggerCurrent != triggerTarget) {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            _animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
        _animator.Update(Time.deltaTime);
    }
}
