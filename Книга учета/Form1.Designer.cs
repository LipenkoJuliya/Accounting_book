namespace Книга_учета
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCategories = new System.Windows.Forms.TabPage();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            this.btnEditCategory = new System.Windows.Forms.Button();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.lblDescriptionCategory = new System.Windows.Forms.Label();
            this.txtDescriptionCategory = new System.Windows.Forms.TextBox();
            this.lblNameCategory = new System.Windows.Forms.Label();
            this.txtNameCategory = new System.Windows.Forms.TextBox();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.tabPageTransactions = new System.Windows.Forms.TabPage();
            this.btnDeleteTransaction = new System.Windows.Forms.Button();
            this.btnEditTransaction = new System.Windows.Forms.Button();
            this.btnAddTransaction = new System.Windows.Forms.Button();
            this.rdbTransactionExpense = new System.Windows.Forms.RadioButton();
            this.rdbTransactionIncome = new System.Windows.Forms.RadioButton();
            this.nudTransactionAmount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTransactionDescription = new System.Windows.Forms.TextBox();
            this.cmbTransactionCategory = new System.Windows.Forms.ComboBox();
            this.dtpTransactionDate = new System.Windows.Forms.DateTimePicker();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.tabPageReports = new System.Windows.Forms.TabPage();
            this.chtCategoryTotals = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lstCategoryTotals = new System.Windows.Forms.ListBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSaveTransactions = new System.Windows.Forms.Button();
            this.btnLoadTransactions = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.tabPageTransactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransactionAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.tabPageReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtCategoryTotals)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCategories);
            this.tabControl1.Controls.Add(this.tabPageTransactions);
            this.tabControl1.Controls.Add(this.tabPageReports);
            this.tabControl1.Location = new System.Drawing.Point(12, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(855, 676);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageCategories
            // 
            this.tabPageCategories.Controls.Add(this.btnDeleteCategory);
            this.tabPageCategories.Controls.Add(this.btnEditCategory);
            this.tabPageCategories.Controls.Add(this.btnAddCategory);
            this.tabPageCategories.Controls.Add(this.lblDescriptionCategory);
            this.tabPageCategories.Controls.Add(this.txtDescriptionCategory);
            this.tabPageCategories.Controls.Add(this.lblNameCategory);
            this.tabPageCategories.Controls.Add(this.txtNameCategory);
            this.tabPageCategories.Controls.Add(this.dgvCategories);
            this.tabPageCategories.Location = new System.Drawing.Point(4, 22);
            this.tabPageCategories.Name = "tabPageCategories";
            this.tabPageCategories.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCategories.Size = new System.Drawing.Size(847, 650);
            this.tabPageCategories.TabIndex = 0;
            this.tabPageCategories.Text = "Категории";
            this.tabPageCategories.UseVisualStyleBackColor = true;
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.Location = new System.Drawing.Point(565, 67);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(113, 23);
            this.btnDeleteCategory.TabIndex = 16;
            this.btnDeleteCategory.Text = "Удалить категорию";
            this.btnDeleteCategory.UseVisualStyleBackColor = true;
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            // 
            // btnEditCategory
            // 
            this.btnEditCategory.Location = new System.Drawing.Point(326, 67);
            this.btnEditCategory.Name = "btnEditCategory";
            this.btnEditCategory.Size = new System.Drawing.Size(113, 23);
            this.btnEditCategory.TabIndex = 15;
            this.btnEditCategory.Text = "Изменить категорию";
            this.btnEditCategory.UseVisualStyleBackColor = true;
            this.btnEditCategory.Click += new System.EventHandler(this.btnEditCategory_Click);
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Location = new System.Drawing.Point(86, 67);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(113, 23);
            this.btnAddCategory.TabIndex = 14;
            this.btnAddCategory.Text = "Добавить категорию";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // lblDescriptionCategory
            // 
            this.lblDescriptionCategory.AutoSize = true;
            this.lblDescriptionCategory.Location = new System.Drawing.Point(434, 12);
            this.lblDescriptionCategory.Name = "lblDescriptionCategory";
            this.lblDescriptionCategory.Size = new System.Drawing.Size(57, 13);
            this.lblDescriptionCategory.TabIndex = 13;
            this.lblDescriptionCategory.Text = "Описание";
            // 
            // txtDescriptionCategory
            // 
            this.txtDescriptionCategory.Location = new System.Drawing.Point(437, 28);
            this.txtDescriptionCategory.Name = "txtDescriptionCategory";
            this.txtDescriptionCategory.Size = new System.Drawing.Size(241, 20);
            this.txtDescriptionCategory.TabIndex = 12;
            // 
            // lblNameCategory
            // 
            this.lblNameCategory.AutoSize = true;
            this.lblNameCategory.Location = new System.Drawing.Point(83, 12);
            this.lblNameCategory.Name = "lblNameCategory";
            this.lblNameCategory.Size = new System.Drawing.Size(57, 13);
            this.lblNameCategory.TabIndex = 11;
            this.lblNameCategory.Text = "Название";
            // 
            // txtNameCategory
            // 
            this.txtNameCategory.Location = new System.Drawing.Point(86, 28);
            this.txtNameCategory.Name = "txtNameCategory";
            this.txtNameCategory.Size = new System.Drawing.Size(238, 20);
            this.txtNameCategory.TabIndex = 10;
            // 
            // dgvCategories
            // 
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Location = new System.Drawing.Point(6, 121);
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.Size = new System.Drawing.Size(835, 523);
            this.dgvCategories.TabIndex = 9;
            // 
            // tabPageTransactions
            // 
            this.tabPageTransactions.Controls.Add(this.btnDeleteTransaction);
            this.tabPageTransactions.Controls.Add(this.btnEditTransaction);
            this.tabPageTransactions.Controls.Add(this.btnAddTransaction);
            this.tabPageTransactions.Controls.Add(this.rdbTransactionExpense);
            this.tabPageTransactions.Controls.Add(this.rdbTransactionIncome);
            this.tabPageTransactions.Controls.Add(this.nudTransactionAmount);
            this.tabPageTransactions.Controls.Add(this.label1);
            this.tabPageTransactions.Controls.Add(this.txtTransactionDescription);
            this.tabPageTransactions.Controls.Add(this.cmbTransactionCategory);
            this.tabPageTransactions.Controls.Add(this.dtpTransactionDate);
            this.tabPageTransactions.Controls.Add(this.dgvTransactions);
            this.tabPageTransactions.Location = new System.Drawing.Point(4, 22);
            this.tabPageTransactions.Name = "tabPageTransactions";
            this.tabPageTransactions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransactions.Size = new System.Drawing.Size(847, 650);
            this.tabPageTransactions.TabIndex = 1;
            this.tabPageTransactions.Text = "Транзакции";
            this.tabPageTransactions.UseVisualStyleBackColor = true;
            // 
            // btnDeleteTransaction
            // 
            this.btnDeleteTransaction.Location = new System.Drawing.Point(543, 137);
            this.btnDeleteTransaction.Name = "btnDeleteTransaction";
            this.btnDeleteTransaction.Size = new System.Drawing.Size(113, 23);
            this.btnDeleteTransaction.TabIndex = 26;
            this.btnDeleteTransaction.Text = "Удалить транзакцию";
            this.btnDeleteTransaction.UseVisualStyleBackColor = true;
            this.btnDeleteTransaction.Click += new System.EventHandler(this.btnDeleteTransaction_Click);
            // 
            // btnEditTransaction
            // 
            this.btnEditTransaction.Location = new System.Drawing.Point(304, 137);
            this.btnEditTransaction.Name = "btnEditTransaction";
            this.btnEditTransaction.Size = new System.Drawing.Size(113, 23);
            this.btnEditTransaction.TabIndex = 25;
            this.btnEditTransaction.Text = "Изменить транзакцию";
            this.btnEditTransaction.UseVisualStyleBackColor = true;
            this.btnEditTransaction.Click += new System.EventHandler(this.btnEditTransaction_Click);
            // 
            // btnAddTransaction
            // 
            this.btnAddTransaction.Location = new System.Drawing.Point(64, 137);
            this.btnAddTransaction.Name = "btnAddTransaction";
            this.btnAddTransaction.Size = new System.Drawing.Size(113, 23);
            this.btnAddTransaction.TabIndex = 24;
            this.btnAddTransaction.Text = "Добавить транзакцию";
            this.btnAddTransaction.UseVisualStyleBackColor = true;
            this.btnAddTransaction.Click += new System.EventHandler(this.btnAddTransaction_Click);
            // 
            // rdbTransactionExpense
            // 
            this.rdbTransactionExpense.AutoSize = true;
            this.rdbTransactionExpense.Location = new System.Drawing.Point(571, 88);
            this.rdbTransactionExpense.Name = "rdbTransactionExpense";
            this.rdbTransactionExpense.Size = new System.Drawing.Size(61, 17);
            this.rdbTransactionExpense.TabIndex = 23;
            this.rdbTransactionExpense.Text = "Расход";
            this.rdbTransactionExpense.UseVisualStyleBackColor = true;
            // 
            // rdbTransactionIncome
            // 
            this.rdbTransactionIncome.AutoSize = true;
            this.rdbTransactionIncome.Checked = true;
            this.rdbTransactionIncome.Location = new System.Drawing.Point(482, 88);
            this.rdbTransactionIncome.Name = "rdbTransactionIncome";
            this.rdbTransactionIncome.Size = new System.Drawing.Size(57, 17);
            this.rdbTransactionIncome.TabIndex = 22;
            this.rdbTransactionIncome.TabStop = true;
            this.rdbTransactionIncome.Text = "Доход";
            this.rdbTransactionIncome.UseVisualStyleBackColor = true;
            // 
            // nudTransactionAmount
            // 
            this.nudTransactionAmount.DecimalPlaces = 2;
            this.nudTransactionAmount.Location = new System.Drawing.Point(64, 85);
            this.nudTransactionAmount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudTransactionAmount.Name = "nudTransactionAmount";
            this.nudTransactionAmount.Size = new System.Drawing.Size(120, 20);
            this.nudTransactionAmount.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(254, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Описание";
            // 
            // txtTransactionDescription
            // 
            this.txtTransactionDescription.Location = new System.Drawing.Point(257, 38);
            this.txtTransactionDescription.Name = "txtTransactionDescription";
            this.txtTransactionDescription.Size = new System.Drawing.Size(200, 20);
            this.txtTransactionDescription.TabIndex = 19;
            // 
            // cmbTransactionCategory
            // 
            this.cmbTransactionCategory.FormattingEnabled = true;
            this.cmbTransactionCategory.Location = new System.Drawing.Point(482, 38);
            this.cmbTransactionCategory.Name = "cmbTransactionCategory";
            this.cmbTransactionCategory.Size = new System.Drawing.Size(150, 21);
            this.cmbTransactionCategory.TabIndex = 18;
            // 
            // dtpTransactionDate
            // 
            this.dtpTransactionDate.Location = new System.Drawing.Point(64, 38);
            this.dtpTransactionDate.Name = "dtpTransactionDate";
            this.dtpTransactionDate.Size = new System.Drawing.Size(150, 20);
            this.dtpTransactionDate.TabIndex = 17;
            // 
            // dgvTransactions
            // 
            this.dgvTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransactions.Location = new System.Drawing.Point(6, 185);
            this.dgvTransactions.Name = "dgvTransactions";
            this.dgvTransactions.Size = new System.Drawing.Size(835, 459);
            this.dgvTransactions.TabIndex = 16;
            // 
            // tabPageReports
            // 
            this.tabPageReports.Controls.Add(this.chtCategoryTotals);
            this.tabPageReports.Controls.Add(this.lstCategoryTotals);
            this.tabPageReports.Location = new System.Drawing.Point(4, 22);
            this.tabPageReports.Name = "tabPageReports";
            this.tabPageReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReports.Size = new System.Drawing.Size(847, 650);
            this.tabPageReports.TabIndex = 2;
            this.tabPageReports.Text = "Отчеты";
            this.tabPageReports.UseVisualStyleBackColor = true;
            // 
            // chtCategoryTotals
            // 
            chartArea2.Name = "ChartArea1";
            this.chtCategoryTotals.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chtCategoryTotals.Legends.Add(legend2);
            this.chtCategoryTotals.Location = new System.Drawing.Point(371, 22);
            this.chtCategoryTotals.Name = "chtCategoryTotals";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chtCategoryTotals.Series.Add(series2);
            this.chtCategoryTotals.Size = new System.Drawing.Size(437, 300);
            this.chtCategoryTotals.TabIndex = 1;
            this.chtCategoryTotals.Text = "chart1";
            // 
            // lstCategoryTotals
            // 
            this.lstCategoryTotals.FormattingEnabled = true;
            this.lstCategoryTotals.Location = new System.Drawing.Point(23, 22);
            this.lstCategoryTotals.Name = "lstCategoryTotals";
            this.lstCategoryTotals.Size = new System.Drawing.Size(300, 303);
            this.lstCategoryTotals.TabIndex = 0;
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblBalance.Location = new System.Drawing.Point(548, 34);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(104, 20);
            this.lblBalance.TabIndex = 27;
            this.lblBalance.Text = "Баланс: 0.00";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSaveTransactions
            // 
            this.btnSaveTransactions.Location = new System.Drawing.Point(16, 12);
            this.btnSaveTransactions.Name = "btnSaveTransactions";
            this.btnSaveTransactions.Size = new System.Drawing.Size(150, 23);
            this.btnSaveTransactions.TabIndex = 30;
            this.btnSaveTransactions.Text = "Сохранить";
            this.btnSaveTransactions.UseVisualStyleBackColor = true;
            this.btnSaveTransactions.Click += new System.EventHandler(this.btnSaveTransactions_Click);
            // 
            // btnLoadTransactions
            // 
            this.btnLoadTransactions.Location = new System.Drawing.Point(190, 12);
            this.btnLoadTransactions.Name = "btnLoadTransactions";
            this.btnLoadTransactions.Size = new System.Drawing.Size(150, 23);
            this.btnLoadTransactions.TabIndex = 31;
            this.btnLoadTransactions.Text = "Загрузить";
            this.btnLoadTransactions.UseVisualStyleBackColor = true;
            this.btnLoadTransactions.Click += new System.EventHandler(this.btnLoadTransactions_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 745);
            this.Controls.Add(this.btnLoadTransactions);
            this.Controls.Add(this.btnSaveTransactions);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Книга учета";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageCategories.ResumeLayout(false);
            this.tabPageCategories.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.tabPageTransactions.ResumeLayout(false);
            this.tabPageTransactions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransactionAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.tabPageReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtCategoryTotals)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCategories;
        private System.Windows.Forms.TabPage tabPageTransactions;
        private System.Windows.Forms.Button btnDeleteCategory;
        private System.Windows.Forms.Button btnEditCategory;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Label lblDescriptionCategory;
        private System.Windows.Forms.TextBox txtDescriptionCategory;
        private System.Windows.Forms.Label lblNameCategory;
        private System.Windows.Forms.TextBox txtNameCategory;
        private System.Windows.Forms.DataGridView dgvCategories;
        private System.Windows.Forms.Button btnDeleteTransaction;
        private System.Windows.Forms.Button btnEditTransaction;
        private System.Windows.Forms.Button btnAddTransaction;
        private System.Windows.Forms.RadioButton rdbTransactionExpense;
        private System.Windows.Forms.RadioButton rdbTransactionIncome;
        private System.Windows.Forms.NumericUpDown nudTransactionAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTransactionDescription;
        private System.Windows.Forms.ComboBox cmbTransactionCategory;
        private System.Windows.Forms.DateTimePicker dtpTransactionDate;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.TabPage tabPageReports;
        private System.Windows.Forms.ListBox lstCategoryTotals;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtCategoryTotals;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnSaveTransactions;
        private System.Windows.Forms.Button btnLoadTransactions;
    }
}