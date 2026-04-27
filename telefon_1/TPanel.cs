using System; // директива для базовых типов данных 
using System.Windows.Forms; // для работы с графическим интерфейсом и кнопками
using PhoneBook;

namespace telefon_1
{
    public partial class TPanel : Form
    {
        private readonly TControl _ctrl;
        private int _editingIndex = -1;

        public TPanel()
        {
            InitializeComponent();
            _ctrl = new TControl();

            UpdateList();
            SetState(isEditing: false);

            listBoxPhoneBook.DoubleClick += btnDelete_Click;
        }

        private void SetState(bool isEditing)
        {
            btnUpdate.Text = isEditing ? "Сохранить" : "Изменить";
            btnAdd.Enabled = !isEditing;
            btnDelete.Enabled = !isEditing;
            btnClear.Enabled = !isEditing;
            btnLoad.Enabled = !isEditing;
            btnSave.Enabled = !isEditing;

            if (!isEditing)
            {
                _editingIndex = -1;
                txtName.Clear();
                txtPhone.Clear();
            }
        }

        private void UpdateList()
        {
            listBoxPhoneBook.Items.Clear();
            for (int i = 0; i < _ctrl.Count; i++)
            {
                listBoxPhoneBook.Items.Add(_ctrl.GetRecord(i));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;

            _ctrl.AddRecord(txtName.Text.Trim(), txtPhone.Text.Trim());
            UpdateList();
            SetState(isEditing: false);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_editingIndex == -1)
            {
                if (listBoxPhoneBook.SelectedIndex >= 0)
                {
                    _editingIndex = listBoxPhoneBook.SelectedIndex;
                    txtName.Text = _ctrl.GetName(_editingIndex);
                    txtPhone.Text = _ctrl.GetPhone(_editingIndex);
                    SetState(isEditing: true);
                }
            }
            else
            {
                // Сначала удаляем старую запись, потом добавляем новую (чтобы сохранить сортировку)
                _ctrl.DeleteByIndex(_editingIndex);
                _ctrl.AddRecord(txtName.Text.Trim(), txtPhone.Text.Trim());
                UpdateList();
                SetState(isEditing: false);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxPhoneBook.SelectedIndex < 0) return;

            _ctrl.DeleteByIndex(listBoxPhoneBook.SelectedIndex);
            UpdateList();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int index = _ctrl.FindRecordFlexible(txtName.Text.Trim(), txtPhone.Text.Trim());
            if (index != -1)
            {
                listBoxPhoneBook.SelectedIndex = index;
                listBoxPhoneBook.TopIndex = index; // прокрутит к найденному, если он не виден
            }
            else
            {
                MessageBox.Show("Запись не найдена", "Поиск");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PhoneBook|*.dat" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    _ctrl.CreateFile(sfd.FileName);
                    _ctrl.SaveToFile();
                    MessageBox.Show("Книга сохранена");
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "PhoneBook|*.dat" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _ctrl.CreateFile(ofd.FileName);
                    _ctrl.LoadFromFile();
                    UpdateList();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _ctrl.ClearBook();
            UpdateList();
        }

        private void listBoxPhoneBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_editingIndex != -1 && listBoxPhoneBook.SelectedIndex != _editingIndex)
            {
                SetState(isEditing: false);
            }
        }
    }
}