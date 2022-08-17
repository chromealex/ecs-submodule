# Version 2.8
* Runtime.Modules: RegisterViewSource now provide optional customId
* Runtime.Core: 
  * FilterBag refactoring
  * StatesHistory refactoring
  * ME.ECS.Collab minor fixes for Unity 2020
* Runtime.Essentials: All essential packages has been moved out from the main repo
* Runtime.Core:
  * Add-ons window added
  * Initializer Settings: Fixed Behaviour type added
  * Plugins storage in state implemented
  * World Static Callbacks added
  * World simulation steps refactoring

05/08/2022
# Version 2.7
* Runtime.Serialization: MemoryAllocator serializer added
* Tests.Tests: 
  * MemoryAllocator List tests added
  * MemoryAllocator Dictionary tests added
  * MemoryAllocator EquatableDictionary tests added
  * MemoryAllocator EquatableHashSet tests added
  * MemoryAllocator NativeHashSet tests added
  * MemoryAllocator HashSet tests added
  * MemoryAllocator Stack tests added
  * MemoryAllocator Queue tests added
* Runtime.Core:
  * ConfigId<> added to store DataConfig assets in components 
  * ViewId<> added to store view assets in components
  * IComponentDisposable deprecated
  * State refactoring to be ready for allocator
  * Collections refactoring
  * MemoryAllocator List collection added
  * MemoryAllocator Dictionary collection added
  * MemoryAllocator EquatableDictionary collection added
  * MemoryAllocator EquatableHashSet collection added
  * MemoryAllocator NativeHashSet collection added
  * MemoryAllocator HashSet collection added
  * MemoryAllocator Stack collection added
  * MemoryAllocator Queue collection added
  * ENTITY_API_VERSION1_DEPRECATED removed
  * Legacy Filters removed

11/07/2022
# Version 2.6
* Runtime.Core: 
  * FILTERS_LAMBDA_DISABLED define added
  * Internal BufferUtils refactoring
  * Components Tag storage implemented
* Runtime.Core:
  * States prewarming enabled
  * STATIC_API_DISABLED define added
  * Static Entity API implemented

08/07/2022
# Version 2.5
* Tests.Tests: MemoryAllocator tests added
* Essentials.GOAP: 
  * fix index out of range actions if planner has one action
  * Job replaced with static burst method
  * GOAPAction: fixed point math support added [Oleg-Grim]
  * GetNearest: distance dependant flag added [Oleg-Grim]
  * OverrideCost: implemented [Oleg-Grim]
* Runtime.Core: 
  * Transform Nodes dispose fix
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
  * Filter::WithinTicks() groupBy parameter added to determine behaviour how this filter needs to skip entities.
  * Filter WithinTicks method added into builder
  * CreateFromData now with lambda support
* Runtime.Core: FilterBag GetIndexByEntityId methods added
* Essentials.Pathfinding: 
  * Tests assembly added
  * NavMeshPath DataObject changed to ListCopyable