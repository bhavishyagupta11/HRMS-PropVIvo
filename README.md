# Global HRMS • Enterprise WorkFlow Management

![License](https://img.shields.io/badge/license-Enterprise_Proprietary-teal.svg)
![Backend](https://img.shields.io/badge/.NET-10.0_Modular_Monolith-blueviolet)
![Frontend](https://img.shields.io/badge/Next.js-15_App_Router-black)
![GraphQL](https://img.shields.io/badge/HotChocolate-GraphQL-e535ab)
![Build](https://img.shields.io/badge/build-passing-brightgreen)
![Tests](https://img.shields.io/badge/tests-8%2F8_passing-brightgreen)

## Architecture Overview
**Global HRMS (WorkFlow)** is an enterprise-grade Human Resource Management System built upon an **ASP.NET Core 10 Modular Monolith** backend architecture and a high-performance **Next.js 15 App Router** frontend. The system is designed to provide complete lifecycle management for modern global workforces, enforcing multi-country compliance (US W-2 & India PF/ESI statutory deductions), strict Role-Based Access Control (RBAC), and enterprise-grade security.

---

## Technology Stack

### Backend
* **Runtime**: `.NET 10.0 (Core)`
* **Architecture**: Modular Monolith (Strict separation of Domain, Application, Infrastructure, and GraphQL per module).
* **CQRS Pattern**: `MediatR` for fully decoupled Command and Query pipelines.
* **ORM & Database**: `Entity Framework Core 10` with `Npgsql` for PostgreSQL context pooling.
* **GraphQL Server**: `HotChocolate.AspNetCore` for schema merging, filtering, and subscriptions.
* **Validation**: `FluentValidation` injected into MediatR pre-processors.
* **Testing**: `xUnit`, `Moq`, `FluentAssertions`.

### Frontend
* **Core Framework**: `Next.js 15.5` (App Router).
* **UI Foundation**: `React 19`, `Lucide React` iconography, `clsx` utility helper.
* **Aesthetic System**: Premium Glassmorphism UI with vibrant slate/teal/amber color palettes, smooth backdrop blurs (`backdrop-blur-md`), and micro-animations.
* **Linting**: `ESLint 9` (Native flat config via `eslint.config.mjs`).

---

## Folder Structure

```text
C:\Users\Sbhav\Coding\HRMS PropVivo
├── API
│   └── HRMS.API                  # Primary Web API host & HotChocolate endpoint (/graphql)
├── Modules                       # Fully isolated functional feature modules
│   ├── AnalyticsFeature
│   ├── AnnouncementFeature
│   ├── AttendanceFeature
│   ├── CopilotFeature            # Embedded AI Policy Assistant
│   ├── DocumentFeature
│   ├── ExpenseFeature
│   ├── IdentityFeature           # Auth, JWT, RBAC, Team Hierarchy
│   ├── LeaveFeature
│   ├── OnboardingFeature
│   ├── PayrollFeature
│   ├── PerformanceFeature
│   ├── RecognitionFeature
│   ├── RecruitmentFeature
│   └── TrainingFeature
├── Shared
│   ├── HRMS.Shared.Application   # Global CQRS behavior & authorization contracts
│   ├── HRMS.Shared.Core          # Vault secrets, OpenTelemetry tracing, Npgsql pools
│   └── HRMS.Shared.Domain        # Shared entity abstractions & value objects
├── Tests
│   └── HRMS.UnitTests            # xUnit business scenario validations
└── client                        # Next.js 15 App Router frontend application
    ├── src/components            # Polished view components & dashboard widgets
    ├── eslint.config.mjs         # ESLint 9 native flat configuration
    └── package.json              # Client dependencies
```

---

## Features & Modules (PSD Alignment)

1. **Identity & Auth**: Encrypted password storage, stateless JWT issuance, claim parsing (`sub`, `role`), multi-level RBAC (Employee, Manager, HR Admin, Executive).
2. **Onboarding**: Configurable new-hire milestones, background check document vaults, and equipment logistics tracking.
3. **Attendance**: Clock-in/out timers, geo-fenced IP validations, and overtime productive hour aggregation.
4. **Leave Management**: Automated leave balance deductions, multi-tier approval flows, and public holiday calendar sync.
5. **Payroll & Compliance**: Dynamic localized salary tax calculations, payslip generation, and statutory contribution tracking (W-4 / EPF).
6. **Document Vault**: Secure contract uploading, HR e-signatures, and verified compliance proofs.
7. **Expense Management**: Optical receipt verification workflows, multi-currency conversion, and managerial budget approval.
8. **Performance & OKRs**: Real-time OKR progress bars, 360-degree peer review cycles, and management feedback loops.
9. **Training & Certifications**: AI/HR corporate catalog enrollment, completion certificate tracking, and upskilling metrics.
10. **Recruitment & ATS**: Multi-stage candidate Kanban pipelines, interview scheduling, and job requisition publishing.
11. **Peer Recognition**: Gamified peer awards, badge attachments, and company value appreciation boards.
12. **Announcements**: Emergency company-wide broadcasts, policy alert banners, and read receipt logging.
13. **People Analytics**: Executive trend charts, attrition metrics, headcount summaries, and diversity monitoring.
14. **HR Copilot AI**: Context-aware AI bot trained directly on the 2026 corporate handbook for rapid HR policy retrieval.

---

## Installation & Configuration

### Prerequisites
* **.NET SDK**: `10.0` or higher
* **Node.js**: `v20.x` or higher
* **PostgreSQL**: `v16` or higher

### Environment Configuration
1. **Backend**: Configure your PostgreSQL connection string and JWT issuer secrets in `API/HRMS.API/appsettings.json`.
2. **Frontend**: Provide your environment variables in `client/.env`:
```env
NEXT_PUBLIC_GRAPHQL_ENDPOINT=http://localhost:5000/graphql
```

---

## Running the Application

### Starting the Backend
```bash
# Clean and restore packages
dotnet clean
dotnet restore

# Build the solution
dotnet build

# Launch Kestrel server on http://localhost:5000 / https://localhost:5001
dotnet run --project API/HRMS.API/HRMS.API.csproj
```

### Starting the Frontend
```bash
cd client

# Install packages
npm install

# Build static production application
npm run build

# Launch Next.js production server on http://localhost:3000
npm run start
```

---

## Testing

The solution includes a comprehensive unit test suite written in `xUnit` and `Moq` to assert real business scenarios.
```bash
dotnet test
```

### Expected Test Output
```text
Passed!  - Failed:     0, Passed:     8, Skipped:     0, Total:     8, Duration: 177 ms - HRMS.UnitTests.dll (net10.0)
```

---

## Deployment

### Docker Deployment
The solution is structured to support multi-stage containerization using Docker.
* **Backend Container**: Base image `mcr.microsoft.com/dotnet/aspnet:10.0`, publishing `HRMS.API.dll`.
* **Frontend Container**: Base image `node:20-alpine`, copying `.next` standalone production output.

---

## Screenshots & UI Polish
* **Dashboard**: Features rich deep teal navigation headers (`#1F6F6B`), warm amber notification tags (`#C2410C`), and beautiful semi-transparent glass cards (`backdrop-blur-md`).
* **HR Copilot**: Fully interactive AI chat drawer with policy confidence scores and immediate citation links.

---

## Roadmap
* **Q3 2026**: Multi-tenant database partitioning for enterprise sub-organizations.
* **Q4 2026**: Integration with global automated payroll disbursement gateways (TransferWise / Razorpay Payroll).

---

## License
* **Proprietary Enterprise Software**. All rights reserved by **PropVivo Inc. (2026)**. Unauthorized copying or distribution of this codebase is strictly prohibited.
