using Microsoft.Xna.Framework;

namespace Game2
{
    //背景
    class Background : Actor
    {
        public Background(Scene _scene) : base(_scene)
        {
            SpriteComponent sc = new SpriteComponent(this, 1);//オーダーは低い
            sc.SetTexture(scene.GetRenderer().GetTexture("texture/background"));
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
    }
}
