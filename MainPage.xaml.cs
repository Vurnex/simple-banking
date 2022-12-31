using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;

namespace App440Project
{
    public partial class MainPage : ContentPage
    {
        MySqlConnection conn;
        public MainPage()
        {
            InitializeComponent();

            Label connStatus = new Label() { };

            Label testLabel = new Label()
            {

            };

            try
            {
                conn = DBUtils.CreateConnection();
                conn.Open();
                connStatus.Text = "Successfully Connected to AWS DB!";

                MySqlDataReader rows = DBUtils.getAccounts(conn);

                connStatus.Text = connStatus.Text + "\nExecuted SQL Command Successfully.";

                while (rows.Read())
                {
                    testLabel.Text = testLabel.Text + "\n" +
                        rows["User"] + " , " + rows["AccountType"] + " , " + rows["AccountBalance"];
                };
                conn.Close();

            }
            catch (Exception e)
            {
                connStatus.Text = "Unsuccessful Opening connection to AWS DB!";
            }
            finally
            {
                conn.Close();
            }

            StackLayout layout = new StackLayout
            {
                Children =
                {
                    connStatus, testLabel
                }
            };

            this.Content = layout;

        }
    }
}
