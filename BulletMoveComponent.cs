using Microsoft.Xna.Framework;

namespace Game2
{
    //バレットの移動コンポーネント
    class BulletMoveComponent : Component
    {
        private float front_speed;//前方スピード

        public BulletMoveComponent(Bullet _owner, int _order = 50) : base(_owner, _order)
        {
            BulletCollisionComponent bcc = new BulletCollisionComponent((Bullet)owner);
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            //移動
            Vector3 position = owner.GetPosition();
            position += owner.GetFront() * front_speed * _delta_time;
            owner.SetPosition(position);
        }

        //セッター
        public void SetFrontSpeed(float _front_speed) { front_speed = _front_speed; }
    }
}
