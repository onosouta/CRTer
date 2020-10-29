using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Game2
{
    //跡
    struct Trail
    {
        public float rotation;      //回転
        public Vector3 position;    //座標

        public Trail(Vector3 _position)
        {
            position = _position;

            //回転
            Random random = GameDevice.GetInstance().GetRandom();
            rotation = MathHelper.ToRadians(random.Next(8) * 45.0f);
        }
    }

    //ペイント
    class Paint : Actor
    {
        private List<Trail> trails = new List<Trail>();//跡
        private List<Block> nones = new List<Block>();//マスク用のブロック

        private Texture2D none;//マスク用
        private Texture2D trail;

        private RenderTarget2D paint_rt;//レンダーターゲット
        private Effect paint_e;//シェーダー

        private SpriteComponent sprite;

        public Paint(Scene _scene) : base(_scene)
        {
            Renderer renderer = scene.GetRenderer();

            //画像を読み込む
            none = renderer.GetTexture("Texture/none");
            trail = renderer.GetTexture("Texture/trail");

            //レンダーターゲットを設定
            paint_rt = new RenderTarget2D(
                renderer.GetGraphics(),
                renderer.GetScreenWidth(),
                renderer.GetScreenHeight());

            //シェーダーを読み込む
            paint_e = renderer.GetContent().Load<Effect>("fx/paint_e");

            sprite = new SpriteComponent(this, 95);
            sprite.SetTexture(paint_rt);
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            //カメラの座標
            Vector3 camera_position = scene.GetCamera().GetPosition();

            //座標設定
            position = new Vector3(
                camera_position.X,
                camera_position.Y,
                0.0f);

            is_recalculate = true;//再計算！
        }

        //※要呼び出し
        public void Draw(Renderer _renderer)
        {
            _renderer.GetGraphics().SetRenderTarget(paint_rt);

            //トレイルを描画
            foreach (var t in trails)
            {
                Matrix w =
                    Matrix.CreateRotationZ(t.rotation) *
                    Matrix.CreateTranslation(t.position);
                _renderer.Draw(paint_e, trail, w);
            }

            //マスク用ブロックを描画
            foreach (var b in nones)
            {
                _renderer.Draw(paint_e, none, Matrix.CreateTranslation(b.GetPosition()));
            }

            _renderer.GetGraphics().SetRenderTarget(null);

            sprite.SetTexture(paint_rt);//スプライトに設定
        }

        //ゲッター
        public List<Trail> GetTrails() { return trails; }
        public List<Block> GetNones() { return nones; }
    }
}
