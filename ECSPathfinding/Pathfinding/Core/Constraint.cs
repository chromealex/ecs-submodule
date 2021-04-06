using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ME.ECS.Pathfinding {

    public struct BurstConstraint {

        public Vector3Int agentSize;

        public byte checkArea;
        public long areaMask;

        public byte checkTags;
        public long tagsMask;
        
        public byte checkWalkability;
        public byte walkable;

        public long graphMask;

    }
    
    [System.Serializable]
    public struct Constraint {

        public static Constraint Empty => new Constraint() {
            graphMask = -1,
        };

        public static Constraint Default => new Constraint() {
            checkWalkability = true,
            walkable = true,
            graphMask = -1,
        };

        public Vector3 agentSize;

        public bool checkArea;
        public long areaMask;

        public bool checkTags;
        public long tagsMask;
        
        public bool checkWalkability;
        public bool walkable;

        public long graphMask;

        public BurstConstraint GetBurstConstraint() {
            
            return new BurstConstraint() {
                agentSize = new Vector3Int((int)this.agentSize.x, (int)this.agentSize.y, (int)this.agentSize.z),
                areaMask = this.areaMask,
                checkTags = this.checkTags == true ? (byte)1 : (byte)0,
                tagsMask = this.tagsMask,
                checkWalkability = this.checkWalkability == true ? (byte)1 : (byte)0,
                walkable = this.walkable == true ? (byte)1 : (byte)0,
                graphMask = this.graphMask,
            };
            
        }
        
        public override string ToString() {
            
            return "AgentSize: " + this.agentSize.ToFullString() +
                   ", Area: " + this.checkArea + " (" + this.areaMask + ")" +
                   ", Tags: " + this.checkTags + " (" + this.tagsMask + ")" +
                   ", Walkable: " + this.checkWalkability + " (" + this.walkable + ")" +
                   ", Graph: " + this.graphMask;
            
        }

    }

}