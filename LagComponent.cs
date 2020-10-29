using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game2
{
    struct Lag
    {
        public Actor owner;
        public SpriteComponent sc;//スプライト
    }
    
    //ラグコンポーネント
    class LagComponent : Component
    {
        private List<Lag> lags = new List<Lag>();//ワールド行列

        public LagComponent(Actor _owner, Texture2D _texture, int _num = 3, int _order = 200)
            : base(_owner, _order)
        {
            //初期化
            for (int i = 1; i <= _num; i++)
            {
                Lag lag = new Lag();
                //所有者
                lag.owner = new Actor(owner.GetScene());
                //スプライト
                lag.sc = new SpriteComponent(lag.owner, 10);
                lag.sc.SetTexture(_texture);
                lag.sc.a = 1.0f - (1.0f / (_num + 1) * i);//アルファ値

                lags.Add(lag);//追加
            }
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            for (int i = 0; i < lags.Count; i++)
            {
                lags[i].owner.SetState(owner.GetState());

                Vector3 translation = owner.GetPosition() + -owner.GetFront() * 7.5f * (i + 1);
                //ワールド行列を計算
                Matrix world =
                    Matrix.CreateScale(owner.GetScale()) *
                    Matrix.CreateFromQuaternion(owner.GetRotation()) *
                    Matrix.CreateTranslation(translation);
                lags[i].owner.SetWorld(world);
            }
        }
    }
}
