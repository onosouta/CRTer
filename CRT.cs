using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    //ブラウン管シェーダー
    class CRT
    {
        private Game1 game1;//Game1クラス

        //レンダーターゲット
        private RenderTarget2D screen_rt;

        //シェーダー
        private Effect screen_e;
        private Effect crt_e;

        private float time;//crt_e用

        private Scene scene;//シーン
        private Vector3 position;//座標

        public CRT(Game1 _game1)
        {
            game1 = _game1;

            Renderer renderer = GameDevice.GetInstance().GetRenderer();

            //レンダーターゲットを設定
            screen_rt = new RenderTarget2D(
                renderer.GetGraphics(),
                renderer.GetScreenWidth(),
                renderer.GetScreenHeight());

            //シェーダーを読み込む
            screen_e = renderer.GetContent().Load<Effect>("fx/screen_e");
            crt_e = renderer.GetContent().Load<Effect>("fx/crt_e");

            time = 0.0f;
        }

        //更新
        public void Update(float _delta_time)
        {
            time += _delta_time;

            scene = SceneManager.GetInstance().GetScene();//シーン
            //カメラの座標
            if (scene.GetCamera() != null)
            {
                position =
                    scene.GetCamera().GetPosition() -
                    Vector3.UnitZ;
            }
            else
            {
                position = Vector3.Zero;
            }
        }

        //描画
        public void Draw(Renderer _renderer)
        {
            //screen_e
            _renderer.GetGraphics().SetRenderTarget(screen_rt);

            //ワールド行列
            Matrix world =
                Matrix.CreateScale(1.0f) *
                Matrix.CreateTranslation(position);

            screen_e.Parameters["tex1"].SetValue(scene.GetWaveManager().GetWaveRT());//ウェーブ
            _renderer.Draw(screen_e, game1.GetSpriteRT(), world);

            _renderer.GetGraphics().SetRenderTarget(null);

            //crt_e
            crt_e.Parameters["time"].SetValue(time);
            _renderer.Draw(crt_e, screen_rt, world);
        }
    }
}
