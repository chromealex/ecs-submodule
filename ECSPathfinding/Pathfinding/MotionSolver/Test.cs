using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Transform target;
    public Test[] allAgents;

    public float radius;
    public float sensorLength;

    public float[] sensorsLength;

    public void OnDrawGizmos() {

        var pos = this.transform.position;
        var forwardDir = this.transform.rotation * Vector3.forward;

        if (this.sensorsLength == null || this.sensorsLength.Length == 0) this.sensorsLength = new float[8];
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, this.radius);
        
        { // default-length sensors
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(pos, pos + forwardDir * this.sensorLength);
            // left
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, -90, 0f) * forwardDir * this.sensorLength);
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, -45, 0f) * forwardDir * this.sensorLength);
            // right
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, 90, 0f) * forwardDir * this.sensorLength);
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, 45, 0f) * forwardDir * this.sensorLength);
            // back
            Gizmos.DrawLine(pos, pos + -forwardDir * this.sensorLength);
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, 45, 0f) * -forwardDir * this.sensorLength);
            Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, -45, 0f) * -forwardDir * this.sensorLength);
        }

        { // sensor length per dir
            this.UpdateSensorsLength();
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pos, pos + forwardDir * this.GetSensorLength(forwardDir, forwardDir));
        }

    }

    private float GetSensorLength(Vector3 forwardDir, Vector3 dir) {

        var angle = Vector3.Angle(forwardDir, dir);
        var dirIdx = (int)Mathf.Repeat(angle != 0f ? 360f / angle : 0f, this.sensorsLength.Length);
        return this.sensorsLength[dirIdx];

    }

    private void UpdateSensorsLength() {

        var maxLength = this.sensorLength;
        var sensorLengthSqr = maxLength * maxLength;
        foreach (var agent in this.allAgents) {
            
            if (agent == this) continue;

            var dist = (agent.transform.position - this.transform.position).sqrMagnitude;
            if (dist <= sensorLengthSqr) {
                
                
                
            }

        }
        
    }
    
}
