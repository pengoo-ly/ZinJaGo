using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using Week1_Practical1.Helpers;

namespace Week1_Practical1
{
    public partial class Profile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["ZinJaGoDBContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsAdminLoggedIn"] == null || !(bool)Session["IsAdminLoggedIn"])
            {
                Response.Redirect("Login.aspx");
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
                            // Decode card data from VARBINARY
                            byte[] cardNumberBytes = (byte[])reader["CardNumber"];
                            string cardNumber = System.Text.Encoding.UTF8.GetString(cardNumberBytes);
                            
                            string cardName = reader["CardName"].ToString();
                            
                            // Format expiry date from DATE to MM/YY
                            DateTime expiryDateObj = (DateTime)reader["ExpireDate"];
                            string expireDate = expiryDateObj.ToString("MM/yy");

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
                string adminEmail = Session["AdminEmail"]?.ToString();
                
                if (string.IsNullOrEmpty(adminEmail))
                {
                    ShowCardError("Session expired. Please login again.");
                    return;
                }

                string cardName = txtCardName.Text.Trim();
                string cardNumber = txtCardNumber.Text.Trim().Replace(" ", "").Replace("-", "");
                string expireDate = txtCardExpiry.Text.Trim();
                string cvv = txtCardCVV.Text.Trim();

                // Log input for debugging
                DbLogger.Log($"[CARD ADD] Attempting to add card for: {adminEmail}");
                DbLogger.Log($"[CARD ADD] Card Name: {cardName}, Card Number Length: {cardNumber.Length}, Expiry: {expireDate}, CVV: {cvv.Length}");

                // Validation
                if (string.IsNullOrEmpty(cardName) || string.IsNullOrEmpty(cardNumber) ||
                    string.IsNullOrEmpty(expireDate) || string.IsNullOrEmpty(cvv))
                {
                    ShowCardError("Please fill in all credit card fields.");
                    DbLogger.Log("[CARD ADD] Validation failed: Missing required fields");
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^\d{13,19}$"))
                {
                    ShowCardError("Invalid card number. Must be 13-19 digits.");
                    DbLogger.Log("[CARD ADD] Validation failed: Invalid card number format");
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(expireDate, @"^\d{2}/\d{2}$"))
                {
                    ShowCardError("Expiry date must be in MM/YY format.");
                    DbLogger.Log("[CARD ADD] Validation failed: Invalid expiry date format");
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(cvv, @"^\d{3,4}$"))
                {
                    ShowCardError("CVV must be 3-4 digits.");
                    DbLogger.Log("[CARD ADD] Validation failed: Invalid CVV format");
                    return;
                }

                // Convert MM/YY to DATE format (YYYY-MM-DD)
                // Assume 20YY for years less than 50, 19YY for years 50 and above
                string[] dateparts = expireDate.Split('/');
                string month = dateparts[0];
                string year = "20" + dateparts[1];
                string convertedExpireDate = year + "-" + month + "-01"; // First day of expiry month

                // Encrypt card data (using basic encoding - in production, use proper encryption)
                byte[] cardNumberBytes = System.Text.Encoding.UTF8.GetBytes(cardNumber);
                byte[] cvvBytes = System.Text.Encoding.UTF8.GetBytes(cvv);

                // All validation passed
                DbLogger.Log("[CARD ADD] All validations passed, attempting database insert");

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    // Update admin's credit card info
                    string updateQuery = @"UPDATE Admins SET 
                                          CardNumber = @cardNumber, 
                                          CardName = @cardName, 
                                          ExpireDate = @expireDate, 
                                          CVV = @cvv
                                          WHERE Email = @email";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@cardNumber", cardNumberBytes);
                        cmd.Parameters.AddWithValue("@cardName", cardName);
                        cmd.Parameters.AddWithValue("@expireDate", DateTime.ParseExact(convertedExpireDate, "yyyy-MM-dd", null));
                        cmd.Parameters.AddWithValue("@cvv", cvvBytes);
                        cmd.Parameters.AddWithValue("@email", adminEmail);

                        try
                        {
                            conn.Open();
                            DbLogger.Log("[CARD ADD] Database connection opened");

                            int result = cmd.ExecuteNonQuery();
                            DbLogger.Log($"[CARD ADD] ExecuteNonQuery returned: {result}");

                            if (result > 0)
                            {
                                DbLogger.Log("[CARD ADD] Successfully added credit card");
                                ShowCardSuccess("Credit card added successfully!");
                                
                                // Clear form fields
                                ClearCardForm();
                                
                                // Reload credit cards
                                LoadCreditCards();
                                
                                // Close modal via JavaScript without redirect
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                                    "closeAddCardModal();", false);
                            }
                            else
                            {
                                DbLogger.Log("[CARD ADD] No records were updated. Email may not exist in database.");
                                ShowCardError("Failed to add card. Please check your information and try again.");
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            DbLogger.Log($"[CARD ADD ERROR] SQL Exception: {sqlEx.Message}");
                            DbLogger.Log($"[CARD ADD ERROR] SQL Number: {sqlEx.Number}");
                            ShowCardError($"Database error: {sqlEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log($"[CARD ADD ERROR] Exception: {ex.Message}");
                DbLogger.Log($"[CARD ADD ERROR] Stack Trace: {ex.StackTrace}");
                ShowCardError($"Error: {ex.Message}");
            }
        }

        private void ShowCardSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cardSuccess",
                $"showMessage('{message}', true);", true);
        }

        private void ShowCardError(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cardError",
                $"showMessage('{message.Replace("'", "\\'")}', false);", true);
        }

        protected void btnConfirmDeleteCard_Click(object sender, EventArgs e)
        {
            try
            {
                string adminEmail = Session["AdminEmail"]?.ToString();
                
                if (string.IsNullOrEmpty(adminEmail))
                {
                    ShowCardError("Session expired. Please login again.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(cs))
                {
                    string deleteQuery = @"UPDATE Admins SET 
                                          CardNumber = NULL, 
                                          CardName = NULL, 
                                          ExpireDate = NULL, 
                                          CVV = NULL
                                          WHERE Email = @email";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", adminEmail);

                        try
                        {
                            conn.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                DbLogger.Log("[CARD DELETE] Successfully deleted credit card");
                                ShowCardSuccess("Credit card deleted successfully!");
                                
                                // Reload credit cards
                                LoadCreditCards();
                                
                                // Close modal without redirect
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeDeleteModal",
                                    "closeDeleteCardModal();", false);
                            }
                            else
                            {
                                ShowCardError("Failed to delete card.");
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            DbLogger.Log($"[CARD DELETE ERROR] SQL Exception: {sqlEx.Message}");
                            ShowCardError($"Database error: {sqlEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbLogger.Log($"[CARD DELETE ERROR] Exception: {ex.Message}");
                ShowCardError($"Error: {ex.Message}");
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                int adminId = 0;
                if (Session["AdminID"] != null && int.TryParse(Session["AdminID"].ToString(), out adminId))
                {
                    DbLogger.Log("Admin logout from profile", adminId);
                }

                // Clear all session variables
                Session["IsAdminLoggedIn"] = false;
                Session["AdminID"] = null;
                Session["AdminName"] = null;
                Session["AdminRole"] = null;
                Session["AdminEmail"] = null;
                Session["AdminInitial"] = null;
                Session["AdminProfileImage"] = null;

                // Clear admin email cookie
                if (Request.Cookies["AdminEmail"] != null)
                {
                    HttpCookie cookie = new HttpCookie("AdminEmail");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }

                // Prevent page caching
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
                Response.AddHeader("Pragma", "no-cache");
                Response.AddHeader("Expires", "0");

                // Abandon session after clearing values
                Session.Abandon();

                // Redirect to login page with absolute path
                Response.Redirect("~/Login.aspx", true);
            }
            catch (Exception ex)
            {
                DbLogger.Log("Admin logout error: " + ex.Message);
                Response.Redirect("~/Login.aspx", true);
            }
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