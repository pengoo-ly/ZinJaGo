<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="ZinJAGO_Project.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .about-section { padding: 60px 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }
        .hero-header { background: #f8f9fa; padding: 50px; border-radius: 15px; margin-bottom: 40px; text-align: center; }
        .vision-box { border-left: 5px solid #28a745; padding: 20px; background: #f0fdf4; margin: 20px 0; }
        .team-card { text-align: center; padding: 20px; transition: transform 0.3s; }
        .team-card:hover { transform: translateY(-5px); }
        .team-img { width: 150px; height: 150px; border-radius: 50%; background: #ddd; margin: 0 auto 15px; display: flex; align-items: center; justify-content: center; color: #666; }
        .badge-eco { background-color: #28a745; color: white; padding: 5px 12px; border-radius: 20px; font-size: 0.8rem; }
        .stat-box { text-align: center; padding: 20px; }
        .stat-number { font-size: 2.5rem; font-weight: bold; color: #007bff; }
    </style>

    <main aria-labelledby="title" class="container about-section">
        
        <section class="hero-header">
            <h1 id="title">Empowering the Next Generation of Commerce</h1>
            <p class="lead text-muted">ZinJaGO is more than a marketplace. We are a seamless bridge between modern lifestyle and conscious consumerism in Southeast Asia.</p>
        </section>

        <div class="row mb-5">
            <div class="col-md-7">
                <h2>Who We Are</h2>
                <p>
                    Founded to mitigate the friction of a busy lifestyle, <strong>ZinJaGO</strong> is a customer-centered e-commerce portal designed specifically for the vibrant Southeast Asian market. 
                    By operating a hybrid <strong>B2C</strong> and <strong>C2C</strong> model, we empower established brands and home-based businesses alike to reach consumers through a secure, high-speed digital environment.
                </p>
                <p>
                    While we stand on the shoulders of industry giants, ZinJaGO sets itself apart through <strong>service innovation</strong> and a commitment to the planet. We utilize smart personalization technology to ensure that your shopping experience isn't just fast—it's yours.
                </p>
            </div>
            <div class="col-md-5">
                <div class="vision-box">
                    <h4>Our Vision <span class="badge-eco">Eco-Conscious</span></h4>
                    <p>To become the dominant marketplace across Southeast Asia, reaching <strong>1000 million customers by 2035</strong> through digital accessibility and carbon-neutral logistics.</p>
                    <a href="Sustainability.aspx" class="btn btn-outline-success btn-sm">Find out more about our impact</a>
                </div>
            </div>
        </div>

        <hr />

        <div class="row text-center my-5">
            <div class="col-md-4">
                <i class="bi bi-lightning-charge-fill" style="font-size: 2rem; color: #ffc107;"></i>
                <h4>Fast & Hybrid</h4>
                <p>Combining brand-official stores with local community sellers for unparalleled variety.</p>
            </div>
            <div class="col-md-4">
                <i class="bi bi-box-seam" style="font-size: 2rem; color: #007bff;"></i>
                <h4>Smart Logistics</h4>
                <p>From door-to-door shipping to convenient <strong>Pick-Up Lockers</strong> near your home.</p>
            </div>
            <div class="col-md-4">
                <i class="bi bi-shield-check" style="font-size: 2rem; color: #28a745;"></i>
                <h4>Secure Payments</h4>
                <p>Seamless transactions via PayNow, eWallets, and major Credit/Debit cards.</p>
            </div>
        </div>

        <hr />

        <section class="mt-5">
            <h2 class="text-center mb-5">The Minds Behind ZinJaGO</h2>
            <div class="row">
                <div class="col-md-3 col-sm-6">
                    <div class="team-card">
                        <div class="team-img">LY</div>
                        <h5>Lim Yue</h5>
                        <p class="text-primary font-weight-bold">Founder & CEO</p>
                        <small class="text-muted">Visionary leading the charge in SEA e-commerce innovation.</small>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6">
                    <div class="team-card">
                        <div class="team-img">TZ</div>
                        <h5>Tan Yong Zhe</h5>
                        <p class="text-primary font-weight-bold">Chief Technology Officer</p>
                        <small class="text-muted">Architect of our smart personalization and AI-driven UI.</small>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6">
                    <div class="team-card">
                        <div class="team-img">SS</div>
                        <h5>S Subhash</h5>
                        <p class="text-primary font-weight-bold">Head of Logistics</p>
                        <small class="text-muted">Driving our Pick-Up Locker network and fast shipping goals.</small>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6">
                    <div class="team-card">
                        <div class="team-img">WY</div>
                        <h5>Wong Hsiu Yin</h5>
                        <p class="text-primary font-weight-bold">Chief Strategy Officer</p>
                        <small class="text-muted">Expanding our B2C & C2C reach to 1 billion users.</small>
                    </div>
                </div>
            </div>
        </section>

        <div class="row mt-5 py-4 bg-light rounded shadow-sm">
            <div class="col-12 stat-box">
                <span class="stat-number">2035</span>
                <p class="text-uppercase tracking-wider">Our Goal: 1 Billion Customers</p>
            </div>
        </div>

    </main>
</asp:Content>
