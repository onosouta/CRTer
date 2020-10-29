using Microsoft.Xna.Framework;

namespace Game2
{
    //バレットの衝突コンポーネント
    class BulletCollisionComponent : Component
    {
        private Physics physics;//物理
        private Vector3 pre_position;

        public BulletCollisionComponent(Bullet _owner, int _order = 150) : base(_owner, _order)
        {
            physics = owner.GetScene().GetPhysics();
            pre_position = owner.GetPosition();
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            Vector3 position = owner.GetPosition();//座標

            Vector3 camera_position = owner.GetScene().GetCamera().GetPosition();//カメラの座標
            float length = (position - camera_position).Length();//カメラとの距離
            if (length > 1920.0f)
            {
                owner.SetState(State.Dead);//範囲外
            }

            Line line = new Line(pre_position, position);   //線分
            Information information = new Information();    //情報
            if (physics.Intersect(line, owner, ref information))
            {
                Bullet bullet = (Bullet)owner;//バレットにキャスト
                if (bullet.GetOwner() != information.actor)//衝突
                {
                    owner.SetState(State.Dead);
                    information.actor.Hit(owner, 0x00);

                    Trail trail = new Trail(information.point);
                    Scene scene = owner.GetScene();
                    scene.GetMap().GetPaint().GetTrails().Add(trail);//追加
                }
            }

            pre_position = position;
        }
    }
}
