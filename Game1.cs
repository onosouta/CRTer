using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;

        private static readonly int SCREEN_WIDTH = 1050;    //幅
        private static readonly int SCREEN_HEIGHT = 700;    //高さ

        private GameDevice game_device; //ゲームデバイス
        private Renderer renderer;      //レンダラー

        private SceneManager scene_manager;//シーン管理

        private RenderTarget2D sprite_rt;//スプライト
        private CRT crt;//ブラウン管
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;   //幅
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT; //高さ

            Window.Title = "CRTer";//タイトル

            IsMouseVisible = true;//マウスカーソルを表示
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            game_device = GameDevice.GetInstance(this);
            renderer = game_device.GetRenderer();

            scene_manager = SceneManager.GetInstance();

            sprite_rt = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
            crt = new CRT(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            float delta_time = (float)gameTime.ElapsedGameTime.TotalSeconds;//デルタタイム

            game_device.Update(delta_time);
            scene_manager.Update(delta_time);
            crt.Update(delta_time);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0x7F, 0x7F, 0x7F));

            // TODO: Add your drawing code here

            //ペイント
            Paint paint = scene_manager.GetScene().GetMap().GetPaint();
            if (paint != null)
            {
                paint.Draw(renderer);
            }

            //スプライト描画
            GraphicsDevice.SetRenderTarget(sprite_rt);
            scene_manager.Draw(renderer);
            GraphicsDevice.SetRenderTarget(null);

            crt.Draw(renderer);

            base.Draw(gameTime);
        }

        //ゲッター
        public int GetScreenWidth() { return SCREEN_WIDTH; }
        public int GetScreenHeight() { return SCREEN_HEIGHT; }
        public RenderTarget2D GetSpriteRT() { return sprite_rt; }
    }
}
