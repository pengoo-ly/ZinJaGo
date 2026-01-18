<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="ZinJAGO_Project.FAQ" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root {
            --zinjago-primary: #2D3436; 
            --zinjago-accent: #00B894; 
            --zinjago-bg: #F9FAFB;
            --text-gray: #636E72;
        }

        .faq-container {
            max-width: 850px;
            margin: 40px auto;
            padding: 20px;
            font-family: 'Segoe UI', Roboto, sans-serif;
            color: var(--zinjago-primary);
        }

        .faq-header { text-align: center; margin-bottom: 40px; }
        .faq-header h1 { font-size: 2.5rem; font-weight: 800; margin-bottom: 10px; }
        .faq-header p { color: var(--text-gray); font-size: 1.1rem; }

        /* Search Section */
        .search-wrapper { margin-bottom: 40px; }
        #faqSearch {
            width: 100%;
            padding: 16px 25px;
            font-size: 1rem;
            border: 2px solid #E0E0E0;
            border-radius: 50px;
            outline: none;
            transition: all 0.3s ease;
        }
        #faqSearch:focus { border-color: var(--zinjago-accent); box-shadow: 0 4px 15px rgba(0, 184, 148, 0.15); }

        /* Accordion Style */
        .faq-category {
            margin-top: 35px;
            font-weight: 700;
            text-transform: uppercase;
            font-size: 0.85rem;
            color: var(--zinjago-accent);
            letter-spacing: 1.5px;
            padding-bottom: 10px;
            border-bottom: 1px solid #EEE;
            margin-bottom: 15px;
        }

        .accordion-item {
            background: #fff;
            border-radius: 12px;
            margin-bottom: 15px;
            border: 1px solid #EAEAEA;
            overflow: hidden;
            transition: all 0.3s ease;
        }

        .accordion-header {
            width: 100%;
            padding: 22px;
            background: none;
            border: none;
            display: flex;
            justify-content: space-between;
            align-items: center;
            cursor: pointer;
            font-size: 1.05rem;
            font-weight: 600;
            text-align: left;
            color: var(--zinjago-primary);
        }

        .icon-plus { transition: transform 0.3s ease; color: #BDBDBD; font-size: 1.3rem; }

        .accordion-content {
            max-height: 0;
            overflow: hidden;
            transition: max-height 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            background-color: #FCFCFC;
        }

        .content-inner { padding: 0 22px 25px 22px; line-height: 1.7; color: var(--text-gray); }

        /* Interactive Feedback */
        .feedback-area {
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px solid #F0F0F0;
            display: flex;
            align-items: center;
            gap: 12px;
            font-size: 0.85rem;
        }

        .btn-feedback {
            padding: 6px 14px;
            border: 1px solid #DDD;
            background: #FFF;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 500;
        }
        .btn-feedback:hover { border-color: var(--zinjago-accent); color: var(--zinjago-accent); background: #F0FDF4; }

        /* Active State */
        .accordion-item.active { border-color: var(--zinjago-accent); box-shadow: 0 4px 12px rgba(0,0,0,0.05); }
        .accordion-item.active .icon-plus { transform: rotate(45deg); color: var(--zinjago-accent); }
        .accordion-item.active .accordion-content { max-height: 500px; }

        /* CTA Footer */
        .help-footer {
            margin-top: 60px;
            padding: 45px;
            background-color: #F0FDF4;
            border-radius: 20px;
            text-align: center;
        }

        .btn-contact {
            display: inline-block;
            margin-top: 15px;
            padding: 14px 35px;
            background-color: var(--zinjago-accent);
            color: white !important;
            text-decoration: none;
            border-radius: 50px;
            font-weight: 700;
            box-shadow: 0 4px 10px rgba(0, 184, 148, 0.3);
        }

        .hidden { display: none; }
    </style>

    <div class="faq-container">
        <div class="faq-header">
            <h1>FAQ</h1>
            <p>Smart solutions for your shopping and selling needs.</p>
        </div>

        <div class="search-wrapper">
            <input type="text" id="faqSearch" placeholder="Search for delivery, security, or selling help..." onkeyup="filterFAQ()">
        </div>

        <div id="faqList">
            <div class="faq-category section-title">Trust & Safety</div>
            
            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">What happens to my money if a C2C seller doesn't ship my item? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        ZinJaGO uses a <strong>Secure Escrow System</strong>. Your payment is held by us and only released to the seller after you confirm receipt. If the item isn't shipped within 3 days, the transaction is automatically cancelled and your money is refunded.
                        <div class="feedback-area">
                            <span>Was this helpful?</span>
                            <button type="button" class="btn-feedback" onclick="alert('Thanks!')">Yes</button>
                            <button type="button" class="btn-feedback" onclick="alert('Sorry about that!')">No</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">How does ZinJaGO’s "Smart Personalisation" save me time? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        Our AI analyzes your lifestyle preferences to show you the most relevant products first. Instead of scrolling through thousands of items, we surface the best deals from both official brands and local sellers that fit your specific needs.
                        <div class="feedback-area"><span>Was this helpful?</span><button type="button" class="btn-feedback">Yes</button><button type="button" class="btn-feedback">No</button></div>
                    </div>
                </div>
            </div>

            <div class="faq-category section-title">Delivery & Sustainability</div>

            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">Is it safe to leave my parcel in a ZinJaGO locker for 2 days? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        Absolutely. Our <strong>Pick-Up Lockers</strong> are monitored 24/7 by CCTV and require a unique encrypted QR code or PIN for access. You have up to 48 hours to collect your parcel at your own convenience.
                        <div class="feedback-area"><span>Was this helpful?</span><button type="button" class="btn-feedback">Yes</button><button type="button" class="btn-feedback">No</button></div>
                    </div>
                </div>
            </div>

            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">Can I choose an "Eco-Shipping" option to reduce my carbon footprint? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        Yes! By selecting <strong>Locker Collection</strong> instead of home delivery, you help us reduce localized van traffic and CO2 emissions by up to 30%. It's the most sustainable way to shop in Singapore.
                        <div class="feedback-area"><span>Was this helpful?</span><button type="button" class="btn-feedback">Yes</button><button type="button" class="btn-feedback">No</button></div>
                    </div>
                </div>
            </div>

            <div class="faq-category section-title">Selling & Earnings</div>

            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">How do I compete with official brands as a home-based business? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        ZinJaGO levels the playing field. Our algorithm promotes high-quality local sellers through our <strong>"Local Gems"</strong> section, and our low commission fees allow you to price your products more competitively.
                        <div class="feedback-area"><span>Was this helpful?</span><button type="button" class="btn-feedback">Yes</button><button type="button" class="btn-feedback">No</button></div>
                    </div>
                </div>
            </div>

            <div class="accordion-item faq-pair">
                <button type="button" class="accordion-header">How quickly can I withdraw my sales earnings to PayNow? <span class="icon-plus">+</span></button>
                <div class="accordion-content">
                    <div class="content-inner">
                        Once the buyer clicks "Order Received," your funds are immediately available in your ZinJaGO Wallet. Withdrawals to <strong>PayNow</strong> are typically processed within 2–4 hours.
                        <div class="feedback-area"><span>Was this helpful?</span><button type="button" class="btn-feedback">Yes</button><button type="button" class="btn-feedback">No</button></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="help-footer">
            <h3>Still have a specific question?</h3>
            <p>Our customer-centered team is ready to help you thrive on ZinJaGO.</p>
            <a href="Contact.aspx" class="btn-contact">Contact ZinJaGO Support</a>
        </div>
    </div>

    <script>
        document.querySelectorAll('.accordion-header').forEach(btn => {
            btn.addEventListener('click', () => {
                const item = btn.parentElement;
                item.classList.toggle('active');
            });
        });

        function filterFAQ() {
            const query = document.getElementById('faqSearch').value.toLowerCase();
            const pairs = document.querySelectorAll('.faq-pair');

            pairs.forEach(p => {
                const text = p.innerText.toLowerCase();
                p.style.display = text.includes(query) ? "block" : "none";
            });

            document.querySelectorAll('.section-title').forEach(title => {
                let next = title.nextElementSibling;
                let visible = false;
                while (next && next.classList.contains('faq-pair')) {
                    if (next.style.display !== "none") visible = true;
                    next = next.nextElementSibling;
                }
                title.style.display = visible ? "block" : "none";
            });
        }
    </script>
</asp:Content>