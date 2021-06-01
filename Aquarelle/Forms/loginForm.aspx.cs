using Aquarelle.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aquarelle
{
    public partial class login : System.Web.UI.Page
    {
        #region Variables
        private static readonly string USERS_FILE_PATH = PATHS.LOGIN_INFO_FILE_PATH;
        private static List<String> USERS;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoginAlert.Visible = false;
            if (!this.IsPostBack)
            {
                USERS = new List<string>();
                GetUsers();
            }
        }

        protected void loginBtn_click(object sender, EventArgs e)
        {
            if (IsValidUser())
            {
                Session["activeUser"] = username.Value;
                Response.Redirect("userForm.aspx");
            }
            else
                DisplayErrorAlert("User not found!", "1. Please verify the login credentials in the users.txt file. Or, add a login credential in the same file");
        }
        #endregion

        #region Private Methods
        private void GetUsers()
        {
            string line;

            try
            {
                StreamReader usersFile = new StreamReader(USERS_FILE_PATH);

                while ((line = usersFile.ReadLine()) != null)
                {
                    USERS.Add(line);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                DisplayErrorAlert("Directory not found!", "1. Go to 'Classes' folder." + Environment.NewLine + "2. Open the PATHS.cs class" + Environment.NewLine + "3. Change the private variable 'pathToChange' to the path this current project is saved on your computer.");
            }


        }

        private Boolean IsValidUser()
        {
            bool isValidated = false;

            foreach (string user in USERS)
            {
                string[] userCredentials = user.Split('-');

                if (username.Value == userCredentials[0] && password.Value == userCredentials[1])
                    isValidated = true;
            }

            return isValidated;
        }

        private void DisplayErrorAlert(string shortMsg, string longMsg)
        {
            LoginAlert.Visible = true;
            LoginErrorShortMessage.InnerText = shortMsg;
            LoginErrorLongMessage.InnerText = longMsg;
        }
        #endregion

    }
}