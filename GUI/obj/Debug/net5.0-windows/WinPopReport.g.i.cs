// Updated by XamlIntelliSenseFileGenerator 11/22/2021 11:53:01 PM
#pragma checksum "..\..\..\WinPopReport.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E14814D8800976C2750B22109368DBA9101D19FD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GUI
{


    /// <summary>
    /// WinPopReport
    /// </summary>
    public partial class WinPopReport : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GUI;V1.0.0.0;component/winpopreport.xaml", System.UriKind.Relative);

#line 1 "..\..\..\WinPopReport.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.DockPanel dpTop;
        internal System.Windows.Controls.Label lblTop;
        internal System.Windows.Controls.Button btnClose;
        internal System.Windows.Controls.Label lblProducts;
        internal System.Windows.Controls.ListView lvwProducts;
        internal System.Windows.Controls.DatePicker dtpPosReportFrom;
        internal System.Windows.Controls.DatePicker dtpPosReportTo;
        internal System.Windows.Controls.Button btnGo;
        internal System.Windows.Shapes.Rectangle rtgProfit;
        internal System.Windows.Controls.Label lblRevenue;
        internal System.Windows.Controls.Label lblCost;
        internal System.Windows.Controls.Label lblProfit;
        internal System.Windows.Controls.Label lblRevenueVar;
        internal System.Windows.Controls.Label lblCostVar;
        internal System.Windows.Controls.Label lblProfitVar;
        internal System.Windows.Controls.Label lblDateFrom;
        internal System.Windows.Controls.Label lblDateTo;
        internal System.Windows.Controls.Label lblNumOfSalesVar;
        internal System.Windows.Controls.Image imgCash;
        internal System.Windows.Controls.Image imgCredit;
        internal System.Windows.Controls.Label lblCashSales;
        internal System.Windows.Controls.Label lblCreditSales;
        internal System.Windows.Controls.GroupBox grpUserSales;
        internal System.Windows.Controls.ListView lvwUserSales;
    }
}

