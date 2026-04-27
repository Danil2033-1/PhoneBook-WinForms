using System;

namespace PhoneBook
{
    public struct TRec
    {
        public string Name;
        public string Phone;
        public TRec(string name, string phone) { Name = name; Phone = phone; }
    }

    public class TAbonent
    {
        private TRec _data;

        public TAbonent(string name = "", string phone = "")
        {
            _data = new TRec(name, phone);
        }

        public string Name { get => _data.Name; set => _data.Name = value; }
        public string Phone { get => _data.Phone; set => _data.Phone = value; }

        public TRec ReadRec() => _data;
        public void WriteRec(TRec rec) => _data = rec;

        public int Less(TRec other)
        {
            int res = string.Compare(_data.Name, other.Name, StringComparison.OrdinalIgnoreCase);
            if (res == 0) res = string.Compare(_data.Phone, other.Phone, StringComparison.OrdinalIgnoreCase);
            return res < 0 ? -1 : (res > 0 ? 1 : 0);
        }

        public bool Equal(TRec other) => _data.Name == other.Name && _data.Phone == other.Phone;

        public override string ToString() => $"{Name}: {Phone}";
    }
}