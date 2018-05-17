using UnityEngine;

public class WaypointProgressTracker : MonoBehaviour
{

    // This script can be used with any object that is supposed to follow a
    // route marked out by waypoints.
    //该脚本能够用于可以遵循一个路径被航点的任何对象

    // This script manages the amount to look ahead along the route,
    // and keeps track of progress and laps.
    //这个脚本管理着沿着路由向前看的数量，并跟踪进步和圈。
    [HideInInspector]
    public WaypointCircuit Circuit
    {
        get
        {
            return circuit;
        }
        set
        {
            circuit = value;
        }
    }

    [SerializeField]
    WaypointCircuit circuit;         // A reference to the waypoint-based route we should follow 参照点的路线应遵循

    [SerializeField]
    float lookAheadForTargetOffset = 5;     // The offset ahead along the route that the we will aim for 我们将瞄准的路线向前偏移
    [SerializeField]
    float lookAheadForTargetFactor = .1f;      // A multiplier adding distance ahead along the route to aim for, based on current speed 以当前速度为目标的路由路径上的一个乘法器
    [SerializeField]
    float lookAheadForSpeedOffset = 10;     // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)前方只有速度调整路线偏移（作为指定目标的旋转变换）
    [SerializeField]
    float lookAheadForSpeedFactor = .2f;        // A multiplier adding distance ahead along the route for speed adjustments 一种沿速度调整路径前进的乘法器

    [SerializeField]
    ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute; // whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.
                                                                  //是否更新位置顺利沿线（弯曲的路径，好）或只是当我们到达每个航路点。
    [SerializeField]
    float pointToPointThreshold = 4;  // proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
                                      //靠近点必须达到目标，下一个关键点：开关只用于点对点模式。

    public enum ProgressStyle
    {
        SmoothAlongRoute,
        PointToPoint,
    }

    // these are public, readable by other objects - i.e. for an AI to know where to head!
    //这些都是公开的，可读的其他对象，-即为一个人工智能知道提供开头
    public WaypointCircuit.RoutePoint targetPoint { get; private set; }
    public WaypointCircuit.RoutePoint speedPoint { get; private set; }
    public WaypointCircuit.RoutePoint progressPoint { get; private set; }

    public Transform target;

    private float progressDistance;         // The progress round the route, used in smooth mode.
    private int progressNum;                // the current waypoint number, used in point-to-point mode.
    private Vector3 lastPosition;           // Used to calculate current speed (since we may not have a rigidbody component)
    private float speed;                    // current speed of this object (calculated from delta since last frame)

    // setup script properties
    void Start()
    {
        ///我们使用一个变换来表示目标点，和将要产生速度变化的点。
        ///不需要进一步的依赖关系，允许该组件将此信息传达给AI。
        ///您可以手动创建一个转换，并将其分配给这个组件*和*的AI，然后这个组件将更新它，AI可以读取它。
        // we use a transform to represent the point to aim for, and the point which
        // is considered for upcoming changes-of-speed. This allows this component 
        // to communicate this information to the AI without requiring further dependencies.

        // You can manually create a transform and assign it to this component *and* the AI,
        // then this component will update it, and the AI can read it.
        if (target == null)
        {
            target = new GameObject(name + " Waypoint Target").transform;
        }

        Reset();

    }

    // reset the object to sensible values
    //将数值重置为预期值
    public void Reset()
    {
        progressDistance = 0;
        progressNum = 0;
        if (progressStyle == ProgressStyle.PointToPoint)
        {
            target.position = circuit.Waypoints[progressNum].position;
            target.rotation = circuit.Waypoints[progressNum].rotation;
        }
    }


    void Update()
    {
        if (progressStyle == ProgressStyle.SmoothAlongRoute)
        {
            // determine the position we should currently be aiming for
            // (this is different to the current progress position, it is a a certain amount ahead along the route)
            // we use lerp as a simple way of smoothing out the speed over time.
            // 随着时间的推移，须确定目前应该瞄准的位置 ，为了使用lerp作这个简单的方法来平滑速度，
            //（这与目前的进展情况不同，它是沿路线前进的一一个一定量）
            if (Time.deltaTime > 0)
            {
                speed = Mathf.Lerp(speed, (lastPosition - transform.position).magnitude / Time.deltaTime, Time.deltaTime);
            }
            target.position = circuit.GetRoutePoint(progressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor * speed).position;
            target.rotation = Quaternion.LookRotation(circuit.GetRoutePoint(progressDistance + lookAheadForSpeedOffset + lookAheadForSpeedFactor * speed).direction);


            // get our current progress along the route
            progressPoint = circuit.GetRoutePoint(progressDistance);
            Vector3 progressDelta = progressPoint.position - transform.position;
            if (Vector3.Dot(progressDelta, progressPoint.direction) < 0)
            {
                progressDistance += progressDelta.magnitude * 0.5f;
            }

            lastPosition = transform.position;
        }
        else
        {
            // point to point mode. Just increase the waypoint if we're close enough:
            //点对点模式。只是增加点如果我们足够近：

            Vector3 targetDelta = target.position - transform.position;
            if (targetDelta.magnitude < pointToPointThreshold)
            {
                progressNum = (progressNum + 1) % circuit.Waypoints.Length;
            }


            target.position = circuit.Waypoints[progressNum].position;
            target.rotation = circuit.Waypoints[progressNum].rotation;

            // get our current progress along the route
            //沿着路线获得我们目前的进展
            progressPoint = circuit.GetRoutePoint(progressDistance);
            Vector3 progressDelta = progressPoint.position - transform.position;
            if (Vector3.Dot(progressDelta, progressPoint.direction) < 0)
            {
                progressDistance += progressDelta.magnitude;
            }
            lastPosition = transform.position;
        }

    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(circuit.GetRoutePosition(progressDistance), 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(target.position, target.position + target.forward);
        }
    }
}
