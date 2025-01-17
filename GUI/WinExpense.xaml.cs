﻿using BLL;
using CUL;
using CUL.Enums;
using DAL;
using DAL;
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
    /// Interaction logic for WinExpense.xaml
    /// </summary>
    public partial class WinExpense : Window
    {
        AccountDAL accountDAL = new AccountDAL();
        SupplierDAL supplierDAL = new SupplierDAL();
        CustomerDAL customerDAL = new CustomerDAL();
        PaymentDAL paymentDAL = new PaymentDAL();
        SourceTypeDAL sourceTypeDAL = new SourceTypeDAL();
        UserBLL userBLL = new UserBLL();
        ExpenseCUL expenseCUL = new ExpenseCUL();
        ExpenseBLL expenseBLL = new ExpenseBLL();
        ExpenseDAL expenseDAL = new ExpenseDAL();
        AssetCUL assetCUL = new AssetCUL();
        AssetDAL assetDAL = new AssetDAL();
        BankDAL bankDAL = new BankDAL();
        CommonBLL commonBLL = new CommonBLL();

        const string calledBy = "tbl_expenses", colTxtName = "name", colTxtId = "id", colTxtIdFrom = "id_from", colTxtIdTo = "id_to", colTxtIdAssetFrom = "id_asset_from", colTxtIdAssetTo = "id_asset_to", colTxtAmount = "amount", colTxtDetails = "details", colTxtDateAdded = "added_date", colTxtPaymentType = "payment_type", colTxtPaymentTypeId = "id_payment_type";
        const int clickedNothing = -1, clickedNew = 0, clickedPrev = 0, clickedNext = 1,clickedEdit = 1, clickedNull = 2;//0 stands for user clicked the button New, and 1 stands for user clicked the button Edit.
        const int expenseSize = 5;
        const int oldBalanceFrom=0, oldBalanceTo=1, oldAssetIdFrom=2, oldAssetIdTo=3, oldAmount=4;
        int clickedNewOrEdit, clickedArrow;
        string[] oldExpense = new string[expenseSize];
        bool isCboSelectionEnabled = true;

        public WinExpense()
        {
            InitializeComponent();
            DisableTools();
            LoadPastExpense();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FirstTimeRun()
        {
            MessageBox.Show("Welcome!\n Thank you for choosing Accounting and Inventory System.");
            btnPrev.IsEnabled = false;//Disabling the btnPrev button because there is no any records in the database for the first time.
            btnNext.IsEnabled = false;//Disabling the btnNext button because there is no any records in the database for the first time.
        }

        private void ClearTools()
        {
            isCboSelectionEnabled = false;//We need to disable the SelectionChanged methods in case any methods below would trig them.
            cboSourceTo.ItemsSource = null;
            cboFrom.ItemsSource = null;
            cboTo.ItemsSource = null;
            rbAccount.IsChecked = false;
            rbBank.IsChecked = false;
            isCboSelectionEnabled = true;

            lblBalanceFrom.Content = "";
            lblBalanceTo.Content = "";
            lblAssetIdFrom.Content = "";
            lblAssetIdTo.Content = "";
            lblDateAdded.Content = "";
            txtDetails.Text = "";
            txtAmount.Text = "";
        }

        private void ClearLabelsFrom()
        {
            lblBalanceFrom.Content = "";
            lblAssetIdFrom.Content = "";
        }

        private void ClearLabelsTo()
        {
            lblBalanceTo.Content = "";
            lblAssetIdTo.Content = "";
        }

        public void DisableTools()
        {
            btnMenuSave.IsEnabled = false;
            btnMenuCancel.IsEnabled = false;
            cboSourceTo.IsEnabled = false;
            cboFrom.IsEnabled = false;
            cboTo.IsEnabled = false;
            cboPaymentType.IsEnabled = false;
            txtAmount.IsEnabled = false;
            txtDetails.IsEnabled = false;
            rbAccount.IsEnabled = false;
            rbBank.IsEnabled = false;
        }

        //public void EnableTools()
        //{
        //    btnMenuSave.IsEnabled = true;
        //    btnMenuCancel.IsEnabled = true;
        //    cboSourceTo.IsEnabled = true;
        //    cboPaymentType.IsEnabled = true;
        //    cboFrom.IsEnabled = true;
        //    cboTo.IsEnabled = true;
        //    txtAmount.IsEnabled = true;
        //    txtDetails.IsEnabled = true;
        //    rbAccount.IsEnabled = true;
        //    rbBank.IsEnabled = true;
        //}

        private void EnableButtonsOnClickSaveCancel()
        {
            btnMenuNew.IsEnabled = true;
            btnMenuEdit.IsEnabled = true;
            btnMenuDelete.IsEnabled = true;
            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
        }

        private void ModifyToolsOnClickBtnNewEdit()
        {
            btnMenuSave.IsEnabled = true;
            btnMenuCancel.IsEnabled = true;
            btnMenuNew.IsEnabled = false;
            btnMenuEdit.IsEnabled = false;
            btnMenuDelete.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            cboPaymentType.IsEnabled = true;
            cboSourceTo.IsEnabled = true;
            cboFrom.IsEnabled = true;
            cboTo.IsEnabled = true;
            txtAmount.IsEnabled = true;
            txtDetails.IsEnabled = true;
            rbAccount.IsEnabled = true;
            rbBank.IsEnabled = true;
        }

        private void LoadNewExpense()
        {
            //ClearBasketTextBox();
            //ClearProductsDataGrid();

            int expenseNo;

            expenseNo = expenseBLL.GetLastExpenseNumber();//Getting the last expense number and assign it to the variable called expenseNo.
            expenseNo +=(int)Numbers.UnitValue;//We are adding one to the last expense number because every new expense number is one greater tham the previous expense number.
            lblExpenseId.Content = expenseNo;//Assigning expenseNo to the content of the expenseNo Label.
        }

        //-1 means user did not clicked either previous or next button which means user just clicked the point of purchase button to open it.
        private void LoadPastExpense(int expenseId = (int)Numbers.InitialIndex, int expenseArrow = clickedNothing)//Optional parameter
        {
            int idAssetFrom, idAssetTo, idFrom, idTo;

            if (expenseId == (int)Numbers.InitialIndex)//If the ID is 0 came from the optional parameter, that means user just clicked the WinPOP button to open it.
            {
                expenseId = commonBLL.GetLastRecordById(calledBy);//Getting the last invoice id and assign it to the variable called invoiceId.
            }

            /*WE CANNOT USE ELSE IF FOR THE CODE BELOW! BOTH IF STATEMENTS ABOVE AND BELOVE MUST WORK.*/
            if (expenseId != (int)Numbers.InitialIndex)// If the invoice number is still 0 even when we get the last invoice number by using code above, that means this is the first sale and do not run this code block.
            {
                DataTable dtExpense = expenseDAL.GetByExpenseId(expenseId);

                if (dtExpense.Rows.Count != (int)Numbers.InitialIndex)
                {
                    expenseId = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtId].ToString());//Getting the id of account.
                    lblExpenseId.Content = expenseId;

                    #region SOURCE TYPE CBO INFORMATION FILLING REGION
                    //RB SOURCE-FROM FILLING
                    idAssetFrom = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtIdAssetFrom].ToString()); //Fetching the id_asset_from in order to get full details about the specific asset later.

                    DataTable dtAssetFrom = assetDAL.SearchById(idAssetFrom);//Sending the idAssetFrom in order the fetch full details of the asset.
                    int idSourceTypeFrom = Convert.ToInt32(dtAssetFrom.Rows[(int)Numbers.InitialIndex]["id_source_type"]);

                    if (idSourceTypeFrom == Convert.ToInt32(Assets.Account))//This code trigs the method LoadCboFrom in the methods rbAccount_Checked and rbBank_Checked!
                        rbAccount.IsChecked = true;
                    else
                        rbBank.IsChecked = true;

                    //CBO SOURCE-TO FILLING
                    idAssetTo = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtIdAssetTo].ToString()); //Fetching the id_asset_to in order to get full details about the specific asset later.

                    DataTable dtAssetTo = assetDAL.SearchById(idAssetTo);//Sending the idAssetFrom in order the fetch full details of the asset.
                    int idSourceTypeTo = Convert.ToInt32(dtAssetTo.Rows[(int)Numbers.InitialIndex]["id_source_type"]);

                    LoadCboSourceTo();//We need to load the cboSourceTo first in order to get which source type the user has clicked below.
                    cboSourceTo.SelectedValue = idSourceTypeTo;//This code trigs the method LoadCboTo in the method cboSourceTo_SelectionChanged!
                    #endregion

                    LoadCboPaymentType();//Loading payment types.
                    cboPaymentType.SelectedValue = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtPaymentTypeId].ToString());//Getting the id of payment type.

                    #region SOURCE CBO INFORMATION FILLING REGION 
                    idFrom = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtIdFrom].ToString());
                    //LoadCboFrom(idSourceTypeFrom);No need for this code because it is automatically trigged by the code line --cboSourceFrom.SelectedValue = idSourceTypeFrom-- above.
                    cboFrom.SelectedValue = idFrom;

                    idTo = Convert.ToInt32(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtIdTo].ToString());
                    //LoadCboTo(idSourceTypeTo);No need for this code because it is automatically trigged by the code line --cboSourceTo.SelectedValue = idSourceTypeTo-- above.
                    cboTo.SelectedValue = idTo;
                    #endregion

                    txtAmount.Text = dtExpense.Rows[(int)Numbers.InitialIndex][colTxtAmount].ToString();
                    txtDetails.Text = dtExpense.Rows[(int)Numbers.InitialIndex][colTxtDetails].ToString();
                    lblDateAdded.Content = Convert.ToDateTime(dtExpense.Rows[(int)Numbers.InitialIndex][colTxtDateAdded]).ToString("f");
                }
                else if (dtExpense.Rows.Count == (int)Numbers.InitialIndex)//If the pop detail row quantity is 0, that means there is no such row so decrease or increase the invoice number according to user preference.
                {
                    if (expenseArrow == (int)Numbers.InitialIndex)//If the invoice arrow is 0, that means user clicked the previous button.
                    {
                        expenseId = expenseId - (int)Numbers.UnitValue;
                    }
                    else
                    {
                        expenseId = expenseId + (int)Numbers.UnitValue;
                    }

                    if (expenseArrow != -(int)Numbers.UnitValue)//If the user has not clicked either previous or next button, then the invoiceArrow will be -1 and no need for recursion.
                    {
                        LoadPastExpense(expenseId, expenseArrow);//Call the method again to get the new past invoice.
                    }

                }
            }
            else
            {
                FirstTimeRun();//This method is called when it is the first time of using this program.
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            int firstExpenseId = (int)Numbers.UnitValue, currentExpenseId = Convert.ToInt32(lblExpenseId.Content);

            if (currentExpenseId != firstExpenseId)
            {
                int prevExpenseId = currentExpenseId - (int)Numbers.UnitValue;

                clickedArrow = clickedPrev;//0 means customer has clicked the previous button.
                ClearTools();
                LoadPastExpense(prevExpenseId, clickedArrow);
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int lastExpenseId = commonBLL.GetLastRecordById(calledBy), currentInvoiceId;

            currentInvoiceId = Convert.ToInt32(lblExpenseId.Content);

            if (currentInvoiceId != lastExpenseId)
            {
                int nextInvoice = currentInvoiceId + (int)Numbers.UnitValue;

                clickedArrow = clickedNext;//1 means customer has clicked the next button.
                ClearTools();
                LoadPastExpense(nextInvoice, clickedArrow);
            }
        }

        private void btnMenuNew_Click(object sender, RoutedEventArgs e)
        {
            clickedNewOrEdit = clickedNew;//0 stands for the user has entered the btnNew.

            ClearTools();
            LoadNewExpense();
            LoadCboSourceTo();
            ModifyToolsOnClickBtnNewEdit();
        }

        private void btnMenuSave_Click(object sender, RoutedEventArgs e)
        {
            int emptyIndex = -1;

            if (cboFrom.SelectedIndex != emptyIndex && cboTo.SelectedIndex != emptyIndex && txtAmount.Text != "")
            {
                int expenseId = Convert.ToInt32(lblExpenseId.Content); /*lblExpenseId stands for the expense id in the database.*/
                int userId = userBLL.GetUserId(WinLogin.loggedInUserName);
                bool isSuccess = false, isSuccessAsset = false, isSuccessAssetSupplier = false;

                #region ASSIGNING CUL SECTION
                expenseCUL.Id = expenseId;
                expenseCUL.IdPaymentType= Convert.ToInt32(cboPaymentType.SelectedValue);
                expenseCUL.IdFrom = Convert.ToInt32(cboFrom.SelectedValue);
                expenseCUL.IdTo = Convert.ToInt32(cboTo.SelectedValue);
                expenseCUL.IdAssetFrom = Convert.ToInt32(lblAssetIdFrom.Content);
                expenseCUL.IdAssetTo = Convert.ToInt32(lblAssetIdTo.Content);
                expenseCUL.Amount = Convert.ToDecimal(txtAmount.Text);
                expenseCUL.Details = txtDetails.Text;
                expenseCUL.AddedBy = userId;
                expenseCUL.AddedDate = DateTime.Now;
                #endregion

                if (clickedNewOrEdit == clickedEdit)
                {
                    isSuccess = expenseBLL.UpdateExpense(expenseCUL);

                    #region TABLE ASSET REVERTING AND UPDATING SECTION
                    //UPDATING THE ASSET FOR EXPENSE OF THE CORPORATION.
                    assetCUL.Id = Convert.ToInt32(oldExpense[oldAssetIdFrom]);
                    assetCUL.SourceBalance = Convert.ToDecimal(oldExpense[oldBalanceFrom]) + Convert.ToDecimal(oldExpense[oldAmount]);//We have to add this amount into company's balance in order to revert the old expense.
                    isSuccessAsset = assetDAL.Update(assetCUL);

                    //UPDATING THE ASSET FOR BALANCE OF THE SUPPLIER.
                    assetCUL.Id = Convert.ToInt32(oldExpense[oldAssetIdTo]);
                    assetCUL.SourceBalance = Convert.ToDecimal(oldExpense[oldBalanceTo]) - Convert.ToDecimal(oldExpense[oldAmount]);//We have to subtract this amount from supplier's balance in order to revert our dept.
                    isSuccessAssetSupplier = assetDAL.Update(assetCUL);
                    #endregion
                }

                else
                {
                    isSuccess = expenseBLL.InsertExpense(expenseCUL);
                }

                #region TABLE ASSET UPDATING SECTION
                //UPDATING THE ASSET FOR EXPENSE OF THE CORPORATION.
                assetCUL.Id = Convert.ToInt32(lblAssetIdFrom.Content);
                assetCUL.SourceBalance = Convert.ToDecimal(GetBalance(Convert.ToInt32(lblAssetIdFrom.Content))) - Convert.ToDecimal(txtAmount.Text);//We have to subtract this amount from company's balance in order to make the payment to the supplier.
                isSuccessAsset = assetDAL.Update(assetCUL);

                //UPDATING THE ASSET FOR BALANCE OF THE SUPPLIER.
                assetCUL.Id = Convert.ToInt32(lblAssetIdTo.Content);
                assetCUL.SourceBalance = Convert.ToDecimal(GetBalance(Convert.ToInt32(lblAssetIdTo.Content))) + Convert.ToDecimal(txtAmount.Text);//We have to add this amount to supplier's balance in order to reset our dept.
                isSuccessAssetSupplier = assetDAL.Update(assetCUL);
                #endregion

                //If the data is inserted successfully, then the value of the variable isSuccess will be true; otherwise it will be false.
                if (isSuccess == true && isSuccessAsset == true && isSuccessAssetSupplier == true)//IsSuccessDetail is always CHANGING in every loop above! IMPROVE THIS!!!!
                {
                    DisableTools();
                    EnableButtonsOnClickSaveCancel();
                }
                else
                {
                    MessageBox.Show("Something went wrong :(");
                }
            }
        }

        private void btnMenuCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Would you really like to cancel the expense page?", "Cancel Expense", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    DisableTools();
                    ClearTools();
                    LoadPastExpense();
                    EnableButtonsOnClickSaveCancel();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Enjoy!", "Enjoy");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Nevermind then...", "Accounting");
                    break;
            }
        }

        private void btnMenuEdit_Click(object sender, RoutedEventArgs e)
        {
            ModifyToolsOnClickBtnNewEdit();

            oldExpense[oldBalanceFrom] = lblBalanceFrom.Content.ToString();
            oldExpense[oldBalanceTo] = lblBalanceTo.Content.ToString();
            oldExpense[oldAssetIdFrom] = lblAssetIdFrom.Content.ToString();
            oldExpense[oldAssetIdTo] = lblAssetIdTo.Content.ToString();
            oldExpense[oldAmount] = txtAmount.Text.ToString();

            clickedNewOrEdit = clickedEdit;//Changing the state of the clicked NewOrEdit in order to update the old expense page.
        }

        private void btnMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Would you really like to delete the expense?", "Delete Expense", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    #region TABLE ASSET REVERTING AND UPDATING SECTION
                    //REVERTING THE ASSET FOR EXPENSE OF THE CORPORATION.
                    assetCUL.Id = Convert.ToInt32(lblAssetIdFrom.Content);
                    assetCUL.SourceBalance = Convert.ToDecimal(GetBalance(Convert.ToInt32(lblAssetIdFrom.Content))) + Convert.ToDecimal(txtAmount.Text);//We have to add this amount into company's balance in order to revert the old expense.
                    assetDAL.Update(assetCUL);

                    //REVERTING THE ASSET FOR BALANCE OF THE SUPPLIER.
                    assetCUL.Id = Convert.ToInt32(lblAssetIdTo.Content);
                    assetCUL.SourceBalance = Convert.ToDecimal(GetBalance(Convert.ToInt32(lblAssetIdTo.Content))) - Convert.ToDecimal(txtAmount.Text);//We have to subtract this amount from supplier's balance in order to revert our dept.
                    assetDAL.Update(assetCUL);
                    #endregion

                    #region DELETE EXPENSE RECORD
                    int expenseId = Convert.ToInt32(lblExpenseId.Content);

                    expenseDAL.Delete(expenseId);
                    #endregion

                    LoadPastExpense();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Enjoy!", "Enjoy");
                    break;
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Nevermind then...", "Accounting");
                    break;
            }
        }

        private void cboFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCboSelectionEnabled == true)
            {
                CboFromSelectionChanged();
            }
        }

        private void cboTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCboSelectionEnabled == true)
            {
                CboToSelectionChanged();
            }
        }
        
        private void cboSourceTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCboSelectionEnabled == true)
            {
                ClearLabelsTo();

                LoadCboTo(Convert.ToInt32(cboSourceTo.SelectedValue));
            }
        }
        private void rbAccount_Checked(object sender, RoutedEventArgs e)
        {
            if (isCboSelectionEnabled == true)
            {
                ClearLabelsFrom();
                LoadCboFrom(Convert.ToInt32(Assets.Account));
            }
        }

        private void rbBank_Checked(object sender, RoutedEventArgs e)
        {
            if (isCboSelectionEnabled == true)
            {
                ClearLabelsFrom();
                LoadCboFrom(Convert.ToInt32(Assets.Bank));
            }
        }

        private void CboFromSelectionChanged()
        {
                #region LBLASSETIDFROM POPULATING SECTION
                int sourceId, assetId;
                int sourceType;

                if (rbAccount.IsChecked == true)
                    sourceType = (int)Assets.Account;
                else
                    sourceType = (int)Assets.Bank;

                sourceId = Convert.ToInt32(cboFrom.SelectedValue);
                assetId = assetDAL.GetAssetIdBySource(sourceId, sourceType);
                lblAssetIdFrom.Content = assetId;
                #endregion

                #region LBLBALANCEFROM POPULATING SECTION
                DataTable dtAsset = assetDAL.SearchById(assetId);

                string balance = dtAsset.Rows[(int)Numbers.InitialIndex]["source_balance"].ToString();

                lblBalanceFrom.Content = balance;
                #endregion
        }

        private void CboToSelectionChanged()
        {
            #region LBLASSETIDTO POPULATING SECTION
            int sourceId, assetId, sourceTypeId;

            sourceTypeId = Convert.ToInt32(cboSourceTo.SelectedValue);
            sourceId = Convert.ToInt32(cboTo.SelectedValue);
            assetId = assetDAL.GetAssetIdBySource(sourceId, sourceTypeId);
            lblAssetIdTo.Content = assetId;
            #endregion

            #region LBLBALANCETO POPULATING SECTION
            DataTable dtAsset = assetDAL.SearchById(assetId);

            string balance = dtAsset.Rows[(int)Numbers.InitialIndex]["source_balance"].ToString();

            lblBalanceTo.Content = balance;
            #endregion
        }

        private decimal GetBalance(int assetId)
        {
            DataTable dtAsset = assetDAL.SearchById(assetId);

            decimal balance = Convert.ToDecimal(dtAsset.Rows[(int)Numbers.InitialIndex]["source_balance"]);

            return balance;
        }


        private DataTable FetchSourceData(int idSourceType)
        {
            DataTable dtSource;

            switch (idSourceType)
            {
                case (int)Assets.Account:
                    dtSource = accountDAL.Select();
                    break;
                case (int)Assets.Bank:
                    dtSource = bankDAL.Select();
                    break;
                case (int)Assets.Supplier:
                    dtSource = supplierDAL.Select();
                    break;
                default:
                    dtSource = customerDAL.Select();
                    break;
            }
            return dtSource;
        }

        private void LoadCboSourceTo()
        {
            isCboSelectionEnabled = false;//Disabling the selection changed method in order to prevent them to work when we reassign the combobox with unselected status.

            DataTable dtSourceTo = sourceTypeDAL.Select();


            //Specifying Items Source for product combobox
            cboSourceTo.ItemsSource = dtSourceTo.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboSourceTo.DisplayMemberPath = colTxtName;

            //SelectedValuePath helps to store values like a hidden field.
            cboSourceTo.SelectedValuePath = colTxtId;

            isCboSelectionEnabled = true;//Enabling the selection changed method in order to allow them to work in case of any future selections.
        }

        private void LoadCboPaymentType()
        {
            //Creating Data Table to hold the products from Database
            DataTable dtPayment = paymentDAL.Select();

            //Specifying Items Source for product combobox
            cboPaymentType.ItemsSource = dtPayment.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboPaymentType.DisplayMemberPath = colTxtPaymentType;

            //SelectedValuePath helps to store values like a hidden field.
            cboPaymentType.SelectedValuePath = colTxtId;
        }

        private void LoadCboFrom(int idSourceType)
        {
            isCboSelectionEnabled = false;//Disabling the selection changed method in order to prevent them to work when we reassign the combobox with unselected status.

            DataTable dtFrom;//Creating Data Table to hold the products from Database.
            if (idSourceType == (int)Assets.Account)
                dtFrom = accountDAL.Select();

            else
                dtFrom = bankDAL.Select();

            //Specifying Items Source for product combobox
            cboFrom.ItemsSource = dtFrom.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboFrom.DisplayMemberPath = colTxtName;

            //SelectedValuePath helps to store values like a hidden field.
            cboFrom.SelectedValuePath = colTxtId;

            isCboSelectionEnabled = true;//Enabling the selection changed method in order to allow them to work in case of any future selections.
        }

        private void LoadCboTo(int idSourceType)
        {
            isCboSelectionEnabled = false;//Disabling the selection changed method in order to prevent them to work when we reassign the combobox with unselected status.

            DataTable dtTo = FetchSourceData(idSourceType);

            //Specifying Items Source for product combobox
            cboTo.ItemsSource = dtTo.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboTo.DisplayMemberPath = colTxtName;

            //SelectedValuePath helps to store values like a hidden field.
            cboTo.SelectedValuePath = colTxtId;

            isCboSelectionEnabled = true;//Enabling the selection changed method in order to allow them to work in case of any future selections.
        }
    }
}
