﻿using BLL;
using KabaAccounting.CUL;
using KabaAccounting.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for WinBanks.xaml
    /// </summary>
    public partial class WinBank : Window
    {
        public WinBank()
        {
            InitializeComponent();
            LoadBankDataGrid();
        }

        BankCUL bankCUL = new BankCUL();
        BankDAL bankDAL = new BankDAL();
        UserDAL userDAL = new UserDAL();
        UserBLL userBLL = new UserBLL();

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            bankCUL.Name = txtBankName.Text;
            bankCUL.AddedDate = DateTime.Now;
            bankCUL.AddedBy = userBLL.GetUserId(WinLogin.loggedInUserName);

            bool isSuccess = bankDAL.Insert(bankCUL);

            if (isSuccess == true)
            {
                MessageBox.Show("New bank has been inserted successfully.");
                dtgBanks.Items.Clear();
                ClearBankTextBox();
                LoadBankDataGrid();
            }
            else
            {
                MessageBox.Show("Something went wrong :(");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bankCUL.Id = Convert.ToInt32(txtBankId.Text);
            bankCUL.Name = txtBankName.Text;
            bankCUL.AddedDate = DateTime.Now;
            bankCUL.AddedBy = userBLL.GetUserId(WinLogin.loggedInUserName);

            bool isSuccess = bankDAL.Update(bankCUL);

            if (isSuccess == true)
            {
                MessageBox.Show("The bank is successfully updated.");
                dtgBanks.Items.Clear();
                ClearBankTextBox();
                LoadBankDataGrid();
            }
            else
            {
                MessageBox.Show("Failed to update the bank.");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bankCUL.Id = Convert.ToInt32(txtBankId.Text);

            bool isSuccess = bankDAL.Delete(bankCUL);

            if (isSuccess == true)
            {
                MessageBox.Show("Bank has been deleted successfully.");
                dtgBanks.Items.Clear();
                ClearBankTextBox();
                LoadBankDataGrid();
            }
            else
            {
                MessageBox.Show("Something went wrong :/");
            }
        }

        private void LoadBankDataGrid()
        {
            //Refreshing Data Grid View
            //DataTable dataTable = bankDAL.Select();
            //dtgBanks.ItemsSource = dataTable.DefaultView;
            //dtgBanks.AutoGenerateColumns = true;
            //dtgBanks.CanUserAddRows = false;

            int firstRowIndex = 0, bankId, addedById;
            string bankName, addedDate, addedByUsername;
            DataTable dataTable = bankDAL.Select();
            DataTable dataTableUserInfo;

            //dtgs.ItemsSource = dataTable.DefaultView; Adds everything at once.
            dtgBanks.AutoGenerateColumns = true;
            dtgBanks.CanUserAddRows = false;

            #region LOADING THE PRODUCT DATA GRID

            for (int currentRow = firstRowIndex; currentRow < dataTable.Rows.Count; currentRow++)
            {
                bankId = Convert.ToInt32(dataTable.Rows[currentRow]["id"]);
                bankName = dataTable.Rows[currentRow]["name"].ToString();
                addedDate = dataTable.Rows[currentRow]["added_date"].ToString();

                addedById = Convert.ToInt32(dataTable.Rows[currentRow]["added_by"]);
                dataTableUserInfo = userDAL.GetUserInfoById(addedById);
                addedByUsername = dataTableUserInfo.Rows[firstRowIndex]["first_name"].ToString() + " " + dataTableUserInfo.Rows[firstRowIndex]["last_name"].ToString();

                dtgBanks.Items.Add(new { Id = bankId, Name = bankName, AddedDate = addedDate, AddedBy = addedByUsername });
            }
            #endregion
        }

        private void ClearBankTextBox()
        {
            txtBankId.Text = "";
            txtBankName.Text = "";
            txtBankSearch.Text = "";
        }

        private void DtgBanksIndexChanged()
        {
            DataRowView drv = (DataRowView)dtgBanks.SelectedItem;

            txtBankId.Text = (drv[0]).ToString();//Selecting the specific row
            txtBankName.Text = (drv["name"]).ToString();//You can also define the column name from your table like here.
        }

        private void dtgBanks_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DtgBanksIndexChanged();
        }

        private void dtgBanks_KeyUp(object sender, KeyEventArgs e)
        {
            DtgBanksIndexChanged();
        }

        private void dtgBanks_KeyDown(object sender, KeyEventArgs e)
        {
            DtgBanksIndexChanged();
        }

        private void txtBankSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get Keyword from Text box
            string keyword = txtBankSearch.Text;

            //Check if the keyword has value or not

            if (keyword != null) /*Do NOT Repeat yourself!!! Improve if statement block!!! You have similar codes in the RefreshBankDataGrid method!!! */
            {
                //Show bank informations based on the keyword
                DataTable dataTable = bankDAL.Search(keyword);
                dtgBanks.ItemsSource = dataTable.DefaultView;
                dtgBanks.AutoGenerateColumns = true;
                dtgBanks.CanUserAddRows = false;
            }
            else
            {
                //Show all banks from the database
                LoadBankDataGrid();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
