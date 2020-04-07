using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyKhachSan
{
    public partial class QuanLyKhachSan : Form
    {
        SqlConnection con;
        public QuanLyKhachSan()
        {
            InitializeComponent();
        }

        private void QuanLyKhachSan_Load(object sender, EventArgs e)
        {
            con = new SqlConnection();
            con.ConnectionString = "Data Source=HH171;Initial Catalog=QuanLyKhachSan;Integrated Security=True";
            con.Open();

            loadDataGridView();
        }
        
        
        public void loadDataGridView()
        {
            string sql = "SELECT * FROM Phong";
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataTable tablePhong = new DataTable();
            adp.Fill(tablePhong);
            dataGridView_Phong.DataSource = tablePhong;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";

        }

        private void dataGridView_Phong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaPhong.Text = dataGridView_Phong.CurrentRow.Cells["MaPhong"].Value.ToString();
            txtTenPhong.Text = dataGridView_Phong.CurrentRow.Cells["TenPhong"].Value.ToString();
            txtDonGia.Text = dataGridView_Phong.CurrentRow.Cells["DonGia"].Value.ToString();
            txtMaPhong.Enabled = false;
        }

        public void RunSQL( string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch( Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtTenPhong.Text == "")
            {
                MessageBox.Show("Bạn phải nhập Tên Phòng");
                txtTenPhong.Focus();
            }
            if (txtDonGia.Text == "")
            {
                MessageBox.Show("Ban phải nhập Đơn giá");
                txtDonGia.Focus();
            }

            sql = "SELECT MaPhong FROM Phong WHERE MaPhong=N'" + txtMaPhong.Text.Trim() + "'";
            RunSQL(sql);
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataTable tablePhong = new DataTable();
            adp.Fill(tablePhong);
            if (tablePhong.Rows.Count > 0)
            {
                MessageBox.Show("Mã hàng đã có, Bạn hãy nhập mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaPhong.Focus();
                txtMaPhong.Text = "";
            }
              
            sql = "INSERT INTO Phong VALUES (N'" + txtMaPhong.Text
                + "',N'" + txtTenPhong.Text + "','" + txtDonGia.Text + "')";
            RunSQL(sql);
            loadDataGridView();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql;
            if(txtTenPhong.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if(txtTenPhong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên chất liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhong.Focus();
                return;
            }
            sql = "UPDATE Phong SET TenPhong=N'" + txtTenPhong.Text.Trim() + "',DonGia=N'" + txtDonGia.Text.Trim() 
                + "' WHERE MaPhong=N'" + txtMaPhong.Text + "'";
            RunSQL(sql);
            loadDataGridView();
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
            btnHuyBo.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if( MessageBox.Show("Bạn có muốn xóa không?","Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)== DialogResult.OK)
            {
                sql = " DELETE Phong WHERE MaPhong=N'" + txtMaPhong.Text + "'";
                RunSQL(sql);

                loadDataGridView();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
            btnHuyBo.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaPhong.Enabled = false;
        }
    }
}
