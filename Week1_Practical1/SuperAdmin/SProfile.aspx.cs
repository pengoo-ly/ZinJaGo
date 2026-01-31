using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Week1_Practical1.Helpers;

namespace Week1_Practical1.SuperAdmin
{
    public partial class SProfile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsAdminLoggedIn"] == null || !(bool)Session["IsAdminLoggedIn"])
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAdminProfile();
                LoadCreditCards();
            }
        }
        private void LoadAdminProfile()
        {
            try
            {
                string email = Session["AdminEmail"].ToString();

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT AdminID, AdminName, Email, Image, PhoneNumber, DOB, Location, Biography 
                          FROM Admins WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string fullName = reader["AdminName"].ToString();
                        string adminEmail = reader["Email"].ToString();
                        string profileImage = reader["Image"] != DBNull.Value ? reader["Image"].ToString() : "";
                        string phoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "";
                        string dob = reader["DOB"] != DBNull.Value ? ((DateTime)reader["DOB"]).ToString("yyyy-MM-dd") : "";
                        string location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : "";
                        string biography = reader["Biography"] != DBNull.Value ? reader["Biography"].ToString() : "";

                        // Split name into first and last
                        string[] nameParts = fullName.Split(' ');
                        string firstName = nameParts.Length > 0 ? nameParts[0] : "";
                        string lastName = nameParts.Length > 1 ? nameParts[nameParts.Length - 1] : "";

                        // Display on profile card
                        lblAdminName.Text = fullName;
                        lblAdminEmail.Text = adminEmail;
                        lblInitial.Text = GetInitials(fullName);
                        lblAvatarPreview.Text = GetInitials(fullName);

                        // Display profile image if exists
                        if (!string.IsNullOrEmpty(profileImage))
                        {
                            string imageWithCacheBuster = profileImage + "?t=" + DateTime.Now.Ticks;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadProfilePhoto",
                                "document.getElementById('avatarDisplay').innerHTML = '<img src=\"" + imageWithCacheBuster + "\" alt=\"Profile Photo\" />';", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadPreviewPhoto",
                                "document.getElementById('uploadPreview').innerHTML = '<img src=\"" + imageWithCacheBuster + "\" alt=\"Profile Photo\" />';", true);
                        }

                        // Fill form fields
                        txtFirstName.Text = firstName;
                        txtLastName.Text = lastName;
                        txtEmail.Text = adminEmail;
                        txtPasswordDisplay.Text = "••••••••";
                        txtPhoneNumber.Text = phoneNumber;
                        txtDateOfBirth.Text = dob;
                        txtLocation.Text = location;
                        txtBiography.Text = biography;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "showMessage('Error loading profile: " + ex.Message.Replace("'", "\\'") + "', false);", true);
            }
        }

        private void LoadCreditCards()
        {
            try
            {
                string email = Session["AdminEmail"].ToString();
                string cardsHtml = "";

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT CardNumber, CardName, ExpireDate 
                          FROM Admins 
                          WHERE Email = @email AND CardNumber IS NOT NULL", conn);

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string cardNumber = reader["CardNumber"].ToString();
                            string cardName = reader["CardName"].ToString();
                            string expireDate = reader["ExpireDate"].ToString();

                            // Mask card number
                            string maskedCard = "**** **** **** " + cardNumber.Substring(Math.Max(0, cardNumber.Length - 4));

                            cardsHtml += @"
                                <div class='credit-card'>
                                    <div class='card-logo'>💳</div>
                                    <div class='card-number'>" + maskedCard + @"</div>
                                    <div class='card-info'>
                                        <div>
                                            <div class='card-holder'>Card Holder</div>
                                            <div class='card-holder-name'>" + cardName + @"</div>
                                        </div>
                                        <div class='card-expiry'>
                                            <div class='card-expiry-label'>Expires</div>
                                            <div class='card-expiry-date'>" + expireDate + @"</div>
                                        </div>
                                    </div>
                                    <div class='card-actions'>
                                        <button type='button' class='card-btn' onclick='openDeleteCardModal(""" + maskedCard + @""")' title='Delete'>🗑️</button>
                                    </div>
                                </div>";
                        }
                    }
                    else
                    {
                        cardsHtml = "<div style='text-align: center; padding: 40px 20px; color: var(--muted);'><div style='font-size: 48px; margin-bottom: 16px;'>💳</div><div>No credit cards added yet</div></div>";
                    }

                    reader.Close();
                    conn.Close();
                }

                // Inject cards HTML into page
                string escapedHtml = cardsHtml.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loadCards",
                    "document.getElementById('creditCardsGrid').innerHTML = \"" + escapedHtml + "\";", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading credit cards: " + ex.Message);
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["AdminEmail"].ToString();
                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string phoneNumber = txtPhoneNumber.Text.Trim();
                string location = txtLocation.Text.Trim();
                string biography = txtBiography.Text.Trim();
                string dobString = txtDateOfBirth.Text.Trim();

                // Validate inputs
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                        "showMessage('Please enter both first and last name.', false);", true);
                    return;
                }

                string fullName = firstName + " " + lastName;
                string profileImagePath = null;

                // Handle file upload
                if (fileUpload.HasFile)
                {
                    try
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                        string fileExtension = Path.GetExtension(fileUpload.FileName).ToLower();

                        if (System.Array.IndexOf(allowedExtensions, fileExtension) == -1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                                "showMessage('Only image files (jpg, jpeg, png, gif) are allowed.', false);", true);
                            return;
                        }

                        if (fileUpload.PostedFile.ContentLength > 5 * 1024 * 1024)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                                "showMessage('File size must be less than 5MB.', false);", true);
                            return;
                        }

                        string uniqueFileName = email.Replace("@", "_").Replace(".", "_") + "_" + DateTime.Now.Ticks + fileExtension;
                        string uploadPath = Server.MapPath("~/Uploads/Profiles/");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        string fullPath = Path.Combine(uploadPath, uniqueFileName);
                        fileUpload.SaveAs(fullPath);

                        profileImagePath = "Uploads/Profiles/" + uniqueFileName;
                    }
                    catch (Exception uploadEx)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                            "showMessage('Error uploading file: " + uploadEx.Message.Replace("'", "\\'") + "', false);", true);
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string updateQuery = @"UPDATE Admins SET 
                                          AdminName = @adminName,
                                          PhoneNumber = @phoneNumber,
                                          DOB = @dob,
                                          Location = @location,
                                          Biography = @biography";

                    if (profileImagePath != null)
                    {
                        updateQuery += ", Image = @image";
                    }

                    updateQuery += " WHERE Email = @email";

                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@adminName", fullName);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@dob", string.IsNullOrEmpty(dobString) ? DBNull.Value : (object)Convert.ToDateTime(dobString));
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@biography", biography);
                    cmd.Parameters.AddWithValue("@email", email);

                    if (profileImagePath != null)
                    {
                        cmd.Parameters.AddWithValue("@image", profileImagePath);
                    }

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result > 0)
                        {
                            Session["AdminName"] = fullName;
                            Session["AdminInitial"] = GetInitials(fullName);
                            if (profileImagePath != null)
                            {
                                Session["AdminProfileImage"] = profileImagePath;
                            }

                            string initials = GetInitials(fullName);
                            string displayImagePath = profileImagePath != null
                                ? profileImagePath + "?t=" + DateTime.Now.Ticks
                                : "";

                            string syncScript = @"
                                (function() {
                                    var initials = '" + initials + @"';
                                    var displayImage = '" + displayImagePath + @"';
                                    
                                    var sidebarAvatar = document.getElementById('sidebarAvatar');
                                    if (sidebarAvatar) {
                                        if (displayImage !== '') {
                                            sidebarAvatar.innerHTML = '<img src=""" + displayImagePath + @""" alt=""Profile"" style=""width: 100%; height: 100%; object-fit: cover;"" />';
                                        } else {
                                            sidebarAvatar.textContent = initials;
                                        }
                                    }
                                    
                                    var topbarAvatar = document.getElementById('topbarAvatar');
                                    if (topbarAvatar) {
                                        if (displayImage !== '') {
                                            topbarAvatar.innerHTML = '<img src=""" + displayImagePath + @""" alt=""Profile"" style=""width: 100%; height: 100%; object-fit: cover;"" />';
                                        } else {
                                            topbarAvatar.innerHTML = '<span class=""profile-initial"">' + initials + '</span>';
                                        }
                                    }
                                })();
                            ";

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "sync",
                                syncScript, true);

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                                "showMessage('Profile updated successfully!', true);", true);

                            LoadAdminProfile();
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                            "showMessage('Database error: " + sqlEx.Message.Replace("'", "\\'") + "', false);", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "showMessage('Error saving profile: " + ex.Message.Replace("'", "\\'") + "', false);", true);
            }
        }

        protected void btnAddCard_Click(object sender, EventArgs e)
        {
            try
            {
                string adminEmail = Session["AdminEmail"].ToString();
                string cardName = txtCardName.Text.Trim();
                string cardNumber = txtCardNumber.Text.Trim().Replace(" ", "").Replace("-", "");
                string expireDate = txtCardExpiry.Text.Trim();
                string cvv = txtCardCVV.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(cardName) || string.IsNullOrEmpty(cardNumber) ||
                    string.IsNullOrEmpty(expireDate) || string.IsNullOrEmpty(cvv))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                        "showMessage('Please fill in all credit card fields.', false);", true);
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^\d{13,19}$"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                        "showMessage('Invalid card number. Must be 13-19 digits.', false);", true);
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(expireDate, @"^\d{2}/\d{2}$"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                        "showMessage('Expiry date must be in MM/YY format.', false);", true);
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(cvv, @"^\d{3,4}$"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                        "showMessage('CVV must be 3-4 digits.', false);", true);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // Update admin's credit card info
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Admins SET 
                          CardNumber = @cardNumber, 
                          CardName = @cardName, 
                          ExpireDate = @expireDate, 
                          CVV = @cvv
                          WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@cardNumber", cardNumber);
                    cmd.Parameters.AddWithValue("@cardName", cardName);
                    cmd.Parameters.AddWithValue("@expireDate", expireDate);
                    cmd.Parameters.AddWithValue("@cvv", cvv);
                    cmd.Parameters.AddWithValue("@email", adminEmail);

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                                "showMessage('Credit card added successfully!', true);", true);

                            ClearCardForm();
                            LoadCreditCards();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                                "setTimeout(function() { closeAddCardModal(); }, 1000);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                                "showMessage('Failed to add card. No records updated.', false);", true);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                            "showMessage('Error adding card: " + sqlEx.Message.Replace("'", "\\'") + "', false);", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "showMessage('Error: " + ex.Message.Replace("'", "\\'") + "', false);", true);
            }
        }

        protected void btnConfirmDeleteCard_Click(object sender, EventArgs e)
        {
            try
            {
                string adminEmail = Session["AdminEmail"].ToString();

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Admins SET CardNumber = NULL, CardName = NULL, ExpireDate = NULL, CVV = NULL
                          WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@email", adminEmail);

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                                "showMessage('Credit card deleted successfully!', true);", true);

                            LoadCreditCards();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                                "setTimeout(function() { closeDeleteCardModal(); }, 500);", true);
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                            "showMessage('Error deleting card: " + sqlEx.Message.Replace("'", "\\'") + "', false);", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "showMessage('Error: " + ex.Message.Replace("'", "\\'") + "', false);", true);
            }
        }

        protected void btnDeletePhoto_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["AdminEmail"].ToString();
                string adminName = Session["AdminName"].ToString();
                string initials = GetInitials(adminName);

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Admins SET Image = NULL WHERE Email = @email", conn);

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (result > 0)
                    {
                        lblAvatarPreview.Text = initials;
                        Session["AdminProfileImage"] = null;

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                            "showMessage('Photo deleted successfully!', true);", true);

                        LoadAdminProfile();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "showMessage('Error deleting photo: " + ex.Message.Replace("'", "\\'") + "', false);", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadAdminProfile();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "info",
                "showMessage('Changes cancelled.', true);", true);
        }

        protected void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal",
               "openDeleteModal();", true);
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["AdminEmail"].ToString();

                if (DeleteAdminAccount(email))
                {
                    Session["IsAdminLoggedIn"] = null;
                    Session["AdminEmail"] = null;
                    Session["AdminName"] = null;
                    Session["AdminInitial"] = null;
                    Session["AdminProfileImage"] = null;
                    Session.Abandon();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect",
                        "setTimeout(function() { window.location = 'Login.aspx'; }, 2000);", true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                        "alert('Your account has been deleted successfully. Redirecting to login page...');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                    "alert('Error: " + ex.Message.Replace("'", "\\'") + "');", true);
            }
        }
        private bool DeleteAdminAccount(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Admins WHERE Email = @email", conn);
                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    conn.Close();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting account: " + ex.Message);
                return false;
            }
        }

        private void ClearCardForm()
        {
            txtCardName.Text = "";
            txtCardNumber.Text = "";
            txtCardExpiry.Text = "";
            txtCardCVV.Text = "";
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "A";

            string[] parts = name.Split(' ');
            if (parts.Length >= 2)
            {
                return (parts[0][0].ToString() + parts[parts.Length - 1][0].ToString()).ToUpper();
            }
            else if (parts.Length == 1 && parts[0].Length > 0)
            {
                return parts[0][0].ToString().ToUpper();
            }

            return "A";
        }
    }
}