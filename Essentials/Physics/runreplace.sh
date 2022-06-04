chmod +x replace.sh
zsh ./replace.sh "using UnityS.Physics" "using ME.ECS.Essentials.Physics"
zsh ./replace.sh "using static UnityS.Physics" "using static ME.ECS.Essentials.Physics"
zsh ./replace.sh "namespace UnityS.Physics" "namespace ME.ECS.Essentials.Physics"
zsh ./replace.sh "using Unity.Entities;" "using ME.ECS;"
zsh ./replace.sh "using UnityS.Mathematics;" "using ME.ECS.Mathematics;"
zsh ./replace.sh "using FloatRange = UnityS.Physics.Math.FloatRange;" "using FloatRange = ME.ECS.Essentials.Physics.Math.FloatRange;"
zsh ./replace.sh "using Unity.Assertions;" ""
zsh ./replace.sh "using UnityS.Transforms" "using ME.ECS.Transform"

zsh ./replace.sh "FixedString32 " "FixedString32Bytes "
zsh ./replace.sh "FixedString64 " "FixedString64Bytes "
zsh ./replace.sh "FixedString128 " "FixedString128Bytes "

zsh ./replace.sh " Translation bodyPosition," " ME.ECS.Transform.Position bodyPosition,"
zsh ./replace.sh " Translation t," " ME.ECS.Transform.Position t,"
zsh ./replace.sh " PhysicsMass" " ME.ECS.Essentials.Physics.Components.PhysicsMass"
zsh ./replace.sh " PhysicsJoint" " ME.ECS.Essentials.Physics.Components.PhysicsJoint"
zsh ./replace.sh "\[PhysicsJoint.k" "\[ME.ECS.Essentials.Physics.Components.PhysicsJoint.k"
zsh ./replace.sh "(PhysicsJoint.k" "(ME.ECS.Essentials.Physics.Components.PhysicsJoint.k"
zsh ./replace.sh " PhysicsVelocity" " ME.ECS.Essentials.Physics.Components.PhysicsVelocity"
zsh ./replace.sh " PhysicsCollider" " ME.ECS.Essentials.Physics.Components.PhysicsCollider"
zsh ./replace.sh " RigidTransform(r.Value, t.Value)" " RigidTransform(r.value, t.value)"
zsh ./replace.sh " PhysicsCollider" " ME.ECS.Essentials.Physics.Components.PhysicsCollider"

zsh ./replace.sh ", IJobParallelForDefer" ", ME.ECS.IJobParallelForDefer"
zsh ./replace.sh "return IJobParallelForDeferExtensions" "return ME.ECS.IJobParallelForDeferExtensions"

zsh ./replace.sh "bodyOrientation.Value" "bodyOrientation.value"
zsh ./replace.sh "bodyPosition.Value" "bodyPosition.value"

zsh ./replace.sh "{ Value = orientation }" "{ value = orientation }"
zsh ./replace.sh "bodyCollider.Value" "bodyCollider.value"

zsh ./replace.sh "public struct AllHitsCollector<T> : ICollector<T> where T : struct, IQueryResult" "public struct AllHitsCollector<T> : ICollector<T> where T : unmanaged, IQueryResult"

zsh ./replace.sh " Assert.IsTrue" " UnityEngine.Assertions.Assert.IsTrue"
zsh ./replace.sh " Unity.Assertions.Assert.IsFalse" " UnityEngine.Assertions.Assert.IsFalse"
zsh ./replace.sh "FixedList128<" "FixedList128Bytes<"

zsh ./replace.sh "math.rotate(bodyOrientation.value, bodyMass.CenterOfMass) + bodyPosition.value;" "math.rotate(bodyOrientation.value, bodyMass.CenterOfMass) + (float3)bodyPosition.value;"
zsh ./replace.sh "com -= bodyPosition.value;" "com -= (float3)bodyPosition.value;"