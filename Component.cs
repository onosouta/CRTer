namespace Game2
{
    //コンポーネント
    abstract class Component
    {
        protected Actor owner;
        protected int order;

        public Component(Actor _owner, int _order = 100)
        {
            owner = _owner;
            order = _order;

            owner.AddComponent(this);//追加
        }

        public virtual void Update(float _delta_time)
        {
            if (owner.GetState() == State.Dead)
            {
                owner.RemoveComponent(this);
            }
        }

        public virtual void WorldTransform() { }

        public Actor GetOwner() { return owner; }
        public int GetOrder() { return order; }
    }
}
