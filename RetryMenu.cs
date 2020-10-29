using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game2
{
    class RetryMenu
    {
        private Game game;

        enum RETRY
        {
            YES,
            NO,
            MAX,
        }
        string[] retry_name = new string[]
        {
            "texture/yes",
            "texture/no",
        };
        private RETRY retry;
        private List<Menu> retry_menu = new List<Menu>();

        private Menu allow;
        private Menu retry_text;
        private Menu click;

        private int frame = 30;

        public RetryMenu(Game _game)
        {
            game = _game;

            for (int i = 0; i < (int)RETRY.MAX; i++)
            {
                Actor a = new Actor(game);
                a.SetPosition(
                    game.GetCamera().GetPosition() - Vector3.UnitZ +
                    new Vector3(0.0f, -50.0f - 25.0f * i, 0.0f));
                SpriteComponent sc = new SpriteComponent(a, 200);
                sc.SetTexture(game.GetRenderer().GetTexture(retry_name[i]));

                Menu m = new Menu();
                m.actor = a;
                m.sprite = sc;
                retry_menu.Add(m);
            }

            allow = new Menu();
            allow.actor = new Actor(game);
            allow.actor.SetPosition(retry_menu[(int)retry].actor.GetPosition() - new Vector3(50.0f, 0.0f, 0.0f));
            allow.sprite = new SpriteComponent(allow.actor, 200);
            allow.sprite.SetTexture(game.GetRenderer().GetTexture("texture/allow"));

            retry_text = new Menu();
            retry_text.actor = new Actor(game);
            retry_text.actor.SetPosition(game.GetCamera().GetPosition() - Vector3.UnitZ);
            retry_text.sprite = new SpriteComponent(retry_text.actor, 200);
            retry_text.sprite.SetTexture(game.GetRenderer().GetTexture("texture/retry"));

            click = new Menu();
            click.actor = new Actor(game);
            click.actor.SetPosition(
                game.GetCamera().GetPosition() - Vector3.UnitZ +
                new Vector3(300.0f, -200.0f, 0.0f));
            click.sprite = new SpriteComponent(click.actor, 200);
            click.sprite.SetTexture(game.GetRenderer().GetTexture("texture/click"));

            GameDevice.GetInstance().GetSE().Load("se/cursor");
            GameDevice.GetInstance().GetSE().Load("se/decision");
        }

        public void Update()
        {
            frame--;

            if (frame < 0)
            {
                if (Input.GetKeyDown(Keys.W))
                {
                    retry--;
                    if (retry < 0) retry = RETRY.MAX - 1;
                    GameDevice.GetInstance().GetSE().Play("se/cursor");
                }
                if (Input.GetKeyDown(Keys.S))
                {
                    retry++;
                    if (retry >= RETRY.MAX) retry = 0;
                    GameDevice.GetInstance().GetSE().Play("se/cursor");
                }

                if (Input.GetLMBDown())
                {
                    if (retry == RETRY.YES)
                    {
                        SceneManager.GetInstance().Load(new Game(SceneManager.GetInstance(), game.GetMapNum()));//リトライ
                    }
                    if (retry == RETRY.NO)
                    {
                        SceneManager.GetInstance().Load(new Title(SceneManager.GetInstance()));
                    }
                    GameDevice.GetInstance().GetSE().Play("se/decision");
                }

                allow.actor.SetPosition(retry_menu[(int)retry].actor.GetPosition() - new Vector3(50.0f, 0.0f, 0.0f));
            }
        }
    }
}
