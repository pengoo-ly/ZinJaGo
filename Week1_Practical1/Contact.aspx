<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ZinJAGO_Project.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@300;400;600;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">

    <style>
        :root {
            --zin-primary: #1e293b; 
            --zin-secondary: #334155; 
            --zin-accent: #3a86ff; 
            --zin-bg: #f1f5f9; 
        }

        body { font-family: 'Plus Jakarta Sans', sans-serif; background-color: var(--zin-bg); }

        .contact-card {
            border: none;
            border-radius: 24px;
            overflow: hidden;
            box-shadow: 0 20px 60px rgba(15, 23, 42, 0.1);
            margin-top: 50px;
        }

        /* Sidebar Styling - Deep Slate/Navy */
        .contact-info-panel {
            background: linear-gradient(135deg, var(--zin-primary) 0%, var(--zin-secondary) 100%);
            color: white;
            padding: 60px 40px;
        }

        #img_logo {
            max-width: 200px;
            height: auto;
            margin-bottom: 30px;
            filter: drop-shadow(0 0 10px rgba(58, 134, 255, 0.2));
        }

        .info-link {
            color: #cbd5e1;
            text-decoration: none;
            transition: 0.3s;
        }

        .info-link:hover { color: white; }

        .icon-box {
            width: 44px;
            height: 44px;
            background: rgba(255, 255, 255, 0.08);
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: 15px;
            color: var(--zin-accent);
        }

        /* Form Styling */
        .form-panel { padding: 60px; background: white; }
        
        .form-control, .form-select {
            border: 1px solid #e2e8f0;
            padding: 12px 18px;
            border-radius: 10px;
            font-size: 0.95rem;
            background-color: #f8fafc;
        }

        .form-control:focus, .form-select:focus {
            box-shadow: 0 0 0 4px rgba(58, 134, 255, 0.1);
            border-color: var(--zin-accent);
            background-color: white;
            outline: none;
        }

        .btn-send {
            background: var(--zin-primary);
            color: white;
            border: none;
            padding: 14px;
            border-radius: 10px;
            font-weight: 600;
            transition: all 0.3s ease;
        }

        .btn-send:hover {
            background: var(--zin-accent);
            transform: translateY(-2px);
            box-shadow: 0 10px 20px rgba(58, 134, 255, 0.2);
        }

        .social-divider {
            border-top: 1px solid rgba(255, 255, 255, 0.1) !important;
        }
    </style>

    <div class="container pb-5">
        <div class="row justify-content-center">
            <div class="col-lg-11">
                <div class="card contact-card">
                    <div class="row g-0">
                        
                        <div class="col-md-5 contact-info-panel">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Home.aspx" CssClass="logo-link">
                                <img id="img_logo" alt="ZinJaGO Logo" src="Boulevard-modified-removebg-preview.png" />
                            </asp:HyperLink>
                            
                            <h2 class="fw-bold mt-4">Contact Us</h2>
                            <p class="mb-5" style="color: #cbd5e1; line-height: 1.6;">
                                Have a question about our services or need assistance with your account? Our team is here to help you.
                            </p>

                            <div class="d-flex align-items-center mb-4">
                                <div class="icon-box"><i class="bi bi-geo-alt"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">Office Address</h6>
                                    <small style="color: #cbd5e1;">One Patterson Road, Singapore 795237</small>
                                </div>
                            </div>

                            <div class="d-flex align-items-center mb-4">
                                <div class="icon-box"><i class="bi bi-envelope-at"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">Email Support</h6>
                                    <a href="mailto:sshash23@gmail.com" class="info-link small">sshash23@gmail.com</a><br />
                                    <a href="mailto:nothuman707@gmail.com" class="info-link small">nothuman707@gmail.com</a>
                                </div>
                            </div>

                            <div class="d-flex align-items-center mb-5">
                                <div class="icon-box"><i class="bi bi-telephone"></i></div>
                                <div>
                                    <h6 class="mb-0 fw-bold">Contact Number</h6>
                                    <small style="color: #cbd5e1;">+65 6789 0123</small>
                                </div>
                            </div>

                            <div class="d-flex gap-3 pt-4 social-divider">
                                <a href="#" class="btn btn-outline-light btn-sm rounded-circle"><i class="bi bi-facebook"></i></a>
                                <a href="#" class="btn btn-outline-light btn-sm rounded-circle"><i class="bi bi-instagram"></i></a>
                                <a href="#" class="btn btn-outline-light btn-sm rounded-circle"><i class="bi bi-linkedin"></i></a>
                            </div>
                        </div>

                        <div class="col-md-7 form-panel">
                            <h3 class="fw-bold mb-4" style="color: var(--zin-primary);">Send a Message</h3>
                            
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label class="form-label">Full Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Alyana Gonzales"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@email.com" TextMode="Email"></asp:TextBox>
                                </div>
                                
                                <div class="col-12">
                                    <label class="form-label">Subject</label>
                                    <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-select">
                                        <asp:ListItem Text="Select a Topic" Value="" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="General Inquiry" Value="General"></asp:ListItem>
                                        <asp:ListItem Text="Order & Shipping Support" Value="Logistics"></asp:ListItem>
                                        <asp:ListItem Text="Seller Inquiries (B2C/C2C)" Value="Sellers"></asp:ListItem>
                                        <asp:ListItem Text="Payment & Security" Value="Payment"></asp:ListItem>
                                        <asp:ListItem Text="Technical Feedback" Value="Technical"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-12">
                                    <label class="form-label">Your Message</label>
                                    <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Write your message here..."></asp:TextBox>
                                </div>
                                <div class="col-12 pt-3">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Inquiry" CssClass="btn btn-send w-100" />
                                </div>
                                <div class="col-12 text-center mt-3">
                                    <small class="text-muted"><i class="bi bi-shield-check"></i> ZinJaGO Secure Communication Hub</small>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>