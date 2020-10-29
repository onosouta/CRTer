using Microsoft.Xna.Framework;
using System;

namespace Game2
{
    //ガラスの破片
    class BrokenGlass : Actor
    {
        private Vector3 velocity;//移動量
        private float gravity = 0.75f;//重力

        private int angle;

        public BrokenGlass(Scene _scene, Vector3 _position) : base(_scene)
        {
            position = _position;

            SpriteComponent sprite = new SpriteComponent(this, 200);
            sprite.SetTexture(scene.GetRenderer().GetTexture("Texture/broken_glass"));

            Random random = GameDevice.GetInstance().GetRandom();
            int a = random.Next(random.Next(360));
            velocity = new Vector3(
                (float)Math.Cos(MathHelper.ToRadians(a)),
                (float)Math.Sin(MathHelper.ToRadians(a)) + 1.0f,
                0.0f) *
                (random.Next(100) / 10.0f);

            //回転
            angle = random.Next(360);
            rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(angle));
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            //回転
            angle += 2;
            rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(angle));

            velocity.Y = velocity.Y - gravity;
            velocity.Y = Math.Max(velocity.Y, -20.0f);//最大値
            SetPosition(GetPosition() + velocity);

            //画面外
            Vector3 camera_position = scene.GetCamera().GetPosition();
            if (position.Y < camera_position.Y - scene.GetRenderer().GetScreenHeight())
            {
                state = State.Dead;
            }
        }
    }
}
