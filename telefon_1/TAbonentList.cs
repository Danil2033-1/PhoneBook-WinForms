using System;
using System.Collections.Generic;

namespace PhoneBook
{
    public class TAbonentList
    {
        private List<TAbonent> _list;

        public TAbonentList() => _list = new List<TAbonent>();

        public int Count => _list.Count;
        public TAbonent this[int index] { get => _list[index]; set => _list[index] = value; }

        public void Add(TAbonent item) { _list.Add(item); Sort(); }
        public void Clear() => _list.Clear();

        public int Find(TRec rec)
        {
            for (int i = 0; i < _list.Count; i++)
                if (_list[i].Equal(rec)) return i;
            return -1;
        }

        public int FindFlexible(string nameSub, string phoneSub)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                bool nameMatch = string.IsNullOrEmpty(nameSub) ||
                                 _list[i].Name.IndexOf(nameSub, StringComparison.OrdinalIgnoreCase) >= 0;
                bool phoneMatch = string.IsNullOrEmpty(phoneSub) ||
                                  _list[i].Phone.IndexOf(phoneSub, StringComparison.OrdinalIgnoreCase) >= 0;

                if (nameMatch && phoneMatch)
                    return i;
            }
            return -1;
        }

        public void DeleteByIndex(int index)
        {
            if (index >= 0 && index < _list.Count) _list.RemoveAt(index);
        }

        public void DeleteByValue(TRec rec)
        {
            int idx = Find(rec);
            if (idx != -1) DeleteByIndex(idx);
        }

        public void Sort() => _list.Sort((x, y) => x.Less(y.ReadRec()));
    }
}