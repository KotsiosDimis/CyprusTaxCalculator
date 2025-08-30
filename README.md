
# 🇨🇾 Cyprus Tax Calculator (2025) – Case Study

This case study showcases a full-stack ASP.NET Core web application that calculates personal tax amounts based on **Cyprus 2025 tax directives**, including **deductions for life insurance policies** and other eligible deductions.

---

## ✅ Features

- Calculate taxable income after deductions  ✅
- Apply official Cyprus tax brackets for 2025  ✅
- Deduct life insurance premiums (within legal limits)  ✅
- Display tax savings and total tax payable  ✅
- Dynamic UI with AJAX support (no page reload)  ✅
- Advanced mode for more complex scenarios (optional) ✅ 
- Data storage using PostgreSQL  ✅
- Clean, modular 3-tier architecture  ✅

---

## 🧱 Solution Structure

CyprusTaxCalculator/
│
├── CyprusTaxCalculator.BLL/     # Business logic and tax calculation services
│   ├── Services/
│   └── Models/
│
├── CyprusTaxCalculator.DAL/     # Data access layer (EF Core)
│   ├── Data/
│   ├── Entities/
│   └── Migrations/
│
├── CyprusTaxCalculator.UI/      # Razor Pages frontend
│   ├── Pages/
│   ├── wwwroot/
│   ├── appsettings.json
│   └── Program.cs
│
└── CyprusTaxCalculator.sln      # Solution file

---

## 🎯 Functional Requirements

### User Inputs

- 📥 **Annual income** (gross)  
- 💸 **Life insurance premiums paid**  
- 🧾 **Other deductible amounts** (optional)  

### Calculations

- Tax is calculated based on Cyprus tax rates for 2025  
- Life insurance deductions are capped according to official limits  
- All eligible deductions are subtracted before computing tax  

### Outputs

- 🧮 Total **taxable income**  
- 💰 Final **tax payable**  
- 🛡️ Calculated **tax savings from life insurance**

---

## 🛠️ Technical Stack

- **Frontend:** Razor Pages, Bootstrap 5, JavaScript  
- **Backend:** ASP.NET Core 9.0, C#  
- **Database:** PostgreSQL with Entity Framework Core  
- **Architecture:**
  - Data Layer (EF Core, PostgreSQL)  
  - Business Logic Layer (Calculation Services)  
  - Presentation Layer (Razor Pages, AJAX)

---

## 🧪 Non-Functional Requirements

- ⚙️ **Modular design:** Separated BLL, DAL, and UI projects  
- 🚀 **Performance:** Fast calculation response with no reload  
- 🔐 **Security:** Prepared for HTTPS and safe data handling  
- 🧩 **Scalability:** Easy to add new deduction types or tax brackets  
- 🎯 **Maintainability:** Clean code structure and naming conventions  

---

## 📦 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/CyprusTaxCalculator.git
   cd CyprusTaxCalculator
   ```

2. **Set up the database**
   - Update the PostgreSQL connection string in:
     CyprusTaxCalculator.UI/appsettings.json
   - Apply migrations:
     ```bash
     dotnet ef database update --project CyprusTaxCalculator.DAL --startup-project CyprusTaxCalculator.UI
     ```

3. **Run the app**
   ```bash
   dotnet run --project CyprusTaxCalculator.UI
   ```

---

## 📌 Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/)
- PostgreSQL (locally or via cloud service)

---

## 📄 License

MIT License

---

## 🙌 Acknowledgments

Made with care for the **Cyprus 2025 Tax Directive Case Study** 🌍
