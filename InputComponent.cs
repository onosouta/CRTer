using Microsoft.Xna.Framework.Input;
using System;

namespace Game2
{
    //入力コンポーネント
    class InputComponent : Component
    {
        private MoveComponent mc;//移動コンポーネント

        public InputComponent(Actor _owner, int _order = 10) : base(_owner, _order)
        {
            mc = new MoveComponent(owner);
            GameDevice.GetInstance().GetSE().Load("se/dush");
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            //移動
            float front_speed = Math.Max(mc.GetFrontSpeed() - 600.0f, 0.0f);

            //上
            if (Input.GetKey(Keys.W))// ||                         //W
                //Input.GetKey(Keys.Up) ||                        //↑
                //Input.GetButton(Buttons.LeftThumbstickUp) ||
                //Input.GetButton(Buttons.DPadUp))
            {
                front_speed += 300.0f;
            }

            //ダッシュ
            if (Input.GetRMBDown())// ||           //RMB
                //Input.GetKeyDown(Keys.X) ||     //X
                //Input.GetButtonDown(Buttons.A)) //A
            {
                front_speed = Math.Min(front_speed + 3000.0f, 3000.0f);

                //ウェーブ
                Scene scene = owner.GetScene();
                Wave wave = new Wave(
                    scene,                  //scene
                    owner.GetPosition(),    //position
                    0.5f,                   //size
                    0.25f);                 //time
                scene.GetWaveManager().AddWave(wave);//追加
                
                //振動
                Display display = GameDevice.GetInstance().GetDisplay();
                display.is_wave = true;
                display.is_x = true;
                display.is_y = true;
                display.speed = 5.0f;

                GameDevice.GetInstance().GetSE().Play("se/dush");
            }

            mc.SetFrontSpeed(front_speed);

            //回転
            float angle_speed = 0.0f;

            //左
            if (Input.GetKey(Keys.A))// ||                         //A
                //Input.GetKey(Keys.Left) ||                      //←
                //Input.GetButton(Buttons.RightThumbstickLeft))
            {
                angle_speed += (float)Math.PI * 2.0f;
            }

            //右
            if (Input.GetKey(Keys.D))// ||                         //D
                //Input.GetKey(Keys.Right) ||                     //→
                //Input.GetButton(Buttons.RightThumbstickRight))
            {
                angle_speed -= (float)Math.PI * 2.0f;
            }

            //発射中
            if (Input.GetLMB())// ||                       //LMB
                //Input.GetKey(Keys.Z) ||                 //Z
                //Input.GetButton(Buttons.RightShoulder)) //RB
            {
                angle_speed /= 1.75f;
            }

            mc.SetAngleSpeed(angle_speed);
        }

        public float GetFrontSpeed() { return mc.GetFrontSpeed(); }
    }
}
