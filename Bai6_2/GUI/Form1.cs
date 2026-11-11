using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Bai6_2.BUS;
using Bai6_2.DTO;

namespace Bai6_2
{
    public partial class Form1 : Form
    {
        private readonly NhanVienBUS nvBUS = new NhanVienBUS();
        private readonly PhongBanBUS pbBUS = new PhongBanBUS();
        private readonly ChamCongBUS ccBUS = new ChamCongBUS();

        private DataTable dtChamCong = new DataTable();

        // Nhân viên
        private DataGridView dgvNhanVien = new DataGridView();

        private TextBox txtMaNV = new TextBox();
        private TextBox txtHoTen = new TextBox();
        private TextBox txtCCCD = new TextBox();
        private TextBox txtLuong = new TextBox();
        private TextBox txtHeSo = new TextBox();
        private TextBox txtTimKiem = new TextBox();

        private DateTimePicker dtpNgaySinh = new DateTimePicker();
        private ComboBox cboPhongBan = new ComboBox();

        // Chấm công
        private DataGridView dgvChamCong = new DataGridView();
        private TextBox txtThangChamCong = new TextBox();

        // Lương
        private DataGridView dgvBangLuong = new DataGridView();
        private TextBox txtThangLuong = new TextBox();

        public Form1()
        {
            InitializeComponent();
            BuildInterface();

            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPhongBan();
                LoadNhanVien();

                string thang = DateTime.Now.ToString("MM/yyyy");

                txtThangChamCong.Text = thang;
                txtThangLuong.Text = thang;

                LoadChamCong();
                LoadBangLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================= GIAO DIỆN =================

        private void BuildInterface()
        {
            TabControl tabs = new TabControl();

            tabs.Dock = DockStyle.Fill;

            TabPage tabNhanVien =
                new TabPage("Quản lý nhân viên");

            TabPage tabChamCong =
                new TabPage("Chấm công");

            TabPage tabLuong =
                new TabPage("Tính lương");

            tabs.TabPages.Add(tabNhanVien);
            tabs.TabPages.Add(tabChamCong);
            tabs.TabPages.Add(tabLuong);

            Controls.Add(tabs);

            BuildNhanVienTab(tabNhanVien);
            BuildChamCongTab(tabChamCong);
            BuildLuongTab(tabLuong);
        }

        private void BuildNhanVienTab(TabPage tab)
        {
            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ NHÂN VIÊN";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 15);

            tab.Controls.Add(lblTitle);

            AddLabel(tab, "Mã nhân viên:", 20, 65);
            txtMaNV.Location = new Point(140, 62);
            txtMaNV.Width = 220;
            txtMaNV.ReadOnly = true;
            tab.Controls.Add(txtMaNV);

            AddLabel(tab, "Họ tên:", 20, 105);
            txtHoTen.Location = new Point(140, 102);
            txtHoTen.Width = 220;
            tab.Controls.Add(txtHoTen);

            AddLabel(tab, "Ngày sinh:", 20, 145);
            dtpNgaySinh.Location = new Point(140, 142);
            dtpNgaySinh.Width = 220;
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            tab.Controls.Add(dtpNgaySinh);

            AddLabel(tab, "CCCD:", 400, 65);
            txtCCCD.Location = new Point(510, 62);
            txtCCCD.Width = 220;
            tab.Controls.Add(txtCCCD);

            AddLabel(tab, "Phòng ban:", 400, 105);
            cboPhongBan.Location = new Point(510, 102);
            cboPhongBan.Width = 220;
            cboPhongBan.DropDownStyle =
                ComboBoxStyle.DropDownList;
            tab.Controls.Add(cboPhongBan);

            AddLabel(tab, "Lương cơ bản:", 400, 145);
            txtLuong.Location = new Point(510, 142);
            txtLuong.Width = 220;
            tab.Controls.Add(txtLuong);

            AddLabel(tab, "Hệ số lương:", 760, 65);
            txtHeSo.Location = new Point(870, 62);
            txtHeSo.Width = 180;
            tab.Controls.Add(txtHeSo);

            Button btnThem = CreateButton(
                "Thêm", 20, 195, 100);

            Button btnSua = CreateButton(
                "Sửa", 130, 195, 100);

            Button btnXoa = CreateButton(
                "Xóa", 240, 195, 100);

            Button btnMoi = CreateButton(
                "Làm mới", 350, 195, 100);

            Button btnTim = CreateButton(
                "Tìm kiếm", 870, 195, 100);

            txtTimKiem.Location = new Point(510, 198);
            txtTimKiem.Width = 350;
            tab.Controls.Add(txtTimKiem);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnMoi.Click += BtnMoi_Click;
            btnTim.Click += BtnTim_Click;

            tab.Controls.AddRange(new Control[]
            {
                btnThem, btnSua, btnXoa, btnMoi, btnTim
            });

            dgvNhanVien.Location = new Point(20, 245);
            dgvNhanVien.Size = new Size(1030, 380);
            SetupGrid(dgvNhanVien);

            dgvNhanVien.CellClick += DgvNhanVien_CellClick;

            tab.Controls.Add(dgvNhanVien);
        }

        private void BuildChamCongTab(TabPage tab)
        {
            Label title = new Label();
            title.Text = "QUẢN LÝ CHẤM CÔNG";
            title.Font = new Font("Arial", 16, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(20, 20);

            tab.Controls.Add(title);

            AddLabel(tab, "Tháng/Năm (MM/yyyy):", 20, 75);

            txtThangChamCong.Location = new Point(200, 72);
            txtThangChamCong.Width = 150;
            tab.Controls.Add(txtThangChamCong);

            Button btnTai = CreateButton(
                "Tải chấm công", 370, 68, 150);

            Button btnLuu = CreateButton(
                "Lưu chấm công", 530, 68, 150);

            Button btnChot = CreateButton(
                "Chốt lương", 690, 68, 150);

            btnTai.Click += BtnTaiChamCong_Click;
            btnLuu.Click += BtnLuuChamCong_Click;
            btnChot.Click += BtnChotLuong_Click;

            tab.Controls.AddRange(new Control[]
            {
                btnTai, btnLuu, btnChot
            });

            dgvChamCong.Location = new Point(20, 125);
            dgvChamCong.Size = new Size(1030, 480);
            SetupGrid(dgvChamCong);

            tab.Controls.Add(dgvChamCong);
        }

        private void BuildLuongTab(TabPage tab)
        {
            Label title = new Label();
            title.Text = "BẢNG LƯƠNG NHÂN VIÊN";
            title.Font = new Font("Arial", 16, FontStyle.Bold);
            title.AutoSize = true;
            title.Location = new Point(20, 20);

            tab.Controls.Add(title);

            AddLabel(tab, "Tháng/Năm (MM/yyyy):", 20, 75);

            txtThangLuong.Location = new Point(200, 72);
            txtThangLuong.Width = 150;
            tab.Controls.Add(txtThangLuong);

            Button btnXem = CreateButton(
                "Xem bảng lương", 370, 68, 150);

            btnXem.Click += BtnXemLuong_Click;

            tab.Controls.Add(btnXem);

            dgvBangLuong.Location = new Point(20, 125);
            dgvBangLuong.Size = new Size(1030, 480);
            SetupGrid(dgvBangLuong);

            tab.Controls.Add(dgvBangLuong);
        }

        private void AddLabel(
            Control parent,
            string text,
            int x,
            int y)
        {
            Label lbl = new Label();

            lbl.Text = text;
            lbl.Location = new Point(x, y);
            lbl.AutoSize = true;
            lbl.Font = new Font("Arial", 9);

            parent.Controls.Add(lbl);
        }

        private Button CreateButton(
            string text,
            int x,
            int y,
            int width)
        {
            Button btn = new Button();

            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(width, 32);
            btn.BackColor = Color.LightSteelBlue;

            return btn;
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
        }

        // ================= PHÒNG BAN =================

        private void LoadPhongBan()
        {
            DataTable dt = pbBUS.GetAll();

            cboPhongBan.DataSource = dt;
            cboPhongBan.DisplayMember = "TenPB";
            cboPhongBan.ValueMember = "MaPB";
        }

        // ================= NHÂN VIÊN =================

        private void LoadNhanVien()
        {
            dgvNhanVien.DataSource = nvBUS.GetAll();

            if (dgvNhanVien.Columns.Contains("MaPB"))
                dgvNhanVien.Columns["MaPB"].Visible = false;

            if (dgvNhanVien.Columns.Contains("NgaySinh"))
                dgvNhanVien.Columns["NgaySinh"].DefaultCellStyle
                    .Format = "dd/MM/yyyy";

            if (dgvNhanVien.Columns.Contains("LuongCoBan"))
                dgvNhanVien.Columns["LuongCoBan"].DefaultCellStyle
                    .Format = "N0";
        }

        private NhanVienDTO GetNhanVienFromForm()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                throw new Exception("Vui lòng nhập họ tên.");

            if (cboPhongBan.SelectedValue == null)
                throw new Exception("Vui lòng chọn phòng ban.");

            if (!decimal.TryParse(
                txtLuong.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out decimal luong))
            {
                throw new Exception("Lương cơ bản không hợp lệ.");
            }

            if (!double.TryParse(
                txtHeSo.Text,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out double heSo))
            {
                throw new Exception("Hệ số lương không hợp lệ.");
            }

            return new NhanVienDTO
            {
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value,
                CCCD = txtCCCD.Text.Trim(),
                MaPB = Convert.ToInt32(cboPhongBan.SelectedValue),
                LuongCoBan = luong,
                HeSoLuong = heSo
            };
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = GetNhanVienFromForm();

                nvBUS.Add(nv);

                LoadNhanVien();

                MessageBox.Show("Thêm nhân viên thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa.");
                    return;
                }

                NhanVienDTO nv = GetNhanVienFromForm();
                nv.MaNV = Convert.ToInt32(txtMaNV.Text);

                nvBUS.Update(nv);

                LoadNhanVien();

                MessageBox.Show("Cập nhật nhân viên thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo");
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa.");
                    return;
                }

                int maNV = Convert.ToInt32(txtMaNV.Text);

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa nhân viên này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                nvBUS.Delete(maNV);

                LoadNhanVien();

                MessageBox.Show("Xóa nhân viên thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa nhân viên. Có thể nhân viên đã có dữ liệu chấm công hoặc bảng lương.\n"
                    + ex.Message);
            }
        }

        private void BtnMoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtLuong.Clear();
            txtHeSo.Clear();

            dtpNgaySinh.Value = DateTime.Now;

            if (cboPhongBan.Items.Count > 0)
                cboPhongBan.SelectedIndex = 0;

            txtHoTen.Focus();
        }

        private void BtnTim_Click(object sender, EventArgs e)
        {
            try
            {
                dgvNhanVien.DataSource =
                    nvBUS.Search(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgvNhanVien_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dgvNhanVien.Rows[e.RowIndex];

            txtMaNV.Text =
                Convert.ToString(row.Cells["MaNV"].Value);

            txtHoTen.Text =
                Convert.ToString(row.Cells["HoTen"].Value);

            txtCCCD.Text =
                Convert.ToString(row.Cells["CCCD"].Value);

            txtLuong.Text =
                Convert.ToString(row.Cells["LuongCoBan"].Value);

            txtHeSo.Text =
                Convert.ToString(row.Cells["HeSoLuong"].Value);

            object ngaySinh = row.Cells["NgaySinh"].Value;

            if (ngaySinh != null && ngaySinh != DBNull.Value)
            {
                dtpNgaySinh.Value = Convert.ToDateTime(ngaySinh);
            }

            object maPB = row.Cells["MaPB"].Value;

            if (maPB != null && maPB != DBNull.Value)
            {
                cboPhongBan.SelectedValue =
                    Convert.ToInt32(maPB);
            }
        }

        // ================= CHẤM CÔNG =================

        private bool IsValidMonth(string value)
        {
            return DateTime.TryParseExact(
                value,
                "MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
        }

        private void LoadChamCong()
        {
            string thangNam = txtThangChamCong.Text.Trim();

            if (!IsValidMonth(thangNam))
            {
                MessageBox.Show(
                    "Tháng/Năm phải có định dạng MM/yyyy.");

                return;
            }

            dtChamCong = ccBUS.LoadChamCong(thangNam);

            dgvChamCong.DataSource = dtChamCong;

            if (dgvChamCong.Columns.Contains("MaNV"))
                dgvChamCong.Columns["MaNV"].HeaderText =
                    "Mã nhân viên";

            if (dgvChamCong.Columns.Contains("HoTen"))
                dgvChamCong.Columns["HoTen"].HeaderText =
                    "Họ tên";

            if (dgvChamCong.Columns.Contains("SoNgayCong"))
            {
                dgvChamCong.Columns["SoNgayCong"].HeaderText =
                    "Số ngày công";

                dgvChamCong.Columns["SoNgayCong"].ReadOnly = false;
            }

            if (dgvChamCong.Columns.Contains("SoGioOT"))
            {
                dgvChamCong.Columns["SoGioOT"].HeaderText =
                    "Số giờ OT";

                dgvChamCong.Columns["SoGioOT"].ReadOnly = false;
            }

            if (dgvChamCong.Columns.Contains("DaChot"))
            {
                dgvChamCong.Columns["DaChot"].Visible = false;
            }

            if (dgvChamCong.Columns.Contains("MaNV"))
                dgvChamCong.Columns["MaNV"].ReadOnly = true;

            if (dgvChamCong.Columns.Contains("HoTen"))
                dgvChamCong.Columns["HoTen"].ReadOnly = true;

            dgvChamCong.ReadOnly = false;
        }

        private void BtnTaiChamCong_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                LoadChamCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnLuuChamCong_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (!IsValidMonth(txtThangChamCong.Text.Trim()))
                {
                    MessageBox.Show(
                        "Tháng/Năm không hợp lệ. Ví dụ: 11/2026.");

                    return;
                }

                dgvChamCong.EndEdit();

                if (dgvChamCong.DataSource is DataTable dt)
                {
                    ccBUS.SaveChamCong(
                        dt,
                        txtThangChamCong.Text.Trim());
                }

                MessageBox.Show("Lưu chấm công thành công.");

                // Tải lại để đồng bộ dữ liệu sau khi INSERT
                LoadChamCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lưu chấm công:\n" + ex.Message);
            }
        }

        // ================= TÍNH LƯƠNG =================

        private void BtnChotLuong_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string thangNam =
                    txtThangChamCong.Text.Trim();

                if (!IsValidMonth(thangNam))
                {
                    MessageBox.Show(
                        "Tháng/Năm không hợp lệ.");

                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn chốt lương tháng "
                    + thangNam + "?\nSau khi chốt sẽ không thể sửa chấm công.",
                    "Xác nhận chốt lương",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                // Lưu dữ liệu chấm công trước khi chốt
                dgvChamCong.EndEdit();

                if (dgvChamCong.DataSource is DataTable dt)
                {
                    ccBUS.SaveChamCong(dt, thangNam);
                }

                ccBUS.ChotLuong(thangNam);

                MessageBox.Show(
                    "Chốt lương thành công.");

                txtThangLuong.Text = thangNam;

                LoadChamCong();
                LoadBangLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể chốt lương:\n" + ex.Message);
            }
        }

        private void LoadBangLuong()
        {
            string thangNam = txtThangLuong.Text.Trim();

            if (!IsValidMonth(thangNam))
                return;

            dgvBangLuong.DataSource =
                ccBUS.GetBangLuong(thangNam);

            if (dgvBangLuong.Columns.Contains("ThucLinh"))
            {
                dgvBangLuong.Columns["ThucLinh"]
                    .DefaultCellStyle.Format = "N0";
            }

            if (dgvBangLuong.Columns.Contains("MaLuong"))
            {
                dgvBangLuong.Columns["MaLuong"].HeaderText =
                    "Mã lương";
            }

            if (dgvBangLuong.Columns.Contains("MaNV"))
            {
                dgvBangLuong.Columns["MaNV"].HeaderText =
                    "Mã nhân viên";
            }

            if (dgvBangLuong.Columns.Contains("HoTen"))
            {
                dgvBangLuong.Columns["HoTen"].HeaderText =
                    "Họ tên";
            }

            if (dgvBangLuong.Columns.Contains("ThangNam"))
            {
                dgvBangLuong.Columns["ThangNam"].HeaderText =
                    "Tháng/Năm";
            }

            if (dgvBangLuong.Columns.Contains("ThucLinh"))
            {
                dgvBangLuong.Columns["ThucLinh"].HeaderText =
                    "Thực lĩnh";
            }
        }

        private void BtnXemLuong_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (!IsValidMonth(txtThangLuong.Text.Trim()))
                {
                    MessageBox.Show(
                        "Tháng/Năm phải có định dạng MM/yyyy.");

                    return;
                }

                LoadBangLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}