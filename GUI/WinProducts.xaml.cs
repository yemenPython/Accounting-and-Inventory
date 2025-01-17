﻿using BLL;
using CUL;
using CUL.Enums;
using DAL;
using System;
using System.Collections;
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
    /// Interaction logic for WinProducts.xaml
    /// </summary>
    public partial class WinProducts : Window
    {
        public WinProducts()
        {
            InitializeComponent();
            LoadProductDataGrid();
            PopulateCboUnits();
        }

        ProductCUL productCUL = new ProductCUL();
        ProductDAL productDAL = new ProductDAL();
        CategoryBLL categoryBLL = new CategoryBLL();
        CategoryDAL categoryDAL = new CategoryDAL();
        UserDAL userDAL = new UserDAL();
        UnitDAL unitDAL = new UnitDAL();
        UserBLL userBLL = new UserBLL();
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadProductDataGrid()
        {
            DataTable dtProduct = productDAL.SelectAllOrByKeyword();

            //dtgs.ItemsSource = dataTable.DefaultView; Adds everything at once.
            dtgProducts.AutoGenerateColumns = true;
            dtgProducts.CanUserAddRows = false;

            //LOADING THE PRODUCT DATA GRID
            LoadDataGrid(dtProduct);            
        }

        private void LoadDataGrid(DataTable dtProduct)
        {
            DataTable dtCategoryInfo;
            DataTable dtUnitRetailInfo, dtUnitWholesaleInfo;
            int initialIndex = 0;

            for (int currentRow = initialIndex; currentRow < dtProduct.Rows.Count; currentRow++)
            {
                dtUnitRetailInfo = unitDAL.GetUnitInfoById(Convert.ToInt32(dtProduct.Rows[currentRow]["unit_retail_id"]));
                dtUnitWholesaleInfo = unitDAL.GetUnitInfoById(Convert.ToInt32(dtProduct.Rows[currentRow]["unit_wholesale_id"]));
                dtCategoryInfo = categoryDAL.GetCategoryInfoById(Convert.ToInt32(dtProduct.Rows[currentRow]["category_id"]));

                dtgProducts.Items.Add(new ProductCUL()
                {
                    Id = Convert.ToInt32(dtProduct.Rows[currentRow]["id"]),
                    CategoryId = Convert.ToInt32(dtProduct.Rows[currentRow]["category_id"]),
                    UnitRetailId = Convert.ToInt32(dtProduct.Rows[currentRow]["unit_retail_id"]),
                    UnitWholesaleId = Convert.ToInt32(dtProduct.Rows[currentRow]["unit_wholesale_id"]),
                    UnitNameRetail = dtUnitRetailInfo.Rows[initialIndex]["Name"].ToString(),
                    UnitNameWholesale = dtUnitWholesaleInfo.Rows[initialIndex]["Name"].ToString(),
                    CategoryName = dtCategoryInfo.Rows[initialIndex]["Name"].ToString(),
                    BarcodeRetail = dtProduct.Rows[currentRow]["barcode_retail"].ToString(),
                    BarcodeWholesale = dtProduct.Rows[currentRow]["barcode_wholesale"].ToString(),
                    Name = dtProduct.Rows[currentRow]["name"].ToString(),
                    Description = dtProduct.Rows[currentRow]["description"].ToString(),
                    QuantityInUnit = Convert.ToDecimal(dtProduct.Rows[currentRow]["quantity_in_unit"].ToString()),
                    QuantityInStock = Convert.ToDecimal(dtProduct.Rows[currentRow]["quantity_in_stock"].ToString()),
                    CostPrice = Convert.ToDecimal(dtProduct.Rows[currentRow]["costprice"].ToString()),
                    SalePrice = Convert.ToDecimal(dtProduct.Rows[currentRow]["saleprice"].ToString()),
                    AddedDate = Convert.ToDateTime(dtProduct.Rows[currentRow]["added_date"].ToString()),
                    AddedByUsername = GetUsernameById(dtProduct, currentRow, initialIndex)
                });
            }
        }

        private string GetUsernameById(DataTable dtProduct,int currentRow,int initialIndex)
        {
            DataTable dataTableUserInfo;

            int addedById = Convert.ToInt32(dtProduct.Rows[currentRow]["added_by"]);
            dataTableUserInfo = userDAL.GetUserInfoById(addedById);
            return dataTableUserInfo.Rows[initialIndex]["first_name"].ToString() + " " + dataTableUserInfo.Rows[initialIndex]["last_name"].ToString();
        }

        private void ClearProductTextBox()
        {
            txtProductId.Text = "";
            cboProductCategory.SelectedIndex = -(int)Numbers.UnitValue;
            txtProductDescription.Text = "";
            cboProductUnitRetail.SelectedIndex = -(int)Numbers.UnitValue;
            txtProductBarcodeRetail.Text = "";
            txtProductName.Text = "";
            txtProductCostPriceRetail.Text = "";
            txtProductSalePriceRetail.Text = "";
            cboProductUnitWholesale.SelectedIndex = -(int)Numbers.UnitValue;
            txtProductBarcodeWholesale.Text = "";
            txtProductQuantityInUnitWholesale.Text = "";
            txtProductCostPriceWholesale.Text = "";
            txtProductSalePriceWholesale.Text = "";
            txtProductSearch.Text = "";
        }

        private void DtgProductsIndexChanged()//Getting the index of a particular row and fill the text boxes with the related columns of the row.
        {
            btnProductAdd.IsEnabled = false;//Disabling this button since the user can only update the product infos filled to the entry.

            object row = dtgProducts.SelectedItem;

            txtProductId.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductId].GetCellContent(row) as TextBlock).Text;
            txtProductBarcodeRetail.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductBarcodeRetail].GetCellContent(row) as TextBlock).Text;
            txtProductBarcodeWholesale.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductBarcodeWholesale].GetCellContent(row) as TextBlock).Text;
            txtProductName.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductName].GetCellContent(row) as TextBlock).Text;
            cboProductCategory.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductCategory].GetCellContent(row) as TextBlock).Text;
            txtProductDescription.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductDescription].GetCellContent(row) as TextBlock).Text;//ANOTHER METHOD: (dtgProducts.SelectedCells[rowProductDescription].Column.GetCellContent(row) as TextBlock).Text;
            txtProductQuantityInUnitWholesale.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductQuantityInUnit].GetCellContent(row) as TextBlock).Text;
            txtProductCostPriceRetail.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductCostPrice].GetCellContent(row) as TextBlock).Text;
            txtProductSalePriceRetail.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductSalePrice].GetCellContent(row) as TextBlock).Text;
            cboProductUnitRetail.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductUnitRetail].GetCellContent(row) as TextBlock).Text;
            cboProductUnitWholesale.Text = (dtgProducts.Columns[(int)ProductColumns.ColProductUnitWholeSale].GetCellContent(row) as TextBlock).Text;
            txtProductCostPriceWholesale.Text = CalculateTotalCostPrice().ToString();
            txtProductSalePriceWholesale.Text = CalculateTotalSalePrice().ToString();
        }

        private decimal CalculateTotalCostPrice()
        {
            if (txtProductCostPriceRetail.Text != "")
            {
                decimal quantity = Convert.ToDecimal(txtProductQuantityInUnitWholesale.Text);
                decimal costPriceRetail = Convert.ToDecimal(txtProductCostPriceRetail.Text);

                return quantity * costPriceRetail;
            }
            else
            {
                return 0;
            }
        }

        private decimal CalculateTotalSalePrice()
        {
            if (txtProductSalePriceRetail.Text != "")
            {
                decimal quantity = Convert.ToDecimal(txtProductQuantityInUnitWholesale.Text);
                decimal salePriceRetail = Convert.ToDecimal(txtProductSalePriceRetail.Text);

                return salePriceRetail * quantity;
            }

            else
            {
                return 0;
            }

        }

        private void dtgProducts_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DtgProductsIndexChanged();
        }

        private void dtgProducts_KeyUp(object sender, KeyEventArgs e)
        {
            DtgProductsIndexChanged();
        }

        private void dtgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            DtgProductsIndexChanged();
        }

        private void txtProductSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Get Keyword from Text box
            string keyword = txtProductSearch.Text;

            //Check if the keyword has value or not

            if (keyword != null) /*Do NOT Repeat yourself!!! Improve if statement block!!! You have similar codes in the RefreshCategoryDataGrid method!!! */
            {
                dtgProducts.Items.Clear();

                //Show product informations based on the keyword
                DataTable dtProduct = productDAL.SelectAllOrByKeyword(keyword: keyword);//The first "keyword" is the parameter name, and the second "keyword" is the local variable.

                LoadDataGrid(dtProduct);

                //for (int rowIndex = initialIndex; rowIndex < dtProduct.Rows.Count; rowIndex++)
                //{
                //    dtgProducts.Items.Add(
                //        new ProductCUL()
                //        {
                //            Id = Convert.ToInt32(dtProduct.Rows[rowIndex]["id"]),
                //            BarcodeRetail = dtProduct.Rows[rowIndex]["barcode_retail"].ToString(),
                //            BarcodeWholesale = dtProduct.Rows[rowIndex]["barcode_wholesale"].ToString(),
                //            Name = dtProduct.Rows[rowIndex]["name"].ToString(),
                //            CategoryName = categoryBLL.GetCategoryName(dtProduct, rowIndex),
                //            Description = dtProduct.Rows[rowIndex]["description"].ToString(),
                //            QuantityInUnit= dtProduct.Rows[rowIndex]["quantity_in_unit"].ToString(),
                //            QuantityInStock= dtProduct.Rows[rowIndex]["quantity_in_stock"].ToString(),
                //            CostPrice= dtProduct.Rows[rowIndex]["costprice"].ToString(),
                //            SalePrice= dtProduct.Rows[rowIndex]["saleprice"].ToString(),
                //            AddedByUserName = GetUsernameById(dtProduct, rowIndex, initialIndex),
                //            AddedDate= dtProduct.Rows[rowIndex]["added_date"].ToString(),
                //            UnitRetailId= dtProduct.Rows[rowIndex]["unit_retail_id"].ToString(),
                //            UnitWholesaleId= dtProduct.Rows[rowIndex]["unit_wholesale_id"].ToString()
                //        });
                //}
            }
            else
            {
                //Show all products from the database.
                LoadProductDataGrid();
            }
        }

        private void cboProductCategory_Loaded(object sender, RoutedEventArgs e)
        {
            //Creating Data Table to hold the products from Database
            DataTable dataTable = categoryDAL.Select();

            //Specifying Items Source for product combobox
            cboProductCategory.ItemsSource = dataTable.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboProductCategory.DisplayMemberPath = "name";

            //SelectedValuePath helps to store values like a hidden field.
            cboProductCategory.SelectedValuePath = "id";
        }

        private void PopulateCboUnits()
        {
            //Creating Data Table to hold the products from Database
            DataTable dataTable = unitDAL.Select();

            //Specifying Items Source for product combobox
            cboProductUnitRetail.ItemsSource = dataTable.DefaultView;
            cboProductUnitWholesale.ItemsSource = dataTable.DefaultView;

            //Here DisplayMemberPath helps to display Text in the ComboBox.
            cboProductUnitRetail.DisplayMemberPath = "name";
            cboProductUnitWholesale.DisplayMemberPath = "name";

            //SelectedValuePath helps to store values like a hidden field.
            cboProductUnitRetail.SelectedValuePath = "id";
            cboProductUnitWholesale.SelectedValuePath = "id";
        }

        private void RunCRUD(string btnType)
        {
            bool isSuccess = false;//Defaultly, it is false.
            string message;

            // THIS IS NOT AN EFFICIENT CODE BLOCK.
            if (txtProductName.Text != "" && cboProductCategory.SelectedValue != null && cboProductUnitRetail.SelectedValue != null && cboProductUnitWholesale.SelectedValue != null && txtProductQuantityInUnitWholesale.Text != "" && txtProductCostPriceRetail.Text != "" && txtProductSalePriceRetail.Text != "")
            {
                productCUL.Name = txtProductName.Text;
                productCUL.CategoryId = Convert.ToInt32(cboProductCategory.SelectedValue); //SelectedValue Property helps you to get the hidden value of Combobox selected Item.
                productCUL.UnitRetailId = Convert.ToInt32(cboProductUnitRetail.SelectedValue);
                productCUL.UnitWholesaleId = Convert.ToInt32(cboProductUnitWholesale.SelectedValue);
                productCUL.QuantityInUnit = Convert.ToDecimal(txtProductQuantityInUnitWholesale.Text);//You can also use ===> Convert.ToInt32(txtProductQuantityInUnitWholesale.Text)
                productCUL.CostPrice = Convert.ToDecimal(txtProductCostPriceRetail.Text);
                productCUL.SalePrice = Convert.ToDecimal(txtProductSalePriceRetail.Text);
                productCUL.Description = txtProductDescription.Text;
                productCUL.BarcodeRetail = txtProductBarcodeRetail.Text;
                productCUL.BarcodeWholesale = txtProductBarcodeWholesale.Text;
                productCUL.AddedDate = DateTime.Now;
                productCUL.AddedBy = userBLL.GetUserId(WinLogin.loggedInUserName);

                #region BUTTON CHECKING PART
                if (btnType == "Add")
                {
                    int initialQuantity = 0;
                    productCUL.Rating = initialQuantity;
                    productCUL.QuantityInStock = Convert.ToDecimal(initialQuantity);//Quantity in stock is always 0 while recording a new product.

                    isSuccess = productDAL.Insert(productCUL);
                    message = "Product has been added successfully.";
                }
                else
                {
                    productCUL.Id = Convert.ToInt32(txtProductId.Text);

                    if (btnType == "Update")
                    {
                        isSuccess = productDAL.Update(productCUL);
                        message = "Product has been updated successfully.";
                    }

                    else
                    {
                        isSuccess = productDAL.Delete(productCUL);
                        message = "Product has been deleted successfully.";
                    }
                }
                #endregion

                if (isSuccess == true)
                {
                    MessageBox.Show(message);
                    dtgProducts.Items.Clear();
                    ClearProductTextBox();
                    LoadProductDataGrid();
                }
            }

            //If any of the properties is null/unassigned in the ProductCUL class, we cannot insert/update a product.
            //bool isNull = productCUL.GetType().GetProperties()
            //                .All(p => p.GetValue(productCUL) != null); 

            else
            {
                MessageBox.Show("Something went wrong :(");
            }
        }
        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            RunCRUD("Add");
        }

        private void btnProductUpdate_Click(object sender, RoutedEventArgs e)
        {
            RunCRUD("Update");
        }

        private void btnProductDelete_Click(object sender, RoutedEventArgs e)
        {
            RunCRUD("Delete");
        }

        private void txtProductQuantityInUnitWholesale_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtProductQuantityInUnitWholesale.Text != "")
            {
                txtProductCostPriceWholesale.Text = CalculateTotalCostPrice().ToString();
                txtProductSalePriceWholesale.Text = CalculateTotalSalePrice().ToString();
            }
            else
            {
                txtProductCostPriceWholesale.Text = "";
                txtProductSalePriceWholesale.Text = "";
            }
        }

        private void txtProductCostPriceRetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtProductQuantityInUnitWholesale.Text != "")
            {
                txtProductCostPriceWholesale.Text = CalculateTotalCostPrice().ToString();
            }
        }

        private void txtProductSalePriceRetail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtProductQuantityInUnitWholesale.Text != "")
            {
                txtProductSalePriceWholesale.Text = CalculateTotalSalePrice().ToString();
            }
        }

        private void PromptBarcodeRetail()
        {
            int initialQuantity = 0;

            DataTable dtProduct = productDAL.SearchDuplications(txtProductBarcodeRetail.Text);

            if (dtProduct.Rows.Count!= initialQuantity)
            {
                string message = "There is already such product!\n Id: " + dtProduct.Rows[initialQuantity]["id"] + "\nName: " + dtProduct.Rows[initialQuantity]["name"];

                MessageBox.Show(message);

                txtProductBarcodeRetail.Text = "";
            }
        }

        private void PromptBarcodeWholesale()
        {
            int initialQuantity = 0;

            DataTable dtProduct = productDAL.SearchDuplications(txtProductBarcodeWholesale.Text);

            if (dtProduct.Rows.Count != initialQuantity)
            {
                string message = "There is already such product!\n Id: " + dtProduct.Rows[initialQuantity]["id"] + "\nName: " + dtProduct.Rows[initialQuantity]["name"];

                MessageBox.Show(message);

                txtProductBarcodeWholesale.Text = "";
            }
        }

        private void txtProductBarcodeRetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PromptBarcodeRetail();
                txtProductName.Focus();
            }
        }

        private void txtProductBarcodeWholesale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PromptBarcodeWholesale();
            }
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtProductCostPriceRetail.Focus();
                txtProductCostPriceRetail.SelectAll();
            }
        }

        private void txtProductCostPriceRetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtProductSalePriceRetail.Focus();
                txtProductSalePriceRetail.SelectAll();
            }
        }

        private void txtProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtProductSearch.SelectAll();
        }
    }
}
