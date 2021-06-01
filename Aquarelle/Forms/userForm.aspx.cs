using Aquarelle.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Aquarelle
{
    public partial class userForm : System.Web.UI.Page
    {
        #region Variables
        private static String folderPath = PATHS.USERS_FOLDER_PATH;
        private static String readFilesFolder = PATHS.USER_READ_FOLDER_PATH;
        private static String activeUser;
        private static List<clsFile> CURRENT_USER_FILES;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Alert.Visible = false;
                if (!this.IsPostBack)
                {
                    if (String.IsNullOrEmpty(Session["activeUser"].ToString()))
                        Response.Redirect("loginForm.aspx");

                    activeUser = Session["activeUser"].ToString();
                    this.WelcomeLabel.InnerText = "WELCOME " + Session["activeUser"].ToString();
                    UpdateGridView(false);
                }
            }
            catch (Exception ex)
            {
                AlertUser(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }

        protected void GrdFilesView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // Retrieve the row index stored in the 
                // CommandArgument property.
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve file path
                string path = CURRENT_USER_FILES.ElementAt<clsFile>(index).filePath;

                if (e.CommandName == "view")
                {
                    // Open the file
                    System.Diagnostics.Process.Start(path);
                }

                if (e.CommandName == "print")
                {
                    // Print the file
                    PrintDocument doc = new PrintDocument()
                    {
                        PrinterSettings = new PrinterSettings()
                        {
                            // set the printer to 'Microsoft Print to PDF'
                            PrinterName = "Microsoft Print to PDF",

                            // tell the object this document will print to file
                            PrintToFile = true,

                            // set the filename to whatever you like (full path)
                            PrintFileName = path,
                        }
                    };

                    doc.Print();
                    AlertUser("Printing successful!");
                }

                if (e.CommandName == "markAsRead")
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = System.IO.Path.GetFileName(path);
                    string destFile = System.IO.Path.Combine(readFilesFolder + activeUser, fileName);

                    // Copy read file to READ_FILES folder of user
                    File.Copy(path, destFile, true);

                    // Update the view
                    UpdateGridView(true);
                }

                if (e.CommandName == "extractInfo")
                {
                    ExtractInfo.StartExtraction(path);
                    AlertUser("Extraction successful! Go to DATA/ADMIN/EXTRACTED_DATA to find the extracted information.");
                    //Response.Write("<script>alert('Extraction successful!');</script>");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
            }

        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TableCell fileNameCell = e.Row.Cells[0];
                    Button extractInfo = (Button)e.Row.FindControl("btnExtractInfo");

                    if (fileNameCell.Text.Contains("CUSINFO"))
                        extractInfo.Enabled = true;
                    else
                        extractInfo.Enabled = false;


                    TableCell statusCell = e.Row.Cells[2];
                    if (statusCell.Text == "Already read.")
                    {
                        Button markAsRead = (Button)e.Row.FindControl("btnMarkAsRead");
                        markAsRead.Enabled = false;
                        statusCell.ForeColor = Color.Black;
                        statusCell.BackColor = Color.Green;
                    }
                    if (statusCell.Text == "New File!")
                    {
                        statusCell.ForeColor = Color.White;
                        statusCell.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
            }

        }

        protected void TimerVerifyNewFile_Tick(object sender, EventArgs e)
        {
            // Boolean to verify if files have been deleted
            bool isFileDeleted = false;

            // verify and get new files
            List<clsFile> lstNewFiles = VerifyAndRetriveNewFiles();

            if ((lstNewFiles != null && lstNewFiles.Count > 0))
            {
                //Response.Write("<script>alert('" + lstNewFiles.Count + " new file(s) detected');</script>");
                AlertUser(lstNewFiles.Count + " new file(s) detected!");
                UpdateGridView(true);
            }

            // verify deleted files
            VerifyDeletedFiles(ref isFileDeleted);

            if (isFileDeleted)
            {
                AlertUser("File(s) deleted!");
                UpdateGridView(true);
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["activeUser"] = null;
            Response.Redirect("loginForm.aspx");
        }
        #endregion

        #region private methods
        private List<clsFile> GetListOfFiles()
        {
            //Declaration
            List<clsFile> lstFilesToDisplay;
            List<clsFile> lstReadFiles;
            int fileIndex;

            try
            {
                // Initialisation
                lstFilesToDisplay = new List<clsFile>();
                lstReadFiles = new List<clsFile>();
                fileIndex = 1;

                //Creating the directories if not exists
                Directory.CreateDirectory(folderPath + activeUser);
                Directory.CreateDirectory(readFilesFolder + activeUser);

                // Getting file entries
                string[] fileEntriesToDisplay = Directory.GetFiles(folderPath + activeUser);
                string[] readFileEntries = Directory.GetFiles(readFilesFolder + activeUser);

                // Fill the list of files to display
                foreach (string fileName in fileEntriesToDisplay)
                {
                    lstFilesToDisplay.Add(new clsFile(fileName.Substring(fileName.LastIndexOf(@"\") + 1), fileName, activeUser, fileIndex++));
                }

                // Update the files isNewFile property to indicate whether a file has already been viewed or not
                foreach (string fileName in readFileEntries)
                {
                    string trimmedName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                    lstFilesToDisplay.Where(x => x.fileName == trimmedName).ToList().ForEach(b => b.isNewFile = false);
                }
                return lstFilesToDisplay;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
                return null;
            }

        }

        private List<clsFile> VerifyAndRetriveNewFiles()
        {
            // Declaration
            List<clsFile> newFiles;

            try
            {
                //Initialisation
                newFiles = new List<clsFile>();

                //Get list of files
                var files = GetListOfFiles();

                // Verify if new files have been added
                if ((CURRENT_USER_FILES != null && files.Count > CURRENT_USER_FILES.Count) ||
                    (CURRENT_USER_FILES == null && files.Count > 0))
                {
                    // Retrieve the new files
                    for (int i = 1; i <= files.Count - CURRENT_USER_FILES.Count; i++)
                    {
                        newFiles.Add(files.ElementAt<clsFile>(files.Count - i));
                    }

                    // Update the current user files
                    CURRENT_USER_FILES = GetListOfFiles();
                }

                return newFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
                return null;
            }
        }

        private void VerifyDeletedFiles(ref bool isFileDeleted)
        {
            try
            {
                // Initialisation
                isFileDeleted = false;

                //Get list of files
                var files = GetListOfFiles();

                // Verify if files have been deleted
                if ((CURRENT_USER_FILES != null && files.Count < CURRENT_USER_FILES.Count))
                {
                    isFileDeleted = true;

                    // Delete files in FILES_READ folder
                    foreach(clsFile objFile in CURRENT_USER_FILES)
                    {
                        clsFile findDeletedFile = files.Find(f => f.filePath == objFile.filePath);
                        if (findDeletedFile == null)
                        {
                            string fileName = objFile.fileName;
                            File.Delete(readFilesFolder+activeUser+@"\"+ objFile.fileName);
                        }
                    }    

                    // Update the current user files
                    CURRENT_USER_FILES = GetListOfFiles();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
            }
        }

        private void UpdateGridView(Boolean reloadData)
        {
            DataTable dt;

            try
            {
                if (!this.IsPostBack || reloadData)
                {
                    dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[2] {
                        new DataColumn("FileName"),
                        new DataColumn("IsFileRead")
                    });

                    CURRENT_USER_FILES = GetListOfFiles();

                    if (CURRENT_USER_FILES.Count == 0)
                    {
                        this.divNoFile.Visible = true;
                        this.GrdFilesView.Visible = false;
                        return;
                    }
                    else
                    {
                        this.divNoFile.Visible = false;
                        this.GrdFilesView.Visible = true;
                    }

                    foreach (clsFile objFile in CURRENT_USER_FILES)
                    {
                        if (objFile.isNewFile)
                            dt.Rows.Add(new object[] { objFile.fileName, "New File!" });
                        else
                            dt.Rows.Add(new object[] { objFile.fileName, "Already read." });
                    }

                    GrdFilesView.DataSource = dt;
                    GrdFilesView.DataBind();
                    GrdFilesView.UseAccessibleHeader = true;
                    GrdFilesView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AlertUser(ex.Message);
            }

        }

        private void AlertUser(string alertMessage)
        {
            this.Alert.Visible = true;
            this.AlertLabel.InnerText = alertMessage;
        }
        #endregion

    }
}