
using System.Linq;

namespace ME.ECS.Pathfinding.RVO {

    public interface ITreeElement {

        UnityEngine.Vector3 position { get; }
        float radius { get; }

    }

    public struct Obstacle : ITreeElement {

        public UnityEngine.Vector3 position { get; set; }
        public float radius { get; set; }

    }

    public struct Agent : IComponent {

        public UnityEngine.Vector3 position;
        public UnityEngine.Vector3 direction;
        public UnityEngine.Vector3 velocity;
        public float radius;
        public float agentSensorSize; // in general it is 2 * radius

    }
    
    public class RVO {

        private class Tree<T> where T : ITreeElement {

            private System.Collections.Generic.List<T> obstacles = new System.Collections.Generic.List<T>();

            public void Add(T item) {
            
                this.obstacles.Add(item);
            
            }

            public System.Collections.Generic.List<T> GetInRange(UnityEngine.Vector3 position, float radius) {

                var sqrRadius = radius * radius;
                return this.obstacles.Where(x => (x.position - position).sqrMagnitude <= sqrRadius).ToList();

            }

            public void DrawGizmos() {

                UnityEngine.Gizmos.color = UnityEngine.Color.red;
                foreach (var obstacle in this.obstacles) {
                    
                    UnityEngine.Gizmos.DrawWireSphere(obstacle.position, obstacle.radius);
                    
                }
                
            }

        }
        
        private Filter agents;
        private Tree<Obstacle> obstacles = new Tree<Obstacle>();

        public RVO() {

            Filter.Create("Agents").With<Agent>().Push(ref this.agents);

        }
        
        public void AddObstacle(Obstacle obstacle) {
            
            this.obstacles.Add(obstacle);
            
        }

        public void Simulate(float dt) {

            foreach (var entity in this.agents) {

                ref var agent = ref entity.Get<Agent>();
                this.ObstacleAvoidance(ref agent);
                this.AgentAvoidance(ref agent);

            }
            
        }

        private void ObstacleAvoidance(ref Agent agent) {

            var desiredDirection = agent.direction;
            var pos = agent.position;
            var list = this.obstacles.GetInRange(pos, agent.agentSensorSize);
            foreach (var item in list) {

                var targetDir = item.position - pos;
                var dot = UnityEngine.Vector3.Dot(targetDir, agent.direction);
                if (dot > 0f) {
                    
                    // right
                    desiredDirection = agent.direction;

                } else {
                    
                    // left
                    desiredDirection = agent.direction;
                    
                }
                
                
                
            }


        }

        private void AgentAvoidance(ref Agent agent) {
            
            
            
        }

        public void OnDrawGizmos() {

            this.obstacles.DrawGizmos();

            foreach (var entity in this.agents) {

                var agent = entity.Read<Agent>();
                
                UnityEngine.Debug.DrawLine(agent.position, agent.position + agent.direction, UnityEngine.Color.green, 0.033f);
                UnityEngine.Debug.DrawLine(agent.position, agent.position + agent.velocity, UnityEngine.Color.blue, 0.033f);

            }

        }

    }

}