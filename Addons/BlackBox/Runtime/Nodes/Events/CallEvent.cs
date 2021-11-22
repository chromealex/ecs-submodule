
namespace ME.ECS.BlackBox {

    [Category("Events")]
    public class CallEvent : BoxNode {

        public World.GlobalEventType eventType = World.GlobalEventType.Visual;
        public GlobalEvent @event;
        
        public override void OnCreate() {

        }
        
        public override Box Execute(in Entity entity, float deltaTime) {

            this.@event.Execute(in entity, this.eventType);
            
            return this.next;
            
        }

    }

}