using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    //入力
    class Input
    {
        #region キーボード

        private static KeyboardState keyboard;
        private static KeyboardState pre_keyboard;//1フレーム前
        
        //押しているか
        public static bool GetKey(Keys _keys)
        {
            return
                keyboard.IsKeyDown(_keys);
        }
        
        //押した瞬間か
        public static bool GetKeyDown(Keys _keys)
        {
            return
                keyboard.IsKeyDown(_keys) &&
                pre_keyboard.IsKeyUp(_keys);
        }

        //離した瞬間か
        public static bool GetKeyUp(Keys _keys)
        {
            return
                keyboard.IsKeyUp(_keys) &&
                pre_keyboard.IsKeyDown(_keys);
        }

        #endregion キーボード

        #region マウス

        private static MouseState mouse;
        private static MouseState pre_mouse;//1フレーム前

        //押しているか
        public static bool GetLMB()
        {
            return
                mouse.LeftButton == ButtonState.Pressed;
        }

        //押した瞬間か
        public static bool GetLMBDown()
        {
            return
                mouse.LeftButton == ButtonState.Pressed &&
                pre_mouse.LeftButton == ButtonState.Released;
        }

        //離した瞬間か
        public static bool GetLMBUp()
        {
            return
                mouse.LeftButton == ButtonState.Released &&
                pre_mouse.LeftButton == ButtonState.Pressed;
        }

        //押しているか
        public static bool GetRMB()
        {
            return
                mouse.RightButton == ButtonState.Pressed;
        }

        //押した瞬間か
        public static bool GetRMBDown()
        {
            return
                mouse.RightButton == ButtonState.Pressed &&
                pre_mouse.RightButton == ButtonState.Released;
        }

        //離した瞬間か
        public static bool GetRMBUp()
        {
            return
                mouse.RightButton == ButtonState.Released &&
                pre_mouse.RightButton == ButtonState.Pressed;
        }

        #endregion マウス

        #region ゲームパッド

        private static GamePadState gamepad;
        private static GamePadState pre_gamepad;//1フレーム前

        //押しているか
        public static bool GetButton(Buttons _buttons)
        {
            return
                gamepad.IsButtonDown(_buttons);
        }

        //押した瞬間か
        public static bool GetButtonDown(Buttons _buttons)
        {
            return
                gamepad.IsButtonDown(_buttons) &&
                pre_gamepad.IsButtonUp(_buttons);
        }

        //離した瞬間か
        public static bool GetButtonsUp(Buttons _buttons)
        {
            return
                gamepad.IsButtonUp(_buttons) &&
                pre_gamepad.IsButtonDown(_buttons);
        }

        #endregion ゲームパッド

        //GameDeviceクラスのみ
        public static void Update(float _delta_time)
        {
            //キーボード更新
            pre_keyboard = keyboard;
            keyboard = Keyboard.GetState();

            //マウス更新
            pre_mouse = mouse;
            mouse = Mouse.GetState();

            //ゲームパッド更新
            pre_gamepad = gamepad;
            gamepad = GamePad.GetState(PlayerIndex.One);
        }
    }
}
