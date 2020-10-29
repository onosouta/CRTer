using Microsoft.Xna.Framework;
using System;

namespace Game2
{
    //衝突コンポーネント
    class CollisionComponent : Component
    {
        private Map map;//マップ

        private Vector3 pre_position;
        private Vector3 pos1, pos2, pos3, pos4;

        public CollisionComponent(Actor _owner, int _width, int _height, int _order = 60) : base(_owner, _order)
        {
            map = owner.GetScene().GetMap();

            pos1 = new Vector3(-_width / 2.0f,  _height / 2.0f, 0.0f);  //左上
            pos2 = new Vector3( _width / 2.0f,  _height / 2.0f, 0.0f);  //右上
            pos3 = new Vector3(-_width / 2.0f, -_height / 2.0f, 0.0f);  //左下
            pos4 = new Vector3( _width / 2.0f, -_height / 2.0f, 0.0f);  //右下
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            map = owner.GetScene().GetMap();//※更新順により必要

            Vector3 position = owner.GetPosition();
            Vector3 velocity = position - pre_position;//差

            DIRECTION d = 0x00;

            if (velocity.Y < 0)//上
            {
                for (int y = 0; y < Math.Abs(Math.Floor(velocity.Y)); y++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(0.0f, -y, 0.0f) +
                        pos3;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.Y = pre_position.Y - y + 1;
                        break;
                    }
                }
                for (int y = 0; y < Math.Abs(Math.Floor(velocity.Y)); y++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(0.0f, -y, 0.0f) +
                        pos4;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.Y = pre_position.Y - y + 1;
                        break;
                    }
                }
            }
            if (velocity.Y > 0)//下
            {
                for (int y = 0; y < Math.Abs(Math.Ceiling(velocity.Y)); y++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(0.0f, y, 0.0f) +
                        pos1;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.Y = pre_position.Y + y - 1;
                        break;
                    }
                }
                for (int y = 0; y < Math.Abs(Math.Ceiling(velocity.Y)); y++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(0.0f, y, 0.0f) +
                        pos2;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.Y = pre_position.Y + y - 1;
                        break;
                    }
                }
            }
            if (velocity.X < 0)//左
            {
                for (int x = 0; x < Math.Abs(Math.Floor(velocity.X)); x++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(-x, 0.0f, 0.0f) +
                        pos1;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.X = pre_position.X - x + 1;
                        break;
                    }
                }
                for (int x = 0; x < Math.Abs(Math.Floor(velocity.X)); x++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(-x, 0.0f, 0.0f) +
                        pos3;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.X = pre_position.X - x + 1;
                        break;
                    }
                }
            }
            if (velocity.X > 0)//右
            {
                for (int x = 0; x < Math.Abs(Math.Ceiling(velocity.X)); x++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(x, 0.0f, 0.0f) +
                        pos2;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.X = pre_position.X + x - 1;
                        break;
                    }
                }
                for (int x = 0; x < Math.Abs(Math.Ceiling(velocity.X)); x++)
                {
                    Vector3 p =
                        pre_position +
                        new Vector3(x, 0.0f, 0.0f) +
                        pos4;
                    if (map.Intersect(p, owner, ref d))
                    {
                        position.X = pre_position.X + x - 1;
                        break;
                    }
                }
            }

            owner.SetPosition(position);
            pre_position = position;
        }

        public void SetPrePosition(Vector3 _pre_position) { pre_position = _pre_position; }
    }
}
