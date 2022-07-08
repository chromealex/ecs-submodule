# Version 2.5
* Tests.Tests: MemoryAllocator tests added
* Essentials.GOAP: 
  * fix index out of range actions if planner has one action
  * Job replaced with static burst method
* Runtime.Core: 
  * ENTITIES_GROUP_DISABLED define added
  * COMPONENTS_VERSION_NO_STATE_DISABLED define added
  * ENTITY_TIMERS_DISABLED define added
  * SHARED_COMPONENTS_DISABLED define added
  * Disposable Components storage fix
  * [fix] Hierarchy clean up properly now
  * Transform hierarchy ToVectorStruct/ToQuaternionStruct/ToScaleStruct methods removed
  * Custom MemoryAllocator implemented
  * NativeBufferArray resize/copy performance improvements
  * Filter::GetEnumerator() static shared data check moved to avoid multiple checks in MoveNext.
  * Filters WithinTicks minChunkSize parameter added
  * NativeQuadTree::GetResults alive entities check added
  * QuadTree UnsafeList* used instead of NativeArray
  * Filter::WithinTicks() groupBy parameter added to determine behvaiour how this filter needs to skip entities.
  * Filter WithinTicks method added into builder
  * CreateFromData now with lambda support
* Runtime.Core: FilterBag GetIndexByEntityId methods added
* Essentials.Pathfinding: 
  * Tests assembly added
  * NavMeshPath DataObject changed to ListCopyable
* GOAPAction: fixed point math support added [Oleg-Grim]
* GetNearest: distance dependant flag added [Oleg-Grim]
* OverrideCost: implemented [Oleg-Grim]

