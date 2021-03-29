using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

    public Agent[] allAgents;

    [ContextMenu("Collect")]
    public void OnValidate() {

        this.allAgents = AgentManager.FindObjectsOfType<Agent>();

    }

    public void Update() {
        
        this.Simulate(Time.deltaTime);
        
    }

    public void Simulate(float dt) {

        for (int i = 0, count = this.allAgents.Length; i < count; ++i) {
            
            this.allAgents[i].Simulate(dt);
            
        }
        
    }

}
