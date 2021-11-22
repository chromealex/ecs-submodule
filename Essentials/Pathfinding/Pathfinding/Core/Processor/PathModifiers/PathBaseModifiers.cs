namespace ME.ECS.Pathfinding {
    
    public interface IPathModifier {

        Path Run(Path path, Constraint constraint);
        bool IsWalkable(int index, Constraint constraint);

    }

    public abstract class PathModifierSeeker : UnityEngine.MonoBehaviour {

        public abstract bool IsWalkable(int index, Constraint constraint);

        public abstract Path Run(Path path, Constraint constraint);

    }

    public struct PathModifierEmpty : IPathModifier {

        public bool IsWalkable(int index, Constraint constraint) {

            return true;

        }

        public Path Run(Path path, Constraint constraint) {

            return path;

        }

    }

}