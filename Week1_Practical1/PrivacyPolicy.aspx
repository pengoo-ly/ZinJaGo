<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrivacyPolicy.aspx.cs" Inherits="ZinJAGO_Project.PrivacyPolicy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="row">
            <div class="col-lg-10 mx-auto">
                <div class="mb-5">
                    <h1 class="display-4 fw-bold text-dark">Privacy Policy</h1>
                    <p class="lead text-muted">Last Updated: January 18, 2026</p>
                </div>
                <hr class="my-5" />

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">1. Introduction</h2>
                    <p>
                        Welcome to <strong>ZinJaGO</strong>. We value your privacy and are committed to protecting your personal data. 
                        As a Singapore-based e-commerce startup, this Privacy Policy explains how we collect, use, and safeguard your information 
                        when you visit our platform and use our B2C and C2C services.
                    </p>
                    <p>
                        By using ZinJaGO, you agree to the collection and use of information in accordance with this policy and the 
                        <strong>Personal Data Protection Act (PDPA)</strong> of Singapore.
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">2. Data We Collect</h2>
                    <p>To provide a seamless and smart shopping experience, we collect the following types of information:</p>
                    <ul class="lh-lg">
                        <li><strong>Identity Data:</strong> Name, date of birth, and gender.</li>
                        <li><strong>Contact Data:</strong> Shipping address, billing address, email address, and phone number.</li>
                        <li><strong>Transaction Data:</strong> Payment records (processed via third-party providers) and purchase history.</li>
                        <li><strong>Technical & Smart Personalization Data:</strong> IP address, browser type, and behavioral data used for our AI-driven personalization.</li>
                        <li><strong>Seller Data:</strong> Business registration details or individual identification for B2C/C2C verification.</li>
                    </ul>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">3. How We Use Your Data</h2>
                    <p>ZinJaGO uses your data to:</p>
                    <ul class="list-group list-group-flush mb-4 shadow-sm border rounded">
                        <li class="list-group-item p-3">Facilitate transactions between buyers and sellers.</li>
                        <li class="list-group-item p-3">Optimize logistics, including delivery tracking and Pick-Up Locker notifications.</li>
                        <li class="list-group-item p-3">Provide personalized product recommendations.</li>
                        <li class="list-group-item p-3">Process commissions and transaction fees.</li>
                    </ul>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">4. Third-Party Services & Payments</h2>
                    <p>
                        We do not store full credit card details on our servers. All financial transactions are handled by secure third-party 
                        payment gateways (e.g., PayNow, Credit/Debit Card processors). 
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">5. Data Security</h2>
                    <p>
                        As a modern tech startup, we implement industry-standard encryption (SSL) to protect your data. However, 
                        no method of transmission over the Internet is 100% secure.
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">6. Your Rights</h2>
                    <p>Under the PDPA, you have the right to access, correct, or request the deletion of your personal data at any time.</p>
                </section>

                <section class="mt-5 pt-4">
                    <div class="card border-0 shadow-lg overflow-hidden" style="border-radius: 15px;">
                        <div class="row g-0">
                            <div class="col-md-6 bg-white p-4 p-md-5">
                                <h2 class="h3 mb-4 text-dark">7. Contact Us</h2>
                                <p class="text-muted mb-4">Questions about your data? Our Data Protection Officer is here to help.</p>
                                
                                <div class="mb-4">
                                    <h6 class="text-uppercase text-primary fw-bold small mb-1">Email Address</h6>
                                    <a href="mailto:dpo@zinjago.com" class="text-decoration-none h5 text-dark">dpo@zinjago.com</a>
                                </div>

                                <div class="mb-4">
                                    <h6 class="text-uppercase text-primary fw-bold small mb-1">Registered Office</h6>
                                    <p class="text-dark mb-0">ZinJaGO PTE. LTD.</p>
                                    <p class="text-muted">1 Patterson Road, Singapore, S795237</p>
                                </div>

                                <a href="https://www.google.com/maps/search/?api=1&query=1+Patterson+Road+Singapore+S795237" target="_blank" class="btn btn-outline-primary btn-sm">
                                    View on Google Maps
                                </a>
                            </div>

                            <div class="col-md-6 bg-light d-flex align-items-center justify-content-center p-0" style="min-height: 300px; background: url('https://images.unsplash.com/photo-1524661135-423995f22d0b?auto=format&fit=crop&q=80&w=1000') center/cover;">
                                <div class="text-center p-4" style="background: rgba(255,255,255,0.8); backdrop-filter: blur(5px); border-radius: 10px;">
                                    <h5 class="mb-1 text-dark">Find us in Singapore</h5>
                                    <p class="small text-muted mb-3">ZinJaGO HQ @ 795237</p>
                                    <a href="https://www.google.com/maps/search/?api=1&query=1+Patterson+Road+Singapore+S795237" target="_blank" class="btn btn-primary btn-sm px-4 shadow">Open Maps</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <p class="text-center mt-5 text-muted small">
                    &copy; 2026 ZinJaGO. All rights reserved.
                </p>
            </div>
        </div>
    </div>
</asp:Content>
