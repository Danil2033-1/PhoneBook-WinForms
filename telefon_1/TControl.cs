using System;
using System.IO;
using System.Text;

namespace PhoneBook
{
    public class TControl
    {
        private TAbonentList _list;
        private string _currentPath;

        public TControl() => _list = new TAbonentList();

        public int Count => _list.Count;
        public string GetName(int i) => _list[i].Name;
        public string GetPhone(int i) => _list[i].Phone;
        public string GetRecord(int i) => _list[i].ToString();

        public void AddRecord(string n, string p) => _list.Add(new TAbonent(n, p));
        public void DeleteByIndex(int i) => _list.DeleteByIndex(i);
        public void ClearBook() => _list.Clear();

        public int FindRecord(string n, string p) => _list.Find(new TRec(n, p));

        public int FindRecordFlexible(string name, string phone) => _list.FindFlexible(name, phone);

        public void CreateFile(string path) => _currentPath = path;

        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(_currentPath)) return;
            using (FileStream fs = new FileStream(_currentPath, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(_list.Count);
                for (int i = 0; i < _list.Count; i++)
                {
                    bw.Write(_list[i].Name);
                    bw.Write(_list[i].Phone);
                }
            }
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_currentPath)) return;
            using (FileStream fs = new FileStream(_currentPath, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                _list.Clear();
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                    _list.Add(new TAbonent(br.ReadString(), br.ReadString()));
            }
        }
    }
}