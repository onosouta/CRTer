using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game2
{
    class NextMenu
    {
        private Game game;

        enum NEXT
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
        private NEXT next;
        private List<Menu> next_menu = new List<Menu>();

        private Menu allow;
        private Menu next_text;
        private Menu click;

        private int frame = 30;

        public NextMenu(Game _game)
        {
            game = _game;

            for (int i = 0; i < (int)NEXT.MAX; i++)
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
                next_menu.Add(m);
            }

            allow = new Menu();
            allow.actor = new Actor(game);
            allow.actor.SetPosition(next_menu[(int)next].actor.GetPosition() - new Vector3(50.0f, 0.0f, 0.0f));
            allow.sprite = new SpriteComponent(allow.actor, 200);
            allow.sprite.SetTexture(game.GetRenderer().GetTexture("texture/allow"));
            
            next_text = new Menu();
            next_text.actor = new Actor(game);
            next_text.actor.SetPosition(game.GetCamera().GetPosition() - Vector3.UnitZ);
            next_text.sprite = new SpriteComponent(next_text.actor, 200);
            next_text.sprite.SetTexture(game.GetRenderer().GetTexture("texture/next"));

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
                    next--;
                    if (next < 0) next = NEXT.MAX - 1;
                    GameDevice.GetInstance().GetSE().Play("se/cursor");
                }
                if (Input.GetKeyDown(Keys.S))
                {
                    next++;
                    if (next >= NEXT.MAX) next = 0;
                    GameDevice.GetInstance().GetSE().Play("se/cursor");
                }

                if (Input.GetLMBDown())
                {
                    GameDevice.GetInstance().GetSE().Play("se/decision");
                    if (game.GetMapNum() + 1 > game.GetMapMax())
                    {
                        SceneManager.GetInstance().Load(new Clear(SceneManager.GetInstance()));
                        return;
                    }

                    if (next == NEXT.YES)
                    {
                        SceneManager.GetInstance().Load(new Game(SceneManager.GetInstance(), game.GetMapNum() + 1));
                    }
                    if (next == NEXT.NO)
                    {
                        SceneManager.GetInstance().Load(new Title(SceneManager.GetInstance()));
                    }
                }

                allow.actor.SetPosition(next_menu[(int)next].actor.GetPosition() - new Vector3(50.0f, 0.0f, 0.0f));
            }
        }
    }
}
