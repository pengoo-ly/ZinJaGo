<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TermsOfService.aspx.cs" Inherits="ZinJAGO_Project.TermsOfService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="row">
            <div class="col-lg-10 mx-auto">
                <div class="mb-5">
                    <h1 class="display-4 fw-bold text-dark">Terms of Service</h1>
                    <p class="lead text-muted">Last Updated: January 18, 2026</p>
                </div>
                <hr class="my-5" />

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">1. Acceptance of Terms</h2>
                    <p>
                        Welcome to <strong>ZinJaGO</strong>. By accessing or using our platform, mobile application, and hybrid marketplace services, you agree to be bound by these Terms of Service. 
                        As a Singapore-based startup catering to the Southeast Asian market, we aim to provide a modern, customer-centered experience for our B2C and C2C users.
                    </p>
                    <p>
                        These terms constitute a legally binding agreement between you and <strong>ZinJaGO Pte. Ltd.</strong> governing your use of our service innovation and smart personalization technology.
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">2. The ZinJaGO Marketplace</h2>
                    <p>ZinJaGO operates a hybrid business model designed for the modern lifestyle:</p>
                    <ul class="lh-lg">
                        <li><strong>Business-to-Consumer (B2C):</strong> Allows official brands and registered businesses to sell directly to users.</li>
                        <li><strong>Consumer-to-Consumer (C2C):</strong> Allows individuals and home-based businesses to list and sell products.</li>
                        <li><strong>Our Role:</strong> ZinJaGO provides the platform and logistics infrastructure. We are not a party to the contract of sale between buyers and sellers unless otherwise specified.</li>
                    </ul>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">3. Payments & Revenue</h2>
                    <p>To ensure a seamless shopping experience, ZinJaGO manages the marketplace ecosystem as follows:</p>
                    <ul class="list-group list-group-flush mb-4 shadow-sm border rounded">
                        <li class="list-group-item p-3"><strong>Secure Transactions:</strong> Payments are collected via third-party providers (PayNow, eWallets, Credit/Debit cards).</li>
                        <li class="list-group-item p-3"><strong>Service Fees:</strong> ZinJaGO earns revenue through commission fees, transaction fees, and optional promotional services for sellers.</li>
                        <li class="list-group-item p-3"><strong>Payouts:</strong> Funds are released to sellers after transaction verification and successful delivery.</li>
                    </ul>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">4. Logistics and Pick-Up Lockers</h2>
                    <p>
                        We pride ourselves on fast and efficient logistics. By using our delivery services, you agree that:
                        Deliveries will be made to the address provided or to a designated <strong>Pick-Up Locker</strong> collection point. 
                        Users must collect parcels from lockers within the timeframe specified in the notification to avoid return-to-sender protocols.
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">5. User Conduct & Personalization</h2>
                    <p>
                        Our platform uses <strong>Smart Personalization Technology</strong> to mitigate the busy lifestyles of our users. 
                        Users agree not to misuse the platform, list prohibited items (counterfeits/illegal goods), or interfere with our eco-friendly service innovations. 
                        We reserve the right to suspend accounts that violate these professional standards.
                    </p>
                </section>

                <section class="mb-5">
                    <h2 class="h3 mb-3 text-dark">6. Governing Law</h2>
                    <p>
                        These Terms shall be governed by and construed in accordance with the laws of the <strong>Republic of Singapore</strong>. 
                        Any disputes arising from the use of ZinJaGO shall be submitted to the non-exclusive jurisdiction of the Singapore courts.
                    </p>
                </section>

                <section class="mt-5 pt-4">
                    <div class="card border-0 shadow-lg overflow-hidden" style="border-radius: 15px;">
                        <div class="row g-0">
                            <div class="col-md-6 bg-white p-4 p-md-5">
                                <h2 class="h3 mb-4 text-dark">7. Contact Us</h2>
                                <p class="text-muted mb-4">Have questions about our Terms? Our team is available to assist you.</p>
                                
                                <div class="mb-4">
                                    <h6 class="text-uppercase text-primary fw-bold small mb-1">Legal Inquiry Email</h6>
                                    <a href="mailto:dpo@zinjago.com" class="text-decoration-none h5 text-dark">dpo@zinjago.com</a>
                                </div>

                                <div class="mb-4">
                                    <h6 class="text-uppercase text-primary fw-bold small mb-1">Registered Office</h6>
                                    <p class="text-dark mb-0">ZinJaGO PTE. LTD.</p>
                                    <p class="text-muted">1 Patterson Road, Singapore, S795237</p>
                                </div>

                                <a href="https://maps.google.com" target="_blank" class="btn btn-outline-primary btn-sm">
                                    View on Google Maps
                                </a>
                            </div>

                            <div class="col-md-6 bg-light d-flex align-items-center justify-content-center p-0" style="min-height: 300px; background: url('https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&q=80&w=1000') center/cover;">
                                <div class="text-center p-4" style="background: rgba(255,255,255,0.8); backdrop-filter: blur(5px); border-radius: 10px;">
                                    <h5 class="mb-1 text-dark">ZinJaGO Logistics HQ</h5>
                                    <p class="small text-muted mb-3">Singapore Corporate Office</p>
                                    <a href="https://maps.google.com" target="_blank" class="btn btn-primary btn-sm px-4 shadow">Open Maps</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <p class="text-center mt-5 text-muted small">
                    © 2026 ZinJaGO. All rights reserved.
                </p>
            </div>
        </div>
    </div>
</asp:Content>