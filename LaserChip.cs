using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game2
{
    class LaserChip : Actor
    {
        private Vector3 velocity;//移動量
        private float gravity = 0.75f;//重力

        private List<Vector3> positions = new List<Vector3>();
        private int num = 5;

        public LaserChip(Scene _scene, Vector3 _position) : base(_scene)
        {
            position = _position;

            SpriteComponent sprite = new SpriteComponent(this, 200);
            sprite.SetTexture(scene.GetRenderer().GetTexture("Texture/laser_chip"));

            Random random = GameDevice.GetInstance().GetRandom();
            int a = random.Next(random.Next(360));
            velocity = new Vector3(
                (float)Math.Cos(MathHelper.ToRadians(a)),
                (float)Math.Sin(MathHelper.ToRadians(a)) + 1.0f,
                0.0f) *
                (random.Next(100) / 10.0f);

            for (int i = 0; i < num; i++)
            {
                positions.Add(position);
            }
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            velocity.Y = velocity.Y - gravity;
            velocity.Y = Math.Max(velocity.Y, -20.0f);//最大値
            SetPosition(GetPosition() + velocity);

            //画面外
            Vector3 camera_position = scene.GetCamera().GetPosition();
            if (position.Y < camera_position.Y - scene.GetRenderer().GetScreenHeight())
            {
                state = State.Dead;
            }
            
            positions.Insert(0, position);
            positions.RemoveAt(positions.Count - 1);
        }

        public override void Draw(Renderer _renderer)
        {
            base.Draw(_renderer);

            //残像を描画
            for (int i = 0; i < num; i++)
            {
                Matrix world_transform = Matrix.CreateScale(scale);
                world_transform *= Matrix.CreateFromQuaternion(rotation);
                world_transform *= Matrix.CreateTranslation(positions[i]);

                float a = 0.4f + 0.1f * (num - i);
                _renderer.Draw(scene.GetRenderer().GetTexture("Texture/laser_chip"), world_transform, a);
            }
        }
    }
}
