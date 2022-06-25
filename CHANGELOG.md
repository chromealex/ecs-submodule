# Version 2.5
* Essentials.GOAP: 
  * fix index out of range actions if planner has one action
  * Job replaced with static burst method
* Runtime.Core: 
  * Filter::WithinTicks() groupBy parameter added to determine behvaiour how this filter needs to skip entities.
  * Filter WithinTicks method added into builder
  * CreateFromData now with lambda support
* Runtime.Core: FilterBag GetIndexByEntityId methods added
* Essentials.Pathfinding: NavMeshPath DataObject changed to ListCopyable
* GOAPAction: fixed point math support added [Oleg-Grim]
* GetNearest: distance dependant flag added [Oleg-Grim]
* OverrideCost: implemented [Oleg-Grim]

