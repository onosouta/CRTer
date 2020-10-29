using Microsoft.Xna.Framework;

namespace Game2
{
    //カメラ
    class Camera : Actor
    {
        public Camera(Scene _scene) : base(_scene)
        {
            position = Vector3.UnitZ;
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            Player player = scene.GetPlayer();//プレイヤーを取得

            if (player != null)
            {
                Vector3 player_position = player.GetPosition() + Vector3.UnitZ;

                //ディスプレイ
                Display display = GameDevice.GetInstance().GetDisplay();
                Vector3 display_position = new Vector3(
                    display.Position().X,
                    display.Position().Y,
                    0.0f);

                //補間
                position = Vector3.Lerp(
                    position,
                    player_position,
                    0.075f);

                position += display_position;

                Block[][] blocks = scene.GetMap().GetBlocks();
                if (blocks.Length != 0)
                {
                    int screen_width = scene.GetRenderer().GetScreenWidth();
                    int screen_height = scene.GetRenderer().GetScreenHeight();
                    if (position.X < screen_width / 2.0f)
                        position.X = screen_width / 2.0f;
                    if (position.X > blocks[0].Length * scene.GetMap().GetBlockWidth() - screen_width / 2.0f)
                        position.X = blocks[0].Length * scene.GetMap().GetBlockWidth() - screen_width / 2.0f;
                    if (position.Y < screen_height / 2.0f)
                        position.Y = screen_height / 2.0f;
                    if (position.Y > blocks.Length * scene.GetMap().GetBlockHeight() - screen_height / 2.0f)
                        position.Y = blocks.Length * scene.GetMap().GetBlockHeight() - screen_height / 2.0f;
                }
            }

            //ビューを設定
            Renderer renderer = scene.GetRenderer();
            renderer.View(position);
        }
    }
}
