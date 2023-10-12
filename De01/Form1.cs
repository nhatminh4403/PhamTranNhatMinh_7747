using BUS;
using DAL.Entitiy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace De01
{
    public partial class frmSinhvien : Form
    {
        private readonly ClassroomService classroom = new ClassroomService();
        private readonly StudentService student = new StudentService();
        QLSVModel model;
        public frmSinhvien()
        {
            InitializeComponent();
        }
        private void LoadList()
        {
            List<Sinhvien> sv = student.GetAllSinhviens();
            BindGrid(sv);
        }
        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                model = new QLSVModel();
                if (txtMaSV.Text == "" || txtHotenSV.Text == "")
                    throw new Exception("Vui lòng điền thông tin vào ô trống");
                if(txtMaSV.Text.Length!=6)
                    throw new Exception("Vui lòng nhập 6 ký tự vào ô Mã Sinh Viên");
                foreach (DataGridViewRow i in dgvSinhVien.Rows)
                {
                    if (txtMaSV.Text == dgvSinhVien.Rows[i.Index].Cells[0].Value.ToString())
                        throw new Exception("ID đã tồn tại");
                }
                var item = new Sinhvien()
                {
                    MaSV = txtMaSV.Text,
                    HotenSV = txtHotenSV.Text,
                    NgaySinh = DateTime.Parse(dtNgaysinh.Value.ToString("dd-MM-yyyy")),
                    MaLop = cboLop.SelectedValue.ToString()
                };
                student.InsertUpdate(item);
                LoadList();
                MessageBox.Show("Thêm mới thành công", "Thông báo", MessageBoxButtons.OK);
                RefreshInput();

                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var svDelete = student.FindById(txtMaSV.Text);
                Sinhvien sv = student.FindById(txtMaSV.Text);
                if (svDelete != null) 
                {
                    DialogResult d = MessageBox.Show("Bạn có muốn xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (d == DialogResult.Yes)
                    {
                        student.DeleteStudent(txtMaSV.Text);         
                        MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK);
                        RefreshInput();
                        LoadList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo",MessageBoxButtons.OK);
            }
        }
        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Bạn muốn thoát?", "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d == DialogResult.Yes)
                this.Close();
        }
        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                model = new QLSVModel();
                Sinhvien sv = student.FindById(txtMaSV.Text);
                if(sv != null)
                {
                    sv.HotenSV = txtHotenSV.Text;
                    sv.NgaySinh = DateTime.Parse(dtNgaysinh.Value.ToString("dd-MM-yyyy"));
                    sv.MaLop = cboLop.SelectedValue.ToString();
                    student.InsertUpdate(sv);
                    RefreshInput();
                    LoadList();
                }
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật", "Thông báo", MessageBoxButtons.OK);
            }
        }
        private void FillComboBox(List<Lop> list)
        {
            this.cboLop.DataSource = list;
            this.cboLop.DisplayMember = "TenLop";
            this.cboLop.ValueMember = "MaLop";
        }
        private void RefreshInput()
        {
            txtMaSV.Clear();
            txtMaSV.Focus();
            txtHotenSV.Clear();
            dtNgaysinh.Value = DateTime.Now;
            cboLop.SelectedValue = 1;
        }
        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            btLuu.Enabled = false;
            btKhong.Enabled = false;
            var listClass = classroom.GetAllClassrooms();
            var listStud = student.GetAllSinhviens();
            BindGrid(listStud);
            FillComboBox(listClass);
        }
        private void BindGrid(List<Sinhvien> sv)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in sv)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaSV;
                dgvSinhVien.Rows[index].Cells[1].Value = item.HotenSV;
                dgvSinhVien.Rows[index].Cells[2].Value = item.NgaySinh;
                dgvSinhVien.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach(DataGridViewRow r in dgvSinhVien.Rows)
            {
                if (r.Selected)
                {
                    txtMaSV.Text = dgvSinhVien.Rows[r.Index].Cells[0].Value.ToString();
                    txtHotenSV.Text = dgvSinhVien.Rows[r.Index].Cells[1].Value.ToString();
                    dtNgaysinh.Value =DateTime.Parse(dgvSinhVien.Rows[r.Index].Cells[2].Value.ToString());
                    cboLop.Text = dgvSinhVien.Rows[r.Index].Cells[3].Value.ToString();
                }
            }
        }

        private void txtHotenSV_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
                    !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            // List < Sinhvien > sinhviens = new List<Sinhvien>();
            string findName = txtFind.Text;
            findName = RemoveDiacritics(findName);
            for (int i = 0; i < dgvSinhVien.Rows.Count; i++)
            {
                string name = dgvSinhVien.Rows[i].Cells[1].Value.ToString();
                name = RemoveDiacritics(name);
                bool contains = name.IndexOf(findName, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    dgvSinhVien.Rows[i].Visible = true;
                }
                else
                {
                    dgvSinhVien.Rows[i].Visible = false;
                }
            }
        }
        private string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
