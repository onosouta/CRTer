using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2
{
    //ゲームデバイス(シングルトン)
    sealed class GameDevice
    {
        private static GameDevice instance;//インスタンス
        
        private Game1 game1;//Game1クラス

        private ContentManager content;     //コンテンツ管理
        private GraphicsDevice graphics;    //グラフィックス
        
        private Renderer renderer;//レンダラー
        private Random random;//ランダム
        private Display display;//ディスプレイ
        private BGM bgm;
        private SE se;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_game1">Game1クラス</param>
        private GameDevice(Game1 _game1)
        {
            game1 = _game1;

            content = game1.Content;
            graphics = _game1.GraphicsDevice;

            renderer = new Renderer(game1);
            random = new Random();
            display = new Display(true);
            bgm = new BGM(content);
            se = new SE(content);
        }

        //Game1クラスのみ
        public static GameDevice GetInstance(Game1 _game1)
        {
            if (instance == null)
            {
                instance = new GameDevice(_game1);
            }

            return instance;
        }

        public static GameDevice GetInstance()
        {
            return instance;
        }

        //Game1クラスのみ
        public void Update(float _delta_time)
        {
            Input.Update(_delta_time);
        }

        //ゲッター
        public ContentManager GetContent() { return content; }
        public GraphicsDevice GetGraphics() { return graphics; }
        public Renderer GetRenderer() { return renderer; }
        public Random GetRandom() { return random; }
        public Display GetDisplay() { return display; }
        public BGM GetBGM() { return bgm; }
        public SE GetSE() { return se; }
    }
}
