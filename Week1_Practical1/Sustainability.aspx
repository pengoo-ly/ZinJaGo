<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sustainability.aspx.cs" Inherits="ZinJAGO_Project.Sustainability" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Base Layout & Animations */
        .sdg-section { padding: 60px 0; border-bottom: 1px solid #eee; transition: background-color 0.3s ease; }
        .sdg-section:hover { background-color: #fafafa; }
        
        .sdg-logo { 
            max-width: 180px; width: 100%; height: auto; border-radius: 12px; 
            box-shadow: 0 4px 12px rgba(0,0,0,0.1); 
            transition: transform 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
        }
        .sdg-logo:hover { transform: scale(1.1) rotate(2deg); }

        .company-logo-container { 
            text-align: center; padding: 60px 0; 
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            margin-bottom: 0; border-radius: 15px 15px 0 0; 
        }

        /* Stats Section Styling */
        .stats-bar {
            background: #19486a; color: white; padding: 40px 0;
            margin-bottom: 40px; border-radius: 0 0 15px 15px;
            position: relative; overflow: hidden;
        }
        .stat-item h2 { font-size: 2.8rem; font-weight: 800; margin-bottom: 5px; color: #ffca28; transition: color 0.3s ease; }
        .stat-item p { font-size: 0.9rem; text-transform: uppercase; letter-spacing: 1.5px; opacity: 0.9; }
        .live-indicator { font-size: 0.7rem; color: #4ade80; display: block; margin-top: 5px; animation: blink 2s infinite; }
        
        @keyframes blink { 0% { opacity: 1; } 50% { opacity: 0.3; } 100% { opacity: 1; } }

        /* Content Styling */
        .sdg-title { color: #333; font-weight: bold; margin-bottom: 15px; }
        .sdg-content { font-size: 1.1rem; line-height: 1.6; color: #555; }
        .how-we-help { 
            background-color: #fff; padding: 20px; border-left: 5px solid #e5243b; 
            margin-top: 20px; border-radius: 0 10px 10px 0;
            box-shadow: 0 2px 5px rgba(0,0,0,0.05); transition: all 0.3s ease;
        }
        .how-we-help:hover { transform: translateX(10px); box-shadow: 0 5px 15px rgba(0,0,0,0.1); }
        .how-we-help h4 { font-size: 1.2rem; font-weight: bold; color: #d32f2f; }

        /* Upcoming Initiatives Styling */
        .upcoming-card {
            background: white; border: 1px solid #ddd; padding: 25px; border-radius: 12px;
            height: 100%; transition: transform 0.3s ease;
        }
        .upcoming-card:hover { transform: translateY(-10px); border-color: #19486a; box-shadow: 0 10px 20px rgba(0,0,0,0.1); }
        .initiative-tag { font-size: 0.75rem; background: #eef2f7; padding: 4px 10px; border-radius: 20px; color: #19486a; font-weight: bold; }

        .reveal { opacity: 0; transform: translateY(30px); transition: all 0.8s ease-out; }
        .reveal.active { opacity: 1; transform: translateY(0); }
    </style>

    <div class="container mt-5">
        <div class="company-logo-container">
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="Home.aspx">
                <img id="img_logo" alt="ZinJaGO Logo" src="Boulevard-modified-removebg-preview.png" style="height: 90px;" />
            </asp:HyperLink>
            <h1 class="mt-4 fw-bold text-dark">Our Sustainability Commitment</h1>
            <p class="lead">ZinJaGO's real-time impact on the UN Sustainable Development Goals.</p>
        </div>

        <div class="row stats-bar text-center">
            <div class="col-md-3 stat-item">
                <h2 class="counter" data-target="5240" id="vendors-count">0</h2>
                <p>Vendors Empowered</p>
                <span class="live-indicator">● LIVE UPDATING</span>
            </div>
            <div class="col-md-3 stat-item">
                <h2 class="counter" data-target="18420" id="meals-count">0</h2>
                <p>Meals Donated</p>
                <span class="live-indicator">● LIVE UPDATING</span>
            </div>
            <div class="col-md-3 stat-item">
                <h2 class="counter" data-target="12">0</h2>
                <p>Country Partners</p>
            </div>
            <div class="col-md-3 stat-item">
                <h2 class="counter" data-target="98">0</h2>
                <p>% Tech Reliability</p>
            </div>
        </div>

        <div class="row sdg-section align-items-center reveal">
            <div class="col-md-3 text-center mb-4 mb-md-0">
                <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTnmRuRbf2ClWGB8z2NoTG1WaN_3TtCyFjaMA&s" alt="SDG 8 Logo" class="sdg-logo" />
            </div>
            <div class="col-md-9">
                <h2 class="sdg-title">SDG 8 – Decent Work & Economic Growth</h2>
                <p class="sdg-content">
                    The role of <strong>ZinJaGO</strong> in Southeast Asia is gigantic since it allows thousands of vendors to take advantage of the digital market which is opened up for small and medium-sized businesses to make money.
                </p>
                <div class="how-we-help">
                    <h4>How do we help?</h4>
                    <ul class="mb-0">
                        <li>We make it easier for small players to earn money.</li>
                        <li>We offer the training related to the digital way of selling and customer support.</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="row sdg-section align-items-center reveal">
            <div class="col-md-3 text-center mb-4 mb-md-0">
                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/13/Sustainable_Development_Goal_09Industry.svg/1280px-Sustainable_Development_Goal_09Industry.svg.png" alt="SDG 9 Logo" class="sdg-logo" />
            </div>
            <div class="col-md-9">
                <h2 class="sdg-title">SDG 9 – Industry, Innovation & Infrastructure</h2>
                <p class="sdg-content">
                    The platform provides an unbroken digital network for all international sales and purchases. We bridge the gap between local quality and global demand.
                </p>
                <div class="how-we-help" style="border-left-color: #f39221;">
                    <h4 style="color: #f39221;">How do we help?</h4>
                    <ul class="mb-0">
                        <li>We put our money into mobile-optimized, quick, and very trustworthy tech.</li>
                        <li>Data is collected and utilized in order to create a personalized shopping experience for each customer.</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="row sdg-section align-items-center reveal">
            <div class="col-md-3 text-center mb-4 mb-md-0">
                <div class="d-flex d-md-block justify-content-center gap-2">
                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Sustainable_Development_Goal_17Partnerships.svg/1200px-Sustainable_Development_Goal_17Partnerships.svg.png" alt="SDG 17 Logo" class="sdg-logo mb-md-2" style="max-width: 140px;" />
                    <img src="https://www.singstat.gov.sg/-/media/images/find-data/sdg/goal-2/theglobalgoals_icons_color_goal_2.ashx" alt="SDG 2 Logo" class="sdg-logo" style="max-width: 140px;" />
                </div>
            </div>
            <div class="col-md-9">
                <h2 class="sdg-title">SDG 17 – Partnership for the Goals & SDG 2 – Zero Hunger</h2>
                <p class="sdg-content">
                    ZinJaGO is a robust collaborator for all stakeholders; small sellers, logistics companies, and governmental institutions. These alliances allow growth to be economically sustainable.
                </p>
                <p class="sdg-content">
                    As a consequence, ZinJaGO also helps achieve <strong>SDG 2 – Zero Hunger</strong> through cooperation with regional NGOs and food banks. Our holistic strategy recognizes that partnerships lead to community well-being.
                </p>
                <div class="how-we-help" style="border-left-color: #19486a;">
                    <h4 style="color: #19486a;">Our Impact:</h4>
                    <ul class="mb-0">
                        <li>A percentage of earnings goes to supplying quality food to people in poverty.</li>
                        <li>Consumer participation is facilitated through seasonal <strong>“Buy & Give”</strong> promotions.</li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="py-5">
            <h2 class="text-center fw-bold mb-5">Future Projects & Initiatives</h2>
            <div class="row g-4">
                <div class="col-md-4 reveal">
                    <div class="upcoming-card">
                        <span class="initiative-tag">Q3 2026</span>
                        <h4 class="mt-3 fw-bold">ZinJaGO Eco-Logistics</h4>
                        <p class="text-muted">Implementing EV-based delivery fleets to reduce our carbon footprint in metropolitan areas.</p>
                    </div>
                </div>
                <div class="col-md-4 reveal">
                    <div class="upcoming-card">
                        <span class="initiative-tag">Q4 2026</span>
                        <h4 class="mt-3 fw-bold">Rural Digital Tour</h4>
                        <p class="text-muted">Traveling workshops to help rural artisans join the digital marketplace for the first time.</p>
                    </div>
                </div>
                <div class="col-md-4 reveal">
                    <div class="upcoming-card">
                        <span class="initiative-tag">2027 Vision</span>
                        <h4 class="mt-3 fw-bold">100% Green Packaging</h4>
                        <p class="text-muted">Working toward eliminating single-use plastics from our entire supply chain ecosystem.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", () => {
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add("active");
                        if (entry.target.classList.contains('stats-bar')) { startCounters(); }
                    }
                });
            }, { threshold: 0.1 });

            document.querySelectorAll('.reveal, .stats-bar').forEach(el => observer.observe(el));

            function startCounters() {
                const counters = document.querySelectorAll('.counter');
                counters.forEach(counter => {
                    const updateCount = () => {
                        const target = +counter.getAttribute('data-target');
                        const countText = counter.innerText.replace(/,/g, '').replace(/\+/g, '').replace(/%/g, '');
                        const count = +countText;
                        const speed = target / 80;

                        if (count < target) {
                            counter.innerText = Math.ceil(count + speed).toLocaleString();
                            setTimeout(updateCount, 30);
                        } else {
                            // Final display with formatting
                            let suffix = (target === 98) ? "%" : "+";
                            counter.innerText = target.toLocaleString() + suffix;

                            // Start real-time updates for specific IDs
                            if (counter.id === "vendors-count" || counter.id === "meals-count") {
                                setInterval(() => updateRealTime(counter), Math.random() * 4000 + 3000);
                            }
                        }
                    };
                    updateCount();
                });
            }

            function updateRealTime(element) {
                let currentText = element.innerText.replace(/,/g, '').replace(/\+/g, '');
                let current = parseInt(currentText);
                let increment = Math.floor(Math.random() * 2) + 1;
                element.innerText = (current + increment).toLocaleString() + "+";

                element.style.color = "#4ade80"; // Brief flash
                setTimeout(() => { element.style.color = "#ffca28"; }, 800);
            }
        });
    </script>
</asp:Content>