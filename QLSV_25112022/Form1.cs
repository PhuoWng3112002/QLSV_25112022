using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLSV_25112022
{
    public partial class Form1 : Form
    {
        private ErrorProvider errorProvider = new ErrorProvider();

        private string connectionString = ConfigurationManager.ConnectionStrings["QLSV"].ConnectionString;

        private DataView dv_tblLop = new DataView();
        private DataView dv_dgv = new DataView();//dataview

        public Form1()
        {
            InitializeComponent();

        }

        private void LoadDataToGridView(string filter = "")
        {
            //ten stored proc or mot cau lenh sql
            string querySelect = "pr_TH1";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    //su dung using de tu dong dispose
                    using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        ////neu querySelect laf storesproc
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.SelectCommand = sqlCommand;
                            using (DataTable tblSinhVien = new DataTable())
                            {
                                adapter.Fill(tblSinhVien);
                                if (tblSinhVien.Rows.Count > 0)
                                {
                                    dv_dgv = tblSinhVien.DefaultView;
                                    if (filter != null)
                                    {
                                        dv_dgv.RowFilter = filter;
                                    }

                                    //loc theo dong
                                    // dv_dgv.RowFilter = "sMaSV LIKE '%1%' AND sHoTen LIKE '%Nguyen%' ";
                                    dgv_dssv.AutoGenerateColumns = false;
                                    dgv_dssv.DataSource = dv_dgv;
                                }
                                else
                                {
                                    MessageBox.Show("Khong ton tai ban ghi nao");
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadDataToComboBox()
        {
            //ten 1 sp or mot cau lenh sql
            string querySelect = "SELECT*FROM tblLop";
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    //su dung using de tu dong dispose  
                    using (SqlCommand sqlCommand = new SqlCommand(querySelect, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        //command type tuyf thuoc vao query o tren
                        //neu querySelect la storesproc
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.SelectCommand = sqlCommand;
                            using (DataTable tblLopHoc = new DataTable())
                            {
                                adapter.Fill(tblLopHoc);
                                if (tblLopHoc.Rows.Count > 0)
                                {
                                    //cach 1
                                    dv_tblLop = tblLopHoc.DefaultView;
                                    DataView dv = new DataView(tblLopHoc);
                                    cbMaLopHC.DataSource = dv;
                                    cbMaLopHC.ValueMember = "sMaLop";
                                    cbMaLopHC.DisplayMember = "sMaLop";
                                    cbMaLopHC.Text = string.Empty;
                                
                                    //txtTenCVHT.Text = string.Empty;

                                    //cach 2

                                    //foreach (DataRow row in tblLopHoc.Rows)
                                    //{

                                    //cbMaLopHC
                                    //    .Items.Add(row["sMaLop"]);
                                    //}

                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataToGridView();
            LoadDataToComboBox();
        }

        private bool IsNumber(string value)
        {
            foreach (Char ch in value)
            {
                if (!Char.IsDigit(ch))
                {
                    return false;
                }
            }
            return true;
        }
        private void txtMaSinhVien_TextChanged(object sender, EventArgs e)
        {
            if (txtMaSinhVien.Text.Length > 0)
            {
                themmoi.Enabled = true;
            }
            else
            {
                themmoi.Enabled = false;
            }

        }
        private void txtMaSinhVien_Validating(object sender, CancelEventArgs e)
        {
            //cancelEvenArgs: chan cac event khac dang hoat dong
            //if (string.IsNullOrEmpty(txtMaSinhVien.Text))
            //{
            //    e.Cancel = true;
            //    errorProvider.SetError(txtMaSinhVien, "Ma SV khong duoc de trong");
            //}
            //else
            //{
            //    e.Cancel = false;
            //    errorProvider.SetError(txtMaSinhVien, null);
            //}
        }

        private void txtHoTenSV_TextChanged(object sender, EventArgs e)
        {
            if (txtHoTenSV.Text.Length > 0)
            {
                themmoi.Enabled = true;
            }
            else
            {
                themmoi.Enabled = false;
            }

        }

        private void txtHoTenSV_Validating(object sender, CancelEventArgs e)
        {
            //cancelEvenArgs: chan cac event khac dang hoat dong
            //if (string.IsNullOrEmpty(txtHoTenSV.Text))
            //{
            //    e.Cancel = true;
            //    errorProvider.SetError(txtHotenSV, "Ten SV khong duoc de trong");
            //}
            //else
            //{
            //    e.Cancel = false;
            //    errorProvider.SetError(txtHoTenSV, null);
            //}

        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            if (txtSDT.Text.Length > 0)
            {
                if (IsNumber(txtSDT.Text))
                {
                    themmoi.Enabled = true;
                }
                else
                {
                    themmoi.Enabled = false;
                }
            }
            else
            {
                themmoi.Enabled = false;
            }

        }

        private void txtSDT_Validating(object sender, CancelEventArgs e)
        {
            //if (txtSDT.Text.Length != 11)
            //{
            //    e.Cancel = true;
            //    errorProvider.SetError(txtSDT, "SDT khong hop le");
            //}
            //else
            //{
            //    e.Cancel = false;
            //    errorProvider.SetError(txtSDT, null);
            //}
        }
        private void cbMaLopHC_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbMaLopHC.SelectedIndex;
            string malop = dv_tblLop[index]["sMaLop"].ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {

                    sqlCommand.CommandText = "pr_tenGVtheoma";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ma", malop);
                    sqlConnection.Open();
                    //thuc thi command


                    var s = sqlCommand.ExecuteScalar();
                    txtTenCVHT.Text = s.ToString();


                    sqlConnection.Close();
                }
            }
        }

        private void themmoi_Click(object sender, EventArgs e)
        {
            bool gioiTinh;//Boolean và bool:same(...)
            if (rb_nam.Checked == true)
            {
                gioiTinh = true;
            }
            else if (rb_nu.Checked == true)
            {
                gioiTinh = false;
            }
            //kiem tra su tồn tại của khóa chính
            string idMaSV = txtMaSinhVien.Text;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "pr_khoachinhSV";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@maSV", idMaSV);
                    sqlConnection.Open();
                    var tmp = sqlCommand.ExecuteScalar();
                    sqlConnection.Close();


                    if (tmp == null)
                    {
                        //them moi du lieu vao datalist
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            //lay DS SV vao Datatable
                            adapter.SelectCommand = new SqlCommand("SELECT*FROM tblSinhVien", sqlConnection);
                            DataTable dt_tblSinhVien = new DataTable("tblSinhVien");
                            adapter.Fill(dt_tblSinhVien);

                            //add tung datatable va dataset
                            DataSet ds = new DataSet();
                            ds.Tables.Add(dt_tblSinhVien);

                            //Them mot ban ghi vao dataTable/dataSet
                            DataRow newRow = dt_tblSinhVien.NewRow();
                            newRow["sMaSV"] = this.txtMaSinhVien.Text;
                            newRow["sHoTen"] = this.txtHoTenSV.Text;
                            // newRow["ngaySinh"] = this.datetimeNgaySinh.Value.ToString("yyyy/MM/d");//?
                            newRow["dNgaySinh"] = this.maskedtxtNgSinh.Text;
                            newRow["sDiaChi"] = this.txtDiaChi.Text;
                            newRow["sSoDienThoai"] = this.txtSDT.Text;
                            //.....con lai

                            dt_tblSinhVien.Rows.Add(newRow);

                            //them ban ghi len DB bang cach thuc hien InsertCommand
                            sqlCommand.CommandText = "pr_ThemSV";
                            sqlCommand.Parameters.Clear();
                            //cach 1
                            sqlCommand.Parameters.Add("@maSV", SqlDbType.Char, 30, "maSV");
                            //cach 2
                            //sqlCommand.Parameters.AddWithValue("@maSV", this.txtHotenSV.Text);
                            //.... them cac tham so tuong ung

                            //them moi DL thong qua Adapter
                            adapter.InsertCommand = sqlCommand;
                            adapter.Update(ds, "tblSinhVien");

                            MessageBox.Show("Them moi thanh cong");

                            //Load lai DL tren dataGridView
                            LoadDataToGridView();


                        }
                    }
                    else
                    {
                        MessageBox.Show("Ma SV " + idMaSV + "da ton tai");

                    }
                }
            }
        }


        private void dgv_dssv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgv_dssv.CurrentRow.Index;
            txtMaSinhVien.Text = dgv_dssv.Rows[index].Cells["masv"].Value.ToString();
            txtMaSinhVien.ReadOnly = true;
            txtHoTenSV.Text = dgv_dssv.Rows[index].Cells["tensv"].Value.ToString();
            txtDiaChi.Text = dv_dgv[index]["sDiaChi"].ToString();
            maskedtxtNgSinh.Text = dv_dgv[index]["dNgaySinh"].ToString();
            txtSDT.Text = dv_dgv[index]["sSoDienThoai"].ToString();
            cbMaLopHC.Text = dv_dgv[index]["sMaLop"].ToString();
            //day het cac truong dl
            //check gioi tinh
            if ((bool)dv_dgv[index]["bGioitinh"] == true)
            {
                rb_nam.Checked = true;
            }
            else
            {
                rb_nu.Checked = true;
            }
        }


        private void xoabo_Click(object sender, EventArgs e)
        {
            int index = dgv_dssv.CurrentRow.Index;
            string maSV = dv_dgv[index]["sMaSV"].ToString();

            //thuc hien connection 
            try
            {
                //kiem tra rang buoc giua cac bang du lieu
                KiemTraRangBuoc_BangDiem(maSV);

                //neu khong co rang buoc==> cho xoa

                DialogResult dialogResult = MessageBox.Show("Co muon xoa ma sinh vien" + maSV + "khong ?", "Canh bao!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    string deleteQuery = "pr_DeleteSV";
                    //thuc hien xoa
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            //lay ra danh sach sv vao dataset thong qua data adapter
                            adapter.SelectCommand = new SqlCommand("SELECT * FROM tblSinhVien", sqlConnection);
                            DataTable dt_tblSinhVien = new DataTable("tblSinhVien");
                            adapter.Fill(dt_tblSinhVien);


                            //add cac datatable vao dataset
                            DataSet ds = new DataSet();
                            ds.Tables.Add(dt_tblSinhVien);

                            //tim ma SV can xoa
                            dt_tblSinhVien.PrimaryKey = new DataColumn[] { dt_tblSinhVien.Columns["sMaSV"] };
                            DataRow dataRow = dt_tblSinhVien.Rows.Find(maSV);
                            dataRow.Delete();//xoa trong dataset

                            //xoa trong database
                            using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                            {
                                sqlCommand.CommandText = deleteQuery;
                                sqlCommand.CommandType = CommandType.StoredProcedure;
                                sqlCommand.Parameters.AddWithValue("@maSV", maSV);

                                adapter.DeleteCommand = sqlCommand;
                                adapter.Update(ds, "tblSinhVien");
                            }

                        }
                    }
                    LoadDataToGridView();
                }
                else
                {
                    return;
                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (error.Contains("fk_Diem_SV"))
                {
                    MessageBox.Show("Ma sinh vien " + maSV + "da phat sinh diem");
                }
                else if (error.Contains("40"))
                {
                    MessageBox.Show("Loi ket noi SQL");

                }
                else
                {
                    MessageBox.Show("Da co loi xay ra");
                }
            }
        }

        private void KiemTraRangBuoc_BangDiem(string maSV)
        {
            //kiem tra maSV muon xoa co ton tai o cac bang khac khong
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "pr_KiemTraMaSV_BangDiem";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@maSV", maSV);
                    sqlConnection.Open();
                    bool exist = (sqlCommand.ExecuteScalar() != null);//=> true
                    sqlConnection.Close();
                    if (exist)
                    {
                        throw new Exception("Rang buoc : Ma SV " + maSV + "da co phat sinh diem");
                    }

                }
            }
        }
        private void boqua_Click(object sender, EventArgs e)
        {
            txtMaSinhVien.Text = string.Empty;
            txtHoTenSV.Text = string.Empty;
            txtDiaChi.Text = string.Empty;
            txtSDT.Text = string.Empty;
            maskedtxtNgSinh.Text = string.Empty;
            cbMaLopHC.Text = string.Empty;
            txtTenCVHT.Text = string.Empty;
            rb_nam.Checked = false;
            rb_nu.Checked = false;


            txtMaSinhVien.Focus();
        }
        private void timkiem_Click(object sender, EventArgs e)
        {
            string filter = " sMaSV IS NOT NULL";
            if (txtMaSinhVien.Text != null)
            {
                filter += string.Format(" AND sMaSV LIKE '%{0}%'", txtMaSinhVien.Text);

            }
            if (!string.IsNullOrEmpty(txtHoTenSV.Text))
            {
                filter += string.Format(" AND sTenSV LIKE '%{0}%'", txtHoTenSV.Text);

                //cac truong du lieu khac tuong ung voi control
            }

            if (!string.IsNullOrEmpty(txtDiaChi.Text))
            {
                filter += string.Format(" AND sDiaChi LIKE '%{0}%'", txtDiaChi.Text);

            }
            LoadDataToGridView(filter);
        }

        private void indssv_Click(object sender, EventArgs e)
        {
            CrystalReportViewer crt = new CrystalReportViewer();
            crt.Show();
        }

        private void thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
