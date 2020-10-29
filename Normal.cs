using Microsoft.Xna.Framework;

namespace Game2
{
    //通常ブロック
    class Normal : Block
    {
        public Normal(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/normal" + (num % 10)));

                if (num < 10)
                {
                    AABB aabb;
                    aabb.min = new Vector2(-12.0f, -12.0f);
                    aabb.max = new Vector2(12.0f, 12.0f);
                    BoxComponent bc = new BoxComponent(this, aabb);
                }
            }
        }

        //コピー
        public override object Clone()
        {
            return new Normal(this);
        }

        //コピーコンストラクタ
        private Normal(Normal _normal) : this(_normal.scene, false, _normal.num) { }
    }
}
