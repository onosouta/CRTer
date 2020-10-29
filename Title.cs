using Microsoft.Xna.Framework;

namespace Game2
{
    //タイトル
    class Title : Scene
    {
        private Menu click;

        public Title(SceneManager _scene_manager) : base(_scene_manager)
        {
            Actor a = new Actor(this);
            SpriteComponent sc = new SpriteComponent(a, 200);
            sc.SetTexture(GetRenderer().GetTexture("Texture/title"));

            click = new Menu();
            click.actor = new Actor(this);
            click.actor.SetPosition(new Vector3(0.0f, -200.0f, 0.0f));
            click.sprite = new SpriteComponent(click.actor, 200);
            click.sprite.SetTexture(this.GetRenderer().GetTexture("texture/click"));
            GameDevice.GetInstance().GetSE().Load("se/decision");
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            if (Input.GetLMBDown())
            {
                scene_manager.Load(new Game(scene_manager, 1));//終了
                GameDevice.GetInstance().GetSE().Play("se/decision");
            }
        }

        public override void Load()
        {
            base.Load();

            Background bg = new Background(this);
        }
    }
}
