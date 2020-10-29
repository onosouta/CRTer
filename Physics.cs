using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game2
{
    //線分
    struct Line
    {
        public Vector3 start;   //始点
        public Vector3 end;     //終点

        public Line(Vector3 _start, Vector3 _end)
        {
            start = _start;
            end = _end;
        }

        public Vector3 Point(float _t)
        {
            return start + (end - start) *_t;
        }
    }

    //情報
    struct Information
    {
        public Vector3 point;       //座標
        public Vector3 normal;      //法線
        public BoxComponent box;    //ボックスコンポーネント
        public Actor actor;         //アクター
    }

    //物理
    class Physics
    {
        private List<BoxComponent> boxes = new List<BoxComponent>();//ボックスコンポーネント

        public Physics() { }

        public void Update(float _delta_time)
        {
            RemoveBox();//AABBを削除
        }

        public void Draw(Renderer _renderer)
        {
            //描画
            foreach (var b in boxes)
            {
                b.Draw(_renderer);
            }
        }
        
        public bool Intersect(Line _line, Actor _actor, ref Information _information)
        {
            bool is_collision = false;

            float t = 1.0f;
            Vector3 normal = Vector3.Zero;//法線
            foreach (var b in boxes)
            {
                if (_actor == b.GetOwner())//自身
                    continue;

                float _t = 1.0f;
                if (Intersect(_line, b.GetWorldAABB(), ref _t, ref normal))
                {
                    if (_t < t)
                    {
                        _information.point = _line.Point(_t);    //座標
                        _information.normal = normal;           //法線
                        _information.box = b;                   //ボックスコンポーネント
                        _information.actor = b.GetOwner();      //アクター
                        is_collision = true;

                        t = _t;
                    }
                }
            }
            
            return is_collision;
        }

        private bool Intersect(Line _line, AABB _aabb, ref float _t, ref Vector3 _normal)
        {
            List<KeyValuePair<float, Vector3>> values = new List<KeyValuePair<float, Vector3>>();

            Vector3 normal;//法線
            normal = -Vector3.UnitX;
            Intersect(_line.start.X, _line.end.X, _aabb.min.X, ref normal, ref values); //左
            normal = Vector3.UnitX;
            Intersect(_line.start.X, _line.end.X, _aabb.max.X, ref normal, ref values); //右
            normal = -Vector3.UnitY;
            Intersect(_line.start.Y, _line.end.Y, _aabb.min.Y, ref normal, ref values); //下
            normal = Vector3.UnitY;
            Intersect(_line.start.Y, _line.end.Y, _aabb.max.Y, ref normal, ref values); //上
            
            values.Sort((a, b) => (int)(a.Key * 100.0f) - (int)(b.Key * 100.0f));//ソート
            
            foreach (var v in values)
            {
                Vector3　point = _line.Point(v.Key);
                if (_aabb.Contain(ref point))
                {
                    _t = v.Key;
                    _normal = v.Value;

                    return true;
                }
            }

            return false;
        }

        private bool Intersect(float _start, float _end, float _side, ref Vector3 _normal, ref List<KeyValuePair<float, Vector3>> _out)
        {
            float t = (-_start + _side) / (_end - _start);

            if (t >= 0.0f && t <= 1.0f)
            {
                _out.Add(new KeyValuePair<float, Vector3>(t, _normal));
                return true;
            }

            return false;
        }

        //AABBを追加
        public void AddBox(BoxComponent _box)
        {
            int order = _box.GetOrder();

            int i = 0;
            for (; i < boxes.Count; i++)
            {
                if (order < boxes[i].GetOrder())
                {
                    break;
                }
            }

            boxes.Insert(i, _box);
        }

        //AABBを削除
        private void RemoveBox()
        {
            List<BoxComponent> remove_boxes = new List<BoxComponent>();
            foreach (var s in boxes)
            {
                if (s.GetOwner().GetState() == State.Dead)
                {
                    remove_boxes.Add(s);
                }
            }

            for (int i = 0; i < remove_boxes.Count; i++)
            {
                boxes.Remove(remove_boxes[i]);//削除
            }
        }
    }
}
