
using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

using Bai6_1.DTO;
using Bai6_1.BUS;
using Bai6_1.DAL;

namespace Bai6_1
{
    public partial class Form1 : Form
    {
        private SanPhamBUS spBUS = new SanPhamBUS();
        private KhachHangBUS khBUS = new KhachHangBUS();
        private NhanVienBUS nvBUS = new NhanVienBUS();
        private HoaDonBUS hdBUS = new HoaDonBUS();
        private BaoCaoBUS bcBUS = new BaoCaoBUS();

        private DataGridView dgvSP, dgvKH, dgvNV;
        private DataGridView dgvGioHang, dgvHoaDon, dgvTop5;

        private TextBox txtMaSP, txtTenSP, txtGiaSP, txtTonSP, txtTimSP;
        private TextBox txtMaKH, txtTenKH, txtSDTKH, txtDiemKH, txtTimKH;
        private TextBox txtMaNV, txtTenNV, txtSDTNV, txtChucVu, txtTimNV;

        private ComboBox cboSanPham, cboKhachHang, cboNhanVien;

        private NumericUpDown numSoLuong;

        private Label lblTongTien;
        private Label lblDoanhThuNgay, lblDoanhThuThang;

        private List<ChiTietHoaDonDTO> gioHang =
            new List<ChiTietHoaDonDTO>();

        public Form1()
        {
            InitializeComponent();

            TaoGiaoDien();

            LoadTatCaDuLieu();
        }

        // ==========================================
        // HÀM TẠO CONTROL
        // ==========================================

        private TabPage TaoTab(string ten)
        {
            TabPage page = new TabPage(ten);
            tabControl.TabPages.Add(page);
            return page;
        }

        private Label TaoLabel(
            Control parent, string text,
            int x, int y, int width = 120)
        {
            Label lbl = new Label();

            lbl.Text = text;
            lbl.SetBounds(x, y, width, 30);
            lbl.TextAlign = ContentAlignment.MiddleLeft;

            parent.Controls.Add(lbl);

            return lbl;
        }

        private TextBox TaoTextBox(
            Control parent, string name,
            int x, int y, int width = 200)
        {
            TextBox txt = new TextBox();

            txt.Name = name;
            txt.SetBounds(x, y, width, 30);

            parent.Controls.Add(txt);

            return txt;
        }

        private Button TaoButton(
            Control parent, string text,
            int x, int y, int width,
            EventHandler click)
        {
            Button btn = new Button();

            btn.Text = text;
            btn.SetBounds(x, y, width, 35);
            btn.Click += click;

            parent.Controls.Add(btn);

            return btn;
        }

        private DataGridView TaoGrid(
            Control parent, int y = 200)
        {
            DataGridView dgv = new DataGridView();

            dgv.SetBounds(20, y, 1100, 400);

            dgv.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.MultiSelect = false;

            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgv.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgv.BackgroundColor = Color.White;

            parent.Controls.Add(dgv);

            return dgv;
        }

        private void TaoGiaoDien()
        {
            TaoTabSanPham();
            TaoTabKhachHang();
            TaoTabNhanVien();
            TaoTabBanHang();
            TaoTabHoaDon();
            TaoTabBaoCao();
        }

        // ==========================================
        // LOAD DỮ LIỆU
        // ==========================================

        private void LoadTatCaDuLieu()
        {
            LoadSanPham();
            LoadKhachHang();
            LoadNhanVien();
            LoadComboBox();
            LoadHoaDon();
            LoadBaoCao();
        }

        private void LoadSanPham()
        {
            dgvSP.DataSource = spBUS.GetAll();
        }

        private void LoadKhachHang()
        {
            dgvKH.DataSource = khBUS.GetAll();
        }

        private void LoadNhanVien()
        {
            dgvNV.DataSource = nvBUS.GetAll();
        }

        private void LoadComboBox()
        {
            DataTable dtSP = spBUS.GetAll();

            cboSanPham.DataSource = dtSP;
            cboSanPham.DisplayMember = "TenSP";
            cboSanPham.ValueMember = "MaSP";
            cboSanPham.SelectedIndex = -1;

            DataTable dtKH = khBUS.GetAll();

            DataTable dtKhach = dtKH.Copy();

            DataRow row = dtKhach.NewRow();
            row["MaKH"] = DBNull.Value;
            row["TenKH"] = "Khách lẻ";

            dtKhach.Rows.InsertAt(row, 0);

            cboKhachHang.DataSource = dtKhach;
            cboKhachHang.DisplayMember = "TenKH";
            cboKhachHang.ValueMember = "MaKH";
            cboKhachHang.SelectedIndex = 0;

            DataTable dtNV = nvBUS.GetAll();

            cboNhanVien.DataSource = dtNV;
            cboNhanVien.DisplayMember = "TenNV";
            cboNhanVien.ValueMember = "MaNV";
            cboNhanVien.SelectedIndex = -1;
        }

        // ==========================================
        // QUẢN LÝ SẢN PHẨM
        // ==========================================

        private void TaoTabSanPham()
        {
            TabPage page = TaoTab("Quản lý sản phẩm");

            TaoLabel(page, "Mã sản phẩm:", 20, 20);
            txtMaSP = TaoTextBox(page, "txtMaSP", 145, 20);

            TaoLabel(page, "Tên sản phẩm:", 20, 65);
            txtTenSP = TaoTextBox(page, "txtTenSP", 145, 65);

            TaoLabel(page, "Giá bán:", 400, 20);
            txtGiaSP = TaoTextBox(page, "txtGiaSP", 520, 20);

            TaoLabel(page, "Số lượng tồn:", 400, 65);
            txtTonSP = TaoTextBox(page, "txtTonSP", 520, 65);

            TaoLabel(page, "Tìm kiếm:", 20, 120);
            txtTimSP = TaoTextBox(page, "txtTimSP", 145, 120, 300);

            TaoButton(page, "Thêm", 700, 20, 90, BtnThemSP_Click);
            TaoButton(page, "Sửa", 800, 20, 90, BtnSuaSP_Click);
            TaoButton(page, "Xóa", 900, 20, 90, BtnXoaSP_Click);
            TaoButton(page, "Làm mới", 1000, 20, 90, BtnMoiSP_Click);

            TaoButton(page, "Tìm kiếm", 470, 120, 100, BtnTimSP_Click);

            dgvSP = TaoGrid(page, 175);
            dgvSP.CellClick += DgvSP_CellClick;
        }

        private void BtnThemSP_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    GiaBan = decimal.Parse(txtGiaSP.Text),
                    SoLuongTon = int.Parse(txtTonSP.Text)
                };

                if (spBUS.Insert(sp))
                    MessageBox.Show("Thêm sản phẩm thành công!");

                LoadSanPham();
                LoadComboBox();
                XoaThongTinSP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSuaSP_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = txtMaSP.Text.Trim(),
                    TenSP = txtTenSP.Text.Trim(),
                    GiaBan = decimal.Parse(txtGiaSP.Text),
                    SoLuongTon = int.Parse(txtTonSP.Text)
                };

                if (spBUS.Update(sp))
                    MessageBox.Show("Cập nhật sản phẩm thành công!");

                LoadSanPham();
                LoadComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnXoaSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!");
                    return;
                }

                if (MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (spBUS.Delete(txtMaSP.Text.Trim()))
                        MessageBox.Show("Xóa sản phẩm thành công!");

                    LoadSanPham();
                    LoadComboBox();
                    XoaThongTinSP();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa sản phẩm đã có trong hóa đơn.\n" + ex.Message);
            }
        }

        private void BtnMoiSP_Click(object sender, EventArgs e)
        {
            XoaThongTinSP();
            LoadSanPham();
        }

        private void BtnTimSP_Click(object sender, EventArgs e)
        {
            dgvSP.DataSource = spBUS.Search(txtTimSP.Text.Trim());
        }

        private void DgvSP_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSP.Rows[e.RowIndex];

            txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
            txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
            txtGiaSP.Text = row.Cells["GiaBan"].Value.ToString();
            txtTonSP.Text = row.Cells["SoLuongTon"].Value.ToString();

            txtMaSP.ReadOnly = true;
        }

        private void XoaThongTinSP()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGiaSP.Clear();
            txtTonSP.Clear();

            txtMaSP.ReadOnly = false;
        }

        // ==========================================
        // QUẢN LÝ KHÁCH HÀNG
        // ==========================================

        private void TaoTabKhachHang()
        {
            TabPage page = TaoTab("Quản lý khách hàng");

            TaoLabel(page, "Mã khách hàng:", 20, 20);
            txtMaKH = TaoTextBox(page, "txtMaKH", 150, 20);

            TaoLabel(page, "Tên khách hàng:", 20, 65);
            txtTenKH = TaoTextBox(page, "txtTenKH", 150, 65);

            TaoLabel(page, "Điện thoại:", 400, 20);
            txtSDTKH = TaoTextBox(page, "txtSDTKH", 530, 20);

            TaoLabel(page, "Điểm tích lũy:", 400, 65);
            txtDiemKH = TaoTextBox(page, "txtDiemKH", 530, 65);

            TaoLabel(page, "Tìm kiếm:", 20, 120);
            txtTimKH = TaoTextBox(page, "txtTimKH", 150, 120, 300);

            TaoButton(page, "Thêm", 700, 20, 90, BtnThemKH_Click);
            TaoButton(page, "Sửa", 800, 20, 90, BtnSuaKH_Click);
            TaoButton(page, "Xóa", 900, 20, 90, BtnXoaKH_Click);
            TaoButton(page, "Làm mới", 1000, 20, 90, BtnMoiKH_Click);

            TaoButton(page, "Tìm kiếm", 470, 120, 100, BtnTimKH_Click);

            dgvKH = TaoGrid(page, 175);
            dgvKH.CellClick += DgvKH_CellClick;
        }

        private void BtnThemKH_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    TenKH = txtTenKH.Text.Trim(),
                    DienThoai = txtSDTKH.Text.Trim(),
                    DiemTichLuy = 0
                };

                if (khBUS.Insert(kh))
                    MessageBox.Show("Thêm khách hàng thành công!");

                LoadKhachHang();
                LoadComboBox();
                XoaThongTinKH();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSuaKH_Click(object sender, EventArgs e)
        {
            try
            {
                KhachHangDTO kh = new KhachHangDTO
                {
                    MaKH = int.Parse(txtMaKH.Text),
                    TenKH = txtTenKH.Text.Trim(),
                    DienThoai = txtSDTKH.Text.Trim(),
                    DiemTichLuy = int.Parse(txtDiemKH.Text)
                };

                if (khBUS.Update(kh))
                    MessageBox.Show("Cập nhật khách hàng thành công!");

                LoadKhachHang();
                LoadComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnXoaKH_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!");
                    return;
                }

                if (MessageBox.Show(
                    "Bạn có chắc muốn xóa khách hàng?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (khBUS.Delete(int.Parse(txtMaKH.Text)))
                        MessageBox.Show("Xóa khách hàng thành công!");

                    LoadKhachHang();
                    LoadComboBox();
                    XoaThongTinKH();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa khách hàng đã có hóa đơn.\n" + ex.Message);
            }
        }

        private void BtnMoiKH_Click(object sender, EventArgs e)
        {
            XoaThongTinKH();
            LoadKhachHang();
        }

        private void BtnTimKH_Click(object sender, EventArgs e)
        {
            dgvKH.DataSource = khBUS.Search(txtTimKH.Text.Trim());
        }

        private void DgvKH_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvKH.Rows[e.RowIndex];

            txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
            txtTenKH.Text = row.Cells["TenKH"].Value.ToString();

            txtSDTKH.Text = row.Cells["DienThoai"].Value == DBNull.Value
                ? ""
                : row.Cells["DienThoai"].Value.ToString();

            txtDiemKH.Text = row.Cells["DiemTichLuy"].Value.ToString();

            txtMaKH.ReadOnly = true;
        }

        private void XoaThongTinKH()
        {
            txtMaKH.Clear();
            txtTenKH.Clear();
            txtSDTKH.Clear();
            txtDiemKH.Text = "0";

            txtMaKH.ReadOnly = true;
        }

        // ==========================================
        // QUẢN LÝ NHÂN VIÊN
        // ==========================================

        private void TaoTabNhanVien()
        {
            TabPage page = TaoTab("Quản lý nhân viên");

            TaoLabel(page, "Mã nhân viên:", 20, 20);
            txtMaNV = TaoTextBox(page, "txtMaNV", 150, 20);

            TaoLabel(page, "Tên nhân viên:", 20, 65);
            txtTenNV = TaoTextBox(page, "txtTenNV", 150, 65);

            TaoLabel(page, "Điện thoại:", 400, 20);
            txtSDTNV = TaoTextBox(page, "txtSDTNV", 530, 20);

            TaoLabel(page, "Chức vụ:", 400, 65);
            txtChucVu = TaoTextBox(page, "txtChucVu", 530, 65);

            TaoLabel(page, "Tìm kiếm:", 20, 120);
            txtTimNV = TaoTextBox(page, "txtTimNV", 150, 120, 300);

            TaoButton(page, "Thêm", 700, 20, 90, BtnThemNV_Click);
            TaoButton(page, "Sửa", 800, 20, 90, BtnSuaNV_Click);
            TaoButton(page, "Xóa", 900, 20, 90, BtnXoaNV_Click);
            TaoButton(page, "Làm mới", 1000, 20, 90, BtnMoiNV_Click);

            TaoButton(page, "Tìm kiếm", 470, 120, 100, BtnTimNV_Click);

            dgvNV = TaoGrid(page, 175);
            dgvNV.CellClick += DgvNV_CellClick;
        }

        private void BtnThemNV_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    TenNV = txtTenNV.Text.Trim(),
                    SoDienThoai = txtSDTNV.Text.Trim(),
                    ChucVu = txtChucVu.Text.Trim()
                };

                if (nvBUS.Insert(nv))
                    MessageBox.Show("Thêm nhân viên thành công!");

                LoadNhanVien();
                LoadComboBox();
                XoaThongTinNV();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSuaNV_Click(object sender, EventArgs e)
        {
            try
            {
                NhanVienDTO nv = new NhanVienDTO
                {
                    MaNV = int.Parse(txtMaNV.Text),
                    TenNV = txtTenNV.Text.Trim(),
                    SoDienThoai = txtSDTNV.Text.Trim(),
                    ChucVu = txtChucVu.Text.Trim()
                };

                if (nvBUS.Update(nv))
                    MessageBox.Show("Cập nhật nhân viên thành công!");

                LoadNhanVien();
                LoadComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnXoaNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!");
                    return;
                }

                if (MessageBox.Show(
                    "Bạn có chắc muốn xóa nhân viên?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (nvBUS.Delete(int.Parse(txtMaNV.Text)))
                        MessageBox.Show("Xóa nhân viên thành công!");

                    LoadNhanVien();
                    LoadComboBox();
                    XoaThongTinNV();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa nhân viên đã lập hóa đơn.\n" + ex.Message);
            }
        }

        private void BtnMoiNV_Click(object sender, EventArgs e)
        {
            XoaThongTinNV();
            LoadNhanVien();
        }

        private void BtnTimNV_Click(object sender, EventArgs e)
        {
            dgvNV.DataSource = nvBUS.Search(txtTimNV.Text.Trim());
        }

        private void DgvNV_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvNV.Rows[e.RowIndex];

            txtMaNV.Text = row.Cells["MaNV"].Value.ToString();
            txtTenNV.Text = row.Cells["TenNV"].Value.ToString();

            txtSDTNV.Text = row.Cells["SoDienThoai"].Value == DBNull.Value
                ? ""
                : row.Cells["SoDienThoai"].Value.ToString();

            txtChucVu.Text = row.Cells["ChucVu"].Value == DBNull.Value
                ? ""
                : row.Cells["ChucVu"].Value.ToString();

            txtMaNV.ReadOnly = true;
        }

        private void XoaThongTinNV()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtSDTNV.Clear();
            txtChucVu.Clear();

            txtMaNV.ReadOnly = true;
        }

        // ==========================================
        // BÁN HÀNG
        // ==========================================

        private void TaoTabBanHang()
        {
            TabPage page = TaoTab("Bán hàng");

            TaoLabel(page, "Nhân viên:", 20, 20, 100);

            cboNhanVien = new ComboBox();
            cboNhanVien.SetBounds(125, 20, 250, 30);
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            page.Controls.Add(cboNhanVien);

            TaoLabel(page, "Khách hàng:", 400, 20, 100);

            cboKhachHang = new ComboBox();
            cboKhachHang.SetBounds(510, 20, 280, 30);
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            page.Controls.Add(cboKhachHang);

            TaoLabel(page, "Sản phẩm:", 20, 70, 100);

            cboSanPham = new ComboBox();
            cboSanPham.SetBounds(125, 70, 300, 30);
            cboSanPham.DropDownStyle = ComboBoxStyle.DropDownList;
            page.Controls.Add(cboSanPham);

            TaoLabel(page, "Số lượng:", 450, 70, 80);

            numSoLuong = new NumericUpDown();
            numSoLuong.SetBounds(530, 70, 100, 30);
            numSoLuong.Minimum = 1;
            numSoLuong.Maximum = 100000;
            numSoLuong.Value = 1;
            page.Controls.Add(numSoLuong);

            TaoButton(page, "Thêm vào giỏ", 660, 68, 140,
                BtnThemGioHang_Click);

            TaoButton(page, "Xóa sản phẩm", 820, 68, 140,
                BtnXoaGioHang_Click);

            dgvGioHang = TaoGrid(page, 125);

            lblTongTien = new Label();
            lblTongTien.SetBounds(20, 550, 500, 40);
            lblTongTien.Font = new Font(
                "Segoe UI", 15, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Text = "Tổng tiền: 0 VNĐ";
            page.Controls.Add(lblTongTien);

            TaoButton(page, "Thanh toán", 700, 550, 150,
                BtnThanhToan_Click);

            TaoButton(page, "Làm mới giỏ hàng", 870, 550, 170,
                BtnLamMoiGioHang_Click);
        }

        private void BtnThemGioHang_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSanPham.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!");
                    return;
                }

                string maSP = cboSanPham.SelectedValue.ToString();
                int soLuong = (int)numSoLuong.Value;

                DataTable dt = spBUS.GetByMaSP(maSP);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!");
                    return;
                }

                DataRow row = dt.Rows[0];

                string tenSP = row["TenSP"].ToString();
                decimal giaBan = Convert.ToDecimal(row["GiaBan"]);
                int tonKho = Convert.ToInt32(row["SoLuongTon"]);

                ChiTietHoaDonDTO ct =
                    gioHang.Find(x => x.MaSP == maSP);

                int soLuongTrongGio = ct == null ? 0 : ct.SoLuong;

                if (soLuongTrongGio + soLuong > tonKho)
                {
                    MessageBox.Show("Số lượng vượt quá tồn kho!");
                    return;
                }

                if (ct == null)
                {
                    ct = new ChiTietHoaDonDTO
                    {
                        MaSP = maSP,
                        TenSP = tenSP,
                        DonGia = giaBan,
                        SoLuong = soLuong
                    };

                    gioHang.Add(ct);
                }
                else
                {
                    ct.SoLuong += soLuong;
                }

                HienThiGioHang();
                numSoLuong.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HienThiGioHang()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("MaSP");
            dt.Columns.Add("TenSP");
            dt.Columns.Add("SoLuong", typeof(int));
            dt.Columns.Add("DonGia", typeof(decimal));
            dt.Columns.Add("ThanhTien", typeof(decimal));

            decimal tongTien = 0;

            foreach (ChiTietHoaDonDTO ct in gioHang)
            {
                decimal thanhTien = ct.SoLuong * ct.DonGia;

                dt.Rows.Add(
                    ct.MaSP,
                    ct.TenSP,
                    ct.SoLuong,
                    ct.DonGia,
                    thanhTien);

                tongTien += thanhTien;
            }

            dgvGioHang.DataSource = dt;

            dgvGioHang.Columns["DonGia"]
                .DefaultCellStyle.Format = "N0";

            dgvGioHang.Columns["ThanhTien"]
                .DefaultCellStyle.Format = "N0";

            lblTongTien.Text =
                "Tổng tiền: " + tongTien.ToString("N0") + " VNĐ";
        }

        private void BtnXoaGioHang_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
                return;
            }

            string maSP = dgvGioHang.CurrentRow
                .Cells["MaSP"].Value.ToString();

            ChiTietHoaDonDTO ct =
                gioHang.Find(x => x.MaSP == maSP);

            if (ct != null)
                gioHang.Remove(ct);

            HienThiGioHang();
        }

        private void BtnLamMoiGioHang_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Bạn muốn xóa toàn bộ giỏ hàng?",
                "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                gioHang.Clear();
                HienThiGioHang();
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                if (gioHang.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng đang trống!");
                    return;
                }

                if (cboNhanVien.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!");
                    return;
                }

                decimal tongTien = 0;

                foreach (ChiTietHoaDonDTO ct in gioHang)
                    tongTien += ct.SoLuong * ct.DonGia;

                if (MessageBox.Show(
                    "Xác nhận thanh toán?\nTổng tiền: " +
                    tongTien.ToString("N0") + " VNĐ",
                    "Thanh toán",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                HoaDonDTO hd = new HoaDonDTO();

                hd.MaNV = Convert.ToInt32(cboNhanVien.SelectedValue);

                if (cboKhachHang.SelectedIndex > 0 &&
                    cboKhachHang.SelectedValue != null &&
                    cboKhachHang.SelectedValue != DBNull.Value)
                {
                    hd.MaKH = Convert.ToInt32(
                        cboKhachHang.SelectedValue);
                }
                else
                {
                    hd.MaKH = null;
                }

                foreach (ChiTietHoaDonDTO ct in gioHang)
                {
                    hd.ChiTiet.Add(new ChiTietHoaDonDTO
                    {
                        MaSP = ct.MaSP,
                        TenSP = ct.TenSP,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia
                    });
                }

                int maHD = hdBUS.ThanhToan(hd);

                MessageBox.Show(
                    "THANH TOÁN THÀNH CÔNG!\n\n" +
                    "Mã hóa đơn: HD" + maHD + "\n" +
                    "Tổng tiền: " + tongTien.ToString("N0") + " VNĐ",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                gioHang.Clear();
                HienThiGioHang();

                LoadSanPham();
                LoadKhachHang();
                LoadComboBox();
                LoadHoaDon();
                LoadBaoCao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "THANH TOÁN THẤT BẠI!\n\n" + ex.Message,
                    "Lỗi giao dịch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================
        // QUẢN LÝ HÓA ĐƠN
        // ==========================================

        private void TaoTabHoaDon()
        {
            TabPage page = TaoTab("Hóa đơn");

            TaoLabel(page, "DANH SÁCH HÓA ĐƠN",
                20, 20, 400);

            dgvHoaDon = TaoGrid(page, 70);

            TaoButton(page, "Làm mới", 20, 490, 120,
                BtnLoadHoaDon_Click);

            TaoButton(page, "Xem chi tiết", 160, 490, 150,
                BtnXemChiTietHD_Click);
        }

        private void LoadHoaDon()
        {
            dgvHoaDon.DataSource = hdBUS.GetAll();

            if (dgvHoaDon.Columns.Contains("TongTien"))
                dgvHoaDon.Columns["TongTien"]
                    .DefaultCellStyle.Format = "N0";

            if (dgvHoaDon.Columns.Contains("NgayLap"))
                dgvHoaDon.Columns["NgayLap"]
                    .DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void BtnLoadHoaDon_Click(object sender, EventArgs e)
        {
            LoadHoaDon();
        }

        private void BtnXemChiTietHD_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn!");
                return;
            }

            int maHD = Convert.ToInt32(
                dgvHoaDon.CurrentRow.Cells["MaHD"].Value);

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    string sql = @"
                        SELECT CT.MaSP, SP.TenSP,
                               CT.SoLuong, CT.DonGia,
                               CT.SoLuong * CT.DonGia AS ThanhTien
                        FROM ChiTietHoaDon CT
                        INNER JOIN SanPham SP ON CT.MaSP=SP.MaSP
                        WHERE CT.MaHD=@MaHD";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@MaHD", SqlDbType.Int)
                            .Value = maHD;

                        DataTable dt = new DataTable();

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }

                        Form frm = new Form();

                        frm.Text = "Chi tiết hóa đơn HD" + maHD;
                        frm.Size = new Size(850, 450);
                        frm.StartPosition =
                            FormStartPosition.CenterParent;

                        DataGridView dgv = new DataGridView();

                        dgv.Dock = DockStyle.Fill;
                        dgv.ReadOnly = true;
                        dgv.AutoSizeColumnsMode =
                            DataGridViewAutoSizeColumnsMode.Fill;

                        dgv.DataSource = dt;

                        dgv.Columns["DonGia"]
                            .DefaultCellStyle.Format = "N0";

                        dgv.Columns["ThanhTien"]
                            .DefaultCellStyle.Format = "N0";

                        frm.Controls.Add(dgv);
                        frm.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ==========================================
        // BÁO CÁO - THỐNG KÊ
        // ==========================================

        private void TaoTabBaoCao()
        {
            TabPage page = TaoTab("Báo cáo - Thống kê");

            GroupBox group = new GroupBox();

            group.Text = "Thống kê doanh thu";
            group.SetBounds(20, 20, 750, 140);

            page.Controls.Add(group);

            TaoLabel(group, "Doanh thu hôm nay:",
                20, 30, 180);

            lblDoanhThuNgay = new Label();

            lblDoanhThuNgay.SetBounds(220, 30, 350, 30);
            lblDoanhThuNgay.Font =
                new Font("Segoe UI", 12, FontStyle.Bold);
            lblDoanhThuNgay.ForeColor = Color.DarkGreen;

            group.Controls.Add(lblDoanhThuNgay);

            TaoLabel(group, "Doanh thu tháng này:",
                20, 75, 180);

            lblDoanhThuThang = new Label();

            lblDoanhThuThang.SetBounds(220, 75, 350, 30);
            lblDoanhThuThang.Font =
                new Font("Segoe UI", 12, FontStyle.Bold);
            lblDoanhThuThang.ForeColor = Color.DarkBlue;

            group.Controls.Add(lblDoanhThuThang);

            TaoLabel(page, "TOP 5 SẢN PHẨM BÁN CHẠY NHẤT",
                20, 185, 500);

            dgvTop5 = TaoGrid(page, 225);

            TaoButton(page, "Cập nhật thống kê",
                20, 640, 180, BtnCapNhatBaoCao_Click);
        }

        private void LoadBaoCao()
        {
            try
            {
                decimal ngay = bcBUS.DoanhThuNgay();
                decimal thang = bcBUS.DoanhThuThang();

                lblDoanhThuNgay.Text = ngay.ToString("N0") + " VNĐ";
                lblDoanhThuThang.Text = thang.ToString("N0") + " VNĐ";

                dgvTop5.DataSource = bcBUS.Top5SanPham();

                if (dgvTop5.Columns.Contains("DoanhThu"))
                    dgvTop5.Columns["DoanhThu"]
                        .DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải báo cáo!\n" + ex.Message);
            }
        }

        private void BtnCapNhatBaoCao_Click(
            object sender, EventArgs e)
        {
            LoadBaoCao();
            MessageBox.Show("Cập nhật báo cáo thành công!");
        }
    }
}
