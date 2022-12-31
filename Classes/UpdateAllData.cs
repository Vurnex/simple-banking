using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

using App440Project.Pages;

namespace App440Project.Classes
{
    class UpdateAllData
    {
        MySqlConnection conn;
        public Action InSufficientFunds;
        public static bool insufficientAmount = false; //prevent further execution if amounts greater than whats stored in the database

        public void Update()
        {
            UpdateAccountsData();
        }
        public void UpdateAccountsData()
        {
            MySqlConnection staticConn;

            double newCheckingTotal = getCheckingTotal();
            double newSavingsTotal = getSavingsTotal();

            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = @"UPDATE Accounts
                                SET AccountBalance=@AccountBalance1
                                WHERE idAccounts = 1;
                                UPDATE Accounts
                                SET AccountBalance=@AccountBalance2
                                WHERE idAccounts = 2;";

            cmd.Parameters.AddWithValue("@AccountBalance1", newCheckingTotal);
            cmd.Parameters.AddWithValue("@AccountBalance2", newSavingsTotal);

            staticConn = DBUtils.CreateConnection();
            staticConn.Open();

            cmd.Connection = staticConn;
            cmd.ExecuteNonQuery();

            staticConn.Close();

            return;
        }

        public double getCheckingTotal()
        {
            double checkingTotal = 0;
            double amount = 0;

            MySqlCommand cmd = new MySqlCommand();

            conn = DBUtils.CreateConnection();
            conn.Open();

            MySqlDataReader checkingData = DBUtils.getCheckingTransactions(conn);

            while (checkingData.Read())
            {
                amount = (double)checkingData["Amount"];

                checkingTotal += amount;

            };

            conn.Close();

            return checkingTotal;
        }

        public double getSavingsTotal()
        {
            double savingsTotal = 0;
            double amount = 0;

            conn = DBUtils.CreateConnection();
            conn.Open();

            MySqlDataReader savingsData = DBUtils.getSavingsTransactions(conn);

            while (savingsData.Read())
            {
                amount = (double)savingsData["Amount"];

                savingsTotal += amount;

            };

            conn.Close();

            return savingsTotal;
        }

        public void UpdateWithTransferData(string transferAccount, double transferAmount)
        {
            double currentCheckingAmount = 0;
            double currentSavingsAmount = 0;

            conn = DBUtils.CreateConnection();
            conn.Open();

            //Get Current Checking Balance

            MySqlCommand getCheckingAmount = new MySqlCommand();

            getCheckingAmount.CommandText = @"SELECT AccountBalance
                                FROM `Database1`.`Accounts`
                                WHERE idAccounts = 1;";

            getCheckingAmount.Connection = conn;

            MySqlDataReader checkingAmountData = getCheckingAmount.ExecuteReader();

            while (checkingAmountData.Read())
            {
                currentCheckingAmount = (double)checkingAmountData["AccountBalance"];
            };

            conn.Close();

            //Get Current Savings Balance

            conn.Open();

            MySqlCommand getSavingsAmount = new MySqlCommand();
            getSavingsAmount.Connection = conn;

            getSavingsAmount.CommandText = @"SELECT AccountBalance
                                FROM `Database1`.`Accounts`
                                WHERE idAccounts = 2;";

            MySqlDataReader savingsAmountData = getSavingsAmount.ExecuteReader();

            while (savingsAmountData.Read())
            {
                currentSavingsAmount = (double)savingsAmountData["AccountBalance"];
            };

            conn.Close();

            //Do calculations
            if (transferAccount == "Checking")
            {
                if (transferAmount > currentCheckingAmount)
                {
                    insufficientAmount = true;
                    InSufficientFunds();
                    return;
                }
                else
                {
                    insufficientAmount = false;
                    currentCheckingAmount -= transferAmount;
                    currentSavingsAmount += transferAmount;
                }
            }
            else if (transferAccount == "Savings")
            {
                if (transferAmount > currentSavingsAmount)
                {
                    insufficientAmount = true;
                    InSufficientFunds();
                    return;
                }
                else
                {
                    insufficientAmount = false;
                    currentSavingsAmount -= transferAmount;
                    currentCheckingAmount += transferAmount;
                }
            }

            //Update data in Database

            MySqlCommand cmd = new MySqlCommand();

            conn.Open();

            cmd.CommandText = @"UPDATE Accounts
                                SET AccountBalance=@AccountBalance1
                                WHERE idAccounts = 1;
                                UPDATE Accounts
                                SET AccountBalance=@AccountBalance2
                                WHERE idAccounts = 2;";

            cmd.Parameters.AddWithValue("@AccountBalance1", currentCheckingAmount);
            cmd.Parameters.AddWithValue("@AccountBalance2", currentSavingsAmount);

            cmd.Connection = conn;

            cmd.ExecuteNonQuery();

            conn.Close();

        }

        public void InsertTransactionRecords(string transactionType, string transferAccount, double transferAmount)
        {
            if (insufficientAmount)
            {
                return;
            }

            conn = DBUtils.CreateConnection();

            MySqlCommand insertCheckingTransaction = new MySqlCommand();
            MySqlCommand insertSavingsTransaction = new MySqlCommand();

            //Convert to negative value
            double negTransferAmount = transferAmount * -1;

            if (transferAccount == "Checking")
            {

                //Create new record in the checking transaction table

                insertCheckingTransaction.CommandText = @"INSERT INTO CheckingTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertCheckingTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertCheckingTransaction.Parameters.AddWithValue("@Amount", negTransferAmount);

                insertCheckingTransaction.Connection = conn;
                conn.Open();

                insertCheckingTransaction.ExecuteNonQuery();

                conn.Close();

                //Create new record in the savings transaction table

                insertSavingsTransaction.CommandText = @"INSERT INTO SavingsTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertSavingsTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertSavingsTransaction.Parameters.AddWithValue("@Amount", transferAmount);

                insertSavingsTransaction.Connection = conn;
                conn.Open();

                insertSavingsTransaction.ExecuteNonQuery();

                conn.Close();

            }
            else if (transferAccount == "Savings")
            {
                //Create new record in the savings transaction table

                insertSavingsTransaction.CommandText = @"INSERT INTO SavingsTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertSavingsTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertSavingsTransaction.Parameters.AddWithValue("@Amount", negTransferAmount);

                insertSavingsTransaction.Connection = conn;
                conn.Open();

                insertSavingsTransaction.ExecuteNonQuery();

                conn.Close();

                //Create new record in the checking transaction table

                insertCheckingTransaction.CommandText = @"INSERT INTO CheckingTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertCheckingTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertCheckingTransaction.Parameters.AddWithValue("@Amount", transferAmount);

                insertCheckingTransaction.Connection = conn;
                conn.Open();

                insertCheckingTransaction.ExecuteNonQuery();

                conn.Close();
            }

        }

        public void DepositCheck(string transactionType, string depositAccount, double depositAmount)
        {
            conn = DBUtils.CreateConnection();

            MySqlCommand insertCheckingTransaction = new MySqlCommand();
            MySqlCommand insertSavingsTransaction = new MySqlCommand();

            if (depositAccount == "Checking")
            {
                //Create new record in the checking transaction table

                insertCheckingTransaction.CommandText = @"INSERT INTO CheckingTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertCheckingTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertCheckingTransaction.Parameters.AddWithValue("@Amount", depositAmount);

                insertCheckingTransaction.Connection = conn;
                conn.Open();

                insertCheckingTransaction.ExecuteNonQuery();

                conn.Close();
            }
            else if (depositAccount == "Savings")
            {
                //Create new record in the savings transaction table

                insertSavingsTransaction.CommandText = @"INSERT INTO SavingsTransactions
                                                          (Transaction, Amount)
                                                          VALUES (@Transaction, @Amount)";

                insertSavingsTransaction.Parameters.AddWithValue("@Transaction", transactionType);
                insertSavingsTransaction.Parameters.AddWithValue("@Amount", depositAmount);

                insertSavingsTransaction.Connection = conn;
                conn.Open();

                insertSavingsTransaction.ExecuteNonQuery();

                conn.Close();
            }
        }

        public void PayBill(string selectedAccount, double paymentAmount, string selectedBill)
        {
            double currentCheckingAmount = 0;
            double currentSavingsAmount = 0;

            conn = DBUtils.CreateConnection();
            conn.Open();

            //Get Current Checking Balance

            MySqlCommand getCheckingAmount = new MySqlCommand();

            getCheckingAmount.CommandText = @"SELECT AccountBalance
                                FROM `Database1`.`Accounts`
                                WHERE idAccounts = 1;";

            getCheckingAmount.Connection = conn;

            MySqlDataReader checkingAmountData = getCheckingAmount.ExecuteReader();

            while (checkingAmountData.Read())
            {
                currentCheckingAmount = (double)checkingAmountData["AccountBalance"];
            };

            conn.Close();

            //Get Current Savings Balance

            conn.Open();

            MySqlCommand getSavingsAmount = new MySqlCommand();
            getSavingsAmount.Connection = conn;

            getSavingsAmount.CommandText = @"SELECT AccountBalance
                                FROM `Database1`.`Accounts`
                                WHERE idAccounts = 2;";

            MySqlDataReader savingsAmountData = getSavingsAmount.ExecuteReader();

            while (savingsAmountData.Read())
            {
                currentSavingsAmount = (double)savingsAmountData["AccountBalance"];
            };

            conn.Close();

            //Do calculations
            if (selectedAccount == "Checking")
            {
                if (paymentAmount > currentCheckingAmount)
                {
                    insufficientAmount = true;
                    InSufficientFunds();
                    return;
                }
                else
                {
                    insufficientAmount = false;
                    currentCheckingAmount -= paymentAmount;
                    currentSavingsAmount += paymentAmount;
                }
            }
            else if (selectedAccount == "Savings")
            {
                if (paymentAmount > currentSavingsAmount)
                {
                    insufficientAmount = true;
                    InSufficientFunds();
                    return;
                }
                else
                {
                    insufficientAmount = false;
                    currentSavingsAmount -= paymentAmount;
                    currentCheckingAmount += paymentAmount;
                }
            }

            //Update data in Database

            MySqlCommand cmd = new MySqlCommand();

            conn.Open();

            cmd.CommandText = @"UPDATE Accounts
                                SET AccountBalance=@AccountBalance1
                                WHERE idAccounts = 1;
                                UPDATE Accounts
                                SET AccountBalance=@AccountBalance2
                                WHERE idAccounts = 2;";

            cmd.Parameters.AddWithValue("@AccountBalance1", currentCheckingAmount);
            cmd.Parameters.AddWithValue("@AccountBalance2", currentSavingsAmount);

            cmd.Connection = conn;

            cmd.ExecuteNonQuery();

            conn.Close();

            //Remove Paid Bills

            MySqlCommand deleteBill = new MySqlCommand();

            deleteBill.CommandText = @"DELETE FROM Bills
                                       WHERE BillTitle=@BillTitle";

            deleteBill.Parameters.AddWithValue("@BillTitle", selectedBill);

            deleteBill.Connection = conn;

            conn.Open();

            deleteBill.ExecuteNonQuery();

            conn.Close();

        }
    }
}
