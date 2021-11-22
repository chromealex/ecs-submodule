using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

    public AgentManager manager;
    public Transform target;

    public float radius;
    public float sensorLength;
    public float[] sensorsLength;
    
    public float step = 45f;
    public float speed;

    public bool simpleGizmos;
    public Vector3 velocity;
    
    public void OnDrawGizmos() {

        var pos = this.transform.position;
        var forwardDir = this.transform.rotation * Vector3.forward;

        var reqSteps = Mathf.RoundToInt(360f / this.step);
        if (this.sensorsLength == null || this.sensorsLength.Length != reqSteps) {
            
            this.sensorsLength = new float[reqSteps];
            
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, this.radius);

        if (this.target != null) {

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(this.target.position, Vector3.one);

        }

        if (this.simpleGizmos == false) {

            { // default-length sensors
                Gizmos.color = Color.gray;
                for (int i = 0; i < this.sensorsLength.Length; ++i) {

                    Gizmos.DrawLine(pos, pos + Quaternion.Euler(0f, i * this.step, 0f) * forwardDir * this.sensorLength);

                }

            }

            Gizmos.color = Color.yellow;
            for (int i = 0; i < this.sensorsLength.Length; ++i) {

                var angle = this.step * i;
                var dir = Quaternion.Euler(0f, angle, 0f) * forwardDir;
                Gizmos.DrawLine(pos, pos + dir * this.GetSensorLength(forwardDir, dir));

            }

            if (this.target != null) {

                var d = this.MakeDecision(forwardDir, pos, this.target.position);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(pos + d * this.sensorLength, pos + d * (this.sensorLength + 1f));

            }

        }

    }

    public void Simulate(float dt) {
        
        var pos = this.transform.position;
        var forwardDir = this.transform.rotation * Vector3.forward;
        
        { // sensor length per dir
            this.UpdateSensorsLength(forwardDir, pos, this.target.position);
        }
        
        if (this.target != null) {

            var d = this.MakeDecision(forwardDir, pos, this.target.position);
            this.transform.rotation = Quaternion.LookRotation(d, Vector3.up);
            this.velocity = d * this.speed;
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + this.velocity, dt * this.speed);

        }

    }

    private float GetSensorLength(Vector3 forwardDir, Vector3 dir) {

        return this.sensorsLength[this.GetSensorIndex(forwardDir, dir)];

    }
    
    private int GetSensorIndex(Vector3 forwardDir, Vector3 dir) {

        var angle = Mathf.Repeat(Vector3.SignedAngle(forwardDir, dir, Vector3.up), 360f);
        return Mathf.RoundToInt(angle / this.step);
        
    }

    public Vector3 MakeDecision(Vector3 forwardDir, Vector3 pos, Vector3 targetPos) {

        var targetDir = targetPos - pos;
        var dirLenIdx = -1;
        var dirAngleIdx = -1;
        var max = 0f;
        var nearestAngle = float.MaxValue;
        for (int i = 0; i < this.sensorsLength.Length; ++i) {

            var len = this.sensorsLength[i];
            if (len >= max) {

                max = len;
                dirLenIdx = i;

            }

            var sensorDir = Quaternion.Euler(0f, this.step * i, 0f) * forwardDir;
            var angle = Mathf.Repeat(Vector3.SignedAngle(sensorDir, targetDir, Vector3.up), 360f);
            if (angle <= nearestAngle && len >= this.sensorLength * 0.3f) {

                nearestAngle = angle;
                dirAngleIdx = i;

            }

        }

        var dirIdx = -1;
        if (dirAngleIdx == dirLenIdx) {

            dirIdx = dirLenIdx;

        } else {

            dirIdx = (dirAngleIdx >= 0 ? dirAngleIdx : dirLenIdx);

        }
        
        return Quaternion.Euler(0f, this.step * dirIdx, 0f) * forwardDir;

    }

    public Vector3 GetPredictedPosition(float timeOffset) {

        return this.transform.position + this.velocity * timeOffset;

    }

    public float stepSize;
    private void UpdateSensorsLength(Vector3 forwardDir, Vector3 pos, Vector3 targetPos) {

        var maxLength = this.sensorLength;
        var sensorLengthSqr = maxLength * maxLength;
        for (int i = 0; i < this.sensorsLength.Length; ++i) {

            var sensorDir = Quaternion.Euler(0f, this.step * i, 0f) * forwardDir;
            var angle = Mathf.Abs(Vector3.SignedAngle(sensorDir, forwardDir, Vector3.up));
            this.sensorsLength[i] = (360f - angle) / 360f * maxLength;

        }

        foreach (var agent in this.manager.allAgents) {
            
            if (agent == this) continue;

            var agentPos = agent.GetPredictedPosition((agent.transform.position - pos).magnitude / this.speed);
            var agentNearestPos = new Ray(agentPos, pos - agentPos).GetPoint(agent.radius);
            var dir = (agentNearestPos - pos);
            var dist = dir.sqrMagnitude;
            if (dist <= sensorLengthSqr) {

                for (int i = 0; i < this.sensorsLength.Length; ++i) {

                    var d = Quaternion.Euler(0f, this.step * i, 0f) * forwardDir;
                    var angle = Mathf.Abs(Vector3.SignedAngle(dir, d, Vector3.up));
                    if (angle <= this.stepSize) {

                        var len = angle / this.step * maxLength;
                        len = Mathf.Min(len, Mathf.Sqrt(dist));
                        this.sensorsLength[i] = Mathf.Min(this.sensorsLength[i], len);

                    }

                }

            }

        }

    }
    
}
