using Microsoft.Xna.Framework;

namespace Game2
{
    //移動コンポーネント
    class MoveComponent : Component
    {
        private float angle_speed;//回転スピード
        private float front_speed;//移動スピード

        public MoveComponent(Actor _owner, int _order = 50) : base(_owner, _order) { }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            //回転
            Quaternion r = owner.GetRotation();
            float a = angle_speed * _delta_time;
            Quaternion i = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, a);
            r = Quaternion.Concatenate(r, i);
            owner.SetRotation(r);

            //移動
            Vector3 p = owner.GetPosition();
            p += owner.GetFront() * front_speed * _delta_time;
            owner.SetPosition(p);
        }

        //ゲッター
        public float GetAngleSpeed() { return angle_speed; }
        public float GetFrontSpeed() { return front_speed; }

        //セッター
        public void SetAngleSpeed(float _angle_speed) { angle_speed = _angle_speed; }
        public void SetFrontSpeed(float _front_speed) { front_speed = _front_speed; }
    }
}
