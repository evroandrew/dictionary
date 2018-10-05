using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class MyDictionary<Tkey, TValue> : IDictionary<Tkey, TValue>
        where Tkey : IComparable<Tkey>
    {
        private class TreeItem
        {
            public KeyValuePair<Tkey, TValue> Pair { get; set; }
            public TreeItem Parent { get; set; }
            public TreeItem Left { get; set; }
            public TreeItem Right { get; set; }
        }
        private TreeItem root = null;
        public bool IsRepeatKeys { get; private set; }
        public MyDictionary(bool isRepeatKeys = false)
        {
            IsRepeatKeys = isRepeatKeys;
        }
        private TreeItem GetItem(Tkey key)
        {
            return Search(root, key);
        }
        private TreeItem Search(TreeItem item, Tkey key)
        {
            if (item == null || item.Pair.Key.CompareTo(key) == 0)
                return item;
            if (item.Pair.Key.CompareTo(key) == 1)
                return Search(item.Left, key);
            else
                return Search(item.Right, key);
        }
        public TValue this[Tkey key]
        {
            get
            {
                TreeItem item = GetItem(key);
                if (item == null) throw new IndexOutOfRangeException();
                return item.Pair.Value;
            }
            set
            {
                TreeItem item = GetItem(key);
                if (item == null)
                    Add(key, value);
                else
                    item.Pair = new KeyValuePair<Tkey, TValue>(key, value);
            }
        }

        public ICollection<Tkey> Keys
        {
            get
            {
                List<Tkey> keys = new List<Tkey>(Count);
                using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
                {
                    while (en.MoveNext())
                    {
                        keys.Add(en.Current.Pair.Key);
                    }
                }
                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>(Count);
                using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
                {
                    while (en.MoveNext())
                    {
                        values.Add(en.Current.Pair.Value);
                    }
                }
                return values;
            }
        }

        public int Count
        {
            get;
            private set;
        } = 0;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(KeyValuePair<Tkey, TValue> pair)
        {
            if (root == null)
            {
                root = new TreeItem { Pair = pair };
                Count++;
            }
            else
            {
                Add(pair, root);
            }
        }
        public void Add(Tkey key, TValue val)
        {
            Add(new KeyValuePair<Tkey, TValue>(key, val));
        }
        private void Add(KeyValuePair<Tkey, TValue> pair, TreeItem item)
        {
            if (!IsRepeatKeys & item.Pair.Key.CompareTo(pair.Key) == 0)
            {
                item.Pair = pair;
                return;
            }

            if (item.Pair.Key.CompareTo(pair.Key) > 0)
                if (item.Left == null)
                {
                    item.Left = new TreeItem { Pair = pair, Parent = item };
                    Count++;
                }
                else
                    Add(pair, item.Left);
            else
            {
                if (item.Right == null)
                {
                    item.Right = new TreeItem { Pair = pair, Parent = item };
                    Count++;
                }
                else
                    Add(pair, item.Right);
            }
        }

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public bool Contains(KeyValuePair<Tkey, TValue> pair)
        {
            TreeItem item = GetItem(pair.Key);
            return item != null && item.Pair.Value.Equals(pair.Value);
        }

        public bool ContainsKey(Tkey key)
        {
            TreeItem item = GetItem(key);
            return item != null && item.Pair.Key.Equals(key);
        }

        public void CopyTo(KeyValuePair<Tkey, TValue>[] array, int arrayIndex)
        {
            using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
            {
                while (en.MoveNext())
                {
                    array[arrayIndex++] = en.Current.Pair;
                }
            }
        }


        public bool Remove(Tkey key)
        {
            TreeItem item = GetItem(key);
            if (item == null) return false;
            RemoveItem(item);
            return true;
        }

        private void RemoveItem(TreeItem item)
        {
            if (item != null)
                Count--;
            if (item.Left == null && item.Right == null)
                RemoveItemWithoutChildren(item);
            else if (item.Left != null && item.Right != null)
                RemoveItemWithTwoChildren(item);
            else
                RemoveItemWithOneChildren(item);
        }
        private void RemoveItemWithoutChildren(TreeItem item)
        {
            if (item == root)
            {
                root = null;
                Count = 0;
                return;
            }
            if (item.Parent.Left == item)
                item.Parent.Left = null;
            else
                item.Parent.Right = null;
        }
        private void RemoveItemWithTwoChildren(TreeItem item)
        {
            TreeItem temp;
            temp = item.Right;
            while (temp.Left != null)
                temp = temp.Left;
            item.Pair =new KeyValuePair<Tkey, TValue>(temp.Pair.Key,temp.Pair.Value);
            temp.Parent = temp.Right;
        }
        private void RemoveItemWithOneChildren(TreeItem item)
        {
            if (item.Left == null)
            {
                if (item == root)
                    root = item.Right;
                if (item.Parent.Left == item)
                    item.Parent.Left = item.Right;
                if(item.Parent.Right == item)
                    item.Parent.Right = item.Right;
            }
            else
            {
                if (item == root)
                    root = item.Left;
                if (item.Parent.Left == item)
                    item.Parent.Left = item.Left;
                if (item.Parent.Right == item)
                    item.Parent.Right = item.Left;
            }
        }

        public bool Remove(KeyValuePair<Tkey, TValue> pair)
        {
            TreeItem item = GetItem(pair.Key);
            if (item == null || !item.Pair.Equals(pair)) return false;
            RemoveItem(item);
            return true;
        }

        public bool TryGetValue(Tkey key, out TValue value)
        {
            TreeItem item = GetItem(key);
            if (item == null)
            {
                value = default(TValue);
                return false;
            }
            value = item.Pair.Value;
            return true;
        }

        private IEnumerator<TreeItem> GetTreeItemEnumerator(TreeItem item)
        {
            Stack<TreeItem> items = new Stack<TreeItem>();
            while (item != null || items.Count != 0)
            {
                if (items.Count != 0)
                {
                    item = items.Pop();
                    yield return item;
                    if (item.Right != null)
                        item = item.Right;
                    else
                        item = null;
                }
                while (item != null)
                {
                    items.Push(item);
                    item = item.Left;
                }
            }
        }

        public IEnumerator<KeyValuePair<Tkey, TValue>> GetEnumerator()
        {
            using (IEnumerator<TreeItem> en = GetTreeItemEnumerator(root))
            {
                while (en.MoveNext())
                {
                    yield return en.Current.Pair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
