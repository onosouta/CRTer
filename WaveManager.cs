using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game2
{
    //ウェーブ管理
    class WaveManager
    {
        private Renderer renderer;
        private RenderTarget2D wave_rt;//ターゲット

        private List<Wave> waves = new List<Wave>();
        
        public WaveManager(Scene _scene)
        {
            renderer = _scene.GetRenderer();

            wave_rt = new RenderTarget2D(
                renderer.GetGraphics(),
                renderer.GetScreenWidth(),
                renderer.GetScreenHeight());
        }

        //更新
        public void Update(float _delta_time)
        {
            foreach (var w in waves)
            {
                w.Update(_delta_time);
            }

            RemoveWave();//削除
        }

        //描画
        public void Draw(Renderer _renderer)
        {
            renderer.GetGraphics().SetRenderTarget(wave_rt);

            foreach (var w in waves)
            {
                w.Draw(_renderer);
            }

            renderer.GetGraphics().SetRenderTarget(null);
        }

        //ウェーブを追加
        public void AddWave(Wave _wave)
        {
            _wave.Initialize();
            waves.Add(_wave);
        }

        //ウェーブを削除
        public void RemoveWave()
        {
            List<Wave> remove_wave = new List<Wave>();
            foreach (var w in waves)
            {
                if (w.GetIsDead())
                {
                    remove_wave.Add(w);
                }
            }

            for (int i = 0; i < remove_wave.Count; i++)
            {
                waves.Remove(remove_wave[i]);//削除
            }
        }

        //ゲッター
        public RenderTarget2D GetWaveRT() { return wave_rt; }
        public List<Wave> GetWaves() { return waves; }
    }
}
