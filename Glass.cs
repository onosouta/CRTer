using Microsoft.Xna.Framework;

namespace Game2
{
    //ガラス
    class Glass : Block
    {
        public Glass(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/glass"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);

                GameDevice.GetInstance().GetSE().Load("se/glass");
            }
        }

        public override void Hit(Actor _actor, DIRECTION _d)
        {
            base.Hit(_actor, _d);

            Player player = scene.GetPlayer();

            if (player != null)
            {
                if (_actor == player &&
                    player.GetFrontSpeed() > 300)//ダッシュ!
                {
                    state = State.Dead;

                    None none = new None(scene, false);
                    none.SetPosition(position);
                    none.SetX(x);
                    none.SetY(y);
                    
                    Map map = scene.GetMap();
                    map.GetBlocks()[y][x] = none;//追加

                    for (int i = 0; i < 2; i++)
                    {
                        BrokenGlass bg = new BrokenGlass(scene, position);
                    }

                    GameDevice.GetInstance().GetSE().Play("se/glass");
                }
            }
        }

        //コピー
        public override object Clone()
        {
            return new Glass(this);
        }

        //コピーコンストラクタ
        private Glass(Glass _glass) : this(_glass.scene, false, _glass.num) { }
    }
}
