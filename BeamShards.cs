using Microsoft.Xna.Framework;
using System;

namespace Game2
{
    class BeamShards : Actor
    {
        private Vector3 velocity;//移動量
        float speed;

        public BeamShards(Scene _scene, Vector3 _position) : base(_scene)
        {
            position = _position;

            SpriteComponent sprite = new SpriteComponent(this, 10);
            sprite.SetTexture(scene.GetRenderer().GetTexture("Texture/beam_shards"));

            Random random = GameDevice.GetInstance().GetRandom();
            int a = random.Next(random.Next(360));
            velocity = new Vector3(
                (float)Math.Cos(MathHelper.ToRadians(a)),
                (float)Math.Sin(MathHelper.ToRadians(a)),
                0.0f) *
                2.0f;

            speed = 1.0f;

            //回転
            int angle = random.Next(8) * 45;
            rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(angle));

            scale = random.Next(50) / 100.0f + 0.5f;
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            speed = Math.Max(speed - 0.05f, 0.0f);
            SetPosition(GetPosition() + velocity * speed);

            SetScale(GetScale() - speed / 25.0f);

            if (speed == 0.0f)
            {
                state = State.Dead;
            }
        }
    }
}
