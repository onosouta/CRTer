using Microsoft.Xna.Framework.Input;
using System;

namespace Game2
{
    //発射コンポーネント
    class ShotComponent : Component
    {
        private float front_speed = 100.0f;//前方スピード
        private float cool_down = 0.0f;//間隔

        public ShotComponent(Actor _owner, int _order = 80) : base(_owner, _order) { }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            cool_down = Math.Max(cool_down - _delta_time, 0.0f);

            if (cool_down <= 0.0f &&
                (Input.GetLMB() || Input.GetKey(Keys.Z) || Input.GetButton(Buttons.RightShoulder)))//発射
            {
                Bullet b = new Bullet(
                    owner.GetScene(),       //Scene
                    owner,                  //owner
                    owner.GetRotation(),    //rotation
                    owner.GetPosition(),    //position
                    front_speed);           //front_speed
                cool_down = 0.1f;
            }
        }

        //セッター
        public void SetFrontSpeed(float _front_speed) { front_speed = _front_speed; }
    }
}
