using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    //ウェーブ
    class Wave
    {
        //ウェーブ用テクスチャ
        struct WaveTexture
        {
            public Texture2D tex;//テクスチャ

            public float scale;         //スケール
            public float init_scale;    //スケール(初期)
        }

        private Scene scene;//シーン

        private WaveTexture wave_in;
        private WaveTexture wave_out;

        private Vector3 position;   //座標
        private float size;         //大きさ
        private float time;         //時間
        private float init_time;    //時間(初期)

        private bool is_dead = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_scene">シーン</param>
        /// <param name="_position">座標</param>
        /// <param name="_size">大きさ</param>
        /// <param name="_time">時間</param>
        public Wave(Scene _scene, Vector3 _position, float _size, float _time)
        {
            scene = _scene;

            position = _position;
            size = _size;
            time = _time;
        }

        //初期化
        public void Initialize()
        {
            init_time = time;

            wave_in.tex = scene.GetRenderer().GetTexture("Texture/wave_in");
            wave_out.tex = scene.GetRenderer().GetTexture("Texture/wave_out");
            wave_in.scale = wave_in.init_scale = 0.0f;
            wave_out.scale = wave_out.init_scale = 0.25f;
        }

        //更新
        public void Update(float _delta_time)
        {
            time = MathHelper.Max(time - _delta_time, 0.0f);
            if (time == 0.0f)
            {
                is_dead = true;
            }

            //スケールを補間
            float t = (init_time - time) / init_time;
            wave_out.scale = Lerp(wave_out.init_scale, t);
            wave_in.scale = Lerp(wave_in.init_scale, t);
        }

        //描画
        public void Draw(Renderer _renderer)
        {
            //out
            Matrix world =
                Matrix.CreateScale(wave_out.scale) *
                Matrix.CreateTranslation(position);
            _renderer.Draw(wave_out.tex, world);

            //in
            world =
                Matrix.CreateScale(wave_in.scale) *
                Matrix.CreateTranslation(position);
            _renderer.Draw(wave_in.tex, world);
        }

        //補間用
        private float Lerp(float _scale, float _t)
        {
            return _scale + (size - _scale) * _t;
        }

        public bool GetIsDead() { return is_dead; }
    }
}
